using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;

namespace ArgesDataCollectionWithWpf.UI.SingletonResource.OrderIndexManagerResource
{
    public class OrderIndexManagerSingleton:ISingletonDependency
    {

        public int ShowOrderIndex { get; set; }
        public int OrderIndex { get; set; }
        public int RunOrderIndex { get; set; }

        private void Init()
        {
            ShowOrderIndex = 0;
            OrderIndex = 0;
            RunOrderIndex = 0;
        }

        
    }
}
