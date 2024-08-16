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
        int InsertOrUpdateOrdersFromMesRunCount(List<AddOrUpdateOrdersFromMesInput> addOrdersFromMesInput);
        int InsertOrUpdateOrdersFromMesScanCount(List<AddOrUpdateOrdersFromMesInput> addOrdersFromMesInput);
        int InsertOrUpdateOrdersFromMesDownAreaSendOKCount(List<AddOrUpdateOrdersFromMesInput> addOrdersFromMesInput);
        int InsertOrUpdateOrdersFromMesLoadAreaSendOKCount(List<AddOrUpdateOrdersFromMesInput> addOrdersFromMesInput);
        int InsertOrUpdateOrdersFromMes(List<AddOrUpdateOrdersFromMesInput> addOrdersFromMesInput);

        int DeleteLineStationParameterByTime(DeleteOrdersFromMesByTimeInput  deleteOrdersFromMesByTimeInput);

        int DeleteLineStationParameterByIndex(int id);

        List<QuerryOrdersFromMesOutput> QuerryAllOrdersFromMesByDate(DateTime startDate, DateTime endDate);
        
    }
}
