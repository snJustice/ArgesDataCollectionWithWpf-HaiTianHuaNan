using ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.UI.sqlFactory
{
    public class TimeRangeGenerateSql : AbstractGetSql
    {
        private readonly DateTime _startTime;
        private readonly DateTime _endTime;
        private readonly string _targetLine;

        public TimeRangeGenerateSql(DateTime startTime,DateTime endTime,string targetLine,List<QuerryConnect_Device_With_PC_Function_DataOutput> mainLineData, params List<QuerryConnect_Device_With_PC_Function_DataOutput>[] fuLines) : base(mainLineData, fuLines)
        {
            this._startTime = startTime;
            this._endTime = endTime;
            this._targetLine = targetLine;
        }

        public override string GetSQL()
        {
            string sql = this.GetPreSqlForSelect() 
                + "where " + GetTimeStringNoChangeToDate($"{TableNamePre}{this._targetLine}.data1") 
                + "between " 
                + GetTimeString(this._startTime) + "and"
                + GetTimeString(this._endTime)+
                "order by " + GetTimeStringNoChangeToDate($"{TableNamePre}{this._targetLine}.data1");


            return sql;
        }


        private string GetTimeString(DateTime time)
        {
            string timeString = time.ToString("yyyy-MM-dd HH-mm-ss");
            string timeConditionSql = $" to_date('{timeString}', 'YYYY-MM-DD HH24:MI:SS')";
            return timeConditionSql;


        }
        private string GetTimeString(string time)
        {
            
            string timeConditionSql = $" to_date('{time}', 'YYYY-MM-DD HH24:MI:SS')";
            return timeConditionSql;


        }

        private string GetTimeStringNoChangeToDate(string time)
        {

            string timeConditionSql = $" to_date({time}, 'YYYY-MM-DD HH24:MI:SS')";
            return timeConditionSql;


        }
    }
}
