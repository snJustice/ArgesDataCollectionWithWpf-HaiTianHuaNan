using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Abp.Dependency;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.ModlingCodesApplication.Dto;
using ArgesDataCollectionWithWpf.Application.OtherModelDto;

namespace ArgesDataCollectionWpf.DataProcedure.Utils.Quene
{
    public class CustomerQueneForCodesFromMes:ISingletonDependency
    {
        //从mes端来的扫码信息先进这个队列，然后分配对应的工站队列中
        public BufferBlock<QuerryModlingCodesOutput> MainScanQuene { get; set; }

        //1号工站的队列
        public BufferBlock<QuerryModlingCodesOutput> StationOneScanQuene { get; set; }

        //2号工站的队列
        public BufferBlock<QuerryModlingCodesOutput> StationTwoScanQuene { get; set; }


        //收到下料区的地轨完成信号后，，会从  工站队列中拿出对应的扫描信息，然后再添加到此主队列中，此主队列再进行数据的保存
        public BufferBlock<QuerryModlingCodesOutputWithEndTime> MainRunedQuene { get; set; }




        //切换订单号的队列
        public BufferBlock<LoadMaterialAreaAndDownMaterialDto> LoadMaterialQuene { get; set; }

        public BufferBlock<LoadMaterialAreaAndDownMaterialDto> DownMaterialQuene { get; set; }

        public CustomerQueneForCodesFromMes()
        {
            StationOneScanQuene = new BufferBlock<QuerryModlingCodesOutput>();
            StationTwoScanQuene = new BufferBlock<QuerryModlingCodesOutput>();
            MainScanQuene = new BufferBlock<QuerryModlingCodesOutput>();
            MainRunedQuene = new BufferBlock<QuerryModlingCodesOutputWithEndTime>();
            LoadMaterialQuene = new BufferBlock<LoadMaterialAreaAndDownMaterialDto>();
            DownMaterialQuene = new BufferBlock<LoadMaterialAreaAndDownMaterialDto>();
        }

        

        public void Init()
        {

            LoadMaterialAreaAndDownMaterialDto data = null; ;
            var resu = this.LoadMaterialQuene.TryReceive(out data);
            LoadMaterialQuene.Complete();
            LoadMaterialQuene = new BufferBlock<LoadMaterialAreaAndDownMaterialDto>();
            if (resu == true)
            {
                
                //LoadMaterialQuene.Post(new LoadMaterialAreaAndDownMaterialDto {  LoadOrDownArea = LoadOrDwonEnum.LoadMaterialArea});
            }
            

            StationOneScanQuene.Complete();
            StationTwoScanQuene.Complete();
            MainScanQuene.Complete();
            MainRunedQuene.Complete();
            DownMaterialQuene.Complete();



            StationOneScanQuene = new BufferBlock<QuerryModlingCodesOutput>();
            StationTwoScanQuene = new BufferBlock<QuerryModlingCodesOutput>();
            MainScanQuene = new BufferBlock<QuerryModlingCodesOutput>();
            MainRunedQuene = new BufferBlock<QuerryModlingCodesOutputWithEndTime>();
            DownMaterialQuene = new BufferBlock<LoadMaterialAreaAndDownMaterialDto>();
        }
    }
}
