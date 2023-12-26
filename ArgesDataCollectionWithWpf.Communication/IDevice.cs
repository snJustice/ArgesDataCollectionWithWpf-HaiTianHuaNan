using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Communication
{
    public interface IDevice
    {
        bool Open();
        Task<bool> OpenAsync();


        bool Close();
        Task<bool> CloseAsync();
    }
}
