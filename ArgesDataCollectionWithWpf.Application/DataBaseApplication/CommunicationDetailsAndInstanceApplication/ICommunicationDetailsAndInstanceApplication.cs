//zy


using Abp.Application.Services;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.CommunicationDetailsAndInstanceApplication.Dto;
using ArgesDataCollectionWithWpf.DbModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Application.DataBaseApplication.CommunicationDetailsAndInstanceApplication
{
    public interface ICommunicationDetailsAndInstanceApplication : IApplicationService
    {
        //插入数据，
        void InsertCommunicationDetailsAndInstance(AddCommunicationDetailsAndInstanceInput addPlcSimensConnectionInput);

        //查询数据，返回list
        List<QuerryCommunicationDetailsAndInstanceOutput> QuerryCommunicationDetailsAndInstanceAll();



        //修改plc参数

        int ModifyCommunicationDetailsAndInstanceById(ModifyCommunicationDetailsAndInstanceByIdInput modifyPlcSimensConnectionByIdInput);


        int DeleteCommunicationDetailsAndInstanceById(DeleteCommunicationDetailsAndInstanceByIdInput deletePlcSimensConnectionByIdInput);
        //int DeleteCommunicationDetailsAndInstanceByUnicode(DeleteCommunicationDetailsAndInstanceByIdInput deletePlcSimensConnectionByIdInput);

        List<QuerryCommunicationDetailsAndInstanceOutput> QuerryCommunicationDetailsAndInstanceByUnicode(string unicode);

        List<QuerryCommunicationDetailsAndInstanceOutput> QuerryCommunicationDetailsAndInstanceByConnectType(ConnectType type);
        
        

    }
}
