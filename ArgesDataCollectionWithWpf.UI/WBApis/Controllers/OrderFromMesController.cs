using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Controllers;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.CommunicationDetailsAndInstanceApplication;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.OrdersFromMesApplication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ArgesDataCollectionWithWpf.UI.WBApis.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class OrderFromMesController:AbpController
    {
        private readonly IOrdersFromMesApplication _ordersFromMesApplication;
        private readonly ILogger _logger;
   

        public OrderFromMesController(IOrdersFromMesApplication ordersFromMesApplication
            ,ILogger logger
            )
        {
            this._ordersFromMesApplication = ordersFromMesApplication;
            this._logger = logger;
            
        }

        [HttpPost]
        public async Task<ActionResult< int> > AddOrder( [FromBody] List<AddOrdersFromMesInput> addOrdersFromMesInput)
        {
            if (addOrdersFromMesInput == null)
            {
                return BadRequest();
            }
            var jsonStr = JsonConvert.SerializeObject(addOrdersFromMesInput);
            this._logger.LogInformation($"收到1个订单信息信息:{jsonStr}");
            try
            {
                this._ordersFromMesApplication.InsertOrdersFromMes(addOrdersFromMesInput);
                

            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                return BadRequest();
                
            }
            
            var count = from m in addOrdersFromMesInput select m.ProduceQuantity;
            int number = count.Sum();
            this._logger.LogInformation($"订单数据插入成功,总数:{number}");
            return Ok(number);
        }
    }
}
