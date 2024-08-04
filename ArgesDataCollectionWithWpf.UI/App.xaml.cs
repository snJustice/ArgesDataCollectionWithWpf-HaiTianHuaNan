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
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Abp.AspNetCore;
using ArgesDataCollectionWithWpf.Core;
using Abp.AspNetCore.Dependency;
using Abp.AspNetCore.Mvc.Antiforgery;
using Abp.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace ArgesDataCollectionWithWpf.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application, IAsyncDisposable
    {
        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    using (var bootstrapper = AbpBootstrapper.Create<ArgesDataCollectionWithWpfUIModule>())
        //    {

        //        bootstrapper.Initialize();

        //        //var mainwindow = IocManager.Instance.Resolve<RuningStatesWindow>();
        //        var mainwindow = IocManager.Instance.Resolve<MainWindow>();



        //        mainwindow.Show();

        //    }
        //}


        public WebApplication? WebApplication { get; private set; }

        public async ValueTask DisposeAsync()
        {
            if (WebApplication is not null)
            {
                await WebApplication.DisposeAsync();
            }
            GC.SuppressFinalize(this);
        }

        private async void ApplicationStartup2(object sender, StartupEventArgs e)
        {
            var builder = CreateHostBuilder(Environment.GetCommandLineArgs()).UseCastleWindsor(IocManager.Instance.IocContainer);
            builder.Build().Run();
        }

        internal static IHostBuilder CreateHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    
                    webBuilder.UseStartup<Startup>();
                })
                ;

        private async void ApplicationStartup(object sender, StartupEventArgs e)
        {



            
            // 这里是创建 ASP.NET 版通用主机的代码
            var builder = WebApplication.CreateBuilder(Environment.GetCommandLineArgs());

            
            builder.Configuration
            //.SetBasePath(Directory.GetCurrentDirectory())
            //AppDomain.CurrentDomain.BaseDirectory是程序集基目录，所以appsettings.json,需要复制一份放在程序集目录下，
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true }).Build();


            


            builder.WebHost.UseUrls($"{builder.Configuration["applicationUrl"]}");



            
            


            // 注册主窗口和其他服务
            builder.Host.UseCastleWindsor(IocManager.Instance.IocContainer);
            builder.Services.AddHttpLogging(o => { });
            builder.Services.AddControllers();
            builder.Services.AddMvc();

            


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<MainWindow>();

     
            
            //builder.Services.AddSingleton(this);
            
            builder.Services.AddAbpWithoutCreatingServiceProvider<ArgesDataCollectionWithWpfUIModule>(optionis => { 
            
               
            
            });



            
            var app = builder.Build();


            app.UseHttpLogging();
            app.UseAbp(options => { options.UseAbpRequestLocalization = false; });
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();

            //app.UseAuthorization();




            
            app.MapControllers();

            WebApplication = app;
            // 处理退出事件，退出 App 时关闭 ASP.NET Core
            Exit += async (s, e) => await WebApplication.StopAsync();
            // 显示主窗口
            
                
            MainWindow = app.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();
            await app.RunAsync().ConfigureAwait(false);
        }

    }
}
