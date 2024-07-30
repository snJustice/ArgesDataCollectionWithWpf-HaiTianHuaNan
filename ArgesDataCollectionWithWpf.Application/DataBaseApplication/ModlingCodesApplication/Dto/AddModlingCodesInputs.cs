using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Application.DataBaseApplication.ModlingCodesApplication.Dto
{
    public class AddModlingCodesInputs
    {
        
        public string Containerno { get; set; }//总成码
        public string Part1 { get; set; }//模板码1
        public string Part2 { get; set; }//模板码2
        public string Part3 { get; set; }//模板码3

        public string StationNumber { set; get; }

        public DateTime Time { get; set; }
    }

    public class AddModlingCodesInputsWebDto
    {

        public string Containerno { get; set; }//总成码
        public string Part1 { get; set; }//模板码1
        public string Part2 { get; set; }//模板码2
        public string Part3 { get; set; }//模板码3

        public string StationNumber { set; get; }

        //public DateTime Time { get; set; }
    }
}
