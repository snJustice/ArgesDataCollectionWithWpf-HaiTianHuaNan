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
    /// DataDescriptionAndValueAndUintUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class DataDescriptionAndValueAndUintUserControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string l_Description="";
        public  string Lbl_Description { 
            get { return l_Description; }
            set { 
                l_Description = value;
                this.lbl_Description.Content = value;
                 } 
        }


        private string t_Value = "";
        public string Txt_Value
        {
            get { return t_Value; }
            set
            {
                t_Value = value;
                this.txt_Value.Text = value;
            }
        }
       
        
        private string t_Uint = "";
        public string Txt_Uint
        {
            get { return t_Uint; }
            set
            {
                t_Uint = value;
                this.txt_Uint.Text = value;
            }
        }
        public DataDescriptionAndValueAndUintUserControl()
        {
            InitializeComponent();
        }

        
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
