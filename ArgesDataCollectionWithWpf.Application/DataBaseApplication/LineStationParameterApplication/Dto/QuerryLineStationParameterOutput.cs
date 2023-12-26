//zy


using ArgesDataCollectionWithWpf.DbModels.Enums;
using ArgesDataCollectionWithWpf.DbModels.Models;

using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Application.DataBaseApplication.LineStationParameterApplication.Dto
{
    public class QuerryLineStationParameterOutput
    {

        public int ID { get; set; }






        //station序号
        public int StationNumber { get; set; }

        //线体的描述
        public string StationDescription { get; set; }

        //属于哪条线，同线的工站数据会查询,根据主站进行查询

        public int BelongLineNumber { get; set; }

        //表明此工站是否启用，是否需要读取和保存数据

        public bool IsUse { get; set; }


        //插入还是可覆盖，有线体只能插入，有线体可覆盖，0是插入，1是可覆盖
        public EnumDataSaveRule IsInsertOrUpdate { get; set; }

        //是否是主站，查询的时候数据根据这个站为开始
        public bool IsMainStation { get; set; }


        //数据要保存到哪个表中去，表名
        public string TargetTableName { get; set; }







        //导航属性，要保存的 所有数据


        public List<Connect_Device_With_PC_Function_Data_Model> DataAddressAndFunction { get; set; }
    }
}
