using Abp.Dependency;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.CommunicationDetailsAndInstanceApplication;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application.Dto;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.LineStationParameterApplication;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.LineStationParameterApplication.Dto;
using ArgesDataCollectionWithWpf.Application.Utils;
using ArgesDataCollectionWithWpf.UI.Utils.UiDataUtils;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;



/*
 * 
 * 1.根据是否有触发信号来判断是否是需要保存
 *  有触发信号，数据进行保存
 *  
 *  没有触发信号，可能就只是一些参数的设置
 * 
 * 
 * 
 * 
 * 
 */


namespace ArgesDataCollectionWithWpf.UI.UIWindows
{
    /// <summary>
    /// DataAddressSettingsWindow.xaml 的交互逻辑
    /// </summary>
    /// 

    
    public partial class DataAddressSettingsWindow : Window, ITransientDependency
    {
        //数据库操作
        private readonly IConnect_Device_With_PC_Function_Data_Application _IConnect_Device_With_PC_Function_Data_Application;
        private readonly ILineStationParameterApplication _iLineStationParameterApplication;
        private readonly ICommunicationDetailsAndInstanceApplication _iCommunicationDetailsAndInstanceApplication;
        private readonly IMapper _imapper;

        UserDataAddressModelDto UserDataAddressParameterDb = new UserDataAddressModelDto();//数据绑定

        private Dictionary<int, DataStatus> _userDataOperation = new Dictionary<int, DataStatus>();//数据修改还是操作

        public DataAddressSettingsWindow(IConnect_Device_With_PC_Function_Data_Application iConnect_Device_With_PC_Function_Data_Application, IMapper imapper, ILineStationParameterApplication iLineStationParameterApplication, ICommunicationDetailsAndInstanceApplication iCommunicationDetailsAndInstanceApplication)
        {

            InitializeComponent();
            this._IConnect_Device_With_PC_Function_Data_Application = iConnect_Device_With_PC_Function_Data_Application;
            this._imapper = imapper;

            this._iCommunicationDetailsAndInstanceApplication = iCommunicationDetailsAndInstanceApplication;
            this._iLineStationParameterApplication = iLineStationParameterApplication;

            this.grid_DataAddressSettingShow.DataContext = UserDataAddressParameterDb;

            //放入线体id集合
            var lines = this._iLineStationParameterApplication.QuerryAllLineStationParameters();
            var lineIds = (from m in lines select m.ID).ToList();
            this.combox_Lines.ItemsSource = lineIds;


            //放入通信id集合
            var communications = this._iCommunicationDetailsAndInstanceApplication.QuerryCommunicationDetailsAndInstanceAll();
            var communicationsIds = (from m in communications select m.ID).ToList();
            this.datagridCobxCommunicationID.ItemsSource = communicationsIds;
            
        }


        #region 界面的操作
        private void btn_AddOne_Click(object sender, RoutedEventArgs e)
        {
            //先获得新的id
            var currentDataInDb =  this._IConnect_Device_With_PC_Function_Data_Application.QuerryConnect_Device_With_PC_Function_DatasAll();
            currentDataInDb.AddRange(this.UserDataAddressParameterDb.DataAddressParameterDb);
            int newIndex = currentDataInDb.GetUniqueIndex("ID");

            this.UserDataAddressParameterDb.DataAddressParameterDb.Add(new QuerryConnect_Device_With_PC_Function_DataOutput
            {

                ID = newIndex,
                DataAddressDescription = "",
                DataLength = 1,
                DataSaveIndex = 0,
                DBDescription = "",
                Func = DbModels.Enums.EnumAddressFunction.ReadAndNeedSaveData,
                LineID = 1,
                ReadOrWrite = DbModels.Enums.EnumReadOrWrite.Read,
                VarType = S7.Net.VarType.Timer,
            });

            AddOneUserOperation(newIndex, DataStatus.Add);

        }

