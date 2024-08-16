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
using Microsoft.Extensions.Logging;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.LineStationTableApplication;
using ArgesDataCollectionWpf.DataProcedure.Utils;

using ArgesDataCollectionWpf.DataProcedure.Utils.Quene;
using ArgesDataCollectionWithWpf.Application.OtherModelDto;
using Microsoft.Extensions.Configuration;

namespace ArgesDataCollectionWpf.DataProcedure.Generate
{
    
    public class OneLogicLine
    {
        private const string preTabelName = "SaveDatas";

        private readonly QuerryLineStationParameterOutput _querryLineStationParameterOutput;

        private readonly List<QuerryConnect_Device_With_PC_Function_DataOutput> _connect_Device_With_PC_Function_Data_Application;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly CommunicationManagerDictionary _communicationManagerDictionary;
        private readonly ILineStationTableApplication _ilineStationTableApplication;
        private readonly IWriteLogForUserControl _controlLog;//地址的显示
        private readonly IWriteLogForUserControl _controlLogs;//日志


        public TriggerChecker LoadTriggerChecker { get; set; }
        public TriggerChecker DownTriggerChecker { get; set; }

        public OneLogicLine(QuerryLineStationParameterOutput querryLineStationParameterOutput,
            List<QuerryConnect_Device_With_PC_Function_DataOutput> connect_Device_With_PC_Function_Data_Application,
            IMapper mapper,
            ILogger logger,
            CommunicationManagerDictionary communicationManagerDictionary,
            ILineStationTableApplication ilineStationTableApplication,
            IWriteLogForUserControl controlLog,
            IWriteLogForUserControl controlLogs)
        {
            this._querryLineStationParameterOutput = querryLineStationParameterOutput;
            this._connect_Device_With_PC_Function_Data_Application = connect_Device_With_PC_Function_Data_Application;
            this._mapper = mapper;
            this._logger = logger;
            this._communicationManagerDictionary = communicationManagerDictionary;
            this._ilineStationTableApplication = ilineStationTableApplication;
            this._controlLog = controlLog;
            this._controlLogs = controlLogs;
        }


        public bool GenerateTable()
        {
            var tableName = preTabelName + this._querryLineStationParameterOutput.TargetTableName;
            ;

            return this._ilineStationTableApplication.InsertTable(tableName) == 1?true:false;
        }


        //不能用注册的方式，注册了的话，就不会消除了。
        public IStarter GetOneLineStarter()
        {
            var quenes = IocManager.Instance.Resolve<CustomerQueneForCodesFromMes>();



            //是否有心跳，
            var socketAddress = GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction.Socket);

            //是否有触发，触发这里还要加一层逻辑，如果不是plc来的数据，要进行解析
            var triggerAddress = GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction.Trigger);

            //是否有界面显示的信息
            var uiShowAddress = GetTargetEnumsFuncConnect_Device_Data(EnumAddressFunction.ReadAndNeedUpShowOnUi);

            //是否有界面写入plc
            var uiShowAndWriteAddress = GetTargetEnumsFuncConnect_Device_Data(EnumAddressFunction.UIWriteData);

