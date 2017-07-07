using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace WebAPI.Empty.ServiceBus.Controllers
{
    [RoutePrefix("api")]
    public class ProductController : ApiController
    {

        // GET: api/Product
        [Route("Products")]
        public IEnumerable<string> Get()
        {
            Thread.Sleep(3000);
            return new string[] { "Gear Box 2.5", "Wheel 5" };
        }

        // GET: api/Product/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Product
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Product/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Product/5
        public void Delete(int id)
        {
        }
    }
}
