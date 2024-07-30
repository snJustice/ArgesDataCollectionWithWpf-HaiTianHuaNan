using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Application.DataBaseApplication.MonthProductionMessageApplication.Dto
{
    public class AddOrInsertMonthProductionMessageInput
    {

        public int ID { get; set; }

        public DateTime Time { get; set; }

        public int MonthCount { set; get; }
    }
}
