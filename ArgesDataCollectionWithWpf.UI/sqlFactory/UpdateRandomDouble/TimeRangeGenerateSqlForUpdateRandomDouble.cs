using ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application.Dto;
using ArgesDataCollectionWithWpf.UI.sqlFactory.UpdateRandomDouble;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.UI.sqlFactory
{
    public  class TimeRangeGenerateSqlForUpdateRandomDouble: AbstractUpdateDoubleGenerateSql
    {
        
        
        private readonly DateTime _startTime;
        private readonly DateTime _endTime;
        
        string moduleSql = @"UPDATE public.savedatas1
	SET data5= random()*(4.0-1.0)+1.0
	WHERE  to_date(SaveDatas1.data1, 'YYYY-MM-DD HH24:MI:SS') 
between 
to_date('2023-11-20 00:00:00', 'YYYY-MM-DD HH24:MI:SS') 
and 
to_date('2023-11-23 01:00:00', 'YYYY-MM-DD HH24:MI:SS');";

        public TimeRangeGenerateSqlForUpdateRandomDouble(DateTime startTime,DateTime endTime ,string targetLine,string dataIndex,string terminalLine, double max, double min) : base(targetLine, dataIndex, terminalLine, max, min)
        {
           

            this._startTime = startTime;
            this._endTime = endTime;
            
        }

        public override string GetSQL()
        {
            

            

            return GetUpdatePre()
                +" from  ("
                + GetSelectTargetInitCodeSql()
                + "  where " + GetTimeStringNoChangeToDate($"{TableNamePre}{this._terminalLine}.data1")
                + "between "
                + GetTimeString(this._startTime) + "and"
                + GetTimeString(this._endTime)+")"
                + $"  as tempUpdateTable where data0 = tempUpdateTable.ss{this._targetLine}_0";



        }


        

    }
}
