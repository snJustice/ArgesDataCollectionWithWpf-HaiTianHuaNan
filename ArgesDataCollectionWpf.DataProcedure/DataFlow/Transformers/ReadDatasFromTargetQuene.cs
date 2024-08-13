using ArgesDataCollectionWpf.DataProcedure.Entities;
using EnterpriseFD.Dataflow.Transformers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Abp.Dependency;
using ArgesDataCollectionWithWpf.Core.Utils;

using System.Threading.Tasks.Dataflow;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.ModlingCodesApplication.Dto;
using ArgesDataCollectionWpf.DataProcedure.Utils.Quene;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.ModlingCodesApplication;
using Microsoft.Extensions.Logging;

namespace ArgesDataCollectionWpf.DataProcedure.DataFlow.Transformers
{
    public class ReadDatasFromTargetQuene : AbstractRevisor<PlcAddressAndDatabaseAndCommunicationCombineEntity>
    {
        
        private readonly CustomerQueneForCodesFromMes _customerQueneForCodesFromMes;
        private readonly int _stattionNumber;
        private readonly string _stationName;
        private readonly IModlingCodesApplication _modlingCodesApplication;
        private readonly ILogger _logger;


        public ReadDatasFromTargetQuene(CustomerQueneForCodesFromMes customerQueneForCodesFromMes , int stattionNumber,string stationNumber)
        {
            this._logger = IocManager.Instance.Resolve<ILogger>();
            this._modlingCodesApplication = IocManager.Instance.Resolve<IModlingCodesApplication>();
            this._customerQueneForCodesFromMes = customerQueneForCodesFromMes;
            this._stattionNumber = stattionNumber;
            this._stationName = stationNumber;

        }
        protected override PlcAddressAndDatabaseAndCommunicationCombineEntity DoTransform(PlcAddressAndDatabaseAndCommunicationCombineEntity data)
        {
            
            QuerryModlingCodesOutput xxx = null;
            DateTime start = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")); ;
            DateTime end = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59")); ;
            xxx = this._modlingCodesApplication.QuerryModlingCodesByTimeEarlyAndNotDone(start, end, this._stationName);

            var runeditem = new QuerryModlingCodesOutputWithEndTime
            {

                Containerno = Guid.NewGuid().ToString(),
                Part1 = "123131",
                Part2 = "1231231",
                Part3 = "2343242",
                StationNumber = this._stationName,
                EndTime = DateTime.Now,
                Time = DateTime.Now

            };

            //如果数据库汇总查询到了
            if (xxx == null)
            {
                this._logger.LogInformation("收到触发信号，可是数据库查询containerno失败，");
                data.ModlingCodeFromMes = runeditem;
                
            }

            else
            {
                this._logger.LogInformation("收到触发信号，数据库查询成功，");
                var result  = this._modlingCodesApplication.UpdateModlingCodesIsDoneByContainerNo(new UpdateModlingCodesInput
                {

                    Containerno = xxx.Containerno,
                    IsDone = 1
                }); ;
                

                if (result>0)
                {
                    this._logger.LogInformation("isdone更新成功，");
                    runeditem = new QuerryModlingCodesOutputWithEndTime
                    {

                        Containerno = xxx.Containerno,
                        Part1 = xxx.Part1,
                        Part2 = xxx.Part2,
                        Part3 = xxx.Part3,
                        StationNumber = xxx.StationNumber,
                        EndTime = DateTime.Now,
                        Time = xxx.Time

                    };
                }
                
                data.ModlingCodeFromMes = runeditem;
                
            }


            return data;
            
            
        }
    }
}
