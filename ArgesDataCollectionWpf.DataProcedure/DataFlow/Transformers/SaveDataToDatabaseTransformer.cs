//zy


using Abp.Dependency;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.SaveDatasApplication;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.SaveDatasApplication.Dto;
using ArgesDataCollectionWithWpf.DbModels.CommunicationParaTransferModel;
using ArgesDataCollectionWpf.DataProcedure.Entities;
using EnterpriseFD.Dataflow.Transformers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWpf.DataProcedure.DataFlow.Transformers
{
    public class SaveDataToDatabaseTransformer : AbstractTransformer<PlcAddressAndDatabaseAndCommunicationCombineEntity, PlcAddressAndDatabaseAndCommunicationCombineEntityWithWriteResult>
    {
        ISaveDatasApplication _iSaveDatasApplication;
        public SaveDataToDatabaseTransformer(  )
        {
            this._iSaveDatasApplication = IocManager.Instance.Resolve<ISaveDatasApplication>();
        }
        protected override PlcAddressAndDatabaseAndCommunicationCombineEntityWithWriteResult DoTransform(PlcAddressAndDatabaseAndCommunicationCombineEntity data)
        {
            

            var dataWithSaveResult = new PlcAddressAndDatabaseAndCommunicationCombineEntityWithWriteResult();
            dataWithSaveResult.Entity = data;

            var saveresult = this._iSaveDatasApplication.AddSaveDatasToDataBase(MapData(data));
            dataWithSaveResult.SaveResult = saveresult;

            if (saveresult==1)
            {
                data.LogAndShowHandler.Channel(new Interceptors.LogMessage { Level = Microsoft.Extensions.Logging.LogLevel.Information, Message = "保存数据成功" });
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


            return addSaveDatasFromPlcInput;



        }
    }
}
