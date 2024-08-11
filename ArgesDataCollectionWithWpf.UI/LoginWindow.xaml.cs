using Abp.Dependency;
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
using System.Windows.Shapes;

namespace ArgesDataCollectionWithWpf.UI
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window, ITransientDependency
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        

        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            if (this.txt_password.Password == "0417" && this.txt_user.Text == "admin")
            {
                this.DialogResult = true;
            }
            else
            {
                //this.DialogResult = false;
            }
        }
    }
}
