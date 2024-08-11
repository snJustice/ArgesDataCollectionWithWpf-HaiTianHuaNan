using ArgesDataCollectionWithWpf.Application.OtherModelDto;
using ArgesDataCollectionWpf.DataProcedure.Entities;
using ArgesDataCollectionWpf.DataProcedure.Utils.Quene;
using EnterpriseFD.Dataflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ArgesDataCollectionWpf.DataProcedure.DataFlow.Handlers
{
    public class LoadMaterialAndDownMaterialAreaHandler : IChannel<PlcAddressAndDatabaseAndCommunicationCombineEntity>
    {
        private readonly CustomerQueneForCodesFromMes _customerQueneForCodesFromMes;
        private readonly LoadOrDwonEnum _loadOrDwonEnum;

        public LoadMaterialAndDownMaterialAreaHandler(CustomerQueneForCodesFromMes  customerQueneForCodesFromMes, LoadOrDwonEnum loadOrDwonEnum)
        {
            this._customerQueneForCodesFromMes = customerQueneForCodesFromMes;
            this._loadOrDwonEnum = loadOrDwonEnum;
        }
        public Task Channel(PlcAddressAndDatabaseAndCommunicationCombineEntity data)
        {
            switch (_loadOrDwonEnum)
            {
                case LoadOrDwonEnum.LoadMaterialArea:
                    this._customerQueneForCodesFromMes.LoadMaterialQuene.Post(new LoadMaterialAreaAndDownMaterialDto
                    {

                        LoadOrDownArea = this._loadOrDwonEnum
                    });

                    break;
                case LoadOrDwonEnum.DownMaterialArea:

                    this._customerQueneForCodesFromMes.DownMaterialQuene.Post(new LoadMaterialAreaAndDownMaterialDto
                    {

                        LoadOrDownArea = this._loadOrDwonEnum
                    });
                    break;
                default:
                    break;
            }

            


            return Task.FromResult(0);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
