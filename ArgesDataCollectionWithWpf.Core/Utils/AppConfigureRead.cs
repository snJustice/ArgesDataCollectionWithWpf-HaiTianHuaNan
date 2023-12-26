//zy


using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Application.Utils
{
    public class AppConfigureRead : IAppConfigureRead
    {
        const string NotFound = "Not Found";
        protected readonly ILogger _logger;

        public AppConfigureRead(ILogger logger)
        {
            this._logger = logger;
        }

        

        public string ReadKey(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? NotFound;
                return result;
                
            }
            catch (ConfigurationErrorsException ex)
            {
                this._logger.LogError(ex.Message); 
                return NotFound;
            }
        }
    }
}
