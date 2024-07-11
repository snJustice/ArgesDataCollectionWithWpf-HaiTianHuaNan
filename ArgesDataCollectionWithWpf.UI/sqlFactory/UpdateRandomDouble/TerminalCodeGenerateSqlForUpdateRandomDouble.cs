using ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.UI.sqlFactory.UpdateRandomDouble
{
    public class TerminalCodeGenerateSqlForUpdateRandomDouble : AbstractUpdateDoubleGenerateSql
    {
        private readonly string _terminalCode;
        

        public TerminalCodeGenerateSqlForUpdateRandomDouble( string terminalCode,string targetLine, string dataIndex, string terminalLine, double max, double min) : base(targetLine, dataIndex, terminalLine, max, min)
        {
            this._terminalCode = terminalCode;
            
        }

        public override string GetSQL()
        {
            
            return  this.GetUpdatePre() 
                + " from  (" 
                + GetSelectTargetInitCodeSql() 
                 +  $" where {TableNamePre}{this._terminalLine}.data3 = '{this._terminalCode}' or  {TableNamePre}{_targetLine}.data0= '{this._terminalCode}' ) "
                 + $"as tempUpdateTable where data0 = tempUpdateTable.ss{this._targetLine}_0"; ;
        }
    }
}
