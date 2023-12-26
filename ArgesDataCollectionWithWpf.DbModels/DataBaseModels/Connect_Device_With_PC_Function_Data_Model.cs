//zy


using ArgesDataCollectionWithWpf.DbModels.Enums;

using S7.Net;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.DbModels.Models
{
    [MappingToDatabase]
    //西门子相关地址的数据结构
    public class Connect_Device_With_PC_Function_Data_Model
    {
        //[SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [SugarColumn(IsPrimaryKey = true)]
        public int ID { get; set; }
        //public string DBAddress { get; set; }//db块的地址
        //public string DBOffset { get; set; }//偏移量

        //数据所在的地址,之前是用db和偏移，现在用一个直接的描述
        public string DataAddressDescription{ get; set; }//读取或写入数据地址的描述
        public EnumAddressFunction Func { get; set; }//地址的功能

        public string DBDescription { get; set; }//地址功能描述

        public EnumReadOrWrite ReadOrWrite { get; set; }//从plc读取还是pc写入

        public VaraType VarType { get; set; }//数据类型
        public int DataLength { get; set; }//数据长度

        //需要知道数据属于哪个plc，一般都是一个plc
        public int CommunicationID { set; get; }

        //[SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToOne, nameof(CommunicationID), nameof(CommunicationDetailsAndInstanceModel.ID))]
        public CommunicationDetailsAndInstanceModel CommunicationDetail { get; set; }


        //需要知道数据在哪条的线体
        public int LineID { set; get; }

        //[SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToOne, nameof(LineID),nameof(LineStationParameterModel.ID))]
        public LineStationParameterModel LineParameter { get; set; }

        public int DataSaveIndex { get; set; }
    }
}
