//zy


using ArgesDataCollectionWithWpf.Application.DataBaseApplication.CommunicationDetailsAndInstanceApplication.Dto;
using ArgesDataCollectionWithWpf.DbModels.CommunicationParaTransferModel;
using ArgesDataCollectionWithWpf.DbModels.Models;
using AutoMapper;
using Castle.Core;
using S7.Net.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Communication
{
    
    public class ArgesDataCollectionWithWpfCommunicationMapperProfile: Profile
    {
        public ArgesDataCollectionWithWpfCommunicationMapperProfile()
        {
            CreateMap<DataItemModel, DataItem>();
            CreateMap<DataItem, DataItemModel>();
        }
    }
}
