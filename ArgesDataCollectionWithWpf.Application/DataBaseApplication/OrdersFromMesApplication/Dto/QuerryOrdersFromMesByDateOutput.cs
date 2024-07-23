﻿namespace ArgesDataCollectionWithWpf.Application.DataBaseApplication.OrdersFromMesApplication
{
    public class QuerryOrdersFromMesByDateOutput
    {
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

        public List<string> Stacks { set; get; } = new List<string> { "0","1","2","3"};
    }
}