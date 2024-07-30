//zy


using ArgesDataCollectionWithWpf.Application.DataBaseApplication.CommunicationDetailsAndInstanceApplication.Dto;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application.Dto;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.DayProductionMessageApplication.Dto;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.LineStationParameterApplication.Dto;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.MonthProductionMessageApplication.Dto;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.OrdersFromMesApplication;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.OrdersFromMesApplication.Dto;
using ArgesDataCollectionWithWpf.DbModels.DataBaseModels;
using ArgesDataCollectionWithWpf.DbModels.Models;

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Application.DataBaseApplication
{
    public class ArgesDataCollectionWithWpfApplicationMapperProfile : Profile
    {
        public ArgesDataCollectionWithWpfApplicationMapperProfile()
        {
            //CommunicationDetailsAndInstanceModel相关的映射
            CreateMap<AddCommunicationDetailsAndInstanceInput, CommunicationDetailsAndInstanceModel>();
            //.ForMember(u => u.PLCIPAddress, options => options.MapFrom(input => input.PLCIPAddress));
            CreateMap<CommunicationDetailsAndInstanceModel, QuerryCommunicationDetailsAndInstanceOutput>();
            CreateMap<ModifyCommunicationDetailsAndInstanceByIdInput, CommunicationDetailsAndInstanceModel>();
            CreateMap<DeleteCommunicationDetailsAndInstanceByIdInput, CommunicationDetailsAndInstanceModel>();


            //LineStationParameterModel相关的映射
            CreateMap<DeleteLineStationParameterByIdInput, LineStationParameterModel>();
            CreateMap<AddLineStationParameterInput, LineStationParameterModel>();
            CreateMap<ModifyLineStationParameterInput, LineStationParameterModel>();
            CreateMap<LineStationParameterModel, QuerryLineStationParameterOutput>();
            CreateMap<QuerryLineStationParameterOutput, AddLineStationParameterInput>();

            CreateMap<QuerryLineStationParameterOutput, ModifyLineStationParameterInput>();


            //PlcSimens_With_PC_Function_Data_Model相关映射
            CreateMap<DeleteConnect_Device_With_PC_Function_DataByIdInput, Connect_Device_With_PC_Function_Data_Model>();
            CreateMap<AddConnect_Device_With_PC_Function_DataInput, Connect_Device_With_PC_Function_Data_Model>();
            CreateMap<ModifyConnect_Device_With_PC_Function_DataByIdInput, Connect_Device_With_PC_Function_Data_Model>();
            CreateMap<Connect_Device_With_PC_Function_Data_Model, QuerryConnect_Device_With_PC_Function_DataOutput>();

            CreateMap<QuerryConnect_Device_With_PC_Function_DataOutput, AddConnect_Device_With_PC_Function_DataInput>();
            CreateMap<QuerryConnect_Device_With_PC_Function_DataOutput, ModifyConnect_Device_With_PC_Function_DataByIdInput>();




            //OrdersFromMes_Model相关映射
            CreateMap<AddOrdersFromMesInput, OrdersFromMes_Model>();
            CreateMap<OrdersFromMes_Model, QuerryOrdersFromMesOutput>();
            CreateMap<AddOrUpdateOrdersFromMesInput, OrdersFromMes_Model>();



            //MonthProductionMessageModel相关映射

            CreateMap<AddOrInsertMonthProductionMessageInput, MonthProductionMessageModel>();
            CreateMap<MonthProductionMessageModel, QuerryMonthProductionMessageOutput>(); 


            //DayProductionMessageModel相关映射
            CreateMap<AddOrInsertDayProductionMessageInput, DayProductionMessageModel>();
            CreateMap<DayProductionMessageModel, QuerryDayProductionMessageOutput>();








        }
    }
}
