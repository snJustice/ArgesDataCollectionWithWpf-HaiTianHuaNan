using Abp.Application.Services;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.SaveDatasApplication.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Application.DataBaseApplication.SaveDatasApplication
{
    public interface ISaveDatasApplication : IApplicationService
    {
        //插入
        int AddSaveDatasToDataBase(AddSaveDatasFromPlcInput addDataFromPlcInput);

        //查询,联合查询，主要是用来显示，把多张表的数据左连接得到整体表
        SaveDatasCombineOutput GetCombineDatasByDefineGenerateSqls(string sqls, int pageIndex, int pageSize);

        //修改，随机修改一批数据
        int ModifyDataByCode(List<ModifyDatasByCodeData0Input> modifyDatasByCodeData0Inputs);

        //直接执行sql语句
        int ModifyDataByRowSql(RowSqlSaveDatas rowSqlSaveDatas);



    }
}
