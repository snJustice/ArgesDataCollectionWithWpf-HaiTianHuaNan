//zy


using S7.Net;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.DbModels.CommunicationParaTransferModel.SimensS7
{
    [Serializable]
    public class PlcSimensConnectionPara
    {

        
       


        public string PLCIPAddress { get; set; }//plc的IP地址
        public int PLCPort { get; set; }//plc的端口号，基本都是502

        public int Rack { get; set; }//一般是0

        public int Slot { get; set; }//一般是1

        public CpuType CpuType { get; set; }//cpu的类型

    }
}
