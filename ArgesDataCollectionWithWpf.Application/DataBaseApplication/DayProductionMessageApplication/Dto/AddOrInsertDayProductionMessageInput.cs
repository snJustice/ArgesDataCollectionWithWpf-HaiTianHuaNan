using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Application.DataBaseApplication.DayProductionMessageApplication.Dto
{
    public class AddOrInsertDayProductionMessageInput
    {



        public int ID { get; set; }
        public DateTime Time { get; set; }

        public int DayCount { set; get; }
    }
}
