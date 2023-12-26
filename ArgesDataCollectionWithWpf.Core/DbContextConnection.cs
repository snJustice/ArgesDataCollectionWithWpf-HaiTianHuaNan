//zy


using Abp.Dependency;
using ArgesDataCollectionWithWpf.Application.Utils;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Core
{
    public class DbContextConnection:ISingletonDependency
    {


        private readonly IAppConfigureRead _appConfigRead;

       
        private readonly ILogger _logger;

        public ISqlSugarClient SugarClient { get; set; }
        public DbContextConnection(ILogger logger, IAppConfigureRead appConfigRead)
        {
            this._logger = logger;
            this._appConfigRead = appConfigRead;


            GenerateSugarClient();




        }


        public void GenerateSugarClient()
        {
            int slaveCount = Convert.ToInt32(this._appConfigRead.ReadKey("SlaveCount"));
            string tempMasterString = this._appConfigRead.ReadKey("masterConnectString" );
            List<SlaveConnectionConfig> slaves = new List<SlaveConnectionConfig>();

            for (int i = 1; i <= slaveCount; i++)
            {
                string tempSlaveString = this._appConfigRead.ReadKey("slaveConnectString" + i);
                slaves.Add(new SlaveConnectionConfig() { ConnectionString = tempSlaveString });
            }
             

            SugarClient = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = tempMasterString,//主库
                DbType = DbType.PostgreSQL,
                IsAutoCloseConnection = true,
                //从库
                SlaveConnectionConfigs = slaves
            });



            SugarClient.Aop.OnLogExecuting = (sql, p) =>
            {
                this._logger.LogInformation(sql + " connectString: " + SugarClient.Ado.Connection.ConnectionString);
            };

            try
            {
                SugarClient.Open();
            }
            catch (Exception ex)
            {

                this._logger.LogError("连接数据库失败:" + ex.Message);
            }
        }



        

    }
}
