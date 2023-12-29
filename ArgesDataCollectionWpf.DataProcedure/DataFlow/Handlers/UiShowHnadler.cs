using ArgesDataCollectionWithWpf.Communication;
using ArgesDataCollectionWithWpf.DbModels.CommunicationParaTransferModel;
using ArgesDataCollectionWpf.DataProcedure.Entities;
using ArgesDataCollectionWpf.DataProcedure.Utils;
using EnterpriseFD.Dataflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWpf.DataProcedure.DataFlow.Handlers
{
    public class UiShowHnadler : IChannel<PlcAddressAndDatabaseAndCommunicationCombineEntity>
    {

        private readonly List<DataItemModel> _ShowAddressAddresses;
        private readonly IWriteLogForUserControl _writeLogForUserControl;

        public UiShowHnadler(List<DataItemModel> showAddressAddresses, IWriteLogForUserControl writeLogForUserControl)
        {
            this._ShowAddressAddresses = showAddressAddresses;
            this._writeLogForUserControl = writeLogForUserControl;
        }

        public Task Channel(PlcAddressAndDatabaseAndCommunicationCombineEntity data)
        {
           
            return Task.Run(() => {

                if ( ((IConnected)data.Communication).IsConnected )
                {
                    var readResult = data.Communication.GetData(this._ShowAddressAddresses);

                    Parallel.ForEach(readResult, x => { this._writeLogForUserControl.ChangeUiValueFromPlc(x.DataInDatabaseIndex, x.Value); });
                }
                else
                {

                }
                
                
                
            });
            

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
