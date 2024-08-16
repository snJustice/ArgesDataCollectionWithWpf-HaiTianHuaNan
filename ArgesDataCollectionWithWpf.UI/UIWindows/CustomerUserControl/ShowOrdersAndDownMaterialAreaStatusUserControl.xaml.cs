using Abp.Dependency;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.OrdersFromMesApplication;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.OrdersFromMesApplication.Dto;
using ArgesDataCollectionWithWpf.Application.OtherModelDto;
using ArgesDataCollectionWithWpf.UI.SingletonResource.ModlingMachineDeviceResource;
using ArgesDataCollectionWithWpf.UI.SingletonResource.SendOrderMessageResource;
using ArgesDataCollectionWpf.DataProcedure.Utils;
using ArgesDataCollectionWpf.DataProcedure.Utils.Quene;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// ShowOrdersAndDownMaterialAreaStatusUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class ShowOrdersAndDownMaterialAreaStatusUserControl : UserControl, ITransientDependency
    {
        CancellationTokenSource cancelToken;

        OrderModlingMachineDownMaterialAreaDto _orderModlingMachineDownMaterialAreaDto = new OrderModlingMachineDownMaterialAreaDto();

        private readonly IOrdersFromMesApplication _ordersFromMesApplication;//订单数据库
        private readonly SendOrderMessageToPlcSingleton _sendOrderMessageToPlcSingleton;//发送信息给plc
        private readonly ModlingMachineTypeAndPullRodSingletonCombineRoules _modlingMachineTypeAndPullRodSingletonCombineRoules;//机型和拉杆对应
        private readonly CustomerQueneForCodesFromMes _customerQueneForCodesFromMes;//队列


        private readonly ILogger _logger;

        private readonly LogUserControl _controlLog;

        public ShowOrdersAndDownMaterialAreaStatusUserControl(IOrdersFromMesApplication ordersFromMesApplication
            , SendOrderMessageToPlcSingleton sendOrderMessageToPlcSingleton, ModlingMachineTypeAndPullRodSingletonCombineRoules modlingMachineTypeAndPullRodSingletonCombineRoules, CustomerQueneForCodesFromMes customerQueneForCodesFromMes, ILogger logger, LogUserControl controlLog)
        {
            InitializeComponent();
            this._ordersFromMesApplication = ordersFromMesApplication;
            this._sendOrderMessageToPlcSingleton = sendOrderMessageToPlcSingleton;
            this._modlingMachineTypeAndPullRodSingletonCombineRoules = modlingMachineTypeAndPullRodSingletonCombineRoules;
            this._customerQueneForCodesFromMes = customerQueneForCodesFromMes;
            this._logger = logger;
            this._controlLog = controlLog;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.grid_ShowDownMaterialAreaStatus.DataContext = this._orderModlingMachineDownMaterialAreaDto;

            DateTime start = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")); ;
            DateTime end = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59")); ;
            var todayOrders = this._ordersFromMesApplication.QuerryAllOrdersFromMesByDate(start, end);

            this._orderModlingMachineDownMaterialAreaDto.OrderModlingMachineDownMaterialArea.Clear();

            foreach (var item in todayOrders)
            {
                this._orderModlingMachineDownMaterialAreaDto.OrderModlingMachineDownMaterialArea.Add(new QuerryOrdersFromMesOutputNotify
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
                    IsLoadMaterialAreaSendOrder = item.IsLoadMaterialAreaSendOrder,
                    IsDownMaterialAreaSendOrder = item.IsDownMaterialAreaSendOrder

                });
            }



            Init();
        }


        private int runeddIndex = 0;



        private void WriteMessageLogAndControl(string message)
        {
            this._logger.LogInformation(message);
            this._controlLog.WriteLog($"{DateTime.Now.ToString("yyyy-MM-dd,HH-mm-ss,ff")}-{message}");
        }

        private void Init()
        {
            

            cancelToken = new CancellationTokenSource();



            

            //下料处线程
            Task.Run(() => {
                
                ForeachRunCountModifyColor();
                while (cancelToken.IsCancellationRequested != true)
                {
                    Thread.Sleep(100);
                    //如果收到了上料区完成信号的话，
                    LoadMaterialAreaAndDownMaterialDto data = null; ;


                    //到达最后一个就不去改变颜色了,并且保持着红色,让人去看
                    if (runeddIndex >= this._orderModlingMachineDownMaterialAreaDto.OrderModlingMachineDownMaterialArea.Count)
                    {




                        //WriteMessageLogAndControl("今日下料处订单已完成，不下发");
                        continue;
                    }


                    var resu = this._customerQueneForCodesFromMes.DownMaterialQuene.TryReceive(out data);
                    if (resu != true)
                    {
                        Thread.Sleep(100);
                        continue; ;
                    }

                    else
                    {

                        if (runeddIndex >= 0)
                        {
                            //显示颜色,把上一个订单颜色变成绿色，
                            //DataRowView drv = this.grid_ShowLoadMaterialAreaStatus.Items[runeddIndex-1] as DataRowView;
                            DataGridRow row = (DataGridRow)this.grid_ShowDownMaterialAreaStatus.ItemContainerGenerator.ContainerFromIndex(runeddIndex);
                            this.Dispatcher.Invoke(() => {

                                string xxx = row.Background.ToString();
                                if (row.Background.ToString() != "#FFFFFF00")
                                {
                                    row.Background = new SolidColorBrush(Colors.Green);
                                }


                            });



                        }

                        runeddIndex++;


                        //下发plc的产量或者订单等信息
                        if (runeddIndex < this._orderModlingMachineDownMaterialAreaDto.OrderModlingMachineDownMaterialArea.Count)
                        {
                            //颜色变化，并且数据更新
                            //下一个订单颜色变成红色
                            DataGridRow row2 = (DataGridRow)this.grid_ShowDownMaterialAreaStatus.ItemContainerGenerator.ContainerFromIndex(runeddIndex);
                            this.Dispatcher.Invoke(new Action(() => { row2.Background = new SolidColorBrush(Colors.Red); }));

                            //这个时候保存数据到数据库
                            var m = this._orderModlingMachineDownMaterialAreaDto.OrderModlingMachineDownMaterialArea[runeddIndex];
                            var add = new AddOrUpdateOrdersFromMesInput
                            {

                                ID = m.ID,

                                IsDownMaterialAreaSendOrder = 1,

                            };
                            List<AddOrUpdateOrdersFromMesInput> update = new List<AddOrUpdateOrdersFromMesInput>();
                            update.Add(add);
                            this._ordersFromMesApplication.InsertOrUpdateOrdersFromMesDownAreaSendOKCount(update);


                            SendOneOrderDownArea(runeddIndex);

                            WriteMessageLogAndControl("下料处已下发一个订单");
                            


                        }
                        else
                        {
                            WriteMessageLogAndControl("下料处今日订单已完成，不下发");
                        }

                    }

                }




            });



        }


        private void ForeachRunCountModifyColor()
        {
            int count = this._orderModlingMachineDownMaterialAreaDto.OrderModlingMachineDownMaterialArea.Count;
            for (runeddIndex = 0; runeddIndex < count; runeddIndex++)
            {
                //DataRowView drv = this.grid_ShowLoadMaterialAreaStatus.Items[runeddIndex] as DataRowView;
                DataGridRow row = (DataGridRow)this.grid_ShowDownMaterialAreaStatus.ItemContainerGenerator.ContainerFromIndex(runeddIndex);
                int isDownMaterialAreaSendOrder = this._orderModlingMachineDownMaterialAreaDto.OrderModlingMachineDownMaterialArea[runeddIndex].IsDownMaterialAreaSendOrder;



                int isjump = this._orderModlingMachineDownMaterialAreaDto.OrderModlingMachineDownMaterialArea[runeddIndex].IsJump;


                if (isjump > 0)
                {
                    //跳单的话就继续

                    this.Dispatcher.Invoke(new Action(() => { row.Background = new SolidColorBrush(Colors.Yellow); }));
                    continue;
                }
                else if (isDownMaterialAreaSendOrder == 0)
                {

                    runeddIndex--;
                    return;
                }
                else if (isDownMaterialAreaSendOrder > 0)
                {

                    this.Dispatcher.Invoke(new Action(() => { row.Background = new SolidColorBrush(Colors.Green); }));
                    


                    continue;
                }





            }
        }


        private void SendOneOrderDownArea(int index)
        {

            //获第一条的数量和型号，然后下发，
            string name = this._orderModlingMachineDownMaterialAreaDto.OrderModlingMachineDownMaterialArea[index].MoldingMachineName;
            int sendIndex = this._modlingMachineTypeAndPullRodSingletonCombineRoules.ModlingMachineTypeAndPullRodCombines[name].ModlingMachineTypeSendToPlcID;


            int quality = this._orderModlingMachineDownMaterialAreaDto.OrderModlingMachineDownMaterialArea[index].ProduceQuantity;
            int runnned = this._orderModlingMachineDownMaterialAreaDto.OrderModlingMachineDownMaterialArea[index].RunnedCount;
            int pollrodID = this._orderModlingMachineDownMaterialAreaDto.OrderModlingMachineDownMaterialArea[index].StackNumber;

            this._sendOrderMessageToPlcSingleton.SendModlingPollRodQualityDownMaterialArea(sendIndex, quality - runnned, pollrodID
                , 0
                , 0);

            
        }

        public void Close()
        {
            cancelToken.Cancel();
            //this.Close();
        }


    }

    public class OrderModlingMachineDownMaterialAreaDto : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, e);
            }

        }

        private ObservableCollection<QuerryOrdersFromMesOutputNotify> orderModlingMachineDownMaterialArea = new ObservableCollection<QuerryOrdersFromMesOutputNotify>();
        public ObservableCollection<QuerryOrdersFromMesOutputNotify> OrderModlingMachineDownMaterialArea
        {
            get { return orderModlingMachineDownMaterialArea; }
            set
            {
                orderModlingMachineDownMaterialArea = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OrderModlingMachineDownMaterialArea"));
            }
        }

    }

}
