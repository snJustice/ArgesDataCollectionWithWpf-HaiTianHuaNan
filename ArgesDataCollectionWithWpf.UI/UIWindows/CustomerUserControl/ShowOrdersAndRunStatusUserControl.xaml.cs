using ArgesDataCollectionWithWpf.Application.DataBaseApplication.OrdersFromMesApplication;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Abp.Dependency;

using ArgesDataCollectionWpf.DataProcedure.Utils.Quene;
using System.Threading.Tasks.Dataflow;
using System.Threading;
using System.Data;
using AutoMapper;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application.Dto;
using ArgesDataCollectionWithWpf.Communication;
using ArgesDataCollectionWpf.DataProcedure.Utils;
using Microsoft.Extensions.Logging;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application;
using ArgesDataCollectionWithWpf.UI.SingletonResource.ModlingMachineDeviceResource;
using ArgesDataCollectionWithWpf.UI.SingletonResource.SendOrderMessageResource;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.OrdersFromMesApplication.Dto;
using ArgesDataCollectionWithWpf.UI.SingletonResource.ProductionMessageResource;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.ModlingCodesApplication.Dto;


//下发信号的逻辑要放到这里
namespace ArgesDataCollectionWithWpf.UI.UIWindows.CustomerUserControl
{
    /// <summary>
    /// ShowOrdersAndRunStatusUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class ShowOrdersAndRunStatusUserControl : UserControl,ITransientDependency
    {


        CancellationTokenSource cancelToken;

        private readonly SendOrderMessageToPlcSingleton _sendOrderMessageToPlcSingleton;

        private readonly CustomerQueneForCodesFromMes _customerQueneForCodesFromMes;
        private readonly IOrdersFromMesApplication _ordersFromMesApplication;
        OrderModlingMachineRunnedCountDto _orderModlingMachineRunnedCountDto = new OrderModlingMachineRunnedCountDto();
        private readonly ModlingMachineTypeAndPullRodSingletonCombineRoules _modlingMachineTypeAndPullRodSingletonCombineRoules;

        private readonly ProductionMessageSingleton _productionMessageSingleton ;

        public ShowOrdersAndRunStatusUserControl(IOrdersFromMesApplication ordersFromMesApplication
            , CustomerQueneForCodesFromMes customerQueneForCodesFromMes
            , IConnect_Device_With_PC_Function_Data_Application iConnectAddressData
            , ModlingMachineTypeAndPullRodSingletonCombineRoules modlingMachineTypeAndPullRodSingletonCombineRoules
            , SendOrderMessageToPlcSingleton sendOrderMessageToPlcSingleton
            , ProductionMessageSingleton productionMessageSingleton)
        {
            InitializeComponent();
            this._ordersFromMesApplication = ordersFromMesApplication;
            this._customerQueneForCodesFromMes = customerQueneForCodesFromMes;
            this._modlingMachineTypeAndPullRodSingletonCombineRoules = modlingMachineTypeAndPullRodSingletonCombineRoules;
            this._sendOrderMessageToPlcSingleton = sendOrderMessageToPlcSingleton;
            this._productionMessageSingleton = productionMessageSingleton;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.grid_ShowRunnedStatus.DataContext = _orderModlingMachineRunnedCountDto;

            DateTime start = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")); ;
            DateTime end = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59")); ;
            var todayOrders = this._ordersFromMesApplication.QuerryAllOrdersFromMesByDate(start, end);

            this._orderModlingMachineRunnedCountDto.OrderModlingMachineRunnedCount.Clear();
            foreach (var item in todayOrders)
            {
                this._orderModlingMachineRunnedCountDto.OrderModlingMachineRunnedCount.Add(new QuerryOrdersFromMesOutputNotify {


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

                //初始化的时候发第一个订单
                //SendOneOrder(runeddIndex);

            });


            Task.Run(() => {

                while (cancelToken.IsCancellationRequested !=true)
                {
                    QuerryModlingCodesOutputWithEndTime data = null; ;

                    var resu = this._customerQueneForCodesFromMes.MainRunedQuene.TryReceive(out data);


                    if (resu != true)
                    {
                        Thread.Sleep(100);
                        continue; ;
                    }

                    
                    if (runeddIndex >= this._orderModlingMachineRunnedCountDto.OrderModlingMachineRunnedCount.Count)
                    {
                        MessageBox.Show("扫描已经超过了今日订单的数量，请确认");
                        continue;
                    }



                    //要增加数量，并且对数量进行判断，如果超过了本订单的数量，则要把index+1
                    int produceQuantity = this._orderModlingMachineRunnedCountDto.OrderModlingMachineRunnedCount[runeddIndex].ProduceQuantity;
                    this._orderModlingMachineRunnedCountDto.OrderModlingMachineRunnedCount[runeddIndex].RunnedCount++;
                    int runnedCount = this._orderModlingMachineRunnedCountDto.OrderModlingMachineRunnedCount[runeddIndex].RunnedCount;
                    

                    
                    //颜色变化，并且数据更新
                    DataRowView drv = this.grid_ShowRunnedStatus.Items[runeddIndex] as DataRowView;
                    DataGridRow row = (DataGridRow)this.grid_ShowRunnedStatus.ItemContainerGenerator.ContainerFromIndex(runeddIndex);


                    //把数据保存到数据库，更新这一条数据
                    var m = this._orderModlingMachineRunnedCountDto.OrderModlingMachineRunnedCount[runeddIndex];
                    var add =  new AddOrUpdateOrdersFromMesInput
                               {

                                   ID = m.ID,
                                   MoldingMachineName = m.MoldingMachineName,
                                   MoldingMachineSerialName = m.MoldingMachineSerialName,
                                   OrderID = m.OrderID,
                                   ProduceDate = m.ProduceDate,
                                   ProduceQuantity = m.ProduceQuantity,
                                   RunnedCount = m.RunnedCount,
                                   ScanedCount = m.ScanedCount,
                                   StackNumber = m.StackNumber,
                                   WorkOrderID = m.WorkOrderID,
                                   ProduceQueneNumber = m.ProduceQueneNumber,
                                   IsJump = m.IsJump

                               };
                    List< AddOrUpdateOrdersFromMesInput > update = new List<AddOrUpdateOrdersFromMesInput>();
                    update.Add(add);
                    this._ordersFromMesApplication.InsertOrUpdateOrdersFromMes(update); 




                    
                    //显示颜色
                    if (runnedCount < produceQuantity)
                    {
                        this.Dispatcher.Invoke(new Action(() => { row.Background = new SolidColorBrush(Colors.Red); }));
                    }
                    else
                    {
                        this.Dispatcher.Invoke(new Action(() => { row.Background = new SolidColorBrush(Colors.Green); }));
                        
                        runeddIndex++;

                        

                        
                    }

                    this._sendOrderMessageToPlcSingleton.SendCtTime(data.Time,data.EndTime);

                    //this._productionMessageSingleton.AddDayAndMonthProduction();

                    /*
                    //下发plc的产量或者订单等信息
                    if (runeddIndex < this._orderModlingMachineRunnedCountDto.OrderModlingMachineRunnedCount.Count)
                    {
                        SendOneOrder(runeddIndex);
                    }
                    else
                    {
                        //SendMonthAndDayCount();
                    }

                    */





                }

            });
        }

        //第一次初始化的时候，进行订单的处理，查看颜色筛选
        private void ForeachRunCountModifyColor()
        {
            int count = this._orderModlingMachineRunnedCountDto.OrderModlingMachineRunnedCount.Count;
            for (runeddIndex = 0; runeddIndex < count; runeddIndex++)
            {
                DataRowView drv = this.grid_ShowRunnedStatus.Items[runeddIndex] as DataRowView;
                DataGridRow row = (DataGridRow)this.grid_ShowRunnedStatus.ItemContainerGenerator.ContainerFromIndex(runeddIndex);
                int produceQuantity = this._orderModlingMachineRunnedCountDto.OrderModlingMachineRunnedCount[runeddIndex].ProduceQuantity;

                int runnedCount = this._orderModlingMachineRunnedCountDto.OrderModlingMachineRunnedCount[runeddIndex].RunnedCount;

                int  isjump = this._orderModlingMachineRunnedCountDto.OrderModlingMachineRunnedCount[runeddIndex].IsJump;
                if (isjump >0)
                {
                    //跳单的话就继续
                    this.Dispatcher.Invoke(new Action(() => { row.Background = new SolidColorBrush(Colors.Yellow); }));
                    continue;
                }
                else if (runnedCount < produceQuantity && runnedCount != 0)
                {
                    this.Dispatcher.Invoke(new Action(() => { row.Background = new SolidColorBrush(Colors.Red); }));
                    return;
                }
                else if (runnedCount == produceQuantity)
                {
                    this.Dispatcher.Invoke(new Action(() => { row.Background = new SolidColorBrush(Colors.Green); }));
                }
                else if (runnedCount == 0)
                {
                    return;
                }


            }
        }


        private void SendOneOrder(int index)
        {
            
            //获第一条的数量和型号，然后下发，
            string name = this._orderModlingMachineRunnedCountDto.OrderModlingMachineRunnedCount[index].MoldingMachineName;
            int sendIndex = this._modlingMachineTypeAndPullRodSingletonCombineRoules.ModlingMachineTypeAndPullRodCombines[name].ModlingMachineTypeSendToPlcID;


            int quality = this._orderModlingMachineRunnedCountDto.OrderModlingMachineRunnedCount[index].ProduceQuantity;
            int runnned = this._orderModlingMachineRunnedCountDto.OrderModlingMachineRunnedCount[index].RunnedCount;
            int pollrodID = this._orderModlingMachineRunnedCountDto.OrderModlingMachineRunnedCount[index].StackNumber;

            this._sendOrderMessageToPlcSingleton.SendModlingPollRodQualityLoadMaterialArea(sendIndex, quality- runnned, pollrodID
                ,this._productionMessageSingleton.DayProduction.DayCount
                ,this._productionMessageSingleton.MonthProduction.MonthCount);
        }




        private void SendMonthAndDayCount()
        {
            this._sendOrderMessageToPlcSingleton.SendMonthDayProduction(this._productionMessageSingleton.MonthProduction.MonthCount
                , this._productionMessageSingleton.DayProduction.DayCount);
        }



        public void Close()
        {
            cancelToken.Cancel();
        }

    }



    public class OrderModlingMachineRunnedCountDto : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, e);
            }

        }

        private ObservableCollection<QuerryOrdersFromMesOutputNotify> orderModlingMachineRunnedCount = new ObservableCollection<QuerryOrdersFromMesOutputNotify>();
        public ObservableCollection<QuerryOrdersFromMesOutputNotify> OrderModlingMachineRunnedCount
        {
            get { return orderModlingMachineRunnedCount; }
            set
            {
                orderModlingMachineRunnedCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OrderModlingScanCodeMachine"));
            }
        }

    }
}
