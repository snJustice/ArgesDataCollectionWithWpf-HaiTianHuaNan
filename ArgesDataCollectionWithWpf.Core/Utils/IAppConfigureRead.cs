using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Application.Utils
{
    public interface IAppConfigureRead: IApplicationService
    {
        string ReadKey(string key);

        bool WriteKey(string key,string value);
    }
}
