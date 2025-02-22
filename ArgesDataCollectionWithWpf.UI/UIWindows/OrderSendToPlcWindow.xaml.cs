﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Abp.Dependency;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.OrdersFromMesApplication;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.OrdersFromMesApplication.Dto;
using ArgesDataCollectionWithWpf.UI.SingletonResource.ModlingMachineDeviceResource;
using ArgesDataCollectionWithWpf.UI.UIWindows.CustomerUserControl;
using ArgesDataCollectionWithWpf.UI.Utils.UiDataUtils;
using Microsoft.Extensions.Logging;


namespace ArgesDataCollectionWithWpf.UI.UIWindows
{
    /// <summary>
    /// OrderSendToPlcWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OrderSendToPlcWindow : Window,ITransientDependency
    {
        private readonly IOrdersFromMesApplication _ordersFromMesApplication;
        OrderModlingMachineDto _orderModlingMachineDto = new OrderModlingMachineDto();

        private readonly ModlingMachineTypeAndPullRodSingletonCombineRoules _modlingMachineTypeAndPullRodSingletonCombineRoules;

        private ILogger _logger;

        public OrderSendToPlcWindow(IOrdersFromMesApplication ordersFromMesApplication
            , ModlingMachineTypeAndPullRodSingletonCombineRoules modlingMachineTypeAndPullRodSingletonCombineRoules, ILogger logger)
        {
            InitializeComponent();
            this._ordersFromMesApplication = ordersFromMesApplication;
            this._modlingMachineTypeAndPullRodSingletonCombineRoules = modlingMachineTypeAndPullRodSingletonCombineRoules;
            this._logger = logger;
        }

        private void btn_GetToDayOrder_Click(object sender, RoutedEventArgs e)
        {


            GetCurrentOrder();




        }


        private void GetCurrentOrder()
        {
            DateTime start = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")); ;
            DateTime end = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59")); ;
            var todayOrders = this._ordersFromMesApplication.QuerryAllOrdersFromMesByDate(start, end);
            var keys = this._modlingMachineTypeAndPullRodSingletonCombineRoules.ModlingMachineTypeAndPullRodCombines.Keys.ToList();

            //先确认一遍所有型号存在于配置表中

            var dda = (from m in todayOrders where !keys.Contains(m.MoldingMachineName) select m.MoldingMachineName).ToList();
            if (dda.Count > 0)
            {
                string message = "有不存在的机型，请先配置:" + string.Join(',', dda);
                MessageBox.Show(message);
                this._logger.LogInformation(message);
                Clipboard.SetText(message);
                return;
            }

            Parallel.For(0, todayOrders.Count, i =>
            {

                var pods = this._modlingMachineTypeAndPullRodSingletonCombineRoules.ModlingMachineTypeAndPullRodCombines[todayOrders[i].MoldingMachineName].PollRods;

                todayOrders[i].Stacks = (from m in pods select m.PollRodSendToPlcID).ToList();
                todayOrders[i].StackNumber = (from m in pods select m.PollRodSendToPlcID).ToList().First();
                todayOrders[i].MoldingTypes = keys;
            });



            this._orderModlingMachineDto.OrderModlingMachine.Clear();
            foreach (var item in todayOrders)
            {
                
                //this._orderModlingMachineDto.OrderModlingMachine.Add(item);
                this._orderModlingMachineDto.OrderModlingMachine.Add(new QuerryOrdersFromMesOutputNotify
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
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            this.grid_OrderSettingShow.DataContext = _orderModlingMachineDto;


            this._logger.LogInformation("进入工单下发设置 画面");

            GetCurrentOrder();

        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            

            
        }

        

        private void grid_OrderSettingShow_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Console.WriteLine();
        }

        private void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            //进行数据的保存

            if (MessageBox.Show("是否确认？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var add = (from m in this._orderModlingMachineDto.OrderModlingMachine
                           select new AddOrUpdateOrdersFromMesInput
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
                               IsLoadMaterialAreaSendOrder = m.IsLoadMaterialAreaSendOrder,
                               IsDownMaterialAreaSendOrder = m.IsDownMaterialAreaSendOrder

                           }).ToList();
                this._ordersFromMesApplication.InsertOrUpdateOrdersFromMes(add.ToList()); ;

                this.DialogResult = true;
                this._logger.LogInformation("订单确认");
                this.Close();
            }
            else
            {
                this.DialogResult = false;
            }

            

            

            
        }

