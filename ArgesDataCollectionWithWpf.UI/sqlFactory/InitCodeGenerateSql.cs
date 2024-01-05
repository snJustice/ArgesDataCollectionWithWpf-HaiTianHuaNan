using ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.UI.sqlFactory
{
    public class InitCodeGenerateSql : AbstractGetSql
    {
        private readonly string _initcode;
        private readonly string _targetLine;

        public InitCodeGenerateSql(string initcode ,string targetLine,List<QuerryConnect_Device_With_PC_Function_DataOutput> mainLineData, params List<QuerryConnect_Device_With_PC_Function_DataOutput>[] fuLines) : base(mainLineData, fuLines)
        {
            this._initcode = initcode;
            this._targetLine = targetLine;
        }

        public override string GetSQL()
        {
            return this.GetPreSqlForSelect() + " where " + $"{TableNamePre}{_targetLine}.data0= '{this._initcode}'";
        }
    }
}
