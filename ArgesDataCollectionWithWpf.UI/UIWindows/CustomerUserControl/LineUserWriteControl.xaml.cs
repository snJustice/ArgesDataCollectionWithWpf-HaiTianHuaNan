using Abp.Dependency;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application.Dto;
using ArgesDataCollectionWithWpf.Application.Utils;
using ArgesDataCollectionWithWpf.Communication;
using ArgesDataCollectionWithWpf.DbModels.CommunicationParaTransferModel;
using ArgesDataCollectionWithWpf.UseFulThirdPartFunction.Excel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ArgesDataCollectionWithWpf.UI.UIWindows.CustomerUserControl
{
    /// <summary>
    /// LineUserWriteControl.xaml 的交互逻辑
    /// </summary>
    public partial class LineUserWriteControl : UserControl, IWriteLogForUserControl, ITransientDependency
    {

        private CommunicationManagerDictionary _senders;

        private int communicationID = 0;


        List<IUserControlGetDataitemValue> _controls = new List<IUserControlGetDataitemValue>();

        private const string OneModifyControl = "OneModifyControl";
        private readonly  IMapper _mapper;
        private readonly IAppConfigureRead _appConfigRead;
        public LineUserWriteControl(IMapper imapper
            , CommunicationManagerDictionary senders
            , IAppConfigureRead appConfigRead)
        {
            InitializeComponent();
            this._mapper = imapper;
            this._senders = senders;
            this._appConfigRead = appConfigRead;

        }


        public void AddUiShowAndModifyControls(List<QuerryConnect_Device_With_PC_Function_DataOutput> querryConnect_Device_With_PC_Function_DataOutputs)
        {
            int showControlsCount = querryConnect_Device_With_PC_Function_DataOutputs.Count;
            GenerateGridRowsAndColumns(showControlsCount);

            
            for (int i = 0; i < showControlsCount; i++)
            {
                int rowindex = i / 4;
                int columindex = i - rowindex * 4;
                switch (querryConnect_Device_With_PC_Function_DataOutputs[i].Func)
                {
                    case DbModels.Enums.EnumAddressFunction.ReadAndNeedSaveData:
                        break;
                    case DbModels.Enums.EnumAddressFunction.SaveOK:
                        break;
                    case DbModels.Enums.EnumAddressFunction.SaveFail:
                        break;
                    case DbModels.Enums.EnumAddressFunction.Socket:
                        break;
                    case DbModels.Enums.EnumAddressFunction.Trigger:
                        break;
                    case DbModels.Enums.EnumAddressFunction.ReadAndNeedUpShowOnUi:
                        
                        break;
                    case DbModels.Enums.EnumAddressFunction.UIWriteData:
                        communicationID = querryConnect_Device_With_PC_Function_DataOutputs[i].CommunicationID;
                        if (querryConnect_Device_With_PC_Function_DataOutputs[i].DataAddressDescription.Contains("列表"))
                        {
                            IExcelGetData excel = new ExcelOperating(this._appConfigRead.ReadKey("TemporaryExcelPath"), "1");
                            var tabless = excel.GetDataTable();
                            Dictionary<string, int> tpes = new Dictionary<string, int>();
                            for (int j = 0; j < tabless.Rows.Count; j+=2)
                            {
                                var  sdasde = tabless.Rows[j][1].ToString();
                                if ( !string.IsNullOrWhiteSpace(tabless.Rows[j][1].ToString()))
                                {
                                    tpes.Add(tabless.Rows[j][1].ToString(), Convert.ToInt32(tabless.Rows[j][0]));
                                }
                                
                            }

                            var dataItem = this._mapper.Map<DataItemModel>(querryConnect_Device_With_PC_Function_DataOutputs[i]);
                            DataWriteComboxUserControl dataModifyAndShowUserControl2 = new DataWriteComboxUserControl(querryConnect_Device_With_PC_Function_DataOutputs[i].DataAddressDescription, dataItem, tpes);
                            dataModifyAndShowUserControl2.Name = OneModifyControl + querryConnect_Device_With_PC_Function_DataOutputs[i].DataSaveIndex;
                            this.grid_writeControls.Children.Add(dataModifyAndShowUserControl2);
                            dataModifyAndShowUserControl2.VerticalAlignment = VerticalAlignment.Center;
                            dataModifyAndShowUserControl2.HorizontalAlignment = HorizontalAlignment.Center;
                            Grid.SetColumn(dataModifyAndShowUserControl2, columindex);
                            Grid.SetRow(dataModifyAndShowUserControl2, rowindex);
                            this._controls.Add(dataModifyAndShowUserControl2);
                        }
                        else
                        {
                            var dataItem = this._mapper.Map<DataItemModel>(querryConnect_Device_With_PC_Function_DataOutputs[i]);
                            
                            DataWriteTextBlockUserControl dataModifyAndShowUserControl2 = new DataWriteTextBlockUserControl(querryConnect_Device_With_PC_Function_DataOutputs[i].DataAddressDescription, dataItem);
                            dataModifyAndShowUserControl2.Name = OneModifyControl + querryConnect_Device_With_PC_Function_DataOutputs[i].DataSaveIndex;
                            this.grid_writeControls.Children.Add(dataModifyAndShowUserControl2);
                            dataModifyAndShowUserControl2.VerticalAlignment = VerticalAlignment.Center;
                            dataModifyAndShowUserControl2.HorizontalAlignment = HorizontalAlignment.Center;
                            Grid.SetColumn(dataModifyAndShowUserControl2, columindex);
                            Grid.SetRow(dataModifyAndShowUserControl2, rowindex);
                            this._controls . Add(dataModifyAndShowUserControl2);
                        }

                        
                        break;
                    default:
                        break;
                }
            }


        }

        public void ChangeUiValueFromPlc(int dataIndex, object value)
        {
            throw new NotImplementedException();
        }

        public void WriteLog(string message)
        {
            throw new NotImplementedException();
        }

        private void GenerateGridRowsAndColumns(int count)
        {
            if (count < 8)
            {
                count = 8;
            }
            else
            {
                count = ((int)(count / 4) + 1) * 4;
            }

            int columsCount = count / 4;
            int rowsCount = 4;

            for (int i = 0; i < columsCount; i++)
            {
                this.grid_writeControls.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < rowsCount; i++)
            {
                this.grid_writeControls.RowDefinitions.Add(new RowDefinition());
            }
        }

        private void btn_Write_Click(object sender, RoutedEventArgs e)
        {
            List<DataItemModel> sendDatas = new List<DataItemModel>();
            foreach (var item in this._controls)
            {
                sendDatas.Add(item.GetDataitem());
            }
            var writeResult = ((ISender)this._senders[communicationID]).SendData(sendDatas);
            if (writeResult )
            {
                MessageBox.Show("写入成功");
                
            }
            else
            {
                MessageBox.Show("写入失败");
            }
        }
    }
}
