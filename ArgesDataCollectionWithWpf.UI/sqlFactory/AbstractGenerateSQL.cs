using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.UI.sqlFactory
{
    public abstract class AbstractGenerateSQL : IGenerateSql
    {
        protected readonly string _target;
        public AbstractGenerateSQL(string target)
        {
            this._target = target;
        }

        protected const string TableNamePre = "savedatas";
        public abstract string GetSQL();

        protected string GetOrderBySql()
        {
            return "order by " + $"{TableNamePre}{this._target}.data1";
        }

        protected string GetTimeString(DateTime time)
        {
            string timeString = time.ToString("yyyy-MM-dd HH-mm-ss");
            string timeConditionSql = $" to_timestamp('{timeString}', 'YYYY-MM-DD HH24:MI:SS')";
            return timeConditionSql;


        }
        protected string GetTimeString(string time)
        {

            string timeConditionSql = $" to_timestamp('{time}', 'YYYY-MM-DD HH24:MI:SS')";
            return timeConditionSql;


        }

        protected string GetTimeStringNoChangeToDate(string time)
        {

            string timeConditionSql = $" to_timestamp({time}, 'YYYY-MM-DD HH24:MI:SS')";
            return timeConditionSql;


        }





    }
}
