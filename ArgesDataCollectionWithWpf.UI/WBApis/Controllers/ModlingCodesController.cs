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
using Abp.Dependency;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace ArgesDataCollectionWithWpf.UI.WBApis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ModlingCodesController: AbpController,ITransientDependency
    {
        private readonly ILogger _logger;
        private readonly CustomerQueneForCodesFromMes _customerQueneForCodesFromMes;
        private readonly IConfiguration _configuration;

        private string _stationNumberOne = "1";
        private string _stationNumberTwo = "2";

        public ModlingCodesController(ILogger logger, CustomerQueneForCodesFromMes customerQueneForCodesFromMes, IConfiguration configuration)
        {
            
            this._logger = logger;
            this._customerQueneForCodesFromMes = customerQueneForCodesFromMes;
            this._configuration = configuration;
            var xxx = this._configuration["StationNumberOne"];
            if (this._configuration["StationNumberOne"]!=null && !string.IsNullOrWhiteSpace(this._configuration["StationNumberOne"]))
            {
                this._stationNumberOne = this._configuration["StationNumberOne"];
            }

            if (this._configuration["StationNumberTwo"] != null && !string.IsNullOrWhiteSpace(this._configuration["StationNumberTwo"]))
            {
                this._stationNumberTwo = this._configuration["StationNumberTwo"];
            }
        }


        [HttpPost]
        public async Task<ActionResult<string>> AddModlingCodes([FromBody] AddModlingCodesInputsWebDto addOrdersFromMesInput)
        {
            if (addOrdersFromMesInput == null)
            {
                return BadRequest();
            }
            //收到的信息提示出来方便查找问题
            var jsonStr = JsonConvert.SerializeObject(addOrdersFromMesInput);
            
            
            this._logger.LogInformation($"收到1个扫码信息:{jsonStr}");
            try
            {
                //根据地轨码，来选择放入哪个队列中
                if (addOrdersFromMesInput.StationNumber == this._stationNumberOne)
                {
                    this._customerQueneForCodesFromMes.MainScanQuene.Post(new QuerryModlingCodesOutput { 
                    
                        Containerno = addOrdersFromMesInput.Containerno,
                        Part1 = addOrdersFromMesInput.Part1,
                        Part2 = addOrdersFromMesInput.Part2,
                        Part3 = addOrdersFromMesInput.Part3,
                        StationNumber = addOrdersFromMesInput.StationNumber,
                        Time = DateTime.Now,
                    
                    });
                    this._logger.LogInformation($"放入1号地轨");
                }


                else if (addOrdersFromMesInput.StationNumber == this._stationNumberTwo)
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
                    this._logger.LogInformation($"放入1号地轨");
                }
                else
                {
                    this._logger.LogInformation($"地轨信息匹配不正确或者无法匹配，返回错误结果");
                    return BadRequest(-1);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                

            }

            this._logger.LogInformation($"扫码后返回正常信息");
            return Ok(1);
        }
    }
}
