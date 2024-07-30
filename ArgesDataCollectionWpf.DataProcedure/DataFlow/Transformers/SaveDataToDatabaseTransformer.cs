//zy


using Abp.Dependency;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.ModlingCodesApplication.Dto;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.SaveDatasApplication;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.SaveDatasApplication.Dto;
using ArgesDataCollectionWithWpf.DbModels.CommunicationParaTransferModel;
using ArgesDataCollectionWpf.DataProcedure.Entities;
using ArgesDataCollectionWpf.DataProcedure.Utils.Quene;
using EnterpriseFD.Dataflow.Transformers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ArgesDataCollectionWpf.DataProcedure.DataFlow.Transformers
{
    public class SaveDataToDatabaseTransformer : AbstractTransformer<PlcAddressAndDatabaseAndCommunicationCombineEntity, PlcAddressAndDatabaseAndCommunicationCombineEntityWithWriteResult>
    {
        ISaveDatasApplication _iSaveDatasApplication;
        BufferBlock<QuerryModlingCodesOutputWithEndTime> _runnedQuene;
        public SaveDataToDatabaseTransformer(  )
        {
            
            this._iSaveDatasApplication = IocManager.Instance.Resolve<ISaveDatasApplication>();
            var quene =   IocManager.Instance.Resolve<CustomerQueneForCodesFromMes>();

            this._runnedQuene = quene.MainRunedQuene;

        }
        protected override PlcAddressAndDatabaseAndCommunicationCombineEntityWithWriteResult DoTransform(PlcAddressAndDatabaseAndCommunicationCombineEntity data)
        {
            

            var dataWithSaveResult = new PlcAddressAndDatabaseAndCommunicationCombineEntityWithWriteResult();
            dataWithSaveResult.Entity = data;

            var saveresult = this._iSaveDatasApplication.AddSaveDatasToDataBase(MapData(data));
            dataWithSaveResult.SaveResult = saveresult;

            //保存OK的时候推送一个post
            if (saveresult==1)
            {
                data.LogAndShowHandler.Channel(new Interceptors.LogMessage { Level = Microsoft.Extensions.Logging.LogLevel.Information, Message = "保存数据成功" });

                this._runnedQuene.Post(data.ModlingCodeFromMes); ;
            }
            else if (saveresult==2)
            {
                data.LogAndShowHandler.Channel(new Interceptors.LogMessage { Level = Microsoft.Extensions.Logging.LogLevel.Information, Message = "保存覆盖写入成功" });
            }
            else if (saveresult == -3)
            {
                data.LogAndShowHandler.Channel(new Interceptors.LogMessage { Level = Microsoft.Extensions.Logging.LogLevel.Information, Message = "保存数据失败，数据重复插入"  });
            }
            else
            {
                data.LogAndShowHandler.Channel(new Interceptors.LogMessage { Level = Microsoft.Extensions.Logging.LogLevel.Information, Message = "保存数据失败，错误原因:"+ saveresult.ToString() });
            }
            return dataWithSaveResult;

           
        }


        private AddSaveDatasFromPlcInput MapData(PlcAddressAndDatabaseAndCommunicationCombineEntity data)
        {

            AddSaveDatasFromPlcInput addSaveDatasFromPlcInput = new AddSaveDatasFromPlcInput();
            addSaveDatasFromPlcInput.tableName = data.TableName;
            addSaveDatasFromPlcInput.IsAllowReWrite = data.IsAllowReWrite;



            //要根据名称来进行 匹配了,


            var plcData = from m in data.DataItems select m;
            //获得最大index
            var  tempMax1 = (from m in data.DataItems select m.DataInDatabaseIndex).ToList() ;
            int maxIndex = ((tempMax1==null) || (tempMax1.Count==0)) ?0 : tempMax1.Max(); ;
            
            var tempMax2 = (from m in data.DataItemsNotFromPLC select m.DataSaveIndex).ToList(); 
            int maxIndex2 = ((tempMax2 == null) || (tempMax2.Count == 0)) ? 0 : tempMax2.Max(); 
            maxIndex = maxIndex > maxIndex2 ? maxIndex : maxIndex2;


            addSaveDatasFromPlcInput.Data0 = data.ModlingCodeFromMes.Containerno;
            addSaveDatasFromPlcInput.Data1 = data.ModlingCodeFromMes.EndTime.ToString();
            addSaveDatasFromPlcInput.Data2 = data.ModlingCodeFromMes.Time.ToString();
            addSaveDatasFromPlcInput.Data3 = data.ModlingCodeFromMes.Part1;
            addSaveDatasFromPlcInput.Data4 = data.ModlingCodeFromMes.Part2;
            addSaveDatasFromPlcInput.Data5 = data.ModlingCodeFromMes.Part3;
            addSaveDatasFromPlcInput.Data6 = data.ModlingCodeFromMes.StationNumber;


            

            


            return addSaveDatasFromPlcInput;



        }



        private void SetValue( Type type, string name, object value)
        {

        }

    }




}
