using ArgesDataCollectionWithWpf.Core;
using ArgesDataCollectionWithWpf.DbModels.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgesDataCollectionWithWpf.Application.DataBaseApplication.OrdersFromMesApplication
{
    public class OrdersFromMesApplication : ArgesDataCollectionWithWpfApplicationBase,IOrdersFromMesApplication
    {


        public OrdersFromMesApplication(DbContextConnection sugarClinet, ILogger logger, IMapper objectMapper) : base(sugarClinet, logger, objectMapper)
        {
        }
        public int DeleteLineStationParameterByTime(DeleteOrdersFromMesByTimeInput deleteOrdersFromMesByTimeInput)
        {
            return _dbContextClinet.SugarClient

                .Deleteable<OrdersFromMes_Model>()
                .Where(s => SqlSugar.SqlFunc.Between(s.ProduceDate, deleteOrdersFromMesByTimeInput.ProduceDate, deleteOrdersFromMesByTimeInput.ProduceDate))
                .ExecuteCommand();
        }

        public int InsertOrdersFromMes(AddOrdersFromMesInput addOrdersFromMesInput)
        {
            return _dbContextClinet.SugarClient
                .Insertable(_objectMapper.Map<OrdersFromMes_Model>(addOrdersFromMesInput))
                .ExecuteCommand();
        }

        public List<QuerryOrdersFromMesByDateOutput> QuerryAllOrdersFromMesByDate(DateTime startDate, DateTime endDate)
        {
            var querryResult = _dbContextClinet.SugarClient.Queryable<OrdersFromMes_Model>()
                .Where(s => SqlSugar.SqlFunc.Between(s.ProduceDate, startDate, endDate));
                
            var querryDto = from m in querryResult.ToList() select _objectMapper.Map<QuerryOrdersFromMesByDateOutput>(m);

            return querryDto.ToList();
        }
    }
}
