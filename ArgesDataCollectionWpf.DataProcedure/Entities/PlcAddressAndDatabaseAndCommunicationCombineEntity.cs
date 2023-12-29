//zy


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
        public List<DataItemModel> DataItems { get; set; }

        public string TableName { get; set; }

        public ISender Communication { get; set; }

        public int IsAllowReWrite { get; set; }

        public CustomerExceptionHandler LogAndShowHandler { get; set; }

    }

    public class PlcAddressAndDatabaseAndCommunicationCombineEntityWithWriteResult
    {
        public PlcAddressAndDatabaseAndCommunicationCombineEntity Entity { get; set; }
        public int SaveResult { get; set; }



    }
}
