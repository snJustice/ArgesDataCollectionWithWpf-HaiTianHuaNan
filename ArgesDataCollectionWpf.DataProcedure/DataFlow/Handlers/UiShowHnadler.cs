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
                    try
                    {
                        
                        var readResult = data.Communication.GetData(this._ShowAddressAddresses);

                        Parallel.ForEach(readResult, x => { this._writeLogForUserControl.ChangeUiValueFromPlc(x.DataInDatabaseIndex, x.Value); });
                    }
                    catch (Exception ex)
                    {

                        data.LogAndShowHandler.Channel(new Interceptors.LogMessage {  Level= Microsoft.Extensions.Logging.LogLevel.Error,Message=$"界面显示数据，plc处地址不对{ex.Message}"});
                    }
                    
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
