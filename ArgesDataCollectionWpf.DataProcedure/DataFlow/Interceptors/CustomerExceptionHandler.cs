using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using ArgesDataCollectionWpf.DataProcedure.Utils;
using EnterpriseFD.Dataflow;
using Microsoft.Extensions.Logging;

namespace ArgesDataCollectionWpf.DataProcedure.DataFlow.Interceptors
{
    public class CustomerExceptionHandler : IChannel<LogMessage>,ITransientDependency
    {
        private readonly ILogger _log;
        private readonly IWriteLogForUserControl _controlLog;

        public CustomerExceptionHandler(ILogger log, IWriteLogForUserControl controlLog)
        {
            this._log = log;
            this._controlLog = controlLog;
        }
        public Task Channel(LogMessage data)
        {
            return Task.Run(() => {

                string message = $"{DateTime.Now.ToString ("yyyy-MM-dd,HH:mm:ss,ff")}---{data.Message}";
                this._controlLog.WriteLog(message);
                switch (data.Level)
                {
                    case LogLevel.Trace:
                        this._log.LogTrace(data.Message);
                        break;
                    case LogLevel.Debug:
                        break;
                    case LogLevel.Information:
                        this._log.LogInformation(data.Message);
                        break;
                    case LogLevel.Warning:
                        break;
                    case LogLevel.Error:
                        this._log.LogError(data.Message);
                        break;
                    case LogLevel.Critical:
                        break;
                    case LogLevel.None:
                        break;
                    default:
                        break;
                }
            });
            
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }


    public class LogMessage
    {
        public Microsoft.Extensions.Logging.LogLevel Level { get; set; }
        public string Message { get; set; }
    }
}
