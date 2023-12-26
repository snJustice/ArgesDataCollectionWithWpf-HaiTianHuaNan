using ArgesDataCollectionWithWpf.DbModels.Enums;
using S7.Net;
using System;
using System.Collections.Generic;
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

namespace ArgesDataCollectionWithWpf.UI.UIWindows.CommunicationDetails
{
    /// <summary>
    /// CommunicationAddTypesWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CommunicationAddTypesWindow : Window
    {

        public ConnectType CommunicatinUserSelectType;

        public CommunicationAddTypesWindow()
        {
            InitializeComponent();
        }

        private void On_Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.combox_CommunicationType.ItemsSource =  System.Enum.GetNames(typeof(ConnectType));
            
        }

        private void On_btn_OK_Click(object sender, RoutedEventArgs e)
        {
            if (this.combox_CommunicationType.SelectedItem == null)
            {
                MessageBox.Show("请选择一个通讯类型！");
                return;
            }

            if (!String.IsNullOrWhiteSpace(this.combox_CommunicationType.SelectedItem.ToString()))
            {

                CommunicatinUserSelectType = (ConnectType)Enum.Parse(typeof(ConnectType), this.combox_CommunicationType.SelectedItem.ToString(), false);
                this.DialogResult = true;
            }

            
        }
    }
}
