//zy


using Castle.MicroKernel;
using EnterpriseFD.Dataflow.Handlers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Abp.Dependency;

namespace ArgesDataCollectionWpf.DataProcedure.DataFlow.Handlers
{
    public class ExceptionHandler: AbstractHandler<Exception>,ITransientDependency
    {
        private readonly ILoggerFactory loggerFactory;
        public ExceptionHandler(ILoggerFactory loggerFactory) : base()
        {
            this.loggerFactory = loggerFactory;
        }

        protected override Task DoHandle(Exception data)
        {
            return Task.Run(() =>
            {
                ILogger logger = this.loggerFactory.CreateLogger<Exception>();
                
                logger.LogError(data.Message, data);
            });
        }
    }
}
