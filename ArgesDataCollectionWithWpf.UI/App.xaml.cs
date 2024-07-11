using Abp.Dependency;
using Abp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ArgesDataCollectionWithWpf.UI.UIWindows;

namespace ArgesDataCollectionWithWpf.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            using (var bootstrapper = AbpBootstrapper.Create<ArgesDataCollectionWithWpfUIModule>())
            {
                
                bootstrapper.Initialize();

                var mainwindow = IocManager.Instance.Resolve<RuningStatesWindow>();
                mainwindow.Show();

            }
        }
    }
}
