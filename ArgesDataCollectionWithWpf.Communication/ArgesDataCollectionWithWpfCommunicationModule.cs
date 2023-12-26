//zy


using Abp.Modules;
using Abp.Reflection.Extensions;
using ArgesDataCollectionWithWpf.Communication.S7Communication;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Communication
{
    public class ArgesDataCollectionWithWpfCommunicationModule: AbpModule
    {
        public override void PreInitialize()
        {
            
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
