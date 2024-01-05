using ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.UI.sqlFactory
{
    public abstract  class AbstractGetSql : IGenerateSql
    {
        protected const string TableNamePre = "savedatas";
        private readonly List<QuerryConnect_Device_With_PC_Function_DataOutput> _mainLineData;
        private readonly List<QuerryConnect_Device_With_PC_Function_DataOutput>[] _fuLines;

        public AbstractGetSql(List<QuerryConnect_Device_With_PC_Function_DataOutput> mainLineData,params List<QuerryConnect_Device_With_PC_Function_DataOutput>[] fuLines)
        {
            this._mainLineData = mainLineData;
            this._fuLines = fuLines;
        }

        protected string GetPreSqlForSelect()
        {

            string sqlLeftJoin = "select ";
            string fromsqlMain = "";

            string fromsqlFu = "";
            foreach (var item in this._mainLineData)
            {
                sqlLeftJoin += $"{TableNamePre}{item.LineID}.data{item.DataSaveIndex} as {item.DataAddressDescription}{item.LineID},";
                fromsqlMain = $"{TableNamePre}{item.LineID}";
            }

            foreach (var item in this._fuLines)
            {
                
                foreach (var item2 in item)
                {
                    sqlLeftJoin += $"{TableNamePre}{item2.LineID}.data{item2.DataSaveIndex} as {item2.DataAddressDescription}{item2.LineID},";
                    
                }
            }
            sqlLeftJoin = sqlLeftJoin.Substring(0, sqlLeftJoin.Length - 1);

            //left join 部分
            sqlLeftJoin += $" from {fromsqlMain}";

            foreach (var item in this._fuLines)
            {
                var oneData = item.First();
                string lineTableName = "";
                if (oneData!=null)
                {
                    lineTableName = $"{TableNamePre}{oneData.LineID}";
                    sqlLeftJoin += $" left join {lineTableName} on {fromsqlMain}.data0 = {lineTableName}.data0 ";
                }


            }




            return sqlLeftJoin;

        }

        public abstract   string GetSQL();
        
    }
}
