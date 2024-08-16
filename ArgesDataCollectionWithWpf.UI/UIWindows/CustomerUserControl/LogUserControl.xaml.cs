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
using Abp.Dependency;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application.Dto;
using ArgesDataCollectionWpf.DataProcedure.Utils;

namespace ArgesDataCollectionWithWpf.UI.UIWindows.CustomerUserControl
{
    /// <summary>
    /// LogUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class LogUserControl : UserControl,ISingletonDependency, IWriteLogForUserControl
    {
        public LogUserControl()
        {
            InitializeComponent();
        }

        public void AddUiShowAndModifyControls(List<QuerryConnect_Device_With_PC_Function_DataOutput> querryConnect_Device_With_PC_Function_DataOutputs)
        {
            throw new NotImplementedException();
        }

        public void ChangeUiValueFromPlc(string dataIndex, object value)
        {
            throw new NotImplementedException();
        }

        public void WriteLog(string message)
        {
            this.Dispatcher.Invoke(new Action(() => {


                this.listBox_Log.Items.Add(message);
                this.listBox_Log.SelectedIndex = this.listBox_Log.Items.Count - 1;
                this.listBox_Log.ScrollIntoView(this.listBox_Log.Items[this.listBox_Log.Items.Count - 1]);
            }));
        }
    }
}
