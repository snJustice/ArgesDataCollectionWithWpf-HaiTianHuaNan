using ArgesDataCollectionWithWpf.Application.DataBaseApplication.MonthProductionMessageApplication.Dto;
using ArgesDataCollectionWithWpf.Core;
using ArgesDataCollectionWithWpf.DbModels.DataBaseModels;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Application.DataBaseApplication.MonthProductionMessageApplication
{
    public class MonthProductionMessageApplication : ArgesDataCollectionWithWpfApplicationBase, IMonthProductionMessageApplication
    {

        public MonthProductionMessageApplication(DbContextConnection sugarClinet, ILogger logger, IMapper objectMapper) : base(sugarClinet, logger, objectMapper)
        {

        }


        public int InsertOrUpdateMonthProductionMessage(AddOrInsertMonthProductionMessageInput addOrInsertMonthProductionMessageInput)
        {
            var obs = this._objectMapper.Map<MonthProductionMessageModel>(addOrInsertMonthProductionMessageInput);
            var x = _dbContextClinet.SugarClient.Storageable(obs).ToStorage();


            var insetCount = x.AsInsertable.ExecuteCommand();//不存在插入
            var updateCount = x.AsUpdateable.ExecuteCommand();//存在更新

            return insetCount + updateCount;
        }

        public QuerryMonthProductionMessageOutput QuerryMonthProductionMessageByMonth(DateTime month)
        {
            DateTime start =Convert.ToDateTime(month.ToString("yyyy-MM-01 00:00:00"));
            DateTime end = start.AddMonths(1).AddDays(-1);




            var querryResult = _dbContextClinet.SugarClient.Queryable<MonthProductionMessageModel>()
                .Where(s => SqlSugar.SqlFunc.Between(s.Time,Convert.ToDateTime(start.ToString("yyyy-MM-dd 00:00:00")),Convert.ToDateTime(end.ToString("yyyy-MM-dd 23:59:59"))))
                .OrderBy(it => it.ID);

            if (querryResult.Count() <= 0)
            {
                return new QuerryMonthProductionMessageOutput
                {

                    ID = -1,
                    MonthCount = 0,
                    Time = month,
                };
            }
            
            var querryDto = from m in querryResult.ToList() select _objectMapper.Map<QuerryMonthProductionMessageOutput>(m);

            
            return querryDto.ToList().First();
        }

        public List<QuerryMonthProductionMessageOutput> QuerryMonthProductionMessageByYear(DateTime year)
        {
            var querryResult = _dbContextClinet.SugarClient.Queryable<DayProductionMessageModel>()
                .Where(s => SqlSugar.SqlFunc.Between(s.Time, year.ToString("yyyy-00-00 00:00:00"), year.ToString("yyyy-00-31 23:59:59"))).OrderBy(it => it.ID);
            var querryDto = from m in querryResult.ToList() select _objectMapper.Map<QuerryMonthProductionMessageOutput>(m);


            return querryDto.ToList();
        }
    }
}
