using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// DateAndTimerPickerUserControl.xaml 的交互逻辑
    /// </summary>
    /// 
    [ToolboxBitmap(typeof(DateAndTimerPickerUserControl), "DateTimePicker.bmp")]
    public partial class DateAndTimerPickerUserControl : UserControl
    {


        public DateTime DateTime { get; set; }

        public DateAndTimerPickerUserControl()
        {
            InitializeComponent();
        }

        private void iconButton1_Click(object sender, RoutedEventArgs e)
        {
            if (popChioce.IsOpen == true)
             {
                 popChioce.IsOpen = false;
             }
 
             TDateTimeViewUserControl dtView = new TDateTimeViewUserControl(textBlock1.Text);// TDateTimeView  构造函数传入日期时间
             dtView.DateTimeOK += (dateTimeStr) => //TDateTimeView 日期时间确定事件
             {
 
                 textBlock1.Text = dateTimeStr;
                 DateTime = Convert.ToDateTime(dateTimeStr);
                 popChioce.IsOpen = false;//TDateTimeView 所在pop  关闭
 
             };
 
             popChioce.Child = dtView;
             popChioce.IsOpen = true;

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DateTime dt = DateTime.Now;
            textBlock1.Text = dt.ToString("yyyy/MM/dd HH:mm:ss");//"yyyyMMddHHmmss"
            DateTime = dt;            
        }
    }
}
