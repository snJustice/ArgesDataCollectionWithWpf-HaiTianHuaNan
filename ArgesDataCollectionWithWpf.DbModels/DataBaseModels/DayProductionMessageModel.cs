using ArgesDataCollectionWithWpf.DbModels.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.DbModels.DataBaseModels
{

    [MappingToDatabase]
    [SugarTable("DayProductionMessage")]
    public class DayProductionMessageModel
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }

        public DateTime Time { get; set; }

        public int DayCount { set; get; }


    }
}
