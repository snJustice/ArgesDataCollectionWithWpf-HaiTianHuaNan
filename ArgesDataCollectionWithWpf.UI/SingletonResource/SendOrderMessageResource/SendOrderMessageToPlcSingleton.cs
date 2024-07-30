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


        DataItemModel _moldingTypeAddress;
        DataItemModel _poolrodsTypeAddress;
        DataItemModel _qualityAddress;
        DataItemModel _sendDownAddress;
        DataItemModel _moonQualityAddress;
        DataItemModel _dayQualityAddress;
        List<DataItemModel> _triggerAddress;



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
            this._poolrodsTypeAddress = (GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction.PollRod)).First();
            this._moldingTypeAddress = (GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction.ModlingTypeName)).First();
            
            this._qualityAddress = (GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction.ProduceQuality)).First();
            this._sendDownAddress = (GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction.SendModlingAndPollRodDone)).First();
            this._moonQualityAddress = (GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction.MonthProductionOutput)).First();
            this._dayQualityAddress = (GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction.DayProductionOutput)).First();


            this._triggerAddress = (GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction.DayProductionOutput)).ToList();

            this._sender = (ISender)this._communicationManagerDictionary[1];


        }


        public bool SendModlingPollRodQuality(int modlingType,int quality,int pollRod, int moonQuality, int dayQuality)
        {
            this._moldingTypeAddress.Value = (ushort)modlingType;
            this._qualityAddress.Value = (ushort)quality;
            this._poolrodsTypeAddress.Value = (ushort)pollRod;
            this._moonQualityAddress.Value = (ushort)moonQuality;
            this._dayQualityAddress.Value = (ushort)dayQuality;

            List<DataItemModel> sends = new List<DataItemModel>();
            sends.Add(this._moldingTypeAddress);
            sends.Add(this._qualityAddress);
            sends.Add(this._poolrodsTypeAddress);
            sends.Add(this._moonQualityAddress);
            sends.Add(this._dayQualityAddress);

            bool sendResult = this._sender.SendData(sends);

            sends.Clear();
            
            Task.Run(async () => {


                this._sendDownAddress.Value = true;
                sends.Add(this._sendDownAddress);
                sendResult &= this._sender.SendData(sends);
                await Task.Delay(1000);
                sends.Clear();
                //清空触发信号
                foreach (var item in _triggerAddress)
                {
                    item.Value = false;
                }
                this._sendDownAddress.Value = false; ;
                sends.Add(this._sendDownAddress);
                sends.AddRange(this._triggerAddress);
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

        private List<DataItemModel> GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction enumAddressFunction)
        {
            var targetAddress = (from m in this._connect_Device_With_PC_Function_Data_Application where m.Func == enumAddressFunction select m).ToList();

            var mm = from m in targetAddress select this._mapper.Map<DataItemModel>(m);

            return mm.ToList();
        }

    }
}
