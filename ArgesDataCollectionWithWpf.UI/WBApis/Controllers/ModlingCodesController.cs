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
using ArgesDataCollectionWithWpf.Application.DataBaseApplication.ModlingCodesApplication;

/*
 *   -1是地轨信息不匹配
 *   -2是数据库插入失败
 *   -3是异常，，查看日志
 * 
 * 
 */

namespace ArgesDataCollectionWithWpf.UI.WBApis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ModlingCodesController: AbpController,ITransientDependency
    {
        private readonly ILogger _logger;
        private readonly CustomerQueneForCodesFromMes _customerQueneForCodesFromMes;
        private readonly IConfiguration _configuration;

        private readonly IModlingCodesApplication _modlingCodesApplication;



        private string _stationNumberOne = "1";
        private string _stationNumberTwo = "2";

        public ModlingCodesController(ILogger logger
            , CustomerQueneForCodesFromMes customerQueneForCodesFromMes
            , IConfiguration configuration
            , IModlingCodesApplication modlingCodesApplication)
        {

            this._logger = logger;
            this._customerQueneForCodesFromMes = customerQueneForCodesFromMes;
            this._configuration = configuration;
            var xxx = this._configuration["StationNumberOne"];
            this._modlingCodesApplication = modlingCodesApplication;
            if (this._configuration["StationNumberOne"] != null && !string.IsNullOrWhiteSpace(this._configuration["StationNumberOne"]))
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

            DateTime time = DateTime.Now;
            if (addOrdersFromMesInput == null)
            {
                return BadRequest();
            }
            //收到的信息提示出来方便查找问题
            var jsonStr = JsonConvert.SerializeObject(addOrdersFromMesInput);
            
            
            this._logger.LogInformation($"收到1个扫码信息:{jsonStr}");

            AddModlingCodesInputs insert = new AddModlingCodesInputs
            {
                Containerno = addOrdersFromMesInput.Containerno,
                Part1 = addOrdersFromMesInput.Part1,
                Part2 = addOrdersFromMesInput.Part2,
                Part3 = addOrdersFromMesInput.Part3,
                StationNumber = addOrdersFromMesInput.StationNumber,
                Time = time,
                IsDone = 0
            };
            

            try
            {
                //根据地轨码，来选择放入哪个队列中
                if (addOrdersFromMesInput.StationNumber == this._stationNumberOne)
                {


                    var count = this._modlingCodesApplication.InsertModlingCodes(insert);
                    if (count <= 0)
                    {
                        return BadRequest(-2);
                    }

                    this._customerQueneForCodesFromMes.MainScanQuene.Post(new QuerryModlingCodesOutput { 
                    
                        Containerno = addOrdersFromMesInput.Containerno,
                        Part1 = addOrdersFromMesInput.Part1,
                        Part2 = addOrdersFromMesInput.Part2,
                        Part3 = addOrdersFromMesInput.Part3,
                        StationNumber = addOrdersFromMesInput.StationNumber,
                        Time = time,
                    
                    });
                    this._logger.LogInformation($"放入1号地轨");
                }


                else if (addOrdersFromMesInput.StationNumber == this._stationNumberTwo)
                {
                    var count = this._modlingCodesApplication.InsertModlingCodes(insert);
                    if (count <= 0)
                    {
                        return BadRequest(-2);
                    }

                    this._customerQueneForCodesFromMes.MainScanQuene.Post(new QuerryModlingCodesOutput
                    {

                        Containerno = addOrdersFromMesInput.Containerno,
                        Part1 = addOrdersFromMesInput.Part1,
                        Part2 = addOrdersFromMesInput.Part2,
                        Part3 = addOrdersFromMesInput.Part3,
                        StationNumber = addOrdersFromMesInput.StationNumber,
                        Time = time,

                    });
                    this._logger.LogInformation($"放入2号地轨");
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
                return Ok(-3);

            }

            this._logger.LogInformation($"扫码后返回正常信息");
            return Ok(1);
        }





        [HttpDelete]
        public async Task<ActionResult<string>> DeleteModlingCodes([FromBody] AddModlingCodesInputsWebDto addOrdersFromMesInput)
        {

            //直接去操作数据库吧，很快的
            if (addOrdersFromMesInput == null)
            {
                return BadRequest();
            }
            //收到的信息提示出来方便查找问题
            var jsonStr = JsonConvert.SerializeObject(addOrdersFromMesInput);
            this._logger.LogInformation($"收到1个撤销删除信息:{jsonStr}");

            DeleteModlingCodesInput deleteModlingCodesInput = new DeleteModlingCodesInput { 
            
                Containerno = addOrdersFromMesInput.Containerno

            };

            if (addOrdersFromMesInput.StationNumber == this._stationNumberOne)
            {
                var result = this._modlingCodesApplication.DeleteModlingCodesByContainerNo(deleteModlingCodesInput);
                if (result<=0)
                {
                    return BadRequest(-2);
                }
                this._customerQueneForCodesFromMes.MainScanDeleteQuene.Post(new QuerryModlingCodesOutput
                {

                    Containerno = addOrdersFromMesInput.Containerno,
                    Part1 = addOrdersFromMesInput.Part1,
                    Part2 = addOrdersFromMesInput.Part2,
                    Part3 = addOrdersFromMesInput.Part3,
                    StationNumber = addOrdersFromMesInput.StationNumber,
                    Time = DateTime.Now,

                });
                this._logger.LogInformation($"开始撤销1号地轨信息");
            }


            else if (addOrdersFromMesInput.StationNumber == this._stationNumberTwo)
            {

                var result = this._modlingCodesApplication.DeleteModlingCodesByContainerNo(deleteModlingCodesInput);
                if (result <=0)
                {
                    return BadRequest(-2);
                }

                this._customerQueneForCodesFromMes.MainScanDeleteQuene.Post(new QuerryModlingCodesOutput
                {

                    Containerno = addOrdersFromMesInput.Containerno,
                    Part1 = addOrdersFromMesInput.Part1,
                    Part2 = addOrdersFromMesInput.Part2,
                    Part3 = addOrdersFromMesInput.Part3,
                    StationNumber = addOrdersFromMesInput.StationNumber,
                    Time = DateTime.Now,

                });
                this._logger.LogInformation($"开始撤销2号地轨信息");
            }
            else
            {
                this._logger.LogInformation($"地轨信息匹配不正确或者无法匹配，返回错误结果");
                return BadRequest(-1);
            }




            return Ok(1);
        }
    }
}
