//zy


using ArgesDataCollectionWithWpf.Communication;
using ArgesDataCollectionWithWpf.DbModels.CommunicationParaTransferModel;
using ArgesDataCollectionWpf.DataProcedure.Entities;
using EnterpriseFD.Dataflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWpf.DataProcedure.DataFlow.Handlers
{
    public class SocketHandler : IChannel<PlcAddressAndDatabaseAndCommunicationCombineEntity>
    {
        private readonly DataItemModel _socketAddress;
        int socketCount;
        const int MaxCount = 5;
        bool socketStatus = false;

        int errorCount = 0;

        public SocketHandler(DataItemModel socketAddress)
        {
            this._socketAddress = socketAddress;
        }
        public Task Channel(PlcAddressAndDatabaseAndCommunicationCombineEntity data)
        {
            return Task.Run(async () => {

                socketCount++;
                if (socketCount > MaxCount)
                {
                    socketCount = 0;
                    socketStatus = !socketStatus;
                    this._socketAddress.Value = socketStatus;
                    bool writeResult = ((ISocketHeart)data.Communication).WriteHeart(this._socketAddress );
                    

                    if (writeResult == false)
                    {
                        errorCount++;
                        
                    }
                    else
                    {
                        errorCount = 0;
                    }

                    if (errorCount> MaxCount)
                    {
                        await ((ISocketHeart)data.Communication).ReConnnect();
                    }

                }

            });

            
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
