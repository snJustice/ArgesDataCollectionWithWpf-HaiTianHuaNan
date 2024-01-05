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

namespace ArgesDataCollectionWithWpf.UI.UIWindows.CustomerUserControl
{
    /// <summary>
    /// IconButtonUserControl.xaml 的交互逻辑
    /// </summary>
    /// 
    
    public partial class IconButtonUserControl : UserControl
    {
        public IconButtonUserControl()
        {
            InitializeComponent();

            this.button.Click += delegate
            {
                 RoutedEventArgs newEvent = new RoutedEventArgs(IconButtonUserControl.ClickEvent, this);
                 this.RaiseEvent(newEvent);
            };

        }


        #region 图标
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon",
            typeof(string),
            typeof(IconButtonUserControl),
            new PropertyMetadata(string.Empty, OnIconChanged));
        public string Icon
        {
            set { SetValue(IconProperty, value); }
            get { return (string)GetValue(IconProperty); }
        }
        private static void OnIconChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            IconButtonUserControl btn = obj as IconButtonUserControl;
            if (btn == null)
            {
                return;
            }
            btn.icon.Source = new BitmapImage(new Uri((string)args.NewValue, UriKind.Relative));
        }
        #endregion

        #region 命令

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command",
            typeof(ICommand),
            typeof(IconButtonUserControl),
            new PropertyMetadata(null, OnSelectCommandChanged));
        public ICommand Command
        {
            set { SetValue(CommandProperty, value); }
            get { return (ICommand)GetValue(CommandProperty); }
        }
        private static void OnSelectCommandChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            IconButtonUserControl btn = obj as IconButtonUserControl;
            if (btn == null)
            {
                return;
            }
            btn.button.Command = (ICommand)args.NewValue;
        }

        #endregion

        #region 点击事件
        public static readonly RoutedEvent ClickEvent =
          EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble,
          typeof(RoutedEventHandler), typeof(IconButtonUserControl));

        public event RoutedEventHandler Click
        {
            add
            {
                base.AddHandler(ClickEvent, value);
            }
            remove
            {
                base.RemoveHandler(ClickEvent, value);
            }

        }
        #endregion
    }
}
