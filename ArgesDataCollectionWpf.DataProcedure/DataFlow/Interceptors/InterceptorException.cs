//zy


using Abp.Dependency;
using Castle.DynamicProxy;
using EnterpriseFD.Dataflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWpf.DataProcedure.DataFlow.Interceptors
{
    public class InterceptorException: StandardInterceptor, ITransientDependency
    {
        private readonly IChannel<Exception> exceptionHandler;
        public InterceptorException(IChannel<Exception> exceptionHandler)
        {
            this.exceptionHandler = exceptionHandler;
        }

        protected override void PerformProceed(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                this.exceptionHandler.Channel(ex);
            }
        }
    }
}
