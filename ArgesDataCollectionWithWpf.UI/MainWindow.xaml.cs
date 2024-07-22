
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
using ArgesDataCollectionWithWpf.UI.UIWindows;
using ArgesDataCollectionWithWpf.UI.UIWindows.CustomerUserControl;
using ArgesDataCollectionWithWpf.UseFulThirdPartFunction.Excel;
using ArgesDataCollectionWpf.DataProcedure.Generate;
using AutoMapper;
using EnterpriseFD.Dataflow;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace ArgesDataCollectionWithWpf.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,ITransientDependency
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

        private readonly ModlingMachineTypeAndPullRodSingletonCombineRoules  _modlingMachineTypeAndPullRodSingletonCombineRoules;






        private readonly Dictionary<string, IStarter> _LineStarters = new Dictionary<string, IStarter>();

        public MainWindow(DbContextConnection dbContext, 
            ILogger logger
            , ICommunicationDetailsAndInstanceApplication communicationDevice
            , IConnect_Device_With_PC_Function_Data_Application iconnectAddressData
            , ILineStationParameterApplication iLineStation, IMapper imapper
            , CommunicationManagerDictionary communicationManagerDictionary
            , ILineStationTableApplication ilineStationTableApplication
            , ISaveDatasApplication saveDatasApplication
            , ModlingMachineTypeAndPullRodSingletonCombineRoules modlingMachineTypeAndPullRodSingletonCombineRoules)//
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
            this.menuitem_Settings.IsEnabled = isEnable;
            this.menuitem_SearchData.IsEnabled = isEnable;
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

                LineUserControl lineControl = new LineUserControl();
                grid_userControls.Children.Add(lineControl);
                Grid.SetColumn(lineControl, i);
                var datas = this._iConnectAddressData.QuerryConnect_Device_With_PC_Function_DataByStationNumber(lines[i].StationNumber);
                OneLogicLine oneLogicLine = new OneLogicLine(lines[i], datas, this._imapper, this._logger, this._communicationManagerDictionary, this._ilineStationTableApplication, lineControl);
                oneLogicLine.GenerateTable();
                var sss = oneLogicLine.GetOneLineStarter();
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

        

            /*
            Task.Run( () => {
                  InsertOne(0,30);
            });

            Task.Run(  () => {
                  InsertOne(90, 110);
            });
            */
        }


        private   void InsertOne(int startIndex,int endIndex)
        {
            for (int i = startIndex; i < endIndex; i++)
            {
                Task.Delay(200);
                int rrr =  _saveDatasApplication.AddSaveDatasToDataBase(new AddSaveDatasFromPlcInput
                {

                    tableName = "testinsert",
                    IsAllowReWrite = 1,
                    Data0 = i.ToString(),
                    Data1 = System.DateTime.Today.ToString("yyyy-MM-dd,HH-mm-ss,fff"),
                    Data2 = "nihao "
                });
                ; ;


                Console.WriteLine(rrr);
            }
        }

        private void menuitem_WorkOrderSetting_Click(object sender, RoutedEventArgs e)
        {
            var orderWindow = IocManager.Instance.Resolve<OrderSendToPlcWindow>();
            orderWindow.Show();
        }

        private void menuitem_SuJiSetting_Click(object sender, RoutedEventArgs e)
        {
            var suijiSettingeWindow = IocManager.Instance.Resolve<SuJiTableSettingWindow>();
            suijiSettingeWindow.Show();
        }
    }
}
