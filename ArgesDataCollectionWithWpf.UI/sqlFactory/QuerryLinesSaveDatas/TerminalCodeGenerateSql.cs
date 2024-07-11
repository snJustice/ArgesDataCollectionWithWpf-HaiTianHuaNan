using ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.UI.sqlFactory
{
    public class TerminalCodeGenerateSql : AbstractGetSql
    {
        //最后一条线支持初始和终端条码的查询
        private readonly string _terminalCode;
        private readonly string _targetLine;

        public TerminalCodeGenerateSql(string terminalCode,string targetLine,List<QuerryConnect_Device_With_PC_Function_DataOutput> mainLineData, params List<QuerryConnect_Device_With_PC_Function_DataOutput>[] fuLines) : base(targetLine,mainLineData, fuLines)
        {
            this._terminalCode = terminalCode;
            this._targetLine = targetLine;
        }

        public override string GetSQL()
        {
            return this.GetPreSqlForSelect() + " where " + $"{TableNamePre}{_targetLine}.data3= '{this._terminalCode}' or  {TableNamePre}{_targetLine}.data0= '{this._terminalCode}'" + GetOrderBySql();
        }
    }
}
