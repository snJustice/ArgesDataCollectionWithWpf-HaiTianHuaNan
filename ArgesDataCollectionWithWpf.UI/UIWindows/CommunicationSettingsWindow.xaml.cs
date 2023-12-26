using Abp.Dependency;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.CommunicationDetailsAndInstanceApplication;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.CommunicationDetailsAndInstanceApplication.Dto;
using ArgesDataCollectionWithWpf.DbModels.Enums;
using ArgesDataCollectionWithWpf.UI.UIWindows.CommunicationDetails;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using static Abp.Domain.Uow.AbpDataFilters;
using ArgesDataCollectionWithWpf.Application.Utils;
using SqlSugar;

namespace ArgesDataCollectionWithWpf.UI.UIWindows
{
    /// <summary>
    /// CommunicationSettingsWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CommunicationSettingsWindow : Window, ITransientDependency
    {

        Label selectedLabel;
        //先去数据库中，把数据都拿出来，方便临时的操作
        
        private readonly ICommunicationDetailsAndInstanceApplication _iCommunicationDetailsAndInstanceApplication;//数据库操作

        
        

        public CommunicationSettingsWindow(ICommunicationDetailsAndInstanceApplication iCommunicationDetailsAndInstanceApplication)
        {
            InitializeComponent();
            this._iCommunicationDetailsAndInstanceApplication = iCommunicationDetailsAndInstanceApplication;
            //
        }


        
        

        private void btn_MenuCommunication_Click(object sender, RoutedEventArgs e)
        {
            CommunicationAddTypesWindow communicationAddTypesWindow = new CommunicationAddTypesWindow();
            communicationAddTypesWindow.WindowStartupLocation= WindowStartupLocation.CenterScreen;
            
            if (communicationAddTypesWindow.ShowDialog() == true)
            {
                var userSelectedCommunication = communicationAddTypesWindow.CommunicatinUserSelectType;
                //获得一个新的增加的通讯类
                var newCommunication = GetOneNewCommunication(userSelectedCommunication);
                this._iCommunicationDetailsAndInstanceApplication.InsertCommunicationDetailsAndInstance(
                    new AddCommunicationDetailsAndInstanceInput { 
                    
                        ID = newCommunication.ID,
                        ConnectType= newCommunication.ConnectType,
                        SerialResult= newCommunication.SerialResult,
                        UniqueCode= newCommunication.UniqueCode,
                    }
                    );
                //在界面上增加一个通讯
                AddCommunicationByLabelInStackPanel(newCommunication);

            }
            




            
        }

        //获得新的通讯dto实例
        private QuerryCommunicationDetailsAndInstanceOutput GetOneNewCommunication(ConnectType connectType)
        {

            int newIndex = this._iCommunicationDetailsAndInstanceApplication.QuerryCommunicationDetailsAndInstanceAll().GetUniqueIndex("ID");
            //int newIndex = GetNewIndex(this._iCommunicationDetailsAndInstanceApplication.QuerryCommunicationDetailsAndInstanceAll());

            string key = connectType.ToString() + "-" + newIndex;

            return new QuerryCommunicationDetailsAndInstanceOutput {
                ID=newIndex,
                ConnectType = connectType,
                UniqueCode = key,
            
            };


        }

        

        //把通讯用label显示在stackpanel中
        private void AddCommunicationByLabelInStackPanel(QuerryCommunicationDetailsAndInstanceOutput querryCommunicationDetailsAndInstanceOutput)
        {
            
            Label label = new Label();
            label.MouseRightButtonDown += Label_MouseRightButtonDown;
            label.MouseLeftButtonDown += Label_MouseLeftButtonDown;
            label.Content = querryCommunicationDetailsAndInstanceOutput.UniqueCode;
            

            this.stackPanel_CommunicationInstances.Children.Add(label);
        }

        //右键按下，显示相应的设置
        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //获得相应的通讯unicode
            //根据唯一码，获得通讯的信息，
            Label targetLabel = (Label)sender;

            //这个时候要判断这个通讯是不是在数据库中，如果不在数据库中，
            QuerryCommunicationDetailsAndInstanceOutput selectedCommunication = this._iCommunicationDetailsAndInstanceApplication.QuerryCommunicationDetailsAndInstanceByUnicode(targetLabel.Content.ToString()).First();

            

            ActiveLabel(targetLabel, Color.FromRgb(209, 127, 31));

            switch (selectedCommunication.ConnectType)
            {
                case  ConnectType.Simens:
                    
                    this.frame_Communication_Child_Setting.Navigate(new S7CommunicationUserControl(selectedCommunication, this._iCommunicationDetailsAndInstanceApplication) );
                    break;
                case  ConnectType.ModbusTCP:
                    this.frame_Communication_Child_Setting.Navigate(new Uri("UIWindows\\CommunicationDetails\\ModbusCommunicationUserControl.xaml", UriKind.Relative));
                    break;
                default:
                    break;


            }


        }

        private void ActiveLabel(Label targetLabel, Color color)
        {
            targetLabel.Background = new SolidColorBrush(color);
            if (selectedLabel!=null)
            {
                selectedLabel.Background = new SolidColorBrush(Color.FromRgb(221,221,221));
            }
            selectedLabel = targetLabel;
        }


        //label 鼠标右键的按钮
        private void Label_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ContextMenu cm = this.FindResource("cmLabel") as ContextMenu;
            
            cm.PlacementTarget = sender as Label;
            cm.IsOpen = true;
        }

        

        

        //右键 删除的 按钮
        private void cmDelete_Click(object sender, RoutedEventArgs e)
        {
            if (sender!=null)
            {
                MenuItem context = (MenuItem)e.Source;
                ContextMenu item = (ContextMenu)context.Parent;
                Label ll =(Label)item.PlacementTarget ;


                this.stackPanel_CommunicationInstances.Children.Remove(ll);
                this._iCommunicationDetailsAndInstanceApplication.DeleteCommunicationDetailsAndInstanceById(
                    new DeleteCommunicationDetailsAndInstanceByIdInput { ID = Convert.ToInt32(ll.Content.ToString().Split('-')[1], CultureInfo.InvariantCulture) }
                   );
            }
           
        }

        private void On_Window_Loaded(object sender, RoutedEventArgs e)
        {
            var currentInDatabase = this._iCommunicationDetailsAndInstanceApplication.QuerryCommunicationDetailsAndInstanceAll();

            foreach (var item in currentInDatabase)
            {
                AddCommunicationByLabelInStackPanel(item);
            }
        
        }

        
    }
}
