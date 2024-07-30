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
        private protected DateTime _startTime;
        private protected DateTime _endTime;
        private protected string _targetLine;

        public TimeRangeGenerateSql(DateTime startTime,DateTime endTime,string targetLine,List<QuerryConnect_Device_With_PC_Function_DataOutput> mainLineData, params List<QuerryConnect_Device_With_PC_Function_DataOutput>[] fuLines) : base(targetLine,mainLineData, fuLines)
        {
            this._startTime = startTime;
            this._endTime = endTime;
            this._targetLine = targetLine;
        }

        public override string GetSQL()
        {
            string sql = this.GetPreSqlForSelect() 
                + " where " + GetTimeStringNoChangeToDate($"{TableNamePre}{this._targetLine}.data1") 
                + " between " 
                + GetTimeString(this._startTime) + " and "
                + GetTimeString(this._endTime)+
                GetOrderBySql();


            return sql;
        }


        
    }
}
