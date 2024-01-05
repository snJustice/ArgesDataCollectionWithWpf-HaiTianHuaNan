using ArgesDataCollectionWithWpf.Core;
using ArgesDataCollectionWithWpf.DbModels.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Application.DataBaseApplication.LineStationTableApplication
{
    public class LineStationTableApplication : ArgesDataCollectionWithWpfApplicationBase, ILineStationTableApplication
    {
        private const string AddTableSql = @"CREATE TABLE @name (data0 VARCHAR(49)  NOT NULL PRIMARY KEY,data1  VARCHAR(255) NOT NULL,
data2  VARCHAR(255) ,
data3  VARCHAR(255) ,
data4  VARCHAR(255) ,
data5  VARCHAR(255) ,
data6  VARCHAR(255) ,
data7  VARCHAR(255) ,
data8  VARCHAR(255) ,
data9  VARCHAR(255) ,
data10 VARCHAR(255) ,
data11 VARCHAR(255) ,
data12 VARCHAR(255) ,
data13 VARCHAR(255) ,
data14 VARCHAR(255) ,
data15 VARCHAR(255) ,
data16 VARCHAR(255) ,
data17 VARCHAR(255) ,
data18 VARCHAR(255) ,
data19 VARCHAR(255) ,
data20 VARCHAR(255) ,
data21 VARCHAR(255) ,
data22 VARCHAR(255) ,
data23 VARCHAR(255) ,
data24 VARCHAR(255) ,
data25 VARCHAR(255) ,
data26 VARCHAR(255) ,
data27 VARCHAR(255) ,
data28 VARCHAR(255) ,
data29 VARCHAR(255) ,
data30 VARCHAR(255) ,
data31 VARCHAR(255) ,
data32 VARCHAR(255) ,
data33 VARCHAR(255) ,
data34 VARCHAR(255) ,
data35 VARCHAR(255) ,
data36 VARCHAR(255) ,
data37 VARCHAR(255) ,
data38 VARCHAR(255) ,
data39 VARCHAR(255) ,
data40 VARCHAR(255) ,
data41 VARCHAR(255) ,
data42 VARCHAR(255) ,
data43 VARCHAR(255) ,
data44 VARCHAR(255) ,
data45 VARCHAR(255) ,
data46 VARCHAR(255) ,
data47 VARCHAR(255) ,
data48 VARCHAR(255) ,
data49 VARCHAR(255) ,
data50 VARCHAR(255) ,
data51 VARCHAR(255) ,
data52 VARCHAR(255) ,
data53 VARCHAR(255) ,
data54 VARCHAR(255) ,
data55 VARCHAR(255) 
);";

        private const string existTableSql = "SELECT count(*) FROM sqlite_master WHERE type='table' AND name = @name;";

        public LineStationTableApplication(DbContextConnection sugarClinet, ILogger logger, IMapper objectMapper) : base(sugarClinet, logger, objectMapper)
        {
        }

        public int InsertTable(string tableName)
        {
            string tempTableName = tableName;
            var isExist = this._dbContextClinet.SugarClient.DbMaintenance.IsAnyTable(tempTableName);
            if (!isExist)
            {
                this._dbContextClinet.SugarClient.Ado.ExecuteCommand(AddTableSql.Replace("@name", tempTableName));
                
                return 1;
            }
            else
            {
                return 2;
            }


        }
    }
}
