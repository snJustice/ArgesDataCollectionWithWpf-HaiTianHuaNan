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

using System.Threading.Tasks.Dataflow;
using System.Data;
using System.Threading;
using ArgesDataCollectionWpf.DataProcedure.Utils.Quene;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.OrdersFromMesApplication.Dto;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.ModlingCodesApplication.Dto;
using Microsoft.Extensions.Configuration;

namespace ArgesDataCollectionWithWpf.UI.UIWindows.CustomerUserControl
{
    /// <summary>
    /// ShowOrdersAndScanCodeAndRunningCountUserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class ShowOrdersAndScanCodeUserControl : UserControl,ITransientDependency
    {

        CancellationTokenSource cancelToken;
        private readonly IOrdersFromMesApplication _ordersFromMesApplication;
        private readonly CustomerQueneForCodesFromMes _customerQueneForCodesFromMes;
        private readonly IConfiguration _configuration;
        OrderModlingMachineScanCodeDto  _orderModlingMachineScanCodeDto = new OrderModlingMachineScanCodeDto();

        private readonly string _stationNumberOne;
        private readonly string _stationNumberTwo;
        public ShowOrdersAndScanCodeUserControl(IOrdersFromMesApplication ordersFromMesApplication
            , CustomerQueneForCodesFromMes customerQueneForCodesFromMes , IConfiguration configuration)
        {
            InitializeComponent();
            this._ordersFromMesApplication = ordersFromMesApplication;
            this._customerQueneForCodesFromMes = customerQueneForCodesFromMes;

            this._configuration = configuration;


            if (this._configuration["StationNumberOne"] != null && !string.IsNullOrWhiteSpace(this._configuration["StationNumberOne"]))
            {
                this._stationNumberOne = this._configuration["StationNumberOne"];
            }

            if (this._configuration["StationNumberTwo"] != null && !string.IsNullOrWhiteSpace(this._configuration["StationNumberTwo"]))
            {
                this._stationNumberTwo = this._configuration["StationNumberTwo"];
            }
        }

        
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            this.grid_ShowScanedStatus.DataContext = _orderModlingMachineScanCodeDto;

            DateTime start = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")); ;
            DateTime end = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59")); ;
            var todayOrders = this._ordersFromMesApplication.QuerryAllOrdersFromMesByDate(start, end);

            this._orderModlingMachineScanCodeDto.OrderModlingScanCodeMachine.Clear();
            foreach (var item in todayOrders)
            {
                this._orderModlingMachineScanCodeDto.OrderModlingScanCodeMachine.Add(new QuerryOrdersFromMesOutputNotify { 
                
                
                    ID = item.ID,
                    MoldingMachineName = item.MoldingMachineName,   
                    MoldingMachineSerialName= item.MoldingMachineSerialName,
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

        int runeddIndex = 0;
        private void Init()
        {
            cancelToken = new CancellationTokenSource();
            this._customerQueneForCodesFromMes.Init();


            Task.Run(async () => { 
            
                await Task.Delay(300);
                ForeachScanCountModifyColor();

            });
            


            //主扫描队列增加的线程
            Task.Run(() => {
                Thread.Sleep(1000);
                while (cancelToken.IsCancellationRequested!=true)
                {
                    
                    QuerryModlingCodesOutput data = null; ;

                    var resu = this._customerQueneForCodesFromMes.MainScanQuene.TryReceive(out data);
                    if (resu != true)
                    {
                        Thread.Sleep(100);
                        continue; ;
                    }


                    
                    if (runeddIndex >= this._orderModlingMachineScanCodeDto.OrderModlingScanCodeMachine.Count)
                    {
                        //MessageBox.Show("生成已经超过了今日订单的数量，请确认");
                        continue;
                    }



                    //要增加数量，并且对数量进行判断，如果超过了本订单的数量，则要把index+1
                    int produceQuantity = this._orderModlingMachineScanCodeDto.OrderModlingScanCodeMachine[runeddIndex].ProduceQuantity;
                    this._orderModlingMachineScanCodeDto.OrderModlingScanCodeMachine[runeddIndex].ScanedCount++;
                    int scanedCount  = this._orderModlingMachineScanCodeDto.OrderModlingScanCodeMachine[runeddIndex].ScanedCount;
                    //this.Dispatcher.Invoke(new Action(() => { this.grid_ShowScanedStatus.DataContext = null; this.grid_ShowScanedStatus.DataContext = _orderModlingMachineScanCodeDto; }));

                    Thread.Sleep(100);
                    //颜色变化，并且数据更新
                    DataRowView drv = this.grid_ShowScanedStatus.Items[runeddIndex] as DataRowView;
                    DataGridRow row = (DataGridRow)this.grid_ShowScanedStatus.ItemContainerGenerator.ContainerFromIndex(runeddIndex);


                    //BindingExpression be = this.datagritext_scaned.GetBindingExpression(TextBox.TextProperty);
                    //be.UpdateSource();

                    //数据保存到数据库
                    //把数据保存到数据库，更新这一条数据,这里，会和下料区重复记录，重复记录有问题。
                    var m = this._orderModlingMachineScanCodeDto.OrderModlingScanCodeMachine[runeddIndex];
                    var add = new AddOrUpdateOrdersFromMesInput
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
                        IsJump = m.IsJump,

                    };
                    List<AddOrUpdateOrdersFromMesInput> update = new List<AddOrUpdateOrdersFromMesInput>();
                    update.Add(add);
                    this._ordersFromMesApplication.InsertOrUpdateOrdersFromMesScanCount(update);


                    //判断颜色
                    if (scanedCount< produceQuantity)
                    {
                        this.Dispatcher.Invoke(new Action(() => { row.Background = new SolidColorBrush(Colors.Red); }));
                    }
                    else
                    {
                        this.Dispatcher.Invoke(new Action(() => { row.Background = new SolidColorBrush(Colors.Green); }));
                        runeddIndex++;
                    }

                    



                    /*不能用队列了。
                    if (data.StationNumber== this._stationNumberOne)
                    {
                        this._customerQueneForCodesFromMes.StationOneScanQuene.Post(data);
                    }
                    if (data.StationNumber == this._stationNumberTwo)
                    {
                        this._customerQueneForCodesFromMes.StationTwoScanQuene.Post(data);
                    }
                    */
                    
                }

            });


            //删除的队列线程
            Task.Run(() => {

                Thread.Sleep(1000);
                while (cancelToken.IsCancellationRequested != true)
                {
                    QuerryModlingCodesOutput data = null; ;

                    var resu = this._customerQueneForCodesFromMes.MainScanDeleteQuene.TryReceive(out data);
                    if (resu != true)
                    {
                        Thread.Sleep(100);
                        continue; ;
                    }

                    //颜色要么先不管，先把index 和数量 判断好，
                    int scanedCount = this._orderModlingMachineScanCodeDto.OrderModlingScanCodeMachine[runeddIndex].ScanedCount;
                    int produceQuantity = this._orderModlingMachineScanCodeDto.OrderModlingScanCodeMachine[runeddIndex].ProduceQuantity;

                    //正常来说，所在位置直接-1就可以了
                    if (scanedCount<=0)
                    {
                        //不做什么事情
                        continue;
                    }
                    this._orderModlingMachineScanCodeDto.OrderModlingScanCodeMachine[runeddIndex].ScanedCount--;

                    
                }

                //

            });
        }




        private void ForeachScanCountModifyColor()
        {
            int count = this._orderModlingMachineScanCodeDto.OrderModlingScanCodeMachine.Count;
            for (runeddIndex = 0; runeddIndex < count; runeddIndex++)
            {
                DataRowView drv = this.grid_ShowScanedStatus.Items[runeddIndex] as DataRowView;
                DataGridRow row = (DataGridRow)this.grid_ShowScanedStatus.ItemContainerGenerator.ContainerFromIndex(runeddIndex);
                int produceQuantity = this._orderModlingMachineScanCodeDto.OrderModlingScanCodeMachine[runeddIndex].ProduceQuantity;
                
                int scanedCount = this._orderModlingMachineScanCodeDto.OrderModlingScanCodeMachine[runeddIndex].ScanedCount;

                int isjump = this._orderModlingMachineScanCodeDto.OrderModlingScanCodeMachine[runeddIndex].IsJump;
                if (isjump > 0)
                {
                    //跳单的话就继续
                    this.Dispatcher.Invoke(new Action(() => { row.Background = new SolidColorBrush(Colors.Yellow); }));
                    continue;
                }
                else if (scanedCount< produceQuantity && scanedCount!=0)
                {
                    this.Dispatcher.Invoke(new Action(() => { row.Background = new SolidColorBrush(Colors.Red); }));
                    return;
                }
                else if (scanedCount >= produceQuantity )
                {
                    this.Dispatcher.Invoke(new Action(() => { row.Background = new SolidColorBrush(Colors.Green); }));
                }
                else if (scanedCount == 0)
                {
                    return;
                }
                

            }
        }


        public void Close()
        {
            cancelToken.Cancel();
            
        }
    }


    public class OrderModlingMachineScanCodeDto : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, e);
            }

        }

        private ObservableCollection<QuerryOrdersFromMesOutputNotify> orderModlingScanCodeMachine = new ObservableCollection<QuerryOrdersFromMesOutputNotify>();
        public ObservableCollection<QuerryOrdersFromMesOutputNotify> OrderModlingScanCodeMachine
        {
            get { return orderModlingScanCodeMachine; }
            set
            {
                orderModlingScanCodeMachine = value;
                
                OnPropertyChanged(new PropertyChangedEventArgs("OrderModlingScanCodeMachine"));
            }
        }
        

    }


    public class QuerryOrdersFromMesOutputNotify: INotifyPropertyChanged
    {
        public int ID { get; set; }


        public string OrderID { get; set; }
        public string WorkOrderID { get; set; }



        public string MoldingMachineSerialName { set; get; }
        public string MoldingMachineName { set; get; }

        public int ProduceQuantity { set; get; }
        public DateTime ProduceDate { set; get; }

        //下发的拉杆的型号
        public int StackNumber { set; get; }

        //此订单的完成状态
        public int Status { set; get; }

        //已经运行了几个产品，为了特殊情况，断电停线，能够继续工作
        private int runnedCount;
        public int RunnedCount
        {
            set { runnedCount = value; OnPropertyChanged(new PropertyChangedEventArgs("RunnedCount")); }
            get { return runnedCount; }
        }

        private int scanedCount;
        public int ScanedCount { set { scanedCount = value;OnPropertyChanged(new PropertyChangedEventArgs("ScanedCount")); } get { return scanedCount; } }

        public List<int> Stacks { set; get; }

        public List<string> MoldingTypes { set; get; }

        public int ProduceQueneNumber { set; get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, e);
            }

        }


        private int isJump;
        public int IsJump { get { return isJump; } set { isJump = value; OnPropertyChanged(new PropertyChangedEventArgs("IsJump")); } }


    }
}
