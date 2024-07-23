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
using System.Windows.Shapes;
using Abp.Dependency;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.OrdersFromMesApplication;

namespace ArgesDataCollectionWithWpf.UI.UIWindows
{
    /// <summary>
    /// OrderSendToPlcWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OrderSendToPlcWindow : Window,ITransientDependency
    {
        private readonly IOrdersFromMesApplication _ordersFromMesApplication;
        OrderModlingMachineDto orderModlingMachineDto = new OrderModlingMachineDto();

        public OrderSendToPlcWindow(IOrdersFromMesApplication ordersFromMesApplication)
        {
            InitializeComponent();
            this._ordersFromMesApplication = ordersFromMesApplication;
        }

        private void btn_GetToDayOrder_Click(object sender, RoutedEventArgs e)
        {
            DateTime start = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")); ;
            DateTime end = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59")); ;
            var todayOrders = this._ordersFromMesApplication.QuerryAllOrdersFromMesByDate(start, end);

            orderModlingMachineDto.OrderModlingMachine.Clear();
            foreach (var item in todayOrders)
            {
                orderModlingMachineDto.OrderModlingMachine.Add(item);
            }

            
            


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.grid_OrderSettingShow.DataContext = orderModlingMachineDto;
            
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            
            foreach (var item in this.orderModlingMachineDto.OrderModlingMachine)
            {
                Console.WriteLine(String.Join(",",item.Stacks));
            }
            
        }

        private void cellComboBox0_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //this.orderModlingMachineDto.OnPropertyChanged(new PropertyChangedEventArgs("StackNumber"));
        }
    }



    public class OrderModlingMachineDto : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private ObservableCollection<QuerryOrdersFromMesByDateOutput> orderModlingMachine = new ObservableCollection<QuerryOrdersFromMesByDateOutput>();
        public ObservableCollection<QuerryOrdersFromMesByDateOutput> OrderModlingMachine
        {
            get { return orderModlingMachine; }
            set
            {
                orderModlingMachine = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OrderModlingMachine"));
            }
        }

    }
}
