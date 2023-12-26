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

            return new PlcAddressAndDatabaseAndCommunicationCombineEntityWithWriteResult {  
                Entity = data,
                SaveResult = this._iSaveDatasApplication.AddSaveDatasToDataBase(MapData(data)) };

           
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
