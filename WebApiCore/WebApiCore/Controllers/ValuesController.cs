﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dnc.Api.Throttle;
using Microsoft.AspNetCore.Mvc;

namespace WebApiCore.Controllers
{
    [Route("api/[controller]")]
    [OAuthController()]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        [RateValve(Policy = Policy.RequestPath, Limit = 5, Duration = 10)]
        //[OAuthFilter()]
        public IEnumerable<string> Get()
        {
            //throw new Exception("心情不爽！");
            return new string[] {
                ConfigHelper.appsettings.GetAppSettingValue("AppSecret"),
                ConfigHelper.users.GetSectionValue("gd:name")
            };
        }


        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
