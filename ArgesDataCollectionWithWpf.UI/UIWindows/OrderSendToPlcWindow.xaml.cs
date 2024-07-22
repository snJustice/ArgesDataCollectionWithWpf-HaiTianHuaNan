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

        OrderModlingMachineDto orderModlingMachineDto = new OrderModlingMachineDto();

        public OrderSendToPlcWindow()
        {
            InitializeComponent();
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
