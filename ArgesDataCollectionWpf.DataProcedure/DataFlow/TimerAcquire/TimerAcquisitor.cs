//zy


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using ArgesDataCollectionWpf.DataProcedure.DataFlow.Interceptors;
using ArgesDataCollectionWpf.DataProcedure.Entities;
using Castle.Core;
using EnterpriseFD.Dataflow;
using EnterpriseFD.Dataflow.Acquisitors;
using EnterpriseFD.Dataflow.Starters;

namespace ArgesDataCollectionWpf.DataProcedure.DataFlow.TimerAcquire
{
    //[Interceptor(typeof(InterceptorException))]
    public class TimerAcquisitor : AbstractTimer
    {
        private readonly IChannel<PlcAddressAndDatabaseAndCommunicationCombineEntity> _channel;
        private readonly PlcAddressAndDatabaseAndCommunicationCombineEntity _entity;

        //这个不用管，只是每次的起始，剩下的具体做法交给后面的handler，传进来一个handler
        public TimerAcquisitor(IChannel<PlcAddressAndDatabaseAndCommunicationCombineEntity> channel, PlcAddressAndDatabaseAndCommunicationCombineEntity entity)
        {
            this._channel = channel;
            this._entity = entity;
        }



        protected override Task Work()
        {
            return this._channel.Channel(this._entity);
        }
    }
}
