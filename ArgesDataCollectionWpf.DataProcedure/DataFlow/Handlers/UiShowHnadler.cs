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
        private readonly List<DataItemModel> _dayAddressAddresses;
        private readonly List<DataItemModel> _monthAddressAddresses;
        private readonly IWriteLogForUserControl _writeLogForUserControl;
        private readonly int _showAddressCount;
        private readonly int _dayAddressCount;
        private readonly int __monthAddressCount;

        public UiShowHnadler(List<DataItemModel> showAddressAddresses, IWriteLogForUserControl writeLogForUserControl, List<DataItemModel> dayAddressAddresses, List<DataItemModel> monthAddressAddresses)
        {
            this._ShowAddressAddresses = showAddressAddresses;
            this._writeLogForUserControl = writeLogForUserControl;
            this._dayAddressAddresses = dayAddressAddresses;
            this._monthAddressAddresses = monthAddressAddresses;
            this._showAddressCount = this._ShowAddressAddresses.Count;
            this._dayAddressCount = this._dayAddressAddresses.Count;
            this.__monthAddressCount = this._monthAddressAddresses.Count;
        }

        public Task Channel(PlcAddressAndDatabaseAndCommunicationCombineEntity data)
        {
            List<DataItemModel> readDatas = new List<DataItemModel>();

            readDatas.Clear();
            return Task.Run(() => {

                
                if ( ((IConnected)data.Communication).IsConnected )
                {
                    try
                    {
                        readDatas.AddRange(this._ShowAddressAddresses);
                        readDatas.AddRange(this._dayAddressAddresses);
                        readDatas.AddRange(this._monthAddressAddresses);
                        var readResult = data.Communication.GetData(readDatas);

                        for (int index = 0; index < readResult.Count; index++)
                        {
                            if (index < this._showAddressCount)
                            {
                                this._writeLogForUserControl.ChangeUiValueFromPlc("ReadAndNeedUpShowOnUi" + readResult[index].DataInDatabaseIndex.ToString(), readResult[index].Value);
                            }

                            else if (index < this._dayAddressCount + this._showAddressCount)
                            {
                                this._writeLogForUserControl.ChangeUiValueFromPlc("DayProductionOutput" + readResult[index].DataInDatabaseIndex.ToString(), readResult[index].Value);
                            }

                            else if (index < this.__monthAddressCount + this._dayAddressCount + this._showAddressCount)
                            {
                                this._writeLogForUserControl.ChangeUiValueFromPlc("MonthProductionOutput" + readResult[index].DataInDatabaseIndex.ToString(), readResult[index].Value);
                            }
                        }
                        
                        //Parallel.For(0,readResult.Count, index => { 
                        //    if(index <this._showAddressCount)
                        //    {
                        //        this._writeLogForUserControl.ChangeUiValueFromPlc("ReadAndNeedUpShowOnUi" + readResult[index].DataInDatabaseIndex.ToString(), readResult[index].Value);
                        //    }

                        //    if (index < this._dayAddressCount+ this._showAddressCount)
                        //    {
                        //        this._writeLogForUserControl.ChangeUiValueFromPlc("DayProductionOutput" + readResult[index].DataInDatabaseIndex.ToString(), readResult[index].Value);
                        //    }

                        //    if (index < this.__monthAddressCount+ this._dayAddressCount + this._showAddressCount)
                        //    {
                        //        this._writeLogForUserControl.ChangeUiValueFromPlc( "MonthProductionOutput" + readResult[index].DataInDatabaseIndex.ToString(), readResult[index].Value);
                        //    }

                        //});
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
