using Abp.Dependency;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.LineStationParameterApplication;
using ArgesDataCollectionWithWpf.UI.UIWindows.CustomerUserControl;
using ArgesDataCollectionWpf.DataProcedure.Utils;
using AutoMapper;
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

namespace ArgesDataCollectionWithWpf.UI.UIWindows
{
    /// <summary>
    /// UIWriteWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UIWriteWindow : Window, ITransientDependency
    {

        private readonly ILineStationParameterApplication _iLineStation;
        private readonly IConnect_Device_With_PC_Function_Data_Application _iConnectAddressData;
        private readonly IMapper _imapper;

        public UIWriteWindow(ILineStationParameterApplication iLineStation
            , IConnect_Device_With_PC_Function_Data_Application iconnectAddressData)
        {
            this._iLineStation = iLineStation;
            this._iConnectAddressData = iconnectAddressData;
            InitializeComponent();
            Init();
        }


        private void Init()
        {
            var lines = this._iLineStation.QuerryAllLineStationParameters();
            int linesCount = lines.Count;

            grid_userControls.ColumnDefinitions.Clear();
            for (int i = 0; i < linesCount; i++)
            {
                grid_userControls.ColumnDefinitions.Add(new ColumnDefinition());
            }


            for (int i = 0; i < linesCount; i++)
            {
                if (lines[i].IsUse)
                {

                }
                
                LineUserWriteControl lineWriteControl = IocManager.Instance.Resolve<LineUserWriteControl>();
                grid_userControls.Children.Add(lineWriteControl);
                Grid.SetColumn(lineWriteControl, i);
                var datas = this._iConnectAddressData.QuerryConnect_Device_With_PC_Function_DataByStationNumber(lines[i].StationNumber);
                var writeAddress = from m in datas where m.Func == DbModels.Enums.EnumAddressFunction.UIWriteData select m;
                ((IWriteLogForUserControl)lineWriteControl).AddUiShowAndModifyControls(writeAddress.ToList());
            }
        }

    }
}