        private void btn_AddOne_Click(object sender, RoutedEventArgs e)
        {
            this.grid_OrderSettingShow.SelectedIndex = -1;
            var modling = this._modlingMachineTypeAndPullRodSingletonCombineRoules.ModlingMachineTypeAndPullRodCombines.First();
            var keys = this._modlingMachineTypeAndPullRodSingletonCombineRoules.ModlingMachineTypeAndPullRodCombines.Keys.ToList();
            //获得当前最大的顺序号

            int max = this._orderModlingMachineDto.OrderModlingMachine.Count==0?1: this._orderModlingMachineDto.OrderModlingMachine.Max(a=>a.ProduceQueneNumber) +1;
            this._orderModlingMachineDto.OrderModlingMachine.Add(new QuerryOrdersFromMesOutputNotify
            {

                MoldingMachineName = modling.Value.ModlingMachineTypeName,
                MoldingMachineSerialName = modling.Value.ModlingMachineTypeName,
                OrderID = "666666666",
                ProduceDate = DateTime.Now,
                ProduceQuantity = 2,
                RunnedCount = 0,
                ScanedCount = 0,
                StackNumber = modling.Value.PollRods.First().PollRodSendToPlcID,
                WorkOrderID = "666666666",
                Stacks = (from m in modling.Value.PollRods select m.PollRodSendToPlcID).ToList(),
                MoldingTypes = keys,
                ProduceQueneNumber = max,
                IsJump = 0,
                IsDownMaterialAreaSendOrder=0,
                IsLoadMaterialAreaSendOrder = 0
            });
        }

        private void cellComboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ComboBox comboBox2 = (ComboBox)sender;
            string name = comboBox2.SelectedItem.ToString();
            
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }
            if (grid_OrderSettingShow.SelectedIndex<0)
            {
                return;

            }
            if (!this._modlingMachineTypeAndPullRodSingletonCombineRoules.ModlingMachineTypeAndPullRodCombines.ContainsKey(name))
            {
                MessageBox.Show($"不存在此型号:是否名称填写错误:{name}");
                return;
            }
            
            var stacks = (from m in this._modlingMachineTypeAndPullRodSingletonCombineRoules.ModlingMachineTypeAndPullRodCombines[name].PollRods select m.PollRodSendToPlcID).ToList();

            //this._orderModlingMachineDto.OrderModlingMachine[grid_OrderSettingShow.SelectedIndex].Stacks = stacks;
            this._orderModlingMachineDto.OrderModlingMachine[grid_OrderSettingShow.SelectedIndex].StackNumber = stacks.First();
            

            //this.grid_OrderSettingShow.DataContext = null;
            //this.grid_OrderSettingShow.DataContext = _orderModlingMachineDto;

        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            this._logger.LogInformation("退出了工单下发设置 画面");
        }

        private void btn_DeleteOne_Click(object sender, RoutedEventArgs e)
        {

            if (this.grid_OrderSettingShow.SelectedIndex<0)
            {
                return;
            }
            //删除这一行，并且把数据库里的内容也清除一下
            //询问确定删除
            if (MessageBox.Show("确认删除？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                int selectedIndex = this.grid_OrderSettingShow.SelectedIndex ;
                var deleteItem = this._orderModlingMachineDto.OrderModlingMachine[selectedIndex];
                this._ordersFromMesApplication.DeleteLineStationParameterByIndex(deleteItem.ID);
                this._orderModlingMachineDto.OrderModlingMachine.RemoveAt(selectedIndex);

            }

        }


        [DllImport("User32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        private void grid_OrderSettingShow_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            
            //keybd_event(0x0D, 0, 0, 0);//按下A

        }

        

        private void grid_OrderSettingShow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (this.grid_OrderSettingShow.SelectedIndex < 0)
                {
                    return;
                }

                int selectedIndex = this.grid_OrderSettingShow.SelectedIndex;
                var deleteItem = this._orderModlingMachineDto.OrderModlingMachine[selectedIndex];
                this._ordersFromMesApplication.DeleteLineStationParameterByIndex(deleteItem.ID);
                this._orderModlingMachineDto.OrderModlingMachine.RemoveAt(selectedIndex);
                
            }
        }

        private void button_IsJump_Click(object sender, RoutedEventArgs e)
        {
            this._orderModlingMachineDto.OrderModlingMachine[grid_OrderSettingShow.SelectedIndex].IsJump = this._orderModlingMachineDto.OrderModlingMachine[grid_OrderSettingShow.SelectedIndex].IsJump == 0 ? 1 : 0;
        }
    }



    public class OrderModlingMachineDto : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, e);
            }
                
        }

        private ObservableCollection<QuerryOrdersFromMesOutputNotify> orderModlingMachine = new ObservableCollection<QuerryOrdersFromMesOutputNotify>();
        public ObservableCollection<QuerryOrdersFromMesOutputNotify> OrderModlingMachine
        {
            get { return orderModlingMachine; }
            set
            {
                orderModlingMachine = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OrderModlingMachine"));
            }
        }

    }


    public class IsJumpToContentConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && int.TryParse(value.ToString(), out int isjump))
            {
                if (isjump ==0)
                {
                    return "正常";
                }
                else
                {
                    return "跳单";
                }
            }
            else
            {
                return "正常";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
