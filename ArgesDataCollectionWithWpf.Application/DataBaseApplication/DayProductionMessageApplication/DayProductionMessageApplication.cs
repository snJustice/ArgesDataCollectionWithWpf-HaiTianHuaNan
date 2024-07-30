using ArgesDataCollectionWithWpf.Application.DataBaseApplication.DayProductionMessageApplication.Dto;
using ArgesDataCollectionWithWpf.Core;
using ArgesDataCollectionWithWpf.DbModels.DataBaseModels;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Application.DataBaseApplication.DayProductionMessageApplication
{
    public class DayProductionMessageApplication : ArgesDataCollectionWithWpfApplicationBase, IDayProductionMessageApplication
    {


        public DayProductionMessageApplication(DbContextConnection sugarClinet, ILogger logger, IMapper objectMapper) : base(sugarClinet, logger, objectMapper)
        {
        }

        public int InsertOrUpdateDayProductionMessage(AddOrInsertDayProductionMessageInput addOrInsertDayProductionMessageInput)
        {
            var obs = this._objectMapper.Map<DayProductionMessageModel>(addOrInsertDayProductionMessageInput);
            var x = _dbContextClinet.SugarClient.Storageable(obs).ToStorage();


            var insetCount = x.AsInsertable.ExecuteCommand();//不存在插入
            var updateCount = x.AsUpdateable.ExecuteCommand();//存在更新

            return insetCount + updateCount;
        }

        public QuerryDayProductionMessageOutput QuerryDayProductionMessageByDay(DateTime day)
        {

            

            var querryResult = _dbContextClinet.SugarClient.Queryable<DayProductionMessageModel>()
                .Where(s => SqlSugar.SqlFunc.Between(s.Time,Convert.ToDateTime( day.ToString("yyyy-MM-dd 00:00:00")),Convert.ToDateTime( day.ToString("yyyy-MM-dd 23:59:59"))))
                .OrderBy(it => it.ID);

            

            if (querryResult.Count() <=0)
            {
                return new QuerryDayProductionMessageOutput { 
                
                    ID = -1,
                    DayCount = 0,
                    Time = day,
                };
            }

            var querryDto = from m in querryResult.ToList() select _objectMapper.Map<QuerryDayProductionMessageOutput>(m);
            return querryDto.ToList().First();
        }

        public List<QuerryDayProductionMessageOutput> QuerryDayProductionMessageByMonth(DateTime Month)
        {



            var querryResult = _dbContextClinet.SugarClient.Queryable<DayProductionMessageModel>()
                .Where(s => SqlSugar.SqlFunc.Between(s.Time, Month.ToString("yyyy-MM-00 00:00:00"), Month.ToString("yyyy-MM-31 23:59:59"))).OrderBy(it => it.ID);
            var querryDto = from m in querryResult.ToList() select _objectMapper.Map<QuerryDayProductionMessageOutput>(m);

            
            return querryDto.ToList();
        }
    }
}
