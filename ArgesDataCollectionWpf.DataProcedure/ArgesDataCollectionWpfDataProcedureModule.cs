//zy


using Abp.Modules;
using ArgesDataCollectionWpf.DataProcedure.DataFlow.TimerAcquire;
using Castle.MicroKernel.Registration;
using EnterpriseFD.Dataflow;
using EnterpriseFD.Dataflow.Starters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWpf.DataProcedure
{
    public class ArgesDataCollectionWpfDataProcedureModule: AbpModule
    {
        public override void Initialize()
        {
            /*
            IocManager.IocContainer.Register(Component.For<IStarter>()
                .ImplementedBy<TimerAcquisitor>()
                .LifestyleTransient());
            */
        }
    }
}
