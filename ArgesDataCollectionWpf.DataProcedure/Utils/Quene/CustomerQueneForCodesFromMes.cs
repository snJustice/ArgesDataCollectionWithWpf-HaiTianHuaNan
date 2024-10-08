﻿using System;
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


        //mes端需要1个撤销功能，撤销一个扫码信息，
        public BufferBlock<QuerryModlingCodesOutput> MainScanDeleteQuene { get; set; }

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
            MainScanDeleteQuene = new BufferBlock<QuerryModlingCodesOutput>();



            MainRunedQuene = new BufferBlock<QuerryModlingCodesOutputWithEndTime>();
            LoadMaterialQuene = new BufferBlock<LoadMaterialAreaAndDownMaterialDto>();
            DownMaterialQuene = new BufferBlock<LoadMaterialAreaAndDownMaterialDto>();

            
        }

        

        public void Init()
        {

          
           
            LoadMaterialQuene.Complete();
            
           
            

            StationOneScanQuene.Complete();
            StationTwoScanQuene.Complete();
            MainScanQuene.Complete();
            MainScanDeleteQuene.Complete();

            MainRunedQuene.Complete();
            DownMaterialQuene.Complete();


            LoadMaterialQuene = new BufferBlock<LoadMaterialAreaAndDownMaterialDto>();
            StationOneScanQuene = new BufferBlock<QuerryModlingCodesOutput>();
            StationTwoScanQuene = new BufferBlock<QuerryModlingCodesOutput>();
            MainScanQuene = new BufferBlock<QuerryModlingCodesOutput>();
            MainScanDeleteQuene = new BufferBlock<QuerryModlingCodesOutput>();

            MainRunedQuene = new BufferBlock<QuerryModlingCodesOutputWithEndTime>();
            DownMaterialQuene = new BufferBlock<LoadMaterialAreaAndDownMaterialDto>();
        }
    }
}
