using Abp.Dependency;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.CommunicationDetailsAndInstanceApplication;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application.Dto;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.LineStationParameterApplication;

using ArgesDataCollectionWithWpf.Application.SerializeApplication;
using ArgesDataCollectionWithWpf.Communication;
using ArgesDataCollectionWithWpf.Communication.Utils;
using ArgesDataCollectionWithWpf.Core;
using ArgesDataCollectionWithWpf.DbModels.CommunicationParaTransferModel;
using ArgesDataCollectionWithWpf.DbModels.CommunicationParaTransferModel.SimensS7;
using ArgesDataCollectionWithWpf.UI.UIWindows;
using ArgesDataCollectionWpf.DataProcedure.DataFlow.Handlers;
using ArgesDataCollectionWpf.DataProcedure.Generate;
using AutoMapper;
using Castle.MicroKernel;
using EnterpriseFD.Dataflow;
using Microsoft.Extensions.Logging;
using S7.Net;
using S7.Net.Types;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        public MainWindow(DbContextConnection dbContext, ILogger logger, ICommunicationDetailsAndInstanceApplication communicationDevice, IConnect_Device_With_PC_Function_Data_Application iconnectAddressData, ILineStationParameterApplication iLineStation, IMapper imapper, CommunicationManagerDictionary communicationManagerDictionary)//
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

            this._logger.LogInformation("软件启动");






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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenCommunications();
            var lines = this._iLineStation.QuerryAllLineStationParameters();
            var line1 = from m in lines where m.ID == 1 select m;
            var datas = this._iConnectAddressData.QuerryConnect_Device_With_PC_Function_DataByStationNumber(line1.First().StationNumber);
            
            

            OneLogicLine oneLogicLine = new OneLogicLine(line1.First(), datas, this._imapper,this._communicationManagerDictionary);
            var sss = oneLogicLine.GetOneLineStarter();
            sss.Start();
        }


        private void OpenCommunications()
        {
            this._communicationManagerDictionary.Load();

            this._communicationManagerDictionary.Open();
        }

        private void btn_Close_On_Click(object sender, RoutedEventArgs e)
        {
            this._communicationManagerDictionary.Close();
        }
    }
}
