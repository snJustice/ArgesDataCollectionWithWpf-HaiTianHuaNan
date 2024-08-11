using Abp.Dependency;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.ModlingCodesApplication.Dto;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.OrdersFromMesApplication;
using ArgesDataCollectionWithWpf.Application.OtherModelDto;
using ArgesDataCollectionWithWpf.UI.SingletonResource.ModlingMachineDeviceResource;
using ArgesDataCollectionWithWpf.UI.SingletonResource.SendOrderMessageResource;
using ArgesDataCollectionWpf.DataProcedure.Utils.Quene;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ArgesDataCollectionWithWpf.UI.UIWindows.CustomerUserControl
{
    /// <summary>
    /// ShowOrdersAndLoadMaterialAreaStatusUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class ShowOrdersAndLoadMaterialAreaStatusUserControl : UserControl, ITransientDependency
    {

        CancellationTokenSource cancelToken;

        OrderModlingMachineLoadMaterialAreaDto _orderModlingMachineLoadMaterialAreaDto = new OrderModlingMachineLoadMaterialAreaDto();

        private readonly IOrdersFromMesApplication _ordersFromMesApplication;//订单数据库
        private readonly SendOrderMessageToPlcSingleton _sendOrderMessageToPlcSingleton;//发送信息给plc
        private readonly ModlingMachineTypeAndPullRodSingletonCombineRoules _modlingMachineTypeAndPullRodSingletonCombineRoules;//机型和拉杆对应
        private readonly CustomerQueneForCodesFromMes _customerQueneForCodesFromMes;//队列

        public ShowOrdersAndLoadMaterialAreaStatusUserControl(IOrdersFromMesApplication ordersFromMesApplication, SendOrderMessageToPlcSingleton sendOrderMessageToPlcSingleton, ModlingMachineTypeAndPullRodSingletonCombineRoules modlingMachineTypeAndPullRodSingletonCombineRoules, CustomerQueneForCodesFromMes customerQueneForCodesFromMes)
        {
            InitializeComponent();
            this._ordersFromMesApplication = ordersFromMesApplication;
            this._sendOrderMessageToPlcSingleton = sendOrderMessageToPlcSingleton;
            this._modlingMachineTypeAndPullRodSingletonCombineRoules = modlingMachineTypeAndPullRodSingletonCombineRoules;
            this._customerQueneForCodesFromMes = customerQueneForCodesFromMes;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.grid_ShowLoadMaterialAreaStatus.DataContext = this._orderModlingMachineLoadMaterialAreaDto;

            DateTime start = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")); ;
            DateTime end = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59")); ;
            var todayOrders = this._ordersFromMesApplication.QuerryAllOrdersFromMesByDate(start, end);

            this._orderModlingMachineLoadMaterialAreaDto.OrderModlingMachineLoadMaterialArea.Clear();
            foreach (var item in todayOrders)
            {
                this._orderModlingMachineLoadMaterialAreaDto.OrderModlingMachineLoadMaterialArea.Add(new QuerryOrdersFromMesOutputNotify
                {


                    ID = item.ID,
                    MoldingMachineName = item.MoldingMachineName,
                    MoldingMachineSerialName = item.MoldingMachineSerialName,
                    MoldingTypes = item.MoldingTypes,
                    ProduceQuantity = item.ProduceQuantity,
                    ProduceQueneNumber = item.ProduceQueneNumber,
                    OrderID = item.OrderID,
                    ProduceDate = item.ProduceDate,
                    RunnedCount = item.RunnedCount,
                    ScanedCount = item.ScanedCount,
                    StackNumber = item.StackNumber,
                    Stacks = item.Stacks,
                    Status = item.Status,
                    WorkOrderID = item.WorkOrderID,
                    IsJump = item.IsJump,

                });
            }


            Init();
        }


        private int runeddIndex = 0;

        private void Init()
        {
            this._sendOrderMessageToPlcSingleton.InitAddress();

            cancelToken = new CancellationTokenSource();
            


            Task.Run(async () => {

                await Task.Delay(300);
                ForeachRunCountModifyColor();

            });


            bool isLoadSave = true;

            //上料处线程
            Task.Run(() => {
                Thread.Sleep(1000);
                while (cancelToken.IsCancellationRequested != true)
                {
                    Thread.Sleep(100);
                    //如果收到了上料区完成信号的话，
                    LoadMaterialAreaAndDownMaterialDto data = null; ;

                    
                    //
                    if (runeddIndex >= this._orderModlingMachineLoadMaterialAreaDto.OrderModlingMachineLoadMaterialArea.Count)
                    {

                        if (isLoadSave)
                        {
                            isLoadSave = false;
                            this._customerQueneForCodesFromMes.LoadMaterialQuene.Post(new LoadMaterialAreaAndDownMaterialDto { LoadOrDownArea = LoadOrDwonEnum.LoadMaterialArea });

                        }
                        DataGridRow row = (DataGridRow)this.grid_ShowLoadMaterialAreaStatus.ItemContainerGenerator.ContainerFromIndex(runeddIndex-1);
                        this.Dispatcher.Invoke(new Action(() => { row.Background = new SolidColorBrush(Colors.Green); }));
                        //
                        
                        continue;
                    }


                    var resu = this._customerQueneForCodesFromMes.LoadMaterialQuene.TryReceive(out data);
                    if (resu != true)
                    {
                        Thread.Sleep(100);
                        continue; ;
                    }
                    //如果是下料区的信号，没有超过订单数量，就直接下发完成信号
                    else 
                    {
                        isLoadSave = true;
                        if (runeddIndex  >= 0)
                        {
                            //DataRowView drv = this.grid_ShowLoadMaterialAreaStatus.Items[runeddIndex-1] as DataRowView;
                            DataGridRow row = (DataGridRow)this.grid_ShowLoadMaterialAreaStatus.ItemContainerGenerator.ContainerFromIndex(runeddIndex);
                            this.Dispatcher.Invoke(() => {

                                string xxx = row.Background.ToString();
                                if (row.Background.ToString() != "#FFFFFF00")
                                {
                                    row.Background = new SolidColorBrush(Colors.Green);
                                }


                            });
                            
                            
                            
                        }

                        runeddIndex++;

                        //颜色变化，并且数据更新
                        //显示颜色,把上一个订单颜色变成绿色，下一个订单颜色变成红色
                        if (runeddIndex < this._orderModlingMachineLoadMaterialAreaDto.OrderModlingMachineLoadMaterialArea.Count)
                        {
                            DataGridRow row2 = (DataGridRow)this.grid_ShowLoadMaterialAreaStatus.ItemContainerGenerator.ContainerFromIndex(runeddIndex);
                            this.Dispatcher.Invoke(new Action(() => { row2.Background = new SolidColorBrush(Colors.Red); }));
                        }
                        //下发plc的产量或者订单等信息
                        if (runeddIndex < this._orderModlingMachineLoadMaterialAreaDto.OrderModlingMachineLoadMaterialArea.Count)
                        {
                            SendOneOrderLoadArea(runeddIndex);
                        }
                        
                    }

                }




            });


            //下料处线程
            Task.Run(() => {

                while (cancelToken.IsCancellationRequested != true)
                {
                    Thread.Sleep(100);
                    //如果收到了上料区完成信号的话，
                    LoadMaterialAreaAndDownMaterialDto data = null; ;

                    var resuDown = this._customerQueneForCodesFromMes.DownMaterialQuene.TryReceive(out data);
                    if (resuDown == true)
                    {
                        SendDownAtDownArea();

                        
                    }

                   




                }




            });
        }


        //第一次初始化的时候，进行订单的处理，查看颜色筛选
        private void ForeachRunCountModifyColor()
        {
            int count = this._orderModlingMachineLoadMaterialAreaDto.OrderModlingMachineLoadMaterialArea.Count;
            for (runeddIndex = 0; runeddIndex < count; runeddIndex++)
            {
                //DataRowView drv = this.grid_ShowLoadMaterialAreaStatus.Items[runeddIndex] as DataRowView;
                DataGridRow row = (DataGridRow)this.grid_ShowLoadMaterialAreaStatus.ItemContainerGenerator.ContainerFromIndex(runeddIndex);
                int produceQuantity = this._orderModlingMachineLoadMaterialAreaDto.OrderModlingMachineLoadMaterialArea[runeddIndex].ProduceQuantity;

                int runnedCount = this._orderModlingMachineLoadMaterialAreaDto.OrderModlingMachineLoadMaterialArea[runeddIndex].RunnedCount;

                int isjump = this._orderModlingMachineLoadMaterialAreaDto.OrderModlingMachineLoadMaterialArea[runeddIndex].IsJump;

                
                if (isjump > 0)
                {
                    //跳单的话就继续

                    this.Dispatcher.Invoke(new Action(() => { row.Background = new SolidColorBrush(Colors.Yellow); }));
                    continue;
                }
                else if (runeddIndex == 0 && runnedCount ==0)
                {
                    this._customerQueneForCodesFromMes.LoadMaterialQuene.Post(new LoadMaterialAreaAndDownMaterialDto { LoadOrDownArea = LoadOrDwonEnum.LoadMaterialArea });
                    runeddIndex = -1;
                    return;
                }
                else if (runnedCount < produceQuantity )
                {
                    runeddIndex--;
                    this.Dispatcher.Invoke(new Action(() => { row.Background = new SolidColorBrush(Colors.Red); }));
                    this._customerQueneForCodesFromMes.LoadMaterialQuene.Post(new LoadMaterialAreaAndDownMaterialDto { LoadOrDownArea = LoadOrDwonEnum.LoadMaterialArea });
                    
                    
                    return;
                }
                else if (runnedCount == produceQuantity)
                {
                    
                    this.Dispatcher.Invoke(new Action(() => { row.Background = new SolidColorBrush(Colors.Green); }));
                }
                


            }
        }


        //上料区下发订单
        private void SendOneOrderLoadArea(int index)
        {

            //获第一条的数量和型号，然后下发，
            string name = this._orderModlingMachineLoadMaterialAreaDto.OrderModlingMachineLoadMaterialArea[index].MoldingMachineName;
            int sendIndex = this._modlingMachineTypeAndPullRodSingletonCombineRoules.ModlingMachineTypeAndPullRodCombines[name].ModlingMachineTypeSendToPlcID;


            int quality = this._orderModlingMachineLoadMaterialAreaDto.OrderModlingMachineLoadMaterialArea[index].ProduceQuantity;
            int runnned = this._orderModlingMachineLoadMaterialAreaDto.OrderModlingMachineLoadMaterialArea[index].RunnedCount;
            int pollrodID = this._orderModlingMachineLoadMaterialAreaDto.OrderModlingMachineLoadMaterialArea[index].StackNumber;

            this._sendOrderMessageToPlcSingleton.SendModlingPollRodQualityLoadMaterialArea(sendIndex, quality - runnned, pollrodID
                , 0
                , 0);

            //第一个订单，直接给下发完成信号
            if (index == 0)
            {
                SendDownAtDownArea();
            }
        }

        private void SendDownAtDownArea()
        {

            this._sendOrderMessageToPlcSingleton.SendDownDownArea();
        }

        public void Close()
        {
            cancelToken.Cancel();
        }





    }




    public class OrderModlingMachineLoadMaterialAreaDto : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, e);
            }

        }

        private ObservableCollection<QuerryOrdersFromMesOutputNotify> orderModlingMachineLoadMaterialArea = new ObservableCollection<QuerryOrdersFromMesOutputNotify>();
        public ObservableCollection<QuerryOrdersFromMesOutputNotify> OrderModlingMachineLoadMaterialArea
        {
            get { return orderModlingMachineLoadMaterialArea; }
            set
            {
                orderModlingMachineLoadMaterialArea = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OrderModlingMachineLoadMaterialArea"));
            }
        }

    }

}
