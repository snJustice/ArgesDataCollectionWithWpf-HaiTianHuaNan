
using Abp.Dependency;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.CommunicationDetailsAndInstanceApplication;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.LineStationParameterApplication;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.LineStationTableApplication;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.SaveDatasApplication;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.SaveDatasApplication.Dto;
using ArgesDataCollectionWithWpf.Communication;
using ArgesDataCollectionWithWpf.Core;
using ArgesDataCollectionWithWpf.UI.SingletonResource.ModlingMachineDeviceResource;
using ArgesDataCollectionWithWpf.UI.SingletonResource.SendOrderMessageResource;
using ArgesDataCollectionWithWpf.UI.UIWindows;
using ArgesDataCollectionWithWpf.UI.UIWindows.CustomerUserControl;
using ArgesDataCollectionWithWpf.UseFulThirdPartFunction.Excel;
using ArgesDataCollectionWpf.DataProcedure.DataFlow.Checkers;
using ArgesDataCollectionWpf.DataProcedure.Generate;
using ArgesDataCollectionWpf.DataProcedure.Utils.Quene;
using AutoMapper;
using EnterpriseFD.Dataflow;
using Microsoft.Extensions.Logging;
using Panuon.WPF.UI;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows;
using System.Windows.Controls;


namespace ArgesDataCollectionWithWpf.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WindowX, ISingletonDependency
    {

        private readonly DbContextConnection _dbContext;
        private readonly ICommunicationDetailsAndInstanceApplication _communicationDeviceApplication;
        private readonly IConnect_Device_With_PC_Function_Data_Application _iConnectAddressData;
        private readonly ILineStationParameterApplication _iLineStation;
        private readonly ILogger _logger;

        private readonly IMapper _imapper;

        private readonly CommunicationManagerDictionary _communicationManagerDictionary;
        private readonly ILineStationTableApplication _ilineStationTableApplication;

        private readonly ISaveDatasApplication  _saveDatasApplication;

        private readonly SendOrderMessageToPlcSingleton _sendOrderMessageToPlcSingleton;

        private readonly ModlingMachineTypeAndPullRodSingletonCombineRoules  _modlingMachineTypeAndPullRodSingletonCombineRoules;
        private readonly CustomerQueneForCodesFromMes _customerQueneForCodesFromMes;//队列






        private readonly Dictionary<string, IStarter> _LineStarters = new Dictionary<string, IStarter>();



        private TriggerChecker _loadCheck;
        private TriggerChecker _downCheck;

        public MainWindow(DbContextConnection dbContext,
            ILogger logger
            , ICommunicationDetailsAndInstanceApplication communicationDevice
            , IConnect_Device_With_PC_Function_Data_Application iconnectAddressData
            , ILineStationParameterApplication iLineStation, IMapper imapper
            , CommunicationManagerDictionary communicationManagerDictionary
            , ILineStationTableApplication ilineStationTableApplication
            , ISaveDatasApplication saveDatasApplication
            , ModlingMachineTypeAndPullRodSingletonCombineRoules modlingMachineTypeAndPullRodSingletonCombineRoules
            , SendOrderMessageToPlcSingleton sendOrderMessageToPlcSingleton
            , CustomerQueneForCodesFromMes customerQueneForCodesFromMes)//
        {
            InitializeComponent();
            this._dbContext = dbContext;

            this._logger = logger;
            this._communicationDeviceApplication = communicationDevice;
            this._communicationDeviceApplication = communicationDevice;
            this._iConnectAddressData = iconnectAddressData;
            this._iLineStation = iLineStation;
            this._imapper = imapper;

            this._communicationManagerDictionary = communicationManagerDictionary;
            this._ilineStationTableApplication = ilineStationTableApplication;
            this._logger.LogInformation("软件启动");
            this._saveDatasApplication = saveDatasApplication;

            this._modlingMachineTypeAndPullRodSingletonCombineRoules = modlingMachineTypeAndPullRodSingletonCombineRoules;
            this._sendOrderMessageToPlcSingleton = sendOrderMessageToPlcSingleton;
            this._customerQueneForCodesFromMes = customerQueneForCodesFromMes;






            /*
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string angleDataPath = "C:/Run.csv";
            FileStream fs = new FileStream(angleDataPath, FileMode.Open);
            byte[] datassss = new byte[fs.Length];
            var readS = fs.Read(datassss,0, Convert.ToInt32(fs.Length));
            fs.Close();
            string[] aaaa = Encoding.GetEncoding("GB2312").GetString(datassss).Replace("\r\n","\r").Split('\r');
            Dictionary<string, string> csvValues = new Dictionary<string, string>();
            foreach (string item in aaaa)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    csvValues.Add(item.Split(',')[0], item.Split(',')[1]);
                    
                }
            }
            */

        }

        private void menuitem_Login_Click(object sender, RoutedEventArgs e)
        {
            var login = IocManager.Instance.Resolve<LoginWindow>();
            if (login.ShowDialog() ==  true)
            {
                EnableUISettings(true);

            }


            

        }


        //登录后显示ui
        private void EnableUISettings(bool isEnable)
        {
            this.menuitem_CommunicationSetting.IsEnabled = isEnable;
            this.treeviewItem_CommunicationSetting.IsEnabled = isEnable;

            this.menuitem_LineSetting.IsEnabled = isEnable;
            this.treeviewItem_LineSetting.IsEnabled = isEnable;

            this.menuitem_SaveDatasSetting.IsEnabled = isEnable;
            this.treeviewItem_DataAddressSetting.IsEnabled = isEnable;

            this.menuitem_SuJiSetting.IsEnabled = isEnable;
            this.treeviewItem_ModelingSetting.IsEnabled = isEnable;
            
            //this.menuitem_SearchData.IsEnabled = isEnable;
            
        }

        private void menuitem_OperatorMode_Click(object sender, RoutedEventArgs e)
        {
            EnableUISettings(false);
        }

        private void menuitem_CommunicationSetting_Click(object sender, RoutedEventArgs e)
        {
            var communicationWindow = IocManager.Instance.Resolve<CommunicationSettingsWindow>();
            communicationWindow.ShowDialog();
        }

        private void menuitem_LineSetting_Click(object sender, RoutedEventArgs e)
        {
            var lineWindow = IocManager.Instance.Resolve<LineSettingsWindow>();
            lineWindow.ShowDialog();
        }

        private void menuitem_SaveDatasSetting_Click(object sender, RoutedEventArgs e)
        {
            var dataSettingWindow = IocManager.Instance.Resolve<DataAddressSettingsWindow>();
            dataSettingWindow.ShowDialog();
        }


        private void ChangeControlStatus(bool IsStart)
        {
            this.btn_Open.IsEnabled = IsStart;
            this.btn_Close.IsEnabled = !IsStart;
            
        }

        private void btn_Start_Click(object sender, RoutedEventArgs e)
        {
            Open();

        }


        private void Open()
        {
            this._LineStarters.Clear();

            //获得线体的数量
            var lines = this._iLineStation.QuerryAllLineStationParameters();
            int linesCount = lines.Count;


            //先把grid的column设置好
            grid_userControls.ColumnDefinitions.Clear();
            for (int i = 0; i < linesCount; i++)
            {
                grid_userControls.ColumnDefinitions.Add(new ColumnDefinition());
            }


            //先打开通讯
            OpenCommunications();


            //生成具体需要几条线,只生成启用的

            for (int i = 0; i < linesCount; i++)
            {
                if (lines[i].IsUse)
                {

                }

                LineUserControl lineControl = IocManager.Instance.Resolve<LineUserControl>();
                grid_userControls.Children.Add(lineControl);
                Grid.SetColumn(lineControl, i);
                var datas = this._iConnectAddressData.QuerryConnect_Device_With_PC_Function_DataByStationNumber(lines[i].StationNumber);
                OneLogicLine oneLogicLine = new OneLogicLine(lines[i], datas, this._imapper, this._logger, this._communicationManagerDictionary, this._ilineStationTableApplication, lineControl,IocManager.Instance.Resolve<LogUserControl>());
               
                oneLogicLine.GenerateTable();
                var sss = oneLogicLine.GetOneLineStarter();
                this._loadCheck = oneLogicLine.LoadTriggerChecker;
                this._downCheck = oneLogicLine.DownTriggerChecker;
                
                this._LineStarters.Add(lines[i].StationNumber.ToString(), sss);

            }



            foreach (var item in this._LineStarters)
            {
                item.Value.Start();
            }

            ChangeControlStatus(false);
        }


        private void OpenCommunications()
        {
            this._communicationManagerDictionary.Load();

            this._communicationManagerDictionary.Open();
        }

        private void btn_Close_On_Click(object sender, RoutedEventArgs e)
        {

            foreach (var item in this._LineStarters)
            {
                item.Value.Stop();
            }
            this._LineStarters.Clear();
            this._communicationManagerDictionary.Close();

            //界面清理，
            this.grid_userControls.Children.Clear();
            this.grid_userControls.ColumnDefinitions.Clear();
            this.grid_userControls.RowDefinitions.Clear();
            ChangeControlStatus(true);
        }

        private void menuitem_SearchData_Click(object sender, RoutedEventArgs e)
        {
            var searchWindow = IocManager.Instance.Resolve<SearchDataWindow>();
            searchWindow.Show();
        }

        private void btn_Test_On_Click(object sender, RoutedEventArgs e)
        {
            this._sendOrderMessageToPlcSingleton.SendCtTime(DateTime.Now.AddSeconds(-648.543), DateTime.Now);


            /*
            Task.Run( () => {
                  InsertOne(0,30);
            });

            Task.Run(  () => {
                  InsertOne(90, 110);
            });
            */
        }


       
        private void OrderSetting()
        {
            var orderWindow = IocManager.Instance.Resolve<OrderSendToPlcWindow>();
            if (orderWindow.ShowDialog() == true)
            {
                this._customerQueneForCodesFromMes.Init();
                this._sendOrderMessageToPlcSingleton.InitAddress();
                //先把原有的给去掉
                foreach (var item in this.grid_MainShowGrid.Children)
                {
                    if (item is ShowOrdersAndScanCodeUserControl)
                    {
                        ((ShowOrdersAndScanCodeUserControl)item).Close();
                        //this.grid_MainShowGrid.Children.Remove((ShowOrdersAndScanCodeUserControl)item);
                    }
                    if (item is ShowOrdersAndRunStatusUserControl)
                    {
                        ((ShowOrdersAndRunStatusUserControl)item).Close();
                    }

                    if (item is ShowOrdersAndLoadMaterialAreaStatusUserControl)
                    {
                        ((ShowOrdersAndLoadMaterialAreaStatusUserControl)item).Close();
                    }
                    if (item is ShowOrdersAndDownMaterialAreaStatusUserControl)
                    {
                        ((ShowOrdersAndDownMaterialAreaStatusUserControl)item).Close();
                    }
                }
                Thread.Sleep(400);
                if (this.grid_MainShowGrid.Children.Count > 4)
                {
                    this.grid_MainShowGrid.Children.RemoveRange(1, 4);
                }



                //要去确认下当前几个信号是否是存在的，切队列中是否存在，
                if (this._LineStarters.Count > 0)
                {
                    if (this._loadCheck.currentState == true)
                    {
                        this._customerQueneForCodesFromMes.LoadMaterialQuene.Post(new Application.OtherModelDto.LoadMaterialAreaAndDownMaterialDto
                        {
                            LoadOrDownArea = Application.OtherModelDto.LoadOrDwonEnum.LoadMaterialArea
                        });
                    }

                    if (this._downCheck.currentState == true)
                    {
                        this._customerQueneForCodesFromMes.DownMaterialQuene.Post(new Application.OtherModelDto.LoadMaterialAreaAndDownMaterialDto
                        {
                            LoadOrDownArea = Application.OtherModelDto.LoadOrDwonEnum.DownMaterialArea
                        });
                    }
                }







                //显示两个下发的订单的 运行状态控件，扫描状态的显示
                var scanShowUserControl = IocManager.Instance.Resolve<ShowOrdersAndScanCodeUserControl>();
                this.grid_MainShowGrid.Children.Add(scanShowUserControl);
                Grid.SetColumn(scanShowUserControl, 0);
                Grid.SetRow(scanShowUserControl, 0);



                //上料区订单下发的显示,上料区主要是订单切换
                var loadMaterialAreaUserControl = IocManager.Instance.Resolve<ShowOrdersAndLoadMaterialAreaStatusUserControl>();
                this.grid_MainShowGrid.Children.Add(loadMaterialAreaUserControl);
                Grid.SetColumn(loadMaterialAreaUserControl, 1);
                Grid.SetRow(loadMaterialAreaUserControl, 0);

                //下料区订单下发的显示,上料区主要是订单切换
                var downMaterialAreaUserControl = IocManager.Instance.Resolve<ShowOrdersAndDownMaterialAreaStatusUserControl>();
                this.grid_MainShowGrid.Children.Add(downMaterialAreaUserControl);
                Grid.SetColumn(downMaterialAreaUserControl, 2);
                Grid.SetRow(downMaterialAreaUserControl, 0);


                //下料区出料口的显示
                var runnedShowUserControl = IocManager.Instance.Resolve<ShowOrdersAndRunStatusUserControl>();
                this.grid_MainShowGrid.Children.Add(runnedShowUserControl);
                Grid.SetColumn(runnedShowUserControl, 3);
                Grid.SetRow(runnedShowUserControl, 0);


                Thread.Sleep(200);
                scanShowUserControl.Init();
                loadMaterialAreaUserControl.Init();
                downMaterialAreaUserControl.Init();
                runnedShowUserControl.Init();




            }
        }
        private void menuitem_WorkOrderSetting_Click(object sender, RoutedEventArgs e)
        {
            OrderSetting();


        }

        private void menuitem_SuJiSetting_Click(object sender, RoutedEventArgs e)
        {
            var suijiSettingeWindow = IocManager.Instance.Resolve<SuJiTableSettingWindow>();
            suijiSettingeWindow.Show();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("确认退出？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                e.Cancel =  false;
            }
            else
            {
                e.Cancel =  true;

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            var logControl = IocManager.Instance.Resolve<LogUserControl>();
            this.grid_LogAndButton.Children.Add(logControl);
            Grid.SetColumn(logControl,0);
            //Grid.SetRow(logControl,0);

            Open();
            EnableUISettings(false);
        }

        

        private void treeviewItem_login_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var login = IocManager.Instance.Resolve<LoginWindow>();
            if (login.ShowDialog() == true)
            {
                EnableUISettings(true);

            }
        }

        private void treeviewItem_operator_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            EnableUISettings(false);
        }

        private void treeviewItem_CommunicationSetting_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var communicationWindow = IocManager.Instance.Resolve<CommunicationSettingsWindow>();
            if (communicationWindow.ShowDialog() == true)
            {

            }
        }

        private void treeviewItem_LineSetting_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var lineWindow = IocManager.Instance.Resolve<LineSettingsWindow>();
            
            if (lineWindow.ShowDialog() == true)
            {

            }
        }

        private void treeviewItem_DataAddressSetting_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var dataSettingWindow = IocManager.Instance.Resolve<DataAddressSettingsWindow>();
            if (dataSettingWindow.ShowDialog() == true)
            {

            }
            
        }

        private void treeviewItem_OrderSend_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OrderSetting();
        }

        private void treeviewItem_ModelingSetting_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var suijiSettingeWindow = IocManager.Instance.Resolve<SuJiTableSettingWindow>();
            if (suijiSettingeWindow.ShowDialog() == true )
            {

            }
            ;
        }

        private void treeviewItem_Searching_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var searchWindow = IocManager.Instance.Resolve<SearchDataWindow>();
            if (searchWindow.ShowDialog() == true)
            {

            }
            
        }
    }
}
