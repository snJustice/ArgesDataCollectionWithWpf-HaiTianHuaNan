using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Application.DataBaseApplication.OrdersFromMesApplication
{
    public  interface IOrdersFromMesApplication: IApplicationService
    {
        int InsertOrdersFromMes(AddOrdersFromMesInput addOrdersFromMesInput);

        int DeleteLineStationParameterByTime(DeleteOrdersFromMesByTimeInput  deleteOrdersFromMesByTimeInput);

        List<QuerryOrdersFromMesByDateOutput> QuerryAllOrdersFromMesByDate(DateTime startDate, DateTime endDate);
    }
}
