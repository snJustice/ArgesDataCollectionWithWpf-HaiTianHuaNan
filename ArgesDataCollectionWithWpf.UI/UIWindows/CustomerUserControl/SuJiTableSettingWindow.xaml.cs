using System;
using System.Collections.Generic;
using System.Data;
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
using Abp.Dependency;
using ArgesDataCollectionWithWpf.Application.Utils;
using ArgesDataCollectionWithWpf.UseFulThirdPartFunction.Excel;

namespace ArgesDataCollectionWithWpf.UI.UIWindows.CustomerUserControl
{
    /// <summary>
    /// SuJiTableSettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SuJiTableSettingWindow : Window,ITransientDependency
    {

        private readonly IAppConfigureRead _appConfigRead;
        public SuJiTableSettingWindow(IAppConfigureRead appConfigRead)
        {
            InitializeComponent();

            this._appConfigRead = appConfigRead;


            InitReadExcel();
        }

        private void InitReadExcel()
        {
            IExcelGetData excel = new ExcelOperating(this._appConfigRead.ReadKey("TemporaryExcelPath"), "1");
            var tabless = excel.GetDataTable();

            this.dataGrid_SuJiSetting.ItemsSource = tabless.DefaultView;
        }

        

        private void btn_SaveExcel_Click(object sender, RoutedEventArgs e)
        {
            IExcelGetData excel = new ExcelOperating(this._appConfigRead.ReadKey("TemporaryExcelPath"), "1");
            var ttt = DataGridToTable(this.dataGrid_SuJiSetting);
            excel.DataTableToExcel(ttt, this._appConfigRead.ReadKey("TemporaryExcelPath"));
        }

        private  DataTable DataGridToTable(DataGrid dg)
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < dg.Columns.Count; i++)
            {
                dt.Columns.Add(dg.Columns[i].Header.ToString());
            }
            for (int i = 0; i < dg.Items.Count; i++)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < dg.Columns.Count; j++)
                {
                    dr[dg.Columns[j].Header.ToString()] = (dg.Columns[j].GetCellContent(dg.Items[i]) as TextBlock).Text.ToString();
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }


    }
}
