using ArgesDataCollectionWithWpf.Application.DataBaseApplication.OrdersFromMesApplication.Dto;
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

        public int DeleteLineStationParameterByIndex(int id)
        {
            return _dbContextClinet.SugarClient

                .Deleteable<OrdersFromMes_Model>()
                .Where(s => s.ID==id)
                .ExecuteCommand();
        }

        public int DeleteLineStationParameterByTime(DeleteOrdersFromMesByTimeInput deleteOrdersFromMesByTimeInput)
        {
            return _dbContextClinet.SugarClient

                .Deleteable<OrdersFromMes_Model>()
                .Where(s => SqlSugar.SqlFunc.Between(s.ProduceDate, deleteOrdersFromMesByTimeInput.ProduceDate, deleteOrdersFromMesByTimeInput.ProduceDate))
                .ExecuteCommand();
        }

        public int InsertOrdersFromMes(List<AddOrdersFromMesInput> addOrdersFromMesInput)
        {
            var obs = (from m in addOrdersFromMesInput select _objectMapper.Map<OrdersFromMes_Model>(m)).ToList();
            


            return _dbContextClinet.SugarClient
                .Insertable<OrdersFromMes_Model>(obs.ToList())
                .ExecuteCommand();
        }

        public int InsertOrUpdateOrdersFromMesRunCount(List<AddOrUpdateOrdersFromMesInput> addOrdersFromMesInput)
        {
            var obs = (from m in addOrdersFromMesInput select _objectMapper.Map<OrdersFromMes_Model>(m)).ToList();
            var x = _dbContextClinet.SugarClient.Storageable(obs).ToStorage();


            var insetCount = x.AsInsertable.ExecuteCommand();//不存在插入
            var updateCount = x.AsUpdateable.UpdateColumns(it=>it.RunnedCount).ExecuteCommand();//存在更新

            return insetCount + updateCount;
            
        }

        public int InsertOrUpdateOrdersFromMesScanCount(List<AddOrUpdateOrdersFromMesInput> addOrdersFromMesInput)
        {
            var obs = (from m in addOrdersFromMesInput select _objectMapper.Map<OrdersFromMes_Model>(m)).ToList();
            var x = _dbContextClinet.SugarClient.Storageable(obs).ToStorage();


            var insetCount = x.AsInsertable.ExecuteCommand();//不存在插入
            var updateCount = x.AsUpdateable.UpdateColumns(it => it.ScanedCount).ExecuteCommand();//存在更新

            
            return insetCount + updateCount;

        }

        public int InsertOrUpdateOrdersFromMes(List<AddOrUpdateOrdersFromMesInput> addOrdersFromMesInput)
        {
            var obs = (from m in addOrdersFromMesInput select _objectMapper.Map<OrdersFromMes_Model>(m)).ToList();
            var x = _dbContextClinet.SugarClient.Storageable(obs).ToStorage();


            var insetCount = x.AsInsertable.ExecuteCommand();//不存在插入
            var updateCount = x.AsUpdateable.ExecuteCommand();//存在更新

            return insetCount + updateCount;

        }

        public List<QuerryOrdersFromMesOutput> QuerryAllOrdersFromMesByDate(DateTime startDate, DateTime endDate)
        {
            var querryResult = _dbContextClinet.SugarClient.Queryable<OrdersFromMes_Model>()
                .Where(s => SqlSugar.SqlFunc.Between(s.ProduceDate, startDate, endDate)).OrderBy(it=>it.ProduceQueneNumber);
                
            var querryDto = from m in querryResult.ToList() select _objectMapper.Map<QuerryOrdersFromMesOutput>(m);

            return querryDto.ToList();
        }

        public int InsertOrUpdateOrdersFromMesDownAreaSendOKCount(List<AddOrUpdateOrdersFromMesInput> addOrdersFromMesInput)
        {
            var obs = (from m in addOrdersFromMesInput select _objectMapper.Map<OrdersFromMes_Model>(m)).ToList();
            var x = _dbContextClinet.SugarClient.Storageable(obs).ToStorage();


            var insetCount = x.AsInsertable.ExecuteCommand();//不存在插入
            var updateCount = x.AsUpdateable.UpdateColumns(it => it.IsDownMaterialAreaSendOrder).ExecuteCommand();//存在更新

            return insetCount + updateCount;
        }

        public int InsertOrUpdateOrdersFromMesLoadAreaSendOKCount(List<AddOrUpdateOrdersFromMesInput> addOrdersFromMesInput)
        {
            var obs = (from m in addOrdersFromMesInput select _objectMapper.Map<OrdersFromMes_Model>(m)).ToList();
            var x = _dbContextClinet.SugarClient.Storageable(obs).ToStorage();


            var insetCount = x.AsInsertable.ExecuteCommand();//不存在插入
            var updateCount = x.AsUpdateable.UpdateColumns(it => it.IsLoadMaterialAreaSendOrder).ExecuteCommand();//存在更新

            return insetCount + updateCount;
        }
    }
}
