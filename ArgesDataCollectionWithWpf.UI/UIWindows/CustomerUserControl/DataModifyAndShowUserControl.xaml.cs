using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// DataModifyAndShowUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class DataModifyAndShowUserControl : UserControl
    {
        LabelDataSourceBinding labelData = new LabelDataSourceBinding();
        TextDataSourceBinding plcData = new TextDataSourceBinding();

        public DataModifyAndShowUserControl(string inputLabelData)
        {
            InitializeComponent();
            this.lbl_Index.DataContext = labelData;
            this.labelData.LabelData = inputLabelData;
            


            this.txt_plcValue.DataContext = plcData;
        }


        public void SetPlcValue(string value)
        {
            plcData.TextData = value;
        }
    }





    class LabelDataSourceBinding : INotifyPropertyChanged
    {
        //必须实现
        public event PropertyChangedEventHandler PropertyChanged;
        private string _labelData;//私有  
        public string LabelData
        {
            //获取值时将私有字段传出；  
            get { return _labelData; }
            set
            {
                //赋值时将值传给私有字段  
                _labelData = value;
                //一旦执行了赋值操作说明其值被修改了，则立马通过INotifyPropertyChanged接口告诉UI(IntValue)被修改了  
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("labelData"));
            }
        }
    }

    class TextDataSourceBinding : INotifyPropertyChanged
    {
        //必须实现
        public event PropertyChangedEventHandler PropertyChanged;
        private string _textData;//私有  
        public string TextData
        {
            //获取值时将私有字段传出；  
            get { return _textData; }
            set
            {
                //赋值时将值传给私有字段  
                _textData = value;
                //一旦执行了赋值操作说明其值被修改了，则立马通过INotifyPropertyChanged接口告诉UI(IntValue)被修改了  
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("textData"));
            }
        }
    }

}
