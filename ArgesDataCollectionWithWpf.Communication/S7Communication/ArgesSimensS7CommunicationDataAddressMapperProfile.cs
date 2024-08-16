//zy


using Abp.Dependency;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.CommunicationDetailsAndInstanceApplication.Dto;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application.Dto;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.LineStationParameterApplication.Dto;
using ArgesDataCollectionWithWpf.Communication.Utils;
using ArgesDataCollectionWithWpf.DbModels.CommunicationParaTransferModel;
using ArgesDataCollectionWithWpf.DbModels.Models;
using AutoMapper;
using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Communication.S7Communication
{
    public class ArgesSimensS7CommunicationDataAddressMapperProfile: Profile
    {
        public ArgesSimensS7CommunicationDataAddressMapperProfile()
        {
            
          
            CreateMap<QuerryConnect_Device_With_PC_Function_DataOutput, DataItemModel>()
                .ForMember(u => u.VarType, options => options.MapFrom(input => input.VarType))
                .ForMember(u => u.DataType, options => options.MapFrom(input => (DataType)Enum.Parse(typeof(DataType), input.DBDescription.GetCharacters() == "DB" ? "DataBlock" : "Input", false) ))



                .ForMember(u => u.DB, options => options.MapFrom(input => Convert.ToInt32( input.DBDescription.GetNumberBeforePoint())))
                .ForMember(u => u.StartByteAdr, options => options.MapFrom(input => Convert.ToInt32(input.DBDescription.GetNumberMiddlePoint())))
                .ForMember(u => u.DataInDatabaseIndex, options => options.MapFrom(input => input.DataSaveIndex))
                .ForMember(u => u.DataAddressDescription, options => options.MapFrom(input => input.DataAddressDescription))
                .ForMember(u => u.Count, options => options.MapFrom(input => input.DataLength))
                .ForMember(u => u.BitAdr, options => options.MapFrom(input => Convert.ToByte(input.DBDescription.GetNumberAfterPoint())));
        
        
        }
    }
}
