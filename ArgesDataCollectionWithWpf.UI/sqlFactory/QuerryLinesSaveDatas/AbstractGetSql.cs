using ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.UI.sqlFactory
{
    public abstract  class AbstractGetSql : AbstractGenerateSQL
    {
        
        private readonly List<QuerryConnect_Device_With_PC_Function_DataOutput> _mainLineData;
        private readonly List<QuerryConnect_Device_With_PC_Function_DataOutput>[] _fuLines;

        public AbstractGetSql(string targetLine,List<QuerryConnect_Device_With_PC_Function_DataOutput> mainLineData,params List<QuerryConnect_Device_With_PC_Function_DataOutput>[] fuLines):base(targetLine)
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
                sqlLeftJoin += $"{TableNamePre}{item.LineID}.data{item.DataSaveIndex} as {item.DataAddressDescription}ss{item.LineID}{TableNameAndDataIndexSplit.TableAndIndexSplitChar}{item.DataSaveIndex},";
                fromsqlMain = $"{TableNamePre}{item.LineID}";
            }

            foreach (var item in this._fuLines)
            {
                
                foreach (var item2 in item)
                {
                    sqlLeftJoin += $"{TableNamePre}{item2.LineID}.data{item2.DataSaveIndex} as {item2.DataAddressDescription}ss{item2.LineID}{TableNameAndDataIndexSplit.TableAndIndexSplitChar}{item2.DataSaveIndex},";
                    
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

        




    }
}