            //是否有保存到数据库地址
            var saveDatabaseAddress = GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction.ReadAndNeedSaveData);

            //是否有okng信息
            var saveOkAddress = GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction.SaveOK);
            var saveNgAddress = GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction.SaveFail);

            //获得要读取并且保存数据的地址
            var saveDatas = GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction.ReadAndNeedSaveData);

            //获得要读取并且保存数据的地址,但不是从PLC来,这里数据要从
            var saveDatasNotFromPLC = (from m in this._connect_Device_With_PC_Function_Data_Application where m.Func == EnumAddressFunction.ReadAndNeedSaveDataNotFromPLC select m).ToList();

            //是否有日生产信息
            var dayProductionAddress = GetTargetEnumsFuncConnect_Device_Data(EnumAddressFunction.DayProductionOutput);

            //是否有月生产信息
            var monthProductionAddress = GetTargetEnumsFuncConnect_Device_Data(EnumAddressFunction.MonthProductionOutput);

            //是否有cttime信息
            var ctTimeAddress = GetTargetEnumsFuncConnect_Device_Data(EnumAddressFunction.CTTime);

            //上料区请求订单信号
            var loadMaterialtriggerAddress = GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction.LoadMaterialAreaNeedNewOrder);
            //上料区订单下发完成信号
            var downMaterialAreatriggerAddress = GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction.DownMaterialAreaNeedNewOrder);


            //先构造一个PlcAddressAndDatabaseAndCommunicationCombineEntity

            PlcAddressAndDatabaseAndCommunicationCombineEntity entity = new PlcAddressAndDatabaseAndCommunicationCombineEntity();

            entity.TableName = preTabelName+ this._querryLineStationParameterOutput.TargetTableName;
            entity.IsAllowReWrite = this._querryLineStationParameterOutput.IsMainStation ? 0 : 1;
            entity.DataItems = saveDatas;
            entity.DataItemsNotFromPLC = saveDatasNotFromPLC;
            entity.ModlingCodeFromMes = null;

            if (this._connect_Device_With_PC_Function_Data_Application==null || this._connect_Device_With_PC_Function_Data_Application.Count()<=0)
            {
                entity.Communication = null;
            }
            else
            {
                entity.Communication = (ISender)this._communicationManagerDictionary[this._connect_Device_With_PC_Function_Data_Application.First().CommunicationID];
            }

            
            entity.LogAndShowHandler = new DataFlow.Interceptors.CustomerExceptionHandler(this._logger, this._controlLogs);

            //定义一个起始的路由
            PolynaryRouter<PlcAddressAndDatabaseAndCommunicationCombineEntity> startRoutersAllHandler = new PolynaryRouter<PlcAddressAndDatabaseAndCommunicationCombineEntity>();
            IStarter starter = new TimerAcquisitor(startRoutersAllHandler, entity);



            //是否存在心跳地址，
            IChannel<PlcAddressAndDatabaseAndCommunicationCombineEntity> socketHandler1;

            IChannel<PlcAddressAndDatabaseAndCommunicationCombineEntity> socketHandler2;
            if (socketAddress!=null && socketAddress.Count>=1)
            {
                var s1 = (from m in socketAddress orderby m.DataInDatabaseIndex select m).ToList();
                socketHandler1 = new SocketHandler(s1.First());
                startRoutersAllHandler.Successors.Add(socketHandler1);

                //socketHandler2 = new SocketHandler(s1[1]);
                //startRoutersAllHandler.Successors.Add(socketHandler2);

            }


            //是否存在触发地址,如果存在两个的话，那就准备多条路径
            IChannel<PlcAddressAndDatabaseAndCommunicationCombineEntity> triggerRouter;//= new CriterionRouter();
            IChannel<PlcAddressAndDatabaseAndCommunicationCombineEntity> triggerRouter2;//= new CriterionRouter();
            IChecker<PlcAddressAndDatabaseAndCommunicationCombineEntity> triggerCheck1;
            IChecker<PlcAddressAndDatabaseAndCommunicationCombineEntity> triggerCheck2;
            AbstractRevisor<PlcAddressAndDatabaseAndCommunicationCombineEntity> readDataFromPLCTransformer; ;
            AbstractChannel<PlcAddressAndDatabaseAndCommunicationCombineEntity, PlcAddressAndDatabaseAndCommunicationCombineEntityWithWriteResult> saveDataToDatabaseTransformer;
            
            if (triggerAddress != null && triggerAddress.Count >=2 )
            {
                var configuration = IocManager.Instance.Resolve<IConfiguration>();
                var s1 = (from m in triggerAddress orderby m.DataInDatabaseIndex select m).ToList();




                triggerCheck1 = new TriggerChecker(s1[0]);
                ReadDatasFromTargetQuene  getCode1= new ReadDatasFromTargetQuene(quenes,1, configuration["StationNumberOne"]);
                readDataFromPLCTransformer = new ReadDataFromPlcTransformer();
                saveDataToDatabaseTransformer = new SaveDataToDatabaseTransformer();
                getCode1.Successor = readDataFromPLCTransformer;
                
                readDataFromPLCTransformer.Successor = saveDataToDatabaseTransformer;
                triggerRouter = new CriterionRouter<PlcAddressAndDatabaseAndCommunicationCombineEntity>(triggerCheck1, getCode1, new EmptyChannel<PlcAddressAndDatabaseAndCommunicationCombineEntity>());
                startRoutersAllHandler.Successors.Add(triggerRouter);


                triggerCheck2 = new TriggerChecker(s1[1]);
                
                ReadDatasFromTargetQuene getCode12= new ReadDatasFromTargetQuene(quenes, 2, configuration["StationNumberTwo"]);
                getCode12.Successor = readDataFromPLCTransformer;
                triggerRouter2 = new CriterionRouter<PlcAddressAndDatabaseAndCommunicationCombineEntity>(triggerCheck2, getCode12, new EmptyChannel<PlcAddressAndDatabaseAndCommunicationCombineEntity>());
                startRoutersAllHandler.Successors.Add(triggerRouter2);





                //是否存在结果发送的地址
                IChannel<PlcAddressAndDatabaseAndCommunicationCombineEntityWithWriteResult> sendOkOrNgHandler;
                if (saveOkAddress != null && saveOkAddress.Count > 0 && saveNgAddress != null && saveNgAddress.Count > 0)
                {
                    sendOkOrNgHandler = new SendSaveResultToPlcHandler(saveOkAddress.First(), saveNgAddress.First());
                    
                    saveDataToDatabaseTransformer.Successor = sendOkOrNgHandler;
                }

            }



            //是否存在上料区订单请求地址
            IChannel<PlcAddressAndDatabaseAndCommunicationCombineEntity> loadMaterialAreaTriggerRouter;//= new CriterionRouter();
            IChecker<PlcAddressAndDatabaseAndCommunicationCombineEntity> loadMaterialAreaTriggerCheck1;
            if (loadMaterialtriggerAddress != null )
            {
                loadMaterialAreaTriggerCheck1 = new TriggerChecker(loadMaterialtriggerAddress.First());
                IChannel<PlcAddressAndDatabaseAndCommunicationCombineEntity>  postLoadMaterialArea = new LoadMaterialAndDownMaterialAreaHandler(quenes, LoadOrDwonEnum.LoadMaterialArea);
                loadMaterialAreaTriggerRouter = new CriterionRouter<PlcAddressAndDatabaseAndCommunicationCombineEntity>(loadMaterialAreaTriggerCheck1, postLoadMaterialArea, new EmptyChannel<PlcAddressAndDatabaseAndCommunicationCombineEntity>());

                startRoutersAllHandler.Successors.Add(loadMaterialAreaTriggerRouter);
                LoadTriggerChecker = (TriggerChecker)loadMaterialAreaTriggerCheck1;

            }

            //是否存在下料区订单请求地址
            IChannel<PlcAddressAndDatabaseAndCommunicationCombineEntity> downMaterialAreaTriggerRouter;//= new CriterionRouter();
            IChecker<PlcAddressAndDatabaseAndCommunicationCombineEntity> downMaterialAreaTriggerCheck1;
            if (downMaterialAreatriggerAddress != null)
            {
                downMaterialAreaTriggerCheck1 = new TriggerChecker(downMaterialAreatriggerAddress.First());
                IChannel<PlcAddressAndDatabaseAndCommunicationCombineEntity> postDownMaterialArea = new LoadMaterialAndDownMaterialAreaHandler(quenes, LoadOrDwonEnum.DownMaterialArea);
                downMaterialAreaTriggerRouter = new CriterionRouter<PlcAddressAndDatabaseAndCommunicationCombineEntity>(downMaterialAreaTriggerCheck1, postDownMaterialArea, new EmptyChannel<PlcAddressAndDatabaseAndCommunicationCombineEntity>());

                startRoutersAllHandler.Successors.Add(downMaterialAreaTriggerRouter);

                DownTriggerChecker = (TriggerChecker)downMaterialAreaTriggerCheck1;
            }


            //是否存在uishow数据
            IChannel<PlcAddressAndDatabaseAndCommunicationCombineEntity> uiShowHnadler; ;
            if (uiShowAddress != null  && dayProductionAddress != null && monthProductionAddress != null && ctTimeAddress!=null)
            {
                var uiShowAddressDataModel = GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction.ReadAndNeedUpShowOnUi );
                var dayProductionShowAddressDataModel = GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction.DayProductionOutput );
                var monthProductionShowAddressDataModel = GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction.MonthProductionOutput );
                var ctTimeShowAddressDataModel = GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction.CTTime );
                
                uiShowHnadler = new UiShowHnadler(uiShowAddressDataModel, this._controlLog, dayProductionShowAddressDataModel, monthProductionShowAddressDataModel, ctTimeShowAddressDataModel);
                startRoutersAllHandler.Successors.Add(uiShowHnadler);
            }



            uiShowAddress.AddRange(dayProductionAddress);
            uiShowAddress.AddRange(monthProductionAddress);
            uiShowAddress.AddRange(ctTimeAddress);

            //形成查看数据地址的ui
            this._controlLog.AddUiShowAndModifyControls(uiShowAddress);

            




            return starter;
        }

        //查询需要的地址 by 地址func，并转换成dataitemmodel
        private List<DataItemModel> GetTargetEnumsFuncConnect_Device_DataMapperToDataModel(EnumAddressFunction enumAddressFunction)
        {
            var targetAddress = (from m in this._connect_Device_With_PC_Function_Data_Application where m.Func == enumAddressFunction select m).ToList();

            var mm = from m in targetAddress select this._mapper.Map<DataItemModel>(m);

            return mm.ToList();
        }

        private List<QuerryConnect_Device_With_PC_Function_DataOutput> GetTargetEnumsFuncConnect_Device_Data(EnumAddressFunction enumAddressFunction)
        {
            var targetAddress = (from m in this._connect_Device_With_PC_Function_Data_Application where m.Func == enumAddressFunction select m).ToList();

           

            return targetAddress.ToList();
        }

    }


    
}
