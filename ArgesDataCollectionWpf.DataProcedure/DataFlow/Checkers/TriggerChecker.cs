//zy


using ArgesDataCollectionWithWpf.DbModels.CommunicationParaTransferModel;
using ArgesDataCollectionWpf.DataProcedure.Entities;
using EnterpriseFD.Dataflow.Checkers;
using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWpf.DataProcedure.DataFlow.Checkers
{

    public class TriggerChecker : AbstractChecker<PlcAddressAndDatabaseAndCommunicationCombineEntity>
    {
        private readonly DataItemModel _triggerAddress;
        bool lastState = false;
        bool currentState = false;

        public TriggerChecker(DataItemModel triggerAddress)
        {
            this._triggerAddress = triggerAddress;
        }

        protected override Expression<Func<PlcAddressAndDatabaseAndCommunicationCombineEntity, bool>> ToExpression()
        {
            return x => CheckTriggerData(x);
        }

        private bool CheckTriggerData(PlcAddressAndDatabaseAndCommunicationCombineEntity a)
        {
            this._triggerAddress.Value = false;//可能需要先把地址数据清零
            a.Communication.GetData(this._triggerAddress);

            try
            {
                currentState = Convert.ToBoolean(this._triggerAddress.Value);
                if (lastState == false && currentState == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }
            finally 
            { 
                lastState = currentState;
            }
            
            
        }
    }
}
