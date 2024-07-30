using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.CommunicationDetailsAndInstanceApplication;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application.Dto;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.LineStationParameterApplication;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.LineStationTableApplication;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.SaveDatasApplication;
using ArgesDataCollectionWithWpf.Core.Utils;
using ArgesDataCollectionWithWpf.UI.sqlFactory;
using AutoMapper;
using ArgesDataCollectionWithWpf.Communication.Utils;
using ArgesDataCollectionWithWpf.UI.sqlFactory.UpdateRandomDouble;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.SaveDatasApplication.Dto;

namespace ArgesDataCollectionWithWpf.UI.UIWindows
{
    /// <summary>
    /// SearchDataWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SearchDataWindow : Window,ITransientDependency
    {
      
        private readonly IConnect_Device_With_PC_Function_Data_Application _iconnectAddressData;
        private readonly ILineStationParameterApplication _iLineStation;
        private readonly IMapper _imapper;
        private readonly ISaveDatasApplication _isaveDatasApplication;


        private Dictionary<string, List<int>> _mainLineAndFuLinesDictionary;

        private Dictionary<string, List<QuerryConnect_Device_With_PC_Function_DataOutput>> _linesAddrssesDictionary;

        private Dictionary<string, int> _terminalToMainLineDictionary;

        private Dictionary<string, int> _lineIDToMainLineDictionary;

        public SearchDataWindow( IConnect_Device_With_PC_Function_Data_Application iconnectAddressData
            , ILineStationParameterApplication iLineStation
            , IMapper imapper
            , ISaveDatasApplication isaveDatasApplication)
        {
            InitializeComponent();
            
            this._iconnectAddressData = iconnectAddressData;
            this._iLineStation = iLineStation;
            this._imapper = imapper;
            this._isaveDatasApplication = isaveDatasApplication;

            this._mainLineAndFuLinesDictionary = new Dictionary<string, List<int>>();
            this._linesAddrssesDictionary = new Dictionary<string, List<QuerryConnect_Device_With_PC_Function_DataOutput>>();
            this._terminalToMainLineDictionary = new Dictionary<string, int>();
            this._lineIDToMainLineDictionary = new Dictionary<string, int>();


           
        }

        


        

        private DataTable StuffDataIntoDatagrid(DataTable dt,List<dynamic> Datas)
        {

            foreach (var item in Datas)
            {
                var newRow = dt.NewRow();
                foreach (var item2 in item)
                {
                    newRow[item2.Key.ToString()] = item2.Value is null ?"": item2.Value.ToString();


                }

                dt.Rows.Add(newRow);
            }
            


            return dt;
        }

        private DataTable GenerateDatagridHeader(dynamic oneData)
        {
            string ss = "ss";
            DataTable dt = new DataTable();
            foreach (var item in oneData)
            {
                DataColumn dataColumn = new DataColumn();
                string rowName = item.Key.ToString();
                dataColumn.ColumnName = rowName;
                string caption = rowName.Replace(rowName.GetNumberWith_(), "").Replace("ss","") ;
                
                dataColumn.Caption = caption;
                dt.Columns.Add(dataColumn);
                
            }

            return dt;
        }

        

        private void btn_ModifyDataLogin_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_ExportExcel_Click(object sender, RoutedEventArgs e)
        {
            var getSql = GetTargetGenerateSqlHandler();
            if (getSql == null)
            {
                return;
            }

            var datas = this._isaveDatasApplication.GetCombineDatasByDefineGenerateSqls(getSql.GetSQL().Replace("\r\n", ""), -1, 50);

            string fileName = $"C:\\{this.cbx_LineNumber.Text}-{DateTime.Now.ToString("yyyy-MM-dd,HH-mm-ss")}.csv";
            if (!CustomerCsvHelper.IsExistFile(fileName))
            {
                CustomerCsvHelper.CreateFile(fileName);
            }
            else
            {
                File.Delete(fileName);
            }
            string header = "";

            foreach (dynamic item in datas.AllCombineDatas[0])
            {
                header += item.Key.ToString() + ";";
            }
            
            CustomerCsvHelper.WriteHeader(fileName, header);

            for (int i = 0; i < datas.AllCombineDatas.Count(); i++)
            {
                string data = "";

                foreach (dynamic item in datas.AllCombineDatas[i])
                {
                    data += "\t" + ((item.Value!=null )? item.Value.ToString():"") + ";";
                }

               
                CustomerCsvHelper.WriteOneLine(fileName, data);
            }

            MessageBox.Show($"保存成功，{fileName}");
        }

        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {

            var getSql = GetTargetGenerateSqlHandler();
            if (getSql==null )
            {
                return;
            }

            var datas = this._isaveDatasApplication.GetCombineDatasByDefineGenerateSqls(getSql.GetSQL().Replace("\r\n", ""), 1, 50);

            if (datas.AllCombineDatas==null ||  datas.AllCombineDatas.Count<=0)
            {
                this.grid_DataSearchShow.DataContext = null;
                return;
            }

            //根据数据生成表头
            var dt = GenerateDatagridHeader(datas.AllCombineDatas[0]);

            var showDt = StuffDataIntoDatagrid(dt, datas.AllCombineDatas);

            //填充数据
            this.grid_DataSearchShow.DataContext = showDt;

            

        }




        private IGenerateSql GetTargetGenerateSqlHandler()
        {
            // 这里其实是工厂，根据当前选择的方式，去选择具体生成哪个sql语句类
            if (string.IsNullOrWhiteSpace(this.cbx_LineNumber.Text))
            {
                MessageBox.Show("请先选择线体");
                return null;

            }


            //获得当前的线体，并判断是不是最后一个线体
            string targetLine = this.cbx_LineNumber.Text;

            bool isTerminalLine = this._terminalToMainLineDictionary.Keys.Contains(targetLine);


            List<QuerryConnect_Device_With_PC_Function_DataOutput> mainAddress = this._linesAddrssesDictionary[this._lineIDToMainLineDictionary[targetLine].ToString()];
            var fudbsId = this._mainLineAndFuLinesDictionary[this._lineIDToMainLineDictionary[targetLine].ToString()];

            List<QuerryConnect_Device_With_PC_Function_DataOutput>[] querryConnect_Device_With_PC_Function_DataOutputs = new List<QuerryConnect_Device_With_PC_Function_DataOutput>[fudbsId.Count];

            for (int i = 0; i < fudbsId.Count; i++)
            {
                querryConnect_Device_With_PC_Function_DataOutputs[i] = this._linesAddrssesDictionary[fudbsId[i].ToString()];
            }

            if (!string.IsNullOrWhiteSpace(this.txt_CodeMessage.Text) && !isTerminalLine)
            {
                return new InitCodeGenerateSql(this.txt_CodeMessage.Text, targetLine
                    ,this._linesAddrssesDictionary[  this._lineIDToMainLineDictionary[targetLine].ToString() ] 
                    , querryConnect_Device_With_PC_Function_DataOutputs);
            }
            if (!string.IsNullOrWhiteSpace(this.txt_CodeMessage.Text) && isTerminalLine)
            {
                return new TerminalCodeGenerateSql(this.txt_CodeMessage.Text, targetLine
                    , this._linesAddrssesDictionary[this._lineIDToMainLineDictionary[targetLine].ToString()]
                    , querryConnect_Device_With_PC_Function_DataOutputs);
            }



            return new TimeRangeGenerateSql( DateTime.Parse(this.dateTimePicker_StartTime.txt_CurrentTime.Text)
                , DateTime.Parse(this.dateTimePicker_EndTime.txt_CurrentTime.Text)
                , targetLine
                , this._linesAddrssesDictionary[this._lineIDToMainLineDictionary[targetLine].ToString()]
                , querryConnect_Device_With_PC_Function_DataOutputs);
        }


        private IGenerateSql GetUpdateDoubleSqlHnadler(string targetLine,string dataIndex,double max,double min)
        {
           
           

            bool isTerminalLine = this._terminalToMainLineDictionary.Keys.Contains(targetLine);
            int tmainLine = this._lineIDToMainLineDictionary[targetLine];
            var fulines = this._mainLineAndFuLinesDictionary[tmainLine.ToString()];
            string terminalLineID = fulines[fulines.Count - 1].ToString();

            if (!string.IsNullOrWhiteSpace(this.txt_CodeMessage.Text) && !isTerminalLine)
            {
                return new InitCodeGenerateSqlForUpdateRandomDouble(this.txt_CodeMessage.Text, targetLine,  dataIndex,terminalLineID, max,min);
            }

            if (!string.IsNullOrWhiteSpace(this.txt_CodeMessage.Text) && isTerminalLine)
            {
                return new TerminalCodeGenerateSqlForUpdateRandomDouble(this.txt_CodeMessage.Text, targetLine, dataIndex, terminalLineID, max, min);
            }

            return new TimeRangeGenerateSqlForUpdateRandomDouble(DateTime.Parse(this.dateTimePicker_StartTime.txt_CurrentTime.Text)
                , DateTime.Parse(this.dateTimePicker_EndTime.txt_CurrentTime.Text)
                , targetLine
                , dataIndex
                , terminalLineID
                , max,min);
        }

        private void LoadLineMessageAndDataAddress()
        {
            var lines = this._iLineStation.QuerryAllLineStationParameters();

            List<int> mainLines = (from m in lines where  m.IsMainStation == true select m.StationNumber).ToList();



            foreach (var item in mainLines)
            {
                var fulines = (from m in lines where m.BelongLineNumber == item&& m.BelongLineNumber!=m.StationNumber orderby m.StationNumber select m.StationNumber ).ToList();
                this._mainLineAndFuLinesDictionary.Add(item.ToString(), fulines);

                if ((fulines==null) || (fulines.Count()==0))
                {
                    continue;
                }
                this._terminalToMainLineDictionary.Add(fulines[fulines.Count-1].ToString(), item);
            }

            foreach (var item in lines)
            {
                var address = this._iconnectAddressData.QuerryConnect_Device_With_PC_Function_DataByStationNumber(item.StationNumber);
                var needAddresses = (from m in address where m.Func == DbModels.Enums.EnumAddressFunction.ReadAndNeedSaveData select m).ToList();
                var needAddresses2 = (from m in address where m.Func == DbModels.Enums.EnumAddressFunction.ReadAndNeedSaveDataNotFromPLC select m).ToList();
                needAddresses.AddRange(needAddresses2);
                this._linesAddrssesDictionary.Add(item.StationNumber.ToString(), needAddresses);


                this._lineIDToMainLineDictionary.Add(item.StationNumber.ToString(),item.BelongLineNumber);
            }




        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //加载当前线体信息
            LoadLineMessageAndDataAddress();
            this.cbx_LineNumber.ItemsSource = this._linesAddrssesDictionary.Keys;
        }

        private void grid_DataSearchShow_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            //e.Column.Header = ((sender as DataGrid).ItemsSource as DataView).Table.Columns[e.PropertyName].Caption; 
            e.Column.Header = new DisplayAndIndexName
            {
                Display = ((sender as DataGrid).ItemsSource as DataView).Table.Columns[e.PropertyName].Caption
                ,Index = ((sender as DataGrid).ItemsSource as DataView).Table.Columns[e.PropertyName].ColumnName

            };
        }

        

        private void grid_DataSearchShow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            
            string sss = ((sender as DataGrid).CurrentCell.Column.Header as DisplayAndIndexName).Index;

            //需要先登录



            string tableNameAndIndex = sss.GetNumberWith_();

            string tableName = tableNameAndIndex.Split(TableNameAndDataIndexSplit.TableAndIndexSplitChar)[0];
            string dataIndex = tableNameAndIndex.Split(TableNameAndDataIndexSplit.TableAndIndexSplitChar)[1];

            //
            double max = 5, min = 2;

            var sql = GetUpdateDoubleSqlHnadler(tableName, dataIndex,max,min).GetSQL();

            var modifyCount = this._isaveDatasApplication.ModifyDataByRowSql(new RowSqlSaveDatas {  RowSql= sql });

            

        }


        
    }



    public class DisplayAndIndexName
    {
        public string Display { get; set; }
        public string Index { get; set; }

        public override string ToString()
        {
            return Display;
        }
    }
}
