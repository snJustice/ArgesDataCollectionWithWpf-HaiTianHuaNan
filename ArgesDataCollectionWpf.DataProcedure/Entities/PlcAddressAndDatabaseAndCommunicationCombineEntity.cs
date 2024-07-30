//zy


using ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application.Dto;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.ModlingCodesApplication.Dto;
using ArgesDataCollectionWithWpf.Communication;
using ArgesDataCollectionWithWpf.DbModels.CommunicationParaTransferModel;
using ArgesDataCollectionWpf.DataProcedure.DataFlow.Interceptors;
using S7.Net.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWpf.DataProcedure.Entities
{
    public class PlcAddressAndDatabaseAndCommunicationCombineEntity
    {
        //plc中的数据
        public List<DataItemModel> DataItems { get; set; }
        //根据名称取得对应的码值
        public List<QuerryConnect_Device_With_PC_Function_DataOutput> DataItemsNotFromPLC { get; set; }

        

        public string TableName { get; set; }

        public ISender Communication { get; set; }

        public int IsAllowReWrite { get; set; }

        public CustomerExceptionHandler LogAndShowHandler { get; set; }

        //这里还要增加从两个队列里面来拿到的数据
        public QuerryModlingCodesOutputWithEndTime ModlingCodeFromMes {set;get;}



    }




    public class PlcAddressAndDatabaseAndCommunicationCombineEntityWithWriteResult
    {
        public PlcAddressAndDatabaseAndCommunicationCombineEntity Entity { get; set; }
        public int SaveResult { get; set; }



    }
}
