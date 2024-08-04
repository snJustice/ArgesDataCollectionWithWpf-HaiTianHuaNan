//zy


using Abp.Modules;
using Castle.MicroKernel.Registration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Core
{
    public class ArgesDataCollectionWithWpfCoreModule:AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }


        public override void PreInitialize()
        {
            
            base.PreInitialize();
            IocManager.IocContainer.Register(Component.For<Microsoft.Extensions.Logging.ILogger>().UsingFactoryMethod(kernel => {


                return new Microsoft.Extensions.Logging.LoggerFactory()
                .AddSerilog(new LoggerConfiguration().WriteTo.File("log/log-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 365).WriteTo.Console().CreateLogger())
                .CreateLogger("log");

            }).LifestyleSingleton());



            IocManager.IocContainer.Register(Component.For<Microsoft.Extensions.Logging.ILoggerFactory>().UsingFactoryMethod(kernel => {
                return new Microsoft.Extensions.Logging.LoggerFactory()
                .AddSerilog(new LoggerConfiguration().WriteTo.File("log/log-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit:365).WriteTo.Console().CreateLogger());
            }).LifestyleSingleton());



            


           
        }
    }
}
