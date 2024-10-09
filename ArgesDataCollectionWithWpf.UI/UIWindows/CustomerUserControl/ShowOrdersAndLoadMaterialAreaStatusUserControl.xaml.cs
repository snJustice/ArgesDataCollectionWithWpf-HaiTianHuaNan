using Abp.Dependency;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.ModlingCodesApplication.Dto;
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
using System.Data;
using System.Globalization;
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


        private readonly ILogger _logger;
        private readonly LogUserControl _controlLog;

        private int runeddIndex21 = 0;

        public ShowOrdersAndLoadMaterialAreaStatusUserControl(IOrdersFromMesApplication ordersFromMesApplication
            , SendOrderMessageToPlcSingleton sendOrderMessageToPlcSingleton
            , ModlingMachineTypeAndPullRodSingletonCombineRoules modlingMachineTypeAndPullRodSingletonCombineRoules
            , CustomerQueneForCodesFromMes customerQueneForCodesFromMes
            , ILogger logger, LogUserControl controlLog)
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
                    IsLoadMaterialAreaSendOrder = item.IsLoadMaterialAreaSendOrder,
                    IsDownMaterialAreaSendOrder = item.IsDownMaterialAreaSendOrder

                });
            }


            //Init();
        }


        

        private void WriteMessageLogAndControl(string message)
        {
            this._logger.LogInformation(message);
            this._controlLog.WriteLog( $"{DateTime.Now.ToString("yyyy-MM-dd,HH-mm-ss,ff")}-{message}" );
        }
        public void Init()
        {

            runeddIndex21 = 0;
            
            cancelToken = new CancellationTokenSource();
            
           

            //上料处线程
            Task.Run(() => {

                Thread.Sleep(1000);
                ForeachRunCountModifyColor();
                //Thread.Sleep(500);



                while (cancelToken.IsCancellationRequested != true)
                {
                    //Thread.Sleep(100);
                    //如果收到了上料区完成信号的话，
                    LoadMaterialAreaAndDownMaterialDto data = null; ;

                    
                    //到达最后一个就不去改变颜色了,并且保持着红色,让人去看
                    if (runeddIndex21 >= this._orderModlingMachineLoadMaterialAreaDto.OrderModlingMachineLoadMaterialArea.Count)
                    {


                        //DataGridRow row = (DataGridRow)this.grid_ShowLoadMaterialAreaStatus.ItemContainerGenerator.ContainerFromIndex(runeddIndex);
                        //this.Dispatcher.Invoke(new Action(() => { row.Background = new SolidColorBrush(Colors.Green); }));

                        //WriteMessageLogAndControl("上料区今日订单已完成，不下发");
                        continue;
                    }

                    
                    var resu = this._customerQueneForCodesFromMes.LoadMaterialQuene.TryReceive(out data);
                    
                    if (resu != true)
                    {
                        Thread.Sleep(100);
                        continue; ;
                    }
                    
                    else 
                    {
                        
                        if (runeddIndex21 >= 0)
                        {
                            //显示颜色,把上一个订单颜色变成绿色，
                            //DataRowView drv = this.grid_ShowLoadMaterialAreaStatus.Items[runeddIndex-1] as DataRowView;
                            DataGridRow row = (DataGridRow)this.grid_ShowLoadMaterialAreaStatus.ItemContainerGenerator.ContainerFromIndex(runeddIndex21);
                            this.Dispatcher.Invoke(() => {

                                string xxx = row.Background.ToString();
                                if (row.Background.ToString() != "#FFFFFF00")
                                {
                                    //row.Background = new SolidColorBrush(Colors.Green);
                                }


                            });
                            
                            
                            
                        }
                        
                        runeddIndex21++;
                        

                        //下发plc的产量或者订单等信息
                        if (runeddIndex21 < this._orderModlingMachineLoadMaterialAreaDto.OrderModlingMachineLoadMaterialArea.Count)
                        {
                            //颜色变化，并且数据更新
                            //下一个订单颜色变成红色
                            //DataGridRow row2 = (DataGridRow)this.grid_ShowLoadMaterialAreaStatus.ItemContainerGenerator.ContainerFromIndex(runeddIndex21);
                            //this.Dispatcher.Invoke(new Action(() => { row2.Background = new SolidColorBrush(Colors.Red); }));
                            
                            //这个时候保存数据到数据库
                            var m = this._orderModlingMachineLoadMaterialAreaDto.OrderModlingMachineLoadMaterialArea[runeddIndex21];
                            this._orderModlingMachineLoadMaterialAreaDto.OrderModlingMachineLoadMaterialArea[runeddIndex21].IsLoadMaterialAreaSendOrder = 1;
                            var add = new AddOrUpdateOrdersFromMesInput
                            {

                                ID = m.ID,
                            
                                IsLoadMaterialAreaSendOrder= 1,

                            };
                            List<AddOrUpdateOrdersFromMesInput> update = new List<AddOrUpdateOrdersFromMesInput>();
                            update.Add(add);
                            this._ordersFromMesApplication.InsertOrUpdateOrdersFromMesLoadAreaSendOKCount(update);
                            

                            SendOneOrderLoadArea(runeddIndex21);

                            WriteMessageLogAndControl("上料区已下发一个订单");
                            


                        }
                        else
                        {
                            WriteMessageLogAndControl("上料区今日订单已完成，不下发");
                        }
                        
                    }

                }




            });


            
        }


        //第一次初始化的时候，进行订单的处理，查看颜色筛选
        

        private void ForeachRunCountModifyColor()
        {
            int count = this._orderModlingMachineLoadMaterialAreaDto.OrderModlingMachineLoadMaterialArea.Count;
            for (runeddIndex21 = 0; runeddIndex21 < count; runeddIndex21++)
            {
                //DataRowView drv = this.grid_ShowLoadMaterialAreaStatus.Items[runeddIndex] as DataRowView;
                //DataGridRow row = (DataGridRow)this.grid_ShowLoadMaterialAreaStatus.ItemContainerGenerator.ContainerFromIndex(runeddIndex21);
                int isLoadMaterialAreaSendOrder = this._orderModlingMachineLoadMaterialAreaDto.OrderModlingMachineLoadMaterialArea[runeddIndex21].IsLoadMaterialAreaSendOrder;

                

                int isjump = this._orderModlingMachineLoadMaterialAreaDto.OrderModlingMachineLoadMaterialArea[runeddIndex21].IsJump;


                if (isjump > 0)
                {
                    //跳单的话就继续

                    //this.Dispatcher.Invoke(new Action(() => { row.Background = new SolidColorBrush(Colors.Black); }));
                    continue;
                }
                else if (isLoadMaterialAreaSendOrder == 0)
                {

                    runeddIndex21--;
                    
                    return;
                }
                else if (isLoadMaterialAreaSendOrder>0)
                {
                    
                    //this.Dispatcher.Invoke(new Action(() => { row.Background = new SolidColorBrush(Colors.Green); }));
                    //this._customerQueneForCodesFromMes.LoadMaterialQuene.Post(new LoadMaterialAreaAndDownMaterialDto { LoadOrDownArea = LoadOrDwonEnum.LoadMaterialArea });


                    continue;
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

            
            
        }

        

        public void Close()
        {
            cancelToken.Cancel();
            //this.Close();
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



    public class ShowOrderSendMultiColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush background = Brushes.Black;
            if (values != null && values.Length == 2)
            {
                var isordersend_result = int.TryParse(values[0].ToString(), out int issender);
                
                var jump_result = int.TryParse(values[1].ToString(), out int jump);
                if (isordersend_result  && jump_result)
                {

                    if (jump == 1)
                    {
                        return background = Brushes.Black;
                    }
                    if (issender == 0)
                    {
                        return background = Brushes.LightBlue;
                    }
                    else  
                    {
                        return background = Brushes.Green;
                    }

                }

            }
            return background = Brushes.LightBlue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
