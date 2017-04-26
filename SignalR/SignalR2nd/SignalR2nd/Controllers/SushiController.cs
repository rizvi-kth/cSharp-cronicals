using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR;

namespace SignalR2nd.Controllers
{
    public class SushiController : ApiController
    {
        // GET: api/Sushi
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Sushi/5
        public string Get(int id)
        {
            var val =  GlobalHost.ConnectionManager.GetHubContext("ChatHub").Clients;
            return "fsdf";
        }

        // POST: api/Sushi
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Sushi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Sushi/5
        public void Delete(int id)
        {
        }
    }
}
