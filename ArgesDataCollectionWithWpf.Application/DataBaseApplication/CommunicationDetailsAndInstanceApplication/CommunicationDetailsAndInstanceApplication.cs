//zy


using ArgesDataCollectionWithWpf.Application.DataBaseApplication;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.CommunicationDetailsAndInstanceApplication.Dto;
using ArgesDataCollectionWithWpf.Core;
using ArgesDataCollectionWithWpf.DbModels.Enums;
using ArgesDataCollectionWithWpf.DbModels.Models;

using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Application.DataBaseApplication.CommunicationDetailsAndInstanceApplication
{
    public class CommunicationDetailsAndInstanceApplication : ArgesDataCollectionWithWpfApplicationBase, ICommunicationDetailsAndInstanceApplication
    {
        public CommunicationDetailsAndInstanceApplication(DbContextConnection sugarClinet, ILogger logger, IMapper objectMapper) : base(sugarClinet, logger, objectMapper)
        {
        }

        public int DeleteCommunicationDetailsAndInstanceById(DeleteCommunicationDetailsAndInstanceByIdInput deletePlcSimensConnectionByIdInput)
        {
            return _dbContextClinet.SugarClient
                .Deleteable(_objectMapper.Map<CommunicationDetailsAndInstanceModel>(deletePlcSimensConnectionByIdInput))
                .ExecuteCommand();
        }

        public void InsertCommunicationDetailsAndInstance(AddCommunicationDetailsAndInstanceInput addPlcSimensConnectionInput)
        {
            var mapResult = _objectMapper.Map<CommunicationDetailsAndInstanceModel>(addPlcSimensConnectionInput);
            _dbContextClinet.SugarClient.Insertable<CommunicationDetailsAndInstanceModel>(mapResult).ExecuteCommand();

        }

        

        public int ModifyCommunicationDetailsAndInstanceById(ModifyCommunicationDetailsAndInstanceByIdInput modifyPlcSimensConnectionByIdInput)
        {
            return _dbContextClinet.SugarClient
                .Updateable<CommunicationDetailsAndInstanceModel>(_objectMapper.Map<CommunicationDetailsAndInstanceModel>(modifyPlcSimensConnectionByIdInput))
                
                .ExecuteCommand();
        }

        public List<QuerryCommunicationDetailsAndInstanceOutput> QuerryCommunicationDetailsAndInstanceAll()
        {

            var querryResult = _dbContextClinet.SugarClient.Queryable<CommunicationDetailsAndInstanceModel>().OrderBy(it=>it.ID);
            
            var querryDto = from m in querryResult.ToList() select _objectMapper.Map<QuerryCommunicationDetailsAndInstanceOutput>(m);

            return querryDto.ToList();

        }

        public List<QuerryCommunicationDetailsAndInstanceOutput> QuerryCommunicationDetailsAndInstanceByConnectType(ConnectType type)
        {
            var querryResult = _dbContextClinet.SugarClient.Queryable<CommunicationDetailsAndInstanceModel>().Where(it => it.ConnectType == type);
            var re = from m in querryResult.ToList() select _objectMapper.Map<QuerryCommunicationDetailsAndInstanceOutput>(m);
            return re.ToList();
        }

        public List<QuerryCommunicationDetailsAndInstanceOutput> QuerryCommunicationDetailsAndInstanceByUnicode(string unicode)
        {
            var querryResult = _dbContextClinet.SugarClient.Queryable<CommunicationDetailsAndInstanceModel>().Where(it => it.UniqueCode == unicode);
            var re = from m in querryResult.ToList() select _objectMapper.Map<QuerryCommunicationDetailsAndInstanceOutput>(m);
            return re.ToList();
        }
    }
}
