using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.UI.sqlFactory.UpdateRandomDouble
{
    public abstract class AbstractUpdateDoubleGenerateSql : AbstractGenerateSQL
    {
        protected readonly string _targetLine;
        protected readonly string _dataIndex;
        protected readonly string _terminalLine;
        protected readonly double _max;
        protected readonly double _min;

        

        public AbstractUpdateDoubleGenerateSql(string targetLine,string dataIndex , string terminalLine, double max, double min):base(targetLine)
        {
            this._targetLine = targetLine;
            this._dataIndex = dataIndex;
            this._terminalLine = terminalLine;
            this._max = max;
            this._min = min;
        }

        protected string GetUpdatePre()
        {
            string pre = $"UPDATE public.{TableNamePre}{this._targetLine} SET data{this._dataIndex}= random()*({this._max}-{this._min})+{this._min}  ";
            return pre;
        }



        protected string GetSelectTargetInitCodeSql()
        {
            string sqlLeftJoin = "select ";

            if (this._targetLine != this._terminalLine)
            {
                sqlLeftJoin += $" {TableNamePre}{this._targetLine}.data0 as ss{this._targetLine}_0, {TableNamePre}{this._targetLine}.data1 as ss{this._targetLine}_1, ";
            }
            

            sqlLeftJoin += $" {TableNamePre}{this._terminalLine}.data0 as ss{this._terminalLine}_0, {TableNamePre}{this._terminalLine}.data1 as ss{this._terminalLine}_1 ";

            sqlLeftJoin += $" from {TableNamePre}{this._terminalLine}";

            if (this._targetLine == this._terminalLine)
            {
                
            }
            else
            {
                sqlLeftJoin += $" left join {TableNamePre}{this._targetLine} on {TableNamePre}{this._terminalLine}.data0 = {TableNamePre}{this._targetLine}.data0 ";
                
            }


            return sqlLeftJoin;




            
        }

        
    }
}
