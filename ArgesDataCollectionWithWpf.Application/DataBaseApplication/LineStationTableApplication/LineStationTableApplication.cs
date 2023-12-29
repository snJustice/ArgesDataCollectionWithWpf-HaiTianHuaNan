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
        private const string AddTableSql = @"CREATE TABLE @name (data0 VARCHAR(49)  NOT NULL PRIMARY KEY,data1  VARCHAR(255) NOT NULL,data2  VARCHAR(255) NOT NULL,data3  VARCHAR(255) NOT NULL,data4  VARCHAR(255) NOT NULL,data5  VARCHAR(255) NOT NULL,
data6  VARCHAR(255) NOT NULL,
data7  VARCHAR(255) NOT NULL,
data8  VARCHAR(255) NOT NULL,
data9  VARCHAR(255) NOT NULL,
data10 VARCHAR(255) NOT NULL,
data11 VARCHAR(255) NOT NULL,
data12 VARCHAR(255) NOT NULL,
data13 VARCHAR(255) NOT NULL,
data14 VARCHAR(255) NOT NULL,
data15 VARCHAR(255) NOT NULL,
data16 VARCHAR(255) NOT NULL,
data17 VARCHAR(255) NOT NULL,
data18 VARCHAR(255) NOT NULL,
data19 VARCHAR(255) NOT NULL,
data20 VARCHAR(255) NOT NULL,
data21 VARCHAR(255) NOT NULL,
data22 VARCHAR(255) NOT NULL,
data23 VARCHAR(255) NOT NULL,
data24 VARCHAR(255) NOT NULL,
data25 VARCHAR(255) NOT NULL,
data26 VARCHAR(255) NOT NULL,
data27 VARCHAR(255) NOT NULL,
data28 VARCHAR(255) NOT NULL,
data29 VARCHAR(255) NOT NULL,
data30 VARCHAR(255) NOT NULL,
data31 VARCHAR(255) NOT NULL,
data32 VARCHAR(255) NOT NULL,
data33 VARCHAR(255) NOT NULL,
data34 VARCHAR(255) NOT NULL,
data35 VARCHAR(255) NOT NULL,
data36 VARCHAR(255) NOT NULL,
data37 VARCHAR(255) NOT NULL,
data38 VARCHAR(255) NOT NULL,
data39 VARCHAR(255) NOT NULL,
data40 VARCHAR(255) NOT NULL,
data41 VARCHAR(255) NOT NULL,
data42 VARCHAR(255) NOT NULL,
data43 VARCHAR(255) NOT NULL,
data44 VARCHAR(255) NOT NULL,
data45 VARCHAR(255) NOT NULL,
data46 VARCHAR(255) NOT NULL,
data47 VARCHAR(255) NOT NULL,
data48 VARCHAR(255) NOT NULL,
data49 VARCHAR(255) NOT NULL,
data50 VARCHAR(255) NOT NULL,
data51 VARCHAR(255) NOT NULL,
data52 VARCHAR(255) NOT NULL,
data53 VARCHAR(255) NOT NULL,
data54 VARCHAR(255) NOT NULL,
data55 VARCHAR(255) NOT NULL
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
