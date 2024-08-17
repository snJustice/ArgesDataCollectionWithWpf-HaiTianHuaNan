using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Abp.Dependency;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application.Dto;
using ArgesDataCollectionWithWpf.Communication;
using ArgesDataCollectionWithWpf.Communication.Utils;
using ArgesDataCollectionWithWpf.DbModels.CommunicationParaTransferModel;
using ArgesDataCollectionWithWpf.DbModels.Enums;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace ArgesDataCollectionWithWpf.UI.SingletonResource.SendOrderMessageResource
{
    public class SendOrderMessageToPlcSingleton:ISingletonDependency
    {

        private readonly List<QuerryConnect_Device_With_PC_Function_DataOutput> _connect_Device_With_PC_Function_Data_Application;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly CommunicationManagerDictionary _communicationManagerDictionary;
        private readonly IConnect_Device_With_PC_Function_Data_Application _iConnectAddressData;

        private  ISender _sender;

        //上料区
        DataItemModel _moldingTypeAddressLoad;
        DataItemModel _poolrodsTypeAddressLoad;
        DataItemModel _qualityAddressLoad;

        //下料区
        DataItemModel _moldingTypeAddressDown;
        DataItemModel _poolrodsTypeAddressDown;
        DataItemModel _qualityAddressDown;

        DataItemModel _sendDownAddress;
        DataItemModel _moonQualityAddress;
        DataItemModel _dayQualityAddress;
        List<DataItemModel> _triggerAddress;


        DataItemModel _downMaterialAreaNeedNewOrderDownAddress;//下料区订单完成信号
        DataItemModel _loadMaterialAreaNeedNewOrderDownAddress;//上料区订单完成信号



        DataItemModel _ctTimeAddress;//节拍时间信号


        public SendOrderMessageToPlcSingleton(IConnect_Device_With_PC_Function_Data_Application iConnectAddressData
            , IMapper mapper
            , ILogger logger
            , CommunicationManagerDictionary communicationManagerDictionary)
        {

            this._iConnectAddressData = iConnectAddressData;
            this._mapper = mapper;
            this._logger = logger;
            this._communicationManagerDictionary = communicationManagerDictionary;
            this._connect_Device_With_PC_Function_Data_Application = this._iConnectAddressData.QuerryConnect_Device_With_PC_Function_DataByStationNumber(1);

            



        }


        public void InitAddress()
        {
            this._poolrodsTypeAddressLoad = (GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction.PollRodLoad)).First();
            this._moldingTypeAddressLoad = (GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction.ModlingTypeNameLoad)).First();
            
            this._qualityAddressLoad = (GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction.ProduceQualityLoad)).First();

            this._poolrodsTypeAddressDown = (GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction.PollRodLoadDown)).First();
            this._moldingTypeAddressDown = (GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction.ModlingTypeNameDown)).First();

            this._qualityAddressDown = (GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction.ProduceQualityLoadDown)).First();

            //this._sendDownAddress = (GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction.SendModlingAndPollRodDone)).First();
            this._moonQualityAddress = (GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction.MonthProductionOutput)).First();
            this._dayQualityAddress = (GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction.DayProductionOutput)).First();


            this._triggerAddress = (GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction.DayProductionOutput)).ToList();


            this._downMaterialAreaNeedNewOrderDownAddress = (GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction.DownMaterialAreaNeedNewOrderDown)).First();
            this._loadMaterialAreaNeedNewOrderDownAddress = (GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction.LoadMaterialNeedNewOrderDown)).First();

            this._ctTimeAddress = (GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction.CTTime)).First();

            



            this._sender = (ISender)this._communicationManagerDictionary[1];


        }


        public bool SendModlingPollRodQualityLoadMaterialArea(int modlingType,int quality,int pollRod, int moonQuality=0, int dayQuality=0)
        {
            this._moldingTypeAddressLoad.Value = (ushort)modlingType;
            this._qualityAddressLoad.Value = (ushort)quality;
            this._poolrodsTypeAddressLoad.Value = (ushort)pollRod;
            

            List<DataItemModel> sends = new List<DataItemModel>();
            sends.Add(this._moldingTypeAddressLoad);
            sends.Add(this._qualityAddressLoad);
            sends.Add(this._poolrodsTypeAddressLoad);
            

            bool sendResult = this._sender.SendData(sends);

            sends.Clear();
            
            //上料区订单下发完成信号
            Task.Run(async () => {


                this._loadMaterialAreaNeedNewOrderDownAddress.Value = true;
                sends.Add(this._loadMaterialAreaNeedNewOrderDownAddress);
                sendResult &= this._sender.SendData(sends);
                await Task.Delay(1000);
                sends.Clear();
                //清空触发信号
                
                this._loadMaterialAreaNeedNewOrderDownAddress.Value = false; ;
                sends.Add(this._loadMaterialAreaNeedNewOrderDownAddress);
                
                sendResult &= this._sender.SendData(sends);
            });
            
            


            return sendResult;

        }




        public bool SendModlingPollRodQualityDownMaterialArea(int modlingType, int quality, int pollRod, int moonQuality = 0, int dayQuality = 0)
        {
            this._moldingTypeAddressDown.Value = (ushort)modlingType;
            this._qualityAddressDown.Value = (ushort)quality;
            this._poolrodsTypeAddressDown.Value = (ushort)pollRod;


            List<DataItemModel> sends = new List<DataItemModel>();
            sends.Add(this._moldingTypeAddressDown);
            sends.Add(this._qualityAddressDown);
            sends.Add(this._poolrodsTypeAddressDown);


            bool sendResult = this._sender.SendData(sends);

            sends.Clear();

            //下料区订单下发完成信号
            Task.Run(async () => {


                this._downMaterialAreaNeedNewOrderDownAddress.Value = true;
                sends.Add(this._downMaterialAreaNeedNewOrderDownAddress);
                sendResult &= this._sender.SendData(sends);
                await Task.Delay(1000);
                sends.Clear();
                //清空触发信号

                this._downMaterialAreaNeedNewOrderDownAddress.Value = false; ;
                sends.Add(this._downMaterialAreaNeedNewOrderDownAddress);

                sendResult &= this._sender.SendData(sends);
            });




            return sendResult;

        }

        public bool SendMonthDayProduction(int moonQuality, int dayQuality)
        {
            this._moonQualityAddress.Value = moonQuality;
            this._dayQualityAddress.Value = dayQuality;
      

            List<DataItemModel> sends = new List<DataItemModel>();
            sends.Add(this._moonQualityAddress);
            sends.Add(this._dayQualityAddress);
 
            return this._sender.SendData(sends);

        }

        //下料区下料完成信号
        

        public bool SendCtTime(DateTime start,DateTime end)
        {
            var time = end - start;
            
            bool sendResult = true;
            List<DataItemModel> sends = new List<DataItemModel>();
            var sendcodes = (Math.Ceiling(time.TotalSeconds)).ToString();
            this._ctTimeAddress.Value = sendcodes.CastingTargetType(this._ctTimeAddress.VarType); 
            sends.Add(this._ctTimeAddress);
            sendResult &= this._sender.SendData(sends);
            return true;

        }

        private List<DataItemModel> GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction enumAddressFunction)
        {
            var targetAddress = (from m in this._connect_Device_With_PC_Function_Data_Application where m.Func == enumAddressFunction select m).ToList();

            var mm = from m in targetAddress select this._mapper.Map<DataItemModel>(m);

            return mm.ToList();
        }

    }
}
