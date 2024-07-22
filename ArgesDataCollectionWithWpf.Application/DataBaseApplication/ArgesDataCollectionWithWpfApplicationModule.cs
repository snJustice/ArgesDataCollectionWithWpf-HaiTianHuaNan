//zy



using Abp.Modules;
using ArgesDataCollectionWithWpf.Core;
using AutoMapper;
using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace ArgesDataCollectionWithWpf.Application.DataBaseApplication
{

    //mapper的配置位置
    [DependsOn(typeof(ArgesDataCollectionWithWpfCoreModule))]
    public class ArgesDataCollectionWithWpfApplicationModule : AbpModule
    {

        public override void PreInitialize()
        {

            
            

        }
        public override void Initialize()
        {


            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }


    public class ArgesDataCollectionApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            
        }

        public override void Initialize()
        {
            
        }
    }
}
