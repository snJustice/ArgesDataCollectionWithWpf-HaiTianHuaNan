using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.UI.sqlFactory.UpdateRandomDouble
{
    public class InitCodeGenerateSqlForUpdateRandomDouble : AbstractUpdateDoubleGenerateSql
    {
        private readonly string _initCode;

        public InitCodeGenerateSqlForUpdateRandomDouble( string initCode ,string targetLine, string dataIndex, string terminalLine, double max, double min) : base(targetLine, dataIndex, terminalLine, max, min)
        {
            this._initCode = initCode;
        }

        public override string GetSQL()
        {
            return this.GetUpdatePre() + " where " + $"{TableNamePre}{_targetLine}.data0= '{this._initCode}'" ;
        }
    }
}
