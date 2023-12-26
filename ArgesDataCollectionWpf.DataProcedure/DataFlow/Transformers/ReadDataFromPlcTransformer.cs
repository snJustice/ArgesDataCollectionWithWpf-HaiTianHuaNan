//zy


using ArgesDataCollectionWithWpf.Communication;
using ArgesDataCollectionWithWpf.DbModels.CommunicationParaTransferModel;
using ArgesDataCollectionWpf.DataProcedure.Entities;
using EnterpriseFD.Dataflow.Transformers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWpf.DataProcedure.DataFlow.Transformers
{
    public class ReadDataFromPlcTransformer : AbstractRevisor<PlcAddressAndDatabaseAndCommunicationCombineEntity>
    {
        

        public ReadDataFromPlcTransformer()
        {
            
        }
        protected override PlcAddressAndDatabaseAndCommunicationCombineEntity DoTransform(PlcAddressAndDatabaseAndCommunicationCombineEntity data)
        {
            data.Communication.GetData(data.DataItems);
            return data;
        }
    }
}
