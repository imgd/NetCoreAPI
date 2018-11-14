using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.CacheOutput;
using Dnc.Api.Throttle;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiCore.Controllers
{
    //[Route("api/[controller]")]
    public class TestController : Controller
    {
        [HttpGet]
        [Route("api/test/get1")]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [RateValve(Policy = Policy.RequestPath, Limit = 5, Duration = 10)]
        public string GetTime()
        {
            return DateTime.Now.ToString();
        }
    }
}