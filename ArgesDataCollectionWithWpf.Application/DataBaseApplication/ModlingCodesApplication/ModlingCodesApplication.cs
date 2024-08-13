using ArgesDataCollectionWithWpf.Application.DataBaseApplication.ModlingCodesApplication.Dto;
using ArgesDataCollectionWithWpf.Core;
using ArgesDataCollectionWithWpf.DbModels.DataBaseModels;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Application.DataBaseApplication.ModlingCodesApplication
{
    public class ModlingCodesApplication : ArgesDataCollectionWithWpfApplicationBase,IModlingCodesApplication
    {
        public ModlingCodesApplication(DbContextConnection sugarClinet, ILogger logger, IMapper objectMapper) : base(sugarClinet, logger, objectMapper)
        {
        }

        public int DeleteModlingCodesByContainerNo(DeleteModlingCodesInput deleteModlingCodesInput)
        {
            
            return _dbContextClinet.SugarClient
                .Deleteable<ModlingCodesModel>().Where(it=>it.Containerno == deleteModlingCodesInput.Containerno)//(_objectMapper.Map<ModlingCodesModel>(deleteModlingCodesInput))
                .ExecuteCommand();
        }

        public int InsertModlingCodes(AddModlingCodesInputs addModlingCodesInputs)
        {
            return _dbContextClinet.SugarClient
                .Insertable(_objectMapper.Map<ModlingCodesModel>(addModlingCodesInputs))
                .ExecuteCommand();
        }

        public QuerryModlingCodesOutput QuerryModlingCodesByTimeEarlyAndNotDone(DateTime startDate, DateTime endDate,string stationNumber)
        {
            var querryResult = _dbContextClinet.SugarClient.Queryable<ModlingCodesModel>()

                .Where(s => SqlSugar.SqlFunc.Between(s.Time, startDate, endDate))
                .Where(s => s.IsDone == 0 )
                .Where (s=>s.StationNumber == stationNumber)
                .OrderBy(it => it.Time);
                


            var querryDto = from m in querryResult.ToList() select _objectMapper.Map<QuerryModlingCodesOutput>(m);
            if (querryDto!=null && querryDto.Count()>0)
            {
                return querryDto.First();
            }
            else
            {
                return null;
            }
            
        }

        public int UpdateModlingCodesIsDoneByContainerNo(UpdateModlingCodesInput updateModlingCodesInput)
        {
            var updateResult = _dbContextClinet.SugarClient.Updateable<ModlingCodesModel>(updateModlingCodesInput)
                .UpdateColumns(s => new {s.Containerno, s.IsDone})
                .ExecuteCommand();

            return updateResult;
        }
    }
}
