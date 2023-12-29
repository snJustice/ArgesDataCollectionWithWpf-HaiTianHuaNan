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
            try
            {
                var readDatas = data.Communication.GetData(data.DataItems);
                data.DataItems = readDatas;
                data.LogAndShowHandler.Channel(new Interceptors.LogMessage { Level = Microsoft.Extensions.Logging.LogLevel.Information, Message = "读取plc数据完成" });

                
            }
            catch (Exception)
            {

                data.LogAndShowHandler.Channel(new Interceptors.LogMessage { Level = Microsoft.Extensions.Logging.LogLevel.Error, Message = "读取plc数据失败，不存在地址，或者其他原因" });
            }
            
            return data;
        }
    }
}
