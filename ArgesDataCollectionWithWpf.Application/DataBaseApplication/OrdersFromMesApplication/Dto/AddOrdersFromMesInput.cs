namespace ArgesDataCollectionWithWpf.Application.DataBaseApplication.OrdersFromMesApplication
{
    public  class AddOrdersFromMesInput
    {
        public int ID { get; set; }
        public string OrderID { get; set; }

        public string MoldingMachineName { set; get; }

        public int Number { set; get; }
        public int ProduceQuantity { set; get; }

        public DateTime ProduceDate { set; get; }

    }
}