using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Abp.Dependency;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.ModlingCodesApplication.Dto;

namespace ArgesDataCollectionWpf.DataProcedure.Utils.Quene
{
    public class CustomerQueneForCodesFromMes:ISingletonDependency
    {
        public BufferBlock<QuerryModlingCodesOutput> MainScanQuene { get; set; }

        public BufferBlock<QuerryModlingCodesOutput> StationOneScanQuene { get; set; }
        public BufferBlock<QuerryModlingCodesOutput> StationTwoScanQuene { get; set; }


        public BufferBlock<QuerryModlingCodesOutputWithEndTime> MainRunedQuene { get; set; }

        public CustomerQueneForCodesFromMes()
        {
            StationOneScanQuene = new BufferBlock<QuerryModlingCodesOutput>();
            StationTwoScanQuene = new BufferBlock<QuerryModlingCodesOutput>();
            MainScanQuene = new BufferBlock<QuerryModlingCodesOutput>();
            MainRunedQuene = new BufferBlock<QuerryModlingCodesOutputWithEndTime>();
        }
    }
}
