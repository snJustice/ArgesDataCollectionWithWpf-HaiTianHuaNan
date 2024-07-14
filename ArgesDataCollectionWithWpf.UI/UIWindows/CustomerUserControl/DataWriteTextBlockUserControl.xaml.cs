using ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application.Dto;
using ArgesDataCollectionWithWpf.Communication.Utils;
using ArgesDataCollectionWithWpf.DbModels.CommunicationParaTransferModel;
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
    /// DataWriteTextBlockUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class DataWriteTextBlockUserControl : UserControl, IUserControlGetDataitemValue
    {
        
        private readonly DataItemModel _dataItem;
        LabelDataSourceBinding labelData = new LabelDataSourceBinding();
        public DataWriteTextBlockUserControl(string  inputLabel, DataItemModel dataItem)
        {
            InitializeComponent();
            this.lbl_Index.DataContext = labelData;
            this.labelData.LabelData = inputLabel;

            this._dataItem = dataItem;


        }

        public DataItemModel GetDataitem()
        {
            
            _dataItem.Value = this.txt_plcValue.Text.CastingTargetType(_dataItem.VarType);
            return _dataItem;
        }
    }


    public interface IUserControlGetDataitemValue
    {
        DataItemModel GetDataitem();
    }
}
