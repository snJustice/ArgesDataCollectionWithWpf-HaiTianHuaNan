using Abp.Application.Services;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.OrdersFromMesApplication.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Application.DataBaseApplication.OrdersFromMesApplication
{
    public  interface IOrdersFromMesApplication: IApplicationService
    {
        int InsertOrdersFromMes(List<AddOrdersFromMesInput> addOrdersFromMesInput);
        int InsertOrUpdateOrdersFromMes(List<AddOrUpdateOrdersFromMesInput> addOrdersFromMesInput);

        int DeleteLineStationParameterByTime(DeleteOrdersFromMesByTimeInput  deleteOrdersFromMesByTimeInput);

        List<QuerryOrdersFromMesOutput> QuerryAllOrdersFromMesByDate(DateTime startDate, DateTime endDate);
        
    }
}