        private void btn_DeleteOne_Click(object sender, RoutedEventArgs e)
        {
            var deleteItem = (QuerryConnect_Device_With_PC_Function_DataOutput)this.grid_DataAddressSettingShow.SelectedItem;
            //删除选择的行数据
            var removeResult = this.UserDataAddressParameterDb.DataAddressParameterDb.Remove((QuerryConnect_Device_With_PC_Function_DataOutput)this.grid_DataAddressSettingShow.SelectedItem);

            AddOneUserOperation(deleteItem.ID, DataStatus.Delete);
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            UpdateDatabseDataByUi();
            MessageBox.Show("保存成功");
            this._userDataOperation.Clear();
        }

        private void grid_LineSettingShow_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var updateItem = (QuerryConnect_Device_With_PC_Function_DataOutput)this.grid_DataAddressSettingShow.SelectedItem;

            AddOneUserOperation(updateItem.ID, DataStatus.Update);
        }

        


        #endregion






        //更新数据库
        public void UpdateDatabseDataByUi()
        {
            //根据_userDataOperation进行数据的具体做法

            foreach (var item in this._userDataOperation)
            {

                var addModel = (from m in UserDataAddressParameterDb.DataAddressParameterDb where m.ID == item.Key select m).FirstOrDefault();
                if (addModel != null)
                {
                    switch (item.Value)
                    {

                        case DataStatus.Add:

                            this._IConnect_Device_With_PC_Function_Data_Application.InsertConnect_Device_With_PC_Function_Data( this._imapper.Map<AddConnect_Device_With_PC_Function_DataInput>(addModel) );

                            break;
                        case DataStatus.Update:
                            this._IConnect_Device_With_PC_Function_Data_Application.ModifyConnect_Device_With_PC_Function_DataById(this._imapper.Map<ModifyConnect_Device_With_PC_Function_DataByIdInput>(addModel));
                            break;
                        case DataStatus.Delete:

                            this._IConnect_Device_With_PC_Function_Data_Application.DeleteConnect_Device_With_PC_Function_DataById(new DeleteConnect_Device_With_PC_Function_DataByIdInput { ID = addModel.ID });
                            break;

                        default:
                            break;

                    }
                }

            }
        }

        //记录数据增删改查的操作
        private void AddOneUserOperation(int index, DataStatus dataStatus)
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

        

        //线体选择进行了修改
        private void combox_Lines_SectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine(this.combox_Lines.SelectedValue);

            var selectedLineDatas = this._IConnect_Device_With_PC_Function_Data_Application.QuerryConnect_Device_With_PC_Function_DataByStationNumber(Convert.ToInt32(this.combox_Lines.SelectedValue));

            this.UserDataAddressParameterDb.DataAddressParameterDb.Clear();
            foreach (var item in selectedLineDatas)
            {
                this.UserDataAddressParameterDb.DataAddressParameterDb.Add(item);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }



    //datagrid数据绑定的结构
    public class UserDataAddressModelDto : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private ObservableCollection<QuerryConnect_Device_With_PC_Function_DataOutput> dataAddressParameterDb = new ObservableCollection<QuerryConnect_Device_With_PC_Function_DataOutput>();
        public ObservableCollection<QuerryConnect_Device_With_PC_Function_DataOutput> DataAddressParameterDb
        {
            get { return dataAddressParameterDb; }
            set
            {
                dataAddressParameterDb = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataAddressParameterDb"));
            }
        }

    }

    public class EnumBindingSourceExtension : MarkupExtension
    {
        private Type _enumType;

        public Type EnumType
        {
            get { return _enumType; }
            set
            {
                if (value != _enumType)
                {
                    if (null != value)
                    {
                        var enumType = Nullable.GetUnderlyingType(value) ?? value;
                        if (!enumType.IsEnum)
                        {
                            throw new ArgumentException("Type must bu for an Enum");
                        }

                    }

                    _enumType = value;
                }
            }
        }

        public EnumBindingSourceExtension()
        {

        }

        public EnumBindingSourceExtension(Type enumType)
        {
            EnumType = enumType;
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (null == _enumType)
            {
                throw new InvalidOperationException("The EnumTYpe must be specified.");
            }

            var actualEnumType = Nullable.GetUnderlyingType(_enumType) ?? _enumType;
            var enumValues = Enum.GetValues(actualEnumType);

            if (actualEnumType == _enumType)
            {
                return enumValues;
            }

            var tempArray = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
            enumValues.CopyTo(tempArray, 1);

            return tempArray;
        }
    }



    

}
