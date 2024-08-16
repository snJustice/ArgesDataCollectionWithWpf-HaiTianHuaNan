using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Application.DataBaseApplication.OrdersFromMesApplication.Dto
{
    public class AddOrUpdateOrdersFromMesInput
    {
        public int ID { get; set; }


        public string WorkOrderID { get; set; }
        public string OrderID { get; set; }




        public string MoldingMachineSerialName { set; get; }
        public string MoldingMachineName { set; get; }

        public int ProduceQuantity { set; get; }
        public DateTime ProduceDate { set; get; }

        //下发的拉杆的型号
        public int StackNumber { set; get; } = 0;

        //此订单的完成状态
        public int Status { set; get; } = 0;

        //已经运行了几个产品，为了特殊情况，断电停线，能够继续工作
        public int RunnedCount { set; get; }
        public int ScanedCount { set; get; } 

        public int ProduceQueneNumber { set; get; }

        public int IsJump { get; set; }

        public int IsLoadMaterialAreaSendOrder { set; get; }

        //是否下料区下发

        public int IsDownMaterialAreaSendOrder { set; get; }
    }
}
