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
            this._logger.LogInformation($"get order ok ,sum:{number}");
            return Ok(number);
        }
    }
}
