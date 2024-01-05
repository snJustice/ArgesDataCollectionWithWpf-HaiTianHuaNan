using Abp.Dependency;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.LineStationParameterApplication;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.LineStationParameterApplication.Dto;
using ArgesDataCollectionWithWpf.Application.Utils;
using ArgesDataCollectionWithWpf.UI.Utils.UiDataUtils;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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



/*
 * 
 * 表名必须要不同，且保存退出后，要再数据库中生成新的线体表，当发现线体少的时候，可能需要删除表，
 * 
 * 
 * 
 */






namespace ArgesDataCollectionWithWpf.UI.UIWindows
{
    /// <summary>
    /// LineSettingsWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LineSettingsWindow : Window, ITransientDependency
    {

        //数据状态的字典

        private Dictionary<int, DataStatus> _userDataOperation=new Dictionary<int, DataStatus>();


        private readonly ILineStationParameterApplication _ilineStationParameterApplication;
        private readonly IMapper _imapper;
        UserLineModelDto LineStationParameterDb = new UserLineModelDto(); 

        public LineSettingsWindow(ILineStationParameterApplication ilineStationParameterApplication ,IMapper imapper)
        {
            InitializeComponent();
            this._ilineStationParameterApplication = ilineStationParameterApplication;
            this._imapper = imapper;
            this.grid_LineSettingShow.DataContext = LineStationParameterDb;
            


        }
        
        private void btn_AddOne_Click(object sender, RoutedEventArgs e)
        {

            //增加一条数据，但是只增加id
            
            int newIndex = this.LineStationParameterDb.LineStationParameterDb.GetUniqueIndex("ID");
            this.LineStationParameterDb.LineStationParameterDb.Add(new QuerryLineStationParameterOutput { 
            
                ID= newIndex,
                StationDescription = "",
                StationNumber= 1,
                BelongLineNumber= 1,
                IsMainStation= false,
                IsInsertOrUpdate =  DbModels.Enums.EnumDataSaveRule.Update,
                IsUse = true,
                TargetTableName = "1"
            
            });

            AddOneUserOperation(newIndex, DataStatus.Add);



        }

        /// <summary>
        /// 更新数据库的数据
        /// </summary>
        public void UpdateDatabseDataByUi()
        {
            //根据_userDataOperation进行数据的具体做法

            foreach (var item in this._userDataOperation)
            {

                var addModel = (from m in LineStationParameterDb.LineStationParameterDb where m.ID == item.Key select m).FirstOrDefault();
                switch (item.Value)
                {

                    case DataStatus.Add:
                        if (addModel != null)
                        {
                            this._ilineStationParameterApplication.InsertLineStationParameter(this._imapper.Map<AddLineStationParameterInput>(addModel));
                        }
                        

                        break;
                    case DataStatus.Update:
                        if (addModel != null)
                        {
                            this._ilineStationParameterApplication.ModifyLineStationParameterById(this._imapper.Map<ModifyLineStationParameterInput>(addModel));
                        }
                        
                        break;
                    case DataStatus.Delete:

                        this._ilineStationParameterApplication.DeleteLineStationParameterByID(new DeleteLineStationParameterByIdInput { ID = item.Key });
                        break;

                    default:
                        break;

                }
                
                
            }
        }

        private void btn_DeleteOne_Click(object sender, RoutedEventArgs e)
        {

            var deleteItem =  (QuerryLineStationParameterOutput)this.grid_LineSettingShow.SelectedItem;
            //删除选择的行数据
            var removeResult = this.LineStationParameterDb.LineStationParameterDb.Remove( (QuerryLineStationParameterOutput)this.grid_LineSettingShow.SelectedItem );

            AddOneUserOperation(deleteItem.ID, DataStatus.Delete);


        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            UpdateDatabseDataByUi();
            MessageBox.Show("保存成功");
            this._userDataOperation.Clear();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var currentDataInDb = this._ilineStationParameterApplication.QuerryAllLineStationParameters();
            foreach (var item in currentDataInDb)
            {
                LineStationParameterDb.LineStationParameterDb.Add(item);
            }
            
        }


        

        private void grid_LineSettingShow_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var updateItem = (QuerryLineStationParameterOutput)this.grid_LineSettingShow.SelectedItem;




            AddOneUserOperation(updateItem.ID, DataStatus.Update);

        }


        private void AddOneUserOperation(int index,DataStatus dataStatus)
        {
            switch (dataStatus) 
            {
                case DataStatus.Add:
                    this._userDataOperation.Add(index, DataStatus.Add);
                    break;
                case DataStatus.Delete:
                    if (this._userDataOperation.ContainsKey(index))
                    {
                        this._userDataOperation.Remove(index);
                    }
                    else
                    {
                        this._userDataOperation.Add(index, DataStatus.Delete);
                    }
                    break;
                case DataStatus.Update:
                    if (!this._userDataOperation.ContainsKey(index))
                    {
                        this._userDataOperation.Add(index, DataStatus.Update);
                    }
                    break;
            }
        }
    }


    //datagrid数据绑定的结构
    public class UserLineModelDto : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private ObservableCollection<QuerryLineStationParameterOutput> lineStationParameterDb = new ObservableCollection<QuerryLineStationParameterOutput>();
        public ObservableCollection<QuerryLineStationParameterOutput> LineStationParameterDb
        {
            get { return lineStationParameterDb; }
            set
            {
                lineStationParameterDb = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LineStationParameterDb"));
            }
        }

    }



    
    
}
