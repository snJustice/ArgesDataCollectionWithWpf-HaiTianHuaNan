//zy
/*
 * 这个类需要把一条线的信息获取到，生成一个线的逻辑，线的信息和数据会和界面进行 绑定，
 * 
 * 方法1：根据线体的信息，生成一个流程，获得一个 开始和停止的starter
 * 
 * 方法2：
 * 
 * */

using Abp.Dependency;
using ArgesDataCollectionWpf.DataProcedure.DataFlow.TimerAcquire;
using EnterpriseFD.Dataflow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.LineStationParameterApplication;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.LineStationParameterApplication.Dto;
using ArgesDataCollectionWithWpf.DbModels.Models;
using ArgesDataCollectionWithWpf.DbModels.Enums;
using ArgesDataCollectionWpf.DataProcedure.Entities;
using ArgesDataCollectionWpf.DataProcedure.DataFlow.Handlers;
using ArgesDataCollectionWithWpf.DbModels.CommunicationParaTransferModel;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application.Dto;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application;
using AutoMapper;
using ArgesDataCollectionWpf.DataProcedure.DataFlow.Transformers;
using EnterpriseFD.Dataflow.Transformers;
using EnterpriseFD.Dataflow.Routers;
using ArgesDataCollectionWpf.DataProcedure.DataFlow.Checkers;
using EnterpriseFD.Dataflow.Checkers;
using ArgesDataCollectionWithWpf.Communication;

namespace ArgesDataCollectionWpf.DataProcedure.Generate
{
    
    public class OneLogicLine
    {
        private readonly QuerryLineStationParameterOutput _querryLineStationParameterOutput;

        private readonly List<QuerryConnect_Device_With_PC_Function_DataOutput> _connect_Device_With_PC_Function_Data_Application;
        private readonly IMapper _mapper;

        private readonly CommunicationManagerDictionary _communicationManagerDictionary;

        public OneLogicLine(QuerryLineStationParameterOutput querryLineStationParameterOutput,
            List<QuerryConnect_Device_With_PC_Function_DataOutput> connect_Device_With_PC_Function_Data_Application,
            IMapper mapper,
            CommunicationManagerDictionary communicationManagerDictionary)
        {
            this._querryLineStationParameterOutput = querryLineStationParameterOutput;
            this._connect_Device_With_PC_Function_Data_Application = connect_Device_With_PC_Function_Data_Application;
            this._mapper = mapper;
            this._communicationManagerDictionary = communicationManagerDictionary;
        }

        //不能用注册的方式，注册了的话，就不会消除了。
        public IStarter GetOneLineStarter()
        {
            //是否有心跳，
            var socketAddress = GetTargetEnumsFuncConnect_Device_Data(EnumAddressFunction.Socket);

            //是否有触发
            var triggerAddress = GetTargetEnumsFuncConnect_Device_Data(EnumAddressFunction.Trigger);

            //是否有界面显示的信息
            var uiShowAddress = GetTargetEnumsFuncConnect_Device_Data(EnumAddressFunction.ReadAndNeedUpShowOnUi);

            //是否有界面写入plc
            var uiShowAndWriteAddress = GetTargetEnumsFuncConnect_Device_Data(EnumAddressFunction.UIWriteData);

            //是否有保存到数据库地址
            var saveDatabaseAddress = GetTargetEnumsFuncConnect_Device_Data(EnumAddressFunction.ReadAndNeedSaveData);

            //是否有okng信息
            var saveOkAddress = GetTargetEnumsFuncConnect_Device_Data(EnumAddressFunction.SaveOK);
            var saveNgAddress = GetTargetEnumsFuncConnect_Device_Data(EnumAddressFunction.SaveFail);

            //获得要读取并且保存数据的地址
            var saveDatas = GetTargetEnumsFuncConnect_Device_Data(EnumAddressFunction.ReadAndNeedSaveData);



            //先构造一个PlcAddressAndDatabaseAndCommunicationCombineEntity

            PlcAddressAndDatabaseAndCommunicationCombineEntity entity = new PlcAddressAndDatabaseAndCommunicationCombineEntity();

            entity.TableName = this._querryLineStationParameterOutput.TargetTableName;
            entity.IsAllowReWrite = this._querryLineStationParameterOutput.IsMainStation ? 0 : 1;
            entity.DataItems = saveDatas;
            entity.Communication = (ISender)this._communicationManagerDictionary[this._connect_Device_With_PC_Function_Data_Application.First().ID];

            //定义一个起始的路由
            PolynaryRouter<PlcAddressAndDatabaseAndCommunicationCombineEntity> startRoutersAllHandler = new PolynaryRouter<PlcAddressAndDatabaseAndCommunicationCombineEntity>();
            IStarter starter = new TimerAcquisitor(startRoutersAllHandler, entity);



            //是否存在心跳地址
            IChannel<PlcAddressAndDatabaseAndCommunicationCombineEntity> socketHandler;
            if (socketAddress!=null && socketAddress.Count>0)
            {
                socketHandler = new SocketHandler(socketAddress.First());
                startRoutersAllHandler.Successors.Add(socketHandler);
            }

            //是否存在触发地址
            IChannel<PlcAddressAndDatabaseAndCommunicationCombineEntity> triggerRouter;//= new CriterionRouter();
            IChecker<PlcAddressAndDatabaseAndCommunicationCombineEntity> triggerCheck;
            AbstractRevisor<PlcAddressAndDatabaseAndCommunicationCombineEntity> readDataFromPLCTransformer; ;
            AbstractChannel<PlcAddressAndDatabaseAndCommunicationCombineEntity, PlcAddressAndDatabaseAndCommunicationCombineEntityWithWriteResult> saveDataToDatabaseTransformer;
            if (triggerAddress != null && triggerAddress.Count > 0)
            {
                triggerCheck = new TriggerChecker(triggerAddress.First());
                readDataFromPLCTransformer = new ReadDataFromPlcTransformer();
                saveDataToDatabaseTransformer = new SaveDataToDatabaseTransformer();
                readDataFromPLCTransformer.Successor = saveDataToDatabaseTransformer;

                triggerRouter = new CriterionRouter<PlcAddressAndDatabaseAndCommunicationCombineEntity>(triggerCheck, readDataFromPLCTransformer,new EmptyChannel<PlcAddressAndDatabaseAndCommunicationCombineEntity>());


                startRoutersAllHandler.Successors.Add(triggerRouter);

                //是否存在结果发送的地址
                IChannel<PlcAddressAndDatabaseAndCommunicationCombineEntityWithWriteResult> sendOkOrNgHandler;
                if (saveOkAddress != null && saveOkAddress.Count > 0 && saveNgAddress != null && saveNgAddress.Count > 0)
                {
                    sendOkOrNgHandler = new SendSaveResultToPlcHandler(saveOkAddress.First(), saveNgAddress.First());
                    
                    saveDataToDatabaseTransformer.Successor = sendOkOrNgHandler;
                }

            }


            
            return starter;
        }

        //查询需要的地址 by 地址func，并转换成dataitemmodel
        private List<DataItemModel> GetTargetEnumsFuncConnect_Device_Data(EnumAddressFunction enumAddressFunction)
        {
            var targetAddress = (from m in this._connect_Device_With_PC_Function_Data_Application where m.Func == enumAddressFunction select m).ToList();

            var mm = from m in targetAddress select this._mapper.Map<DataItemModel>(m);

            return mm.ToList();
        }


        
    }
}
