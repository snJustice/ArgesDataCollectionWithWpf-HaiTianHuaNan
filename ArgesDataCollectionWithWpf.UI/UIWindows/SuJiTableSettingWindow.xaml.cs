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
using ArgesDataCollectionWithWpf.UI.SingletonResource.ModlingMachineDeviceResource;
using ArgesDataCollectionWithWpf.UseFulThirdPartFunction.CSV;
using ArgesDataCollectionWithWpf.UseFulThirdPartFunction.Excel;

namespace ArgesDataCollectionWithWpf.UI.UIWindows.CustomerUserControl
{
    /// <summary>
    /// SuJiTableSettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SuJiTableSettingWindow : Window,ITransientDependency
    {

        private readonly IAppConfigureRead _appConfigRead;
        private readonly ModlingMachineTypeAndPullRodSingletonCombineRoules _modlingMachineTypeAndPullRodSingletonCombineRoules;

        public SuJiTableSettingWindow(IAppConfigureRead appConfigRead , ModlingMachineTypeAndPullRodSingletonCombineRoules modlingMachineTypeAndPullRodSingletonCombineRoules)
        {
            InitializeComponent();

            this._appConfigRead = appConfigRead;
            this._modlingMachineTypeAndPullRodSingletonCombineRoules = modlingMachineTypeAndPullRodSingletonCombineRoules;
            InitReadExcel();
        }

        private void InitReadExcel()
        {
            IGetDataFromFile excel = new CsvOperating(this._appConfigRead.ReadKey("TemporaryExcelPath"));
            var tabless = excel.GetDataTable();

            this.dataGrid_SuJiSetting.ItemsSource = tabless.DefaultView;
        }

        

        private void btn_SaveExcel_Click(object sender, RoutedEventArgs e)
        {
            IGetDataFromFile excel = new CsvOperating(this._appConfigRead.ReadKey("TemporaryExcelPath"));
            var ttt = DataGridToTable(this.dataGrid_SuJiSetting);

            try
            {
                excel.DataTableToFile(ttt, this._appConfigRead.ReadKey("TemporaryExcelPath"));
                MessageBox.Show("保存成功");
                //保存成功后还需要把机型更新
                this._modlingMachineTypeAndPullRodSingletonCombineRoules.Clear();
                this._modlingMachineTypeAndPullRodSingletonCombineRoules.InitRoules();
            }
            catch (Exception ex)
            {

                MessageBox.Show("保存失败"+ ex.Message);
            }
            
            
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
