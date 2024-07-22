using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.UI.SingletonResource.ModlingMachineDeviceResource
{
    public class ModlingMachineTypeAndPullRodCombine
    {
        public string ModlingMachineTypeName { get; set; }

        public int ModlingMachineTypeSendToPlcID { get; set; }

        public List<PollRodType> PollRods { get; set; }
    }

    public class PollRodType
    {
        public string PollRodDescription { get; set; }
        public int PollRodSendToPlcID { get; set; }
    }
}
