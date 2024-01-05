//zy


using Abp.Extensions;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.SaveDatasApplication.Dto;
using ArgesDataCollectionWithWpf.Core;
using ArgesDataCollectionWithWpf.DbModels.Models;
using AutoMapper;
using AutoMapper.Internal.Mappers;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Npgsql;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Application.DataBaseApplication.SaveDatasApplication
{
    public class SaveDatasApplication : ArgesDataCollectionWithWpfApplicationBase, ISaveDatasApplication
    {




        public SaveDatasApplication(DbContextConnection sugarClinet, ILogger logger, IMapper objectMapper) : base(sugarClinet, logger, objectMapper)
        {



        }

        public int AddSaveDatasToDataBase(AddSaveDatasFromPlcInput addDataFromPlcInput)
        {
            SaveDatasModel model = new SaveDatasModel();
            int insertResult = -1;
            try
            {
                
                insertResult = _dbContextClinet.SugarClient.Insertable(model).AS(addDataFromPlcInput.tableName).ExecuteCommand();
                //this._logger.LogInformation("写入数据成功");
                insertResult = 1;
            }
            catch (PostgresException ex)
            {
                this._logger.LogInformation(ex.Message);
                if (ex.Code == "23505")
                {
                    if (addDataFromPlcInput.IsAllowReWrite == 1)//可覆盖
                    {
                        insertResult = _dbContextClinet.SugarClient
                            .Updateable(model)
                            .WhereColumns(it => new { it.Data0 })
                            .AS(addDataFromPlcInput.tableName)
                            .ExecuteCommand();
                        //this._logger.LogInformation("复写数据成功");
                        insertResult = 2;
                    }
                    else
                    {
                        //"数据重复";违反唯一约束了
                        //写入plc
                        //this._logger.LogError("不可覆盖的工站，插入数据重复");
                        insertResult = -3;
                    }
                }

                if (ex.Message.Contains("不存在") && ex.Message.Contains("关系"))
                {
                    insertResult = -2;
                }
               
            }



            return insertResult;
        }

        public SaveDatasCombineOutput GetCombineDatasByDefineGenerateSqls(string sqls, int pageIndex, int pageSize)
        {
            var query = _dbContextClinet.SugarClient.SqlQueryable<dynamic>(sqls);
            var sqlstringtemp = query.ToSqlString();

            if (pageIndex <= -1)
            {
                //一般用来导出
                return new SaveDatasCombineOutput { AllCombineDatas = query.ToList(), PageCount = 1 };

            }
            else
            {
                int count = 0;
                var templist = query.ToPageList(pageIndex, pageSize, ref count);
                //一般用来查看数据
                return new SaveDatasCombineOutput { AllCombineDatas = templist, PageCount = count / pageSize };
            }




        }

        public int ModifyDataByCode(List<ModifyDatasByCodeData0Input> modifyDatasByCodeData0Inputs)
        {

            return _dbContextClinet.SugarClient.Ado.ExecuteCommand(GetUpdateSqlString(modifyDatasByCodeData0Inputs));
        }


        private string GetUpdateSqlString(List<ModifyDatasByCodeData0Input> modifyDatasByCodeData0Inputs)
        {

            string sqlUpdateString = "";
            Parallel.ForEach(modifyDatasByCodeData0Inputs, (m) =>
            {
                sqlUpdateString += $"UPDATE {m.tableName} set data{m.modifyIndex}={m.modifyData}  where data0={m.Data0}";
            });

            return sqlUpdateString;
        }
    }
}
