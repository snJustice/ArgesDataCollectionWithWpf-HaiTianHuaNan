using ArgesDataCollectionWpf.DataProcedure.Entities;
using EnterpriseFD.Dataflow.Transformers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Abp.Dependency;
using ArgesDataCollectionWithWpf.Core.Utils;

using System.Threading.Tasks.Dataflow;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.ModlingCodesApplication.Dto;

namespace ArgesDataCollectionWpf.DataProcedure.DataFlow.Transformers
{
    public class ReadDatasFromTargetQuene : AbstractRevisor<PlcAddressAndDatabaseAndCommunicationCombineEntity>
    {
        private readonly BufferBlock<QuerryModlingCodesOutput> _queneBuffer;
        private readonly int _stattionNumber;


        public ReadDatasFromTargetQuene(BufferBlock<QuerryModlingCodesOutput> queneBuffer ,int stattionNumber)
        {
            this._queneBuffer = queneBuffer;
            this._stattionNumber = stattionNumber;

        }
        protected override PlcAddressAndDatabaseAndCommunicationCombineEntity DoTransform(PlcAddressAndDatabaseAndCommunicationCombineEntity data)
        {
            QuerryModlingCodesOutput xxx = null;
            this._queneBuffer.TryReceive(out xxx);
            if (xxx is null)
            {
                var runeditem = new QuerryModlingCodesOutputWithEndTime
                {

                    Containerno = Guid.NewGuid().ToString(),
                    Part1 = "123131",
                    Part2 = "1231231",
                    Part3 = "2343242",
                    StationNumber = this._stattionNumber.ToString(),
                    EndTime = DateTime.Now,
                    Time = DateTime.Now

                };
                data.ModlingCodeFromMes = runeditem;
                
            }

            else
            {
                var runeditem = new QuerryModlingCodesOutputWithEndTime
                {

                    Containerno = xxx.Containerno,
                    Part1 = xxx.Part1,
                    Part2 = xxx.Part2,
                    Part3 = xxx.Part3,
                    StationNumber = xxx.StationNumber,
                    EndTime = DateTime.Now,
                    Time = xxx.Time

                };
                data.ModlingCodeFromMes = runeditem;
                
            }


            return data;
            
            
        }
    }
}
