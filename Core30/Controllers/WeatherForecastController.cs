using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNet.Comm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Snowflake.Core;

namespace Core30.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 测试MQ生产者
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> AddMessageTest()

        {

            //声明一个实体对象
            var model = new ActionLog();
            model.ActionLogId = Guid.NewGuid().ToString();
            model.CreateTime = DateTime.Now;
            model.UpdateTime = DateTime.Now;
            //调用MQ
            await RabbitMQHelp.PushMessage(RabbitMQHelp.actionLogExchange, model);

            return "操作成功";

        }

        //[HttpGet]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    var rng = new Random();
        //    var worker = new IdWorker(1, 1);
        //    //long id = worker.NextId();

        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        UID = worker.NextId(),
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = rng.Next(-20, 55),
        //        Summary = Summaries[rng.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}

        public static Task<string> GetName()
        {
            var worker = new IdWorker(1, 1);
            long id = worker.NextId();

            return Task.FromResult($"ID：{id}，name：碌云");
        }

        public static async Task<string> GetName2()
        {
            return await Task.FromResult("碌云");
        }
    }
}
