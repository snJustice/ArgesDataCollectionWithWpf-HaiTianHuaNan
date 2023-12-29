using ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWpf.DataProcedure.Utils
{
    public  interface IWriteLogForUserControl
    {
        public void WriteLog(string message);

        public void  AddUiShowAndModifyControls(List<QuerryConnect_Device_With_PC_Function_DataOutput> querryConnect_Device_With_PC_Function_DataOutputs);

        public void ChangeUiValueFromPlc(int dataIndex, object value);
        
    }
}
