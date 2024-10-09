using ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application.Dto;
using ArgesDataCollectionWpf.DataProcedure.Utils;
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

namespace ArgesDataCollectionWithWpf.UI.UIWindows.CustomerUserControl
{
    /// <summary>
    /// LineUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class LineUserControl : UserControl, IWriteLogForUserControl,ITransientDependency
    {

        private const string OneModifyControl = "OneModifyControl";

        Dictionary<string, DataModifyAndShowUserControl> _data = new Dictionary<string, DataModifyAndShowUserControl>();

        public LineUserControl()
        {
            InitializeComponent();
            //AddOneControl();
        }

        public void WriteLog(string messagee)
        {
            this.Dispatcher.Invoke(new Action(() => {


                this.listBox_Log.Items.Add(messagee);
                this.listBox_Log.SelectedIndex = this.listBox_Log.Items.Count - 1;
                this.listBox_Log.ScrollIntoView(this.listBox_Log.Items[this.listBox_Log.Items.Count-1]);
            }));


            


        }

        //进行数据的显示，这里要把 plc只展示的数据，和 展示可修改的数据   以控件形式加载再界面。双向绑定能修改
        public void AddUiShowAndModifyControls(List<QuerryConnect_Device_With_PC_Function_DataOutput>  querryConnect_Device_With_PC_Function_DataOutputs)
        {
            int showControlsCount = querryConnect_Device_With_PC_Function_DataOutputs.Count;
            GenerateGridRowsAndColumns(showControlsCount);


            for (int i = 0; i < showControlsCount; i++)
            {
                int rowindex = i;
                int columindex =0;
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
                        DataModifyAndShowUserControl dataModifyAndShowUserControl = new DataModifyAndShowUserControl(querryConnect_Device_With_PC_Function_DataOutputs[i].DataAddressDescription);
                        dataModifyAndShowUserControl.Name = OneModifyControl + "_ReadAndNeedUpShowOnUi_" + querryConnect_Device_With_PC_Function_DataOutputs[i].DataSaveIndex;
                        this.grid_PlcDataModifyAndShow.Children.Add(dataModifyAndShowUserControl);

                        //VerticalAlignment="Center" HorizontalAlignment="Center"
                        dataModifyAndShowUserControl.VerticalAlignment = VerticalAlignment.Stretch;
                        dataModifyAndShowUserControl.HorizontalAlignment = HorizontalAlignment.Stretch;
                        //dataModifyAndShowUserControl.Margin = new Thickness(0, 0, 0, 0);
                        Grid.SetColumn(dataModifyAndShowUserControl, columindex);
                        Grid.SetRow(dataModifyAndShowUserControl, rowindex);

                        _data.Add("ReadAndNeedUpShowOnUi"+querryConnect_Device_With_PC_Function_DataOutputs[i].DataSaveIndex.ToString(), dataModifyAndShowUserControl);
                        break;
                    case DbModels.Enums.EnumAddressFunction.UIWriteData:
                        /*
                        DataModifyAndShowUserControl dataModifyAndShowUserControl2 = new DataModifyAndShowUserControl(querryConnect_Device_With_PC_Function_DataOutputs[i].DataAddressDescription);
                        dataModifyAndShowUserControl2.Name = OneModifyControl + querryConnect_Device_With_PC_Function_DataOutputs[i].DataSaveIndex;
                        this.grid_PlcDataModifyAndShow.Children.Add(dataModifyAndShowUserControl2);
                        dataModifyAndShowUserControl2.VerticalAlignment = VerticalAlignment.Center;
                        dataModifyAndShowUserControl2.HorizontalAlignment = HorizontalAlignment.Center;
                        Grid.SetColumn(dataModifyAndShowUserControl2, columindex);
                        Grid.SetRow(dataModifyAndShowUserControl2, rowindex);*/
                        break;
                    case DbModels.Enums.EnumAddressFunction.DayProductionOutput:
                        DataModifyAndShowUserControl dataModifyAndShowUserControl2 = new DataModifyAndShowUserControl(querryConnect_Device_With_PC_Function_DataOutputs[i].DataAddressDescription);
                        dataModifyAndShowUserControl2.Name = OneModifyControl + "_DayProductionOutput_" + querryConnect_Device_With_PC_Function_DataOutputs[i].DataSaveIndex;
                        this.grid_PlcDataModifyAndShow.Children.Add(dataModifyAndShowUserControl2);

                        //VerticalAlignment="Center" HorizontalAlignment="Center"
                        dataModifyAndShowUserControl2.VerticalAlignment = VerticalAlignment.Stretch;
                        dataModifyAndShowUserControl2.HorizontalAlignment = HorizontalAlignment.Stretch;
                        //dataModifyAndShowUserControl.Margin = new Thickness(0, 0, 0, 0);
                        Grid.SetColumn(dataModifyAndShowUserControl2, columindex);
                        Grid.SetRow(dataModifyAndShowUserControl2, rowindex);

                        _data.Add("DayProductionOutput" + querryConnect_Device_With_PC_Function_DataOutputs[i].DataSaveIndex.ToString(), dataModifyAndShowUserControl2);

                        break;
                    case DbModels.Enums.EnumAddressFunction.MonthProductionOutput:
                        string name = DbModels.Enums.EnumAddressFunction.MonthProductionOutput.ToString();
                        DataModifyAndShowUserControl dataModifyAndShowUserControl3 = new DataModifyAndShowUserControl(querryConnect_Device_With_PC_Function_DataOutputs[i].DataAddressDescription);
                        dataModifyAndShowUserControl3.Name = OneModifyControl + "_MonthProductionOutput_" + querryConnect_Device_With_PC_Function_DataOutputs[i].DataSaveIndex;
                        this.grid_PlcDataModifyAndShow.Children.Add(dataModifyAndShowUserControl3);

                        //VerticalAlignment="Center" HorizontalAlignment="Center"
                        dataModifyAndShowUserControl3.VerticalAlignment = VerticalAlignment.Stretch;
                        dataModifyAndShowUserControl3.HorizontalAlignment = HorizontalAlignment.Stretch;
                        //dataModifyAndShowUserControl.Margin = new Thickness(0, 0, 0, 0);
                        Grid.SetColumn(dataModifyAndShowUserControl3, columindex);
                        Grid.SetRow(dataModifyAndShowUserControl3, rowindex);

                        _data.Add("MonthProductionOutput"+querryConnect_Device_With_PC_Function_DataOutputs[i].DataSaveIndex.ToString(), dataModifyAndShowUserControl3);

                        break;

                    case DbModels.Enums.EnumAddressFunction.CTTime:

                        //string name = DbModels.Enums.EnumAddressFunction.MonthProductionOutput.ToString();
                        DataModifyAndShowUserControl dataModifyAndShowUserControl4 = new DataModifyAndShowUserControl(querryConnect_Device_With_PC_Function_DataOutputs[i].DataAddressDescription);
                        dataModifyAndShowUserControl4.Name = OneModifyControl + "_CTTime_" + querryConnect_Device_With_PC_Function_DataOutputs[i].DataSaveIndex;
                        this.grid_PlcDataModifyAndShow.Children.Add(dataModifyAndShowUserControl4);

                        //VerticalAlignment="Center" HorizontalAlignment="Center"
                        dataModifyAndShowUserControl4.VerticalAlignment = VerticalAlignment.Stretch;
                        dataModifyAndShowUserControl4.HorizontalAlignment = HorizontalAlignment.Stretch;
                        //dataModifyAndShowUserControl.Margin = new Thickness(0, 0, 0, 0);
                        Grid.SetColumn(dataModifyAndShowUserControl4, columindex);
                        Grid.SetRow(dataModifyAndShowUserControl4, rowindex);

                        _data.Add("CTTime" + querryConnect_Device_With_PC_Function_DataOutputs[i].DataSaveIndex.ToString(), dataModifyAndShowUserControl4);


                        break;
                    default:
                        break;
                }
            }

            
        }


        private void GenerateGridRowsAndColumns(int count)
        {


           

            int columsCount = 1;
            int rowsCount = count ;

            
            for (int i = 0; i < rowsCount; i++)
            {
                var ccs = new RowDefinition();
                ccs.Height = new GridLength(35);
                this.grid_PlcDataModifyAndShow.RowDefinitions.Add(ccs);
            }


           
        }

        private void AddOneControl(string dataName,string controlName)
        {

        }

        public void ChangeUiValueFromPlc(string dataIndex, object value)
        {

            this.Dispatcher.BeginInvoke(new Action(() => {

                _data[dataIndex.ToString()].SetPlcValue(value==null?"null": value.ToString());
                //var ccc = this.grid_PlcDataModifyAndShow.FindResource(OneModifyControl + dataIndex) as DataModifyAndShowUserControl;
                //this.grid_PlcDataModifyAndShow.Children.
                //ccc.SetPlcValue(value.ToString()); 

            }));

            
        }
    }
}
