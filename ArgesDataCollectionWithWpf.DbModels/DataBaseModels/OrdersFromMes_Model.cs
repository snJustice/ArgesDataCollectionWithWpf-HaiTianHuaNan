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
        [SugarColumn(IsPrimaryKey = true)]
        public int ID { get; set; }

        
        public string OrderID { get; set; }
        public string WorkOrderID { get; set; }



        public string MoldingMachineSerialName { set; get; }
        public string MoldingMachineName { set; get; }

        public int ProduceQuantity { set; get; }
        public DateTime ProduceDate { set; get; }




        


    }
}
