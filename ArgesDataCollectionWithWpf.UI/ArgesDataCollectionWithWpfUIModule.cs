//zy


using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication;
using ArgesDataCollectionWithWpf.Communication;
using ArgesDataCollectionWithWpf.Communication.S7Communication;
using ArgesDataCollectionWithWpf.Core;
using ArgesDataCollectionWithWpf.DbModels;
using ArgesDataCollectionWpf.DataProcedure;
using AutoMapper;
using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace ArgesDataCollectionWithWpf.UI
{

    [DependsOn(typeof(ArgesDataCollectionWithWpfCoreModule))
        
        , DependsOn(typeof(ArgesDataCollectionWithWpfDbModelsModbule))
        , DependsOn(typeof(ArgesDataCollectionWithWpfCommunicationModule))
        ,DependsOn(typeof(ArgesDataCollectionWpfDataProcedureModule))
        , DependsOn(typeof(ArgesDataCollectionWithWpfApplicationModule ))
        , DependsOn(typeof(AbpAspNetCoreModule))]
        
    public class ArgesDataCollectionWithWpfUIModule:AbpModule
    {

        public override void PreInitialize()
        {
            var configuration = new MapperConfiguration(cfg =>
            {

                cfg.AddMaps(typeof(ArgesDataCollectionWithWpfApplicationMapperProfile).GetAssembly());
                cfg.AddMaps(typeof(ArgesSimensS7CommunicationDataAddressMapperProfile).GetAssembly());
                cfg.AddMaps(typeof(ArgesDataCollectionWithWpfCommunicationMapperProfile).GetAssembly());
              

            });

            IocManager.IocContainer.Register(
               Component.For<IMapper>().UsingFactoryMethod(kernel =>
               {


                   //return new Mapper(kernel.Resolve<IConfigurationProvider>(), kernel.Resolve);
                   return new Mapper(configuration, kernel.Resolve);
               }).LifestyleSingleton());


            IMapper aaa = IocManager.Resolve<IMapper>();


            Configuration.Modules.AbpAspNetCore()
                 .CreateControllersForAppServices(
                     typeof(ArgesDataCollectionApplicationModule).GetAssembly()
                 );
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }


        

    }
}
