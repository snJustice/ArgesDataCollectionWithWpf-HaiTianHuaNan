using Abp.AspNetCore.Mvc.Controllers;
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.ModlingCodesApplication.Dto;

using ArgesDataCollectionWpf.DataProcedure.Utils.Quene;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ArgesDataCollectionWithWpf.UI.WBApis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ModlingCodesController: AbpController
    {
        private readonly ILogger _logger;
        private readonly CustomerQueneForCodesFromMes _customerQueneForCodesFromMes;

        public ModlingCodesController(ILogger logger, CustomerQueneForCodesFromMes customerQueneForCodesFromMes)
        {
            
            this._logger = logger;
            this._customerQueneForCodesFromMes = customerQueneForCodesFromMes;
        }


        [HttpPost]
        public async Task<ActionResult<string>> AddModlingCodes([FromBody] AddModlingCodesInputsWebDto addOrdersFromMesInput)
        {
            if (addOrdersFromMesInput == null)
            {
                return BadRequest();
            }

            try
            {
                //根据地轨码，来选择放入哪个队列中
                if (addOrdersFromMesInput.StationNumber == "1")
                {
                    this._customerQueneForCodesFromMes.MainScanQuene.Post(new QuerryModlingCodesOutput { 
                    
                        Containerno = addOrdersFromMesInput.Containerno,
                        Part1 = addOrdersFromMesInput.Part1,
                        Part2 = addOrdersFromMesInput.Part2,
                        Part3 = addOrdersFromMesInput.Part3,
                        StationNumber = addOrdersFromMesInput.StationNumber,
                        Time = DateTime.Now,
                    
                    });
                }
                else if (addOrdersFromMesInput.StationNumber == "2")
                {
                    this._customerQueneForCodesFromMes.MainScanQuene.Post(new QuerryModlingCodesOutput
                    {

                        Containerno = addOrdersFromMesInput.Containerno,
                        Part1 = addOrdersFromMesInput.Part1,
                        Part2 = addOrdersFromMesInput.Part2,
                        Part3 = addOrdersFromMesInput.Part3,
                        StationNumber = addOrdersFromMesInput.StationNumber,
                        Time = DateTime.Now,

                    });
                }
                else
                {
                    return BadRequest("Station Error");
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                

            }

            
            return Ok(1);
        }
    }
}
