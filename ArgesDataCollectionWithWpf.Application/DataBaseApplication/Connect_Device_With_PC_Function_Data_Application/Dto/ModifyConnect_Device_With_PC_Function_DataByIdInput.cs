//zy


using ArgesDataCollectionWithWpf.DbModels.Enums;
using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Application.DataBaseApplication.Connect_Device_With_PC_Function_Data_Application.Dto
{
    public class ModifyConnect_Device_With_PC_Function_DataByIdInput
    {
        public int ID { get; set; }
        //public string DBAddress { get; set; }//db块的地址
        //public string DBOffset { get; set; }//偏移量

        public string DataAddressDescription { get; set; }//读取或写入数据地址的描述


        public EnumAddressFunction Func { get; set; }//地址的功能

        public string DBDescription { get; set; }//地址功能描述

        public EnumReadOrWrite ReadOrWrite { get; set; }//从plc读取还是pc写入

        public VarType VarType { get; set; }//数据类型
        public int DataLength { get; set; }//数据长度

        //需要知道数据属于哪个plc，一般都是一个plc
        public int CommunicationID { set; get; }





        //需要知道数据在哪条的线体
        public int LineID { set; get; }




        public int DataSaveIndex { get; set; }
    }
}
