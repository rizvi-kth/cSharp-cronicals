﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OWIN.WebAPI.AutoFac.Controllers
{
    [RoutePrefix("api")]
    public class ValuesController : ApiController
    {

        // GET api/values
        [Route("Products")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value13", "value2" };
        }

        // GET api/values/5
        [Route("Products/{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
