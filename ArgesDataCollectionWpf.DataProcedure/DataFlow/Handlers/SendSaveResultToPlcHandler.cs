//zy


using ArgesDataCollectionWithWpf.Communication;
using ArgesDataCollectionWithWpf.DbModels.CommunicationParaTransferModel;
using ArgesDataCollectionWpf.DataProcedure.Entities;
using EnterpriseFD.Dataflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWpf.DataProcedure.DataFlow.Handlers
{
    public class SendSaveResultToPlcHandler : IChannel<PlcAddressAndDatabaseAndCommunicationCombineEntityWithWriteResult>
    {
        private readonly DataItemModel _saveOkAddress;
        private readonly DataItemModel _saveNgAddress;

        public SendSaveResultToPlcHandler(DataItemModel saveOkAddress,DataItemModel saveNgAddress)
        {
            
            this._saveOkAddress = saveOkAddress;
            this._saveNgAddress = saveNgAddress;
        }

        

        public Task Channel(PlcAddressAndDatabaseAndCommunicationCombineEntityWithWriteResult data)
        {
            return Task.Run(() => {


                this._saveOkAddress.Value = data.SaveResult == 1 ? true : false;
                this._saveNgAddress.Value = data.SaveResult == 1 ? false : true;
                data.Entity.Communication.SendData(new List<DataItemModel> { this._saveOkAddress, this._saveNgAddress });


            });
        }

        public void Dispose()
        {
            
        }
    }
}
