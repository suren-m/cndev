﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceStack.Redis;

namespace strings_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StringLengthController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly RedisManagerPool _redisPool;

        public StringLengthController(ILogger<WeatherForecastController> logger, RedisManagerPool redisPool)
        {
            _logger = logger;
            _redisPool = redisPool;
        }

        [HttpGet]
        [Route("{input}/")]
        public ActionResult Get(string input)
        {
            var res = new StringResult();
            res.Input = input;

            int status = 200; 
            if (int.TryParse(input, out status)) // simulate errors
            {
                return StatusCode(status);
            }

           if (input == "retry") // simulate retry example based on some randomization
            {
                var time = DateTime.UtcNow;
                if (time.Second % 2 == 0) {
                    return StatusCode(503);
                 }  else  {
                    res.CacheHit = true;
                    res.Length = 5;
                    return Ok(res);
                }
            }
                        
            using (var cache = _redisPool.GetClient())
            {              
                var data = cache.Get<string>(input);
                if(data !=null) {
                    res.CacheHit = true;                    
                    int.TryParse(data, out int len);
                    res.Length = len;
                } else {
                    var length = input.Length;
                    res.Length = length;
                    res.CacheHit = false;                                    
                    cache.Set(input, length);                    
                }                
            }                 
            return Ok(res);          
        }

        public class StringResult {
            public string Input { get; set; }
            public int Length {get; set; }

            public bool CacheHit { get; set;}
        }
    }
}
