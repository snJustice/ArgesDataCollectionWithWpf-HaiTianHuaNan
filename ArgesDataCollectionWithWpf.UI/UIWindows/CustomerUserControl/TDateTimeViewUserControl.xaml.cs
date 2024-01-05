using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// TDateTimeViewUserControl.xaml 的交互逻辑
    /// </summary>
    /// 

    [ToolboxBitmap(typeof(TDateTimeViewUserControl), "DateTimePicker.bmp")]  
    public partial class TDateTimeViewUserControl : UserControl
    {
        public TDateTimeViewUserControl()
        {
            InitializeComponent();
        }
        public TDateTimeViewUserControl(string txt) :this()
        {
            this.formerDateTimeStr = txt;
        }


        #region 全局变量
 
         /// <summary>
         /// 从 DateTimePicker 传入的日期时间字符串
         /// </summary>
         private string formerDateTimeStr = string.Empty;
 
         // private string selectDate = string.Empty;    
 
         #endregion   
 
         #region 事件
 
         /// <summary>
         /// TDateTimeView 窗体登录事件
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
         private void UserControl_Loaded(object sender, RoutedEventArgs e)
         {
            //当前时间
            DateTime dt = DateTime.Now;
            textBlockhh.Text = dt.Hour.ToString().PadLeft(2, '0');
            textBlockmm.Text = dt.Minute.ToString().PadLeft(2, '0');
            textBlockss.Text = dt.Second.ToString().PadLeft(2, '0');
            this.expander.BringIntoView();
            txt_CurrentTime.Text = DateTime.Now.ToString();
            this.expander.IsExpanded = false;

        }
 
 
         /// <summary>
         /// 关闭按钮事件
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
         private void iBtnCloseView_Click(object sender, RoutedEventArgs e)
         {
             OnDateTimeContent(this.formerDateTimeStr);
         }
 
         /// <summary>
         /// 确定按钮事件
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
         private void btnOK_Click(object sender, RoutedEventArgs e)
         {
             DateTime? dt = new DateTime?();
 
             if (calDate.SelectedDate == null)
             {
                 dt = DateTime.Now.Date;
             }
             else
             {
                 dt = calDate.SelectedDate;
             }
 
             DateTime dtCal = Convert.ToDateTime(dt);           
 
             string timeStr = "00:00:00";
             timeStr = textBlockhh.Text + ":" + textBlockmm.Text + ":" + textBlockss.Text;          
 
             string dateStr;         
             dateStr = dtCal.ToString("yyyy/MM/dd");
 
             string dateTimeStr;
             dateTimeStr = dateStr + " " + timeStr;        
 
             string str1 = string.Empty; ;
             str1 = dateTimeStr;
             OnDateTimeContent(str1);
            this.txt_CurrentTime.Text = str1;

            this.expander.IsExpanded = false;



         }
 
         /// <summary>
         /// 当前按钮事件
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
         private void btnNow_Click(object sender, RoutedEventArgs e)
         {
             popChioce.IsOpen = false;//THourView 或 TMinSexView 所在pop 的关闭动作
 
             if (btnNow.Content == "零点")
             {                
                 textBlockhh.Text = "00";
                 textBlockmm.Text = "00";
                 textBlockss.Text = "00";
                 btnNow.Content = "当前";
                 btnNow.Background = System.Windows.Media.Brushes.LightBlue;
             }
             else
             {
                 DateTime dt = DateTime.Now;
                 textBlockhh.Text = dt.Hour.ToString().PadLeft(2, '0');
                 textBlockmm.Text = dt.Minute.ToString().PadLeft(2, '0');
                 textBlockss.Text = dt.Second.ToString().PadLeft(2, '0');                
                 btnNow.Content = "零点";
                 btnNow.Background = System.Windows.Media.Brushes.LightGreen;
             }
 
             
         }
 
         /// <summary>
         /// 小时点击事件
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
         private void textBlockhh_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
         {
 
             if (popChioce.IsOpen == true)
             {
                 popChioce.IsOpen = false;
             }
 
             THourViewUserControl hourView = new THourViewUserControl(textBlockhh.Text);// THourView 构造函数传递小时数据
             hourView.HourClick += (hourstr) => //THourView 点击所选小时后的 传递动作
             {
 
                 textBlockhh.Text = hourstr;
                 popChioce.IsOpen = false;//THourView 所在pop 的关闭动作
             };
 
             popChioce.Child = hourView;
             popChioce.IsOpen = true;
 
 
             
             //HG.ViewClose += (Flag) =>
             //{
             //    popChioce.IsOpen = false;
 
             //};
         }
 
         /// <summary>
         /// 分钟点击事件
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
         private void textBlockmm_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
         {
 
             if (popChioce.IsOpen == true)
             {
                 popChioce.IsOpen = false;
             }

            TMinSexViewUserControl minView = new TMinSexViewUserControl(textBlockmm.Text);//TMinSexView 构造函数传递 分钟数据
             minView.MinClick += (minStr) => //TMinSexView 中 点击选择的分钟数据的 传递动作
             {
 
                 textBlockmm.Text = minStr;
                 popChioce.IsOpen = false;//TMinSexView 所在的 pop 关闭动作
             };
 
             popChioce.Child = minView;
             popChioce.IsOpen = true;
         }
 
         /// <summary>
         /// 秒钟点击事件
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
         private void textBlockss_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
         {
             if (popChioce.IsOpen == true)
             {
                 popChioce.IsOpen = false;
             }

            //秒钟 跟分钟 都是60，所有秒钟共用 分钟的窗体即可
            TMinSexViewUserControl sexView = new TMinSexViewUserControl(textBlockss.Text);//TMinSexView 构造函数 传入秒钟数据
             sexView.textBlockTitle.Text = "秒    钟";//修改 TMinSexView 的标题名称为秒钟
             sexView.MinClick += (sexStr) => //TMinSexView 中 所选择确定的 秒钟数据 的传递动作
             {
                 textBlockss.Text = sexStr;
                 popChioce.IsOpen = false;//TMinSexView 所在的 pop 关闭动作
             };
 
             popChioce.Child = sexView;
             popChioce.IsOpen = true;
         }
 
 
         /// <summary>
         /// 解除calendar点击后 影响其他按钮首次点击无效的问题
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
         private void calDate_PreviewMouseUp(object sender, MouseButtonEventArgs e)
         {
             if (Mouse.Captured is CalendarItem)
             {
                 Mouse.Capture(null);
             }
         }
 
 
         #endregion
 
         #region Action交互
 
         /// <summary>
         /// 时间确定后的传递事件
         /// </summary>
         public Action<string> DateTimeOK;
 
         /// <summary>
         /// 时间确定后传递的时间内容
         /// </summary>
         /// <param name="dateTimeStr"></param>
         protected void OnDateTimeContent(string dateTimeStr)
         {
             if (DateTimeOK != null)
                 DateTimeOK(dateTimeStr);
         }
 
         #endregion

       

    }
}
