//zy


using ArgesDataCollectionWithWpf.Application.DataBaseApplication;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.LineStationParameterApplication.Dto;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.CommunicationDetailsAndInstanceApplication.Dto;
using ArgesDataCollectionWithWpf.Core;
using ArgesDataCollectionWithWpf.DbModels.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Application.DataBaseApplication.LineStationParameterApplication
{
    public class LineStationParameterApplication : ArgesDataCollectionWithWpfApplicationBase, ILineStationParameterApplication
    {
        public LineStationParameterApplication(DbContextConnection sugarClinet, ILogger logger, IMapper objectMapper) : base(sugarClinet, logger, objectMapper)
        {
        }

        public int DeleteLineStationParameterByID(DeleteLineStationParameterByIdInput deleteLineStationParameterByIdInput)
        {
            return _dbContextClinet.SugarClient
                .Deleteable(_objectMapper.Map<LineStationParameterModel>(deleteLineStationParameterByIdInput))
                .ExecuteCommand();
        }

        public int InsertLineStationParameter(AddLineStationParameterInput addLineStationParameterInput)
        {
            return _dbContextClinet.SugarClient
                .Insertable(_objectMapper.Map<LineStationParameterModel>(addLineStationParameterInput))
                .ExecuteCommand();
        }

        public int ModifyLineStationParameterById(ModifyLineStationParameterInput modifyLineStationParameterInput)
        {
            return _dbContextClinet.SugarClient
                .Updateable(_objectMapper.Map<LineStationParameterModel>(modifyLineStationParameterInput))
                .ExecuteCommand();
        }

        public List<QuerryLineStationParameterOutput> QuerryAllLineStationParameters()
        {
            var querryResult = _dbContextClinet.SugarClient.Queryable<LineStationParameterModel>()

                .Includes(aaa => aaa.DataAddressAndFunction)
                .OrderBy(it=>it.ID);

            
            var querryDto = from m in querryResult.ToList() select _objectMapper.Map<QuerryLineStationParameterOutput>(m);

            return querryDto.ToList();

        }
    }
}
