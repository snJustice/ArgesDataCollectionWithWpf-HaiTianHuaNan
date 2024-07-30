﻿using ArgesDataCollectionWithWpf.DbModels.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.DbModels.DataBaseModels
{
    [MappingToDatabase]
    [SugarTable("MonthProductionMessage")]
    public class MonthProductionMessageModel
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }

        public DateTime Time { get; set; }

        public int MonthCount { set; get; }
    }
}
