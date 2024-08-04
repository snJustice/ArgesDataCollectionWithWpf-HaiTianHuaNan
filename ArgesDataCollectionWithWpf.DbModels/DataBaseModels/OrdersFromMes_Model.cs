using ArgesDataCollectionWithWpf.DbModels.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.DbModels.Models
{
    [MappingToDatabase]
    [SugarTable("OrdersFromMes")]
    public class OrdersFromMes_Model
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }

        
        public string OrderID { get; set; }
        public string WorkOrderID { get; set; }



        public string MoldingMachineSerialName { set; get; }
        public string MoldingMachineName { set; get; }

        public int ProduceQuantity { set; get; }
        public DateTime ProduceDate { set; get; }

        //下发的拉杆的型号
        public int StackNumber { set; get; } 

        //此订单的完成状态
        public int Status { set; get; } 

        //已经运行了几个产品，为了特殊情况，断电停线，能够继续工作
        public int RunnedCount { set; get; }

        //扫描了几个产品，这个信息可能没用，可能有用
        public int ScanedCount { set; get; }

        public int ProduceQueneNumber { set; get; }


        //是否跳单
        public int IsJump { get; set; }








    }
}
