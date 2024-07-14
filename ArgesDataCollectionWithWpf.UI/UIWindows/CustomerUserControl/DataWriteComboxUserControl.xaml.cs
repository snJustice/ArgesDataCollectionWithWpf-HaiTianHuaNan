using ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application.Dto;
using ArgesDataCollectionWithWpf.Communication.Utils;
using ArgesDataCollectionWithWpf.DbModels.CommunicationParaTransferModel;
using ArgesDataCollectionWithWpf.UseFulThirdPartFunction.Excel;
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
    /// DataWriteComboxUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class DataWriteComboxUserControl : UserControl, IUserControlGetDataitemValue
    {
        private readonly DataItemModel _dataItem;
        LabelDataSourceBinding labelData = new LabelDataSourceBinding();
        
        Dictionary<string, int> _typeAndValue = new Dictionary<string, int>();
        public DataWriteComboxUserControl(string inputLabel, DataItemModel dataItem, Dictionary<string, int> typeAndValue)
        {
            InitializeComponent();

            this.lbl_Index.DataContext = labelData;
            this.labelData.LabelData = inputLabel;
            this._typeAndValue = typeAndValue;
            this._dataItem = dataItem;


            var conboxValues = from m in this._typeAndValue select m.Key;

            this.cbx_plcValue.ItemsSource = conboxValues;
            this.cbx_plcValue.SelectedIndex = 0;

        }

        public DataItemModel GetDataitem()
        {
            int sendIndex = this._typeAndValue[this.cbx_plcValue.Text];
            
            this._dataItem.Value = sendIndex.ToString().CastingTargetType(_dataItem.VarType);
            return _dataItem;
        }
    }



}
