//zy


using ArgesDataCollectionWithWpf.Application.DataBaseApplication;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application.Dto;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.CommunicationDetailsAndInstanceApplication.Dto;
using ArgesDataCollectionWithWpf.Core;
using ArgesDataCollectionWithWpf.DbModels.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArgesDataCollectionWithWpf.DbModels.CommunicationParaTransferModel.SimensS7;

namespace ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application
{
    public class Connect_Device_With_PC_Function_Data_Application : ArgesDataCollectionWithWpfApplicationBase, IConnect_Device_With_PC_Function_Data_Application
    {
        public Connect_Device_With_PC_Function_Data_Application(DbContextConnection sugarClinet, ILogger logger, IMapper mapper)
            : base(sugarClinet, logger, mapper)
        {
        }

        public bool ClearAll()
        {
            int deletelien = _dbContextClinet.SugarClient.Deleteable<Connect_Device_With_PC_Function_Data_Model>().ExecuteCommand();
            return deletelien > 0;
        }

        public int DeleteConnect_Device_With_PC_Function_DataById(DeleteConnect_Device_With_PC_Function_DataByIdInput deletePlcSimens_With_PC_Function_DataByIdInput)
        {
            int deletelien = _dbContextClinet.SugarClient
                .Deleteable(_objectMapper.Map<Connect_Device_With_PC_Function_Data_Model>(deletePlcSimens_With_PC_Function_DataByIdInput))
                .ExecuteCommand();
            return deletelien;
        }

        public int InsertConnect_Device_With_PC_Function_Data(AddConnect_Device_With_PC_Function_DataInput addPlcSimens_With_PC_Function_DataInput)
        {
            var mapResult = _objectMapper.Map<Connect_Device_With_PC_Function_Data_Model>(addPlcSimens_With_PC_Function_DataInput);
            int insertNum = _dbContextClinet.SugarClient.Insertable<Connect_Device_With_PC_Function_Data_Model>(mapResult).ExecuteCommand();
            return insertNum;
        }

        public int ModifyConnect_Device_With_PC_Function_DataById(ModifyConnect_Device_With_PC_Function_DataByIdInput modifyPlcSimens_With_PC_Function_DataByIdInput)
        {
            return _dbContextClinet.SugarClient
                .Updateable(_objectMapper.Map<Connect_Device_With_PC_Function_Data_Model>(modifyPlcSimens_With_PC_Function_DataByIdInput))
                .ExecuteCommand();
        }

        public List<QuerryConnect_Device_With_PC_Function_DataOutput> QuerryConnect_Device_With_PC_Function_DataByStationNumber(int stationNumber)
        {
            var querryResult = _dbContextClinet.SugarClient.Queryable<Connect_Device_With_PC_Function_Data_Model>().Where(it=>it.LineID == stationNumber);

            var querryDto = from m in querryResult.ToList() select _objectMapper.Map<QuerryConnect_Device_With_PC_Function_DataOutput>(m);

            return querryDto.ToList();
        }

        public List<QuerryConnect_Device_With_PC_Function_DataOutput> QuerryConnect_Device_With_PC_Function_DatasAll()
        {
            var querryResult = _dbContextClinet.SugarClient.Queryable<Connect_Device_With_PC_Function_Data_Model>();

            var querryDto = from m in querryResult.ToList() select _objectMapper.Map<QuerryConnect_Device_With_PC_Function_DataOutput>(m);

            return querryDto.ToList();
        }
    }
}
