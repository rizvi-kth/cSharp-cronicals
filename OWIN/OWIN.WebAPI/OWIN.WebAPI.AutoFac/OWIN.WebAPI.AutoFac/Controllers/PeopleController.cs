using OWIN.WebAPI.AutoFac.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OWIN.WebAPI.AutoFac.Controllers
{
    [RoutePrefix("api")]
    public class PersonController : ApiController
    {
        private ILogger logger;
        public PersonController(ILogger logger)
        {
            this.logger = logger;
        }

        // GET: api/Persons
        [Route("Persons")]
        public IEnumerable<string> Get()
        {
            logger.Write("GET: api/Persons");
            return new string[] { "Jim", "Hil" };
        }

        // GET: api/Person/5
        [Route("Person/{id}")]
        public string Get(int id)
        {
            logger.Write("GET: api/Person/{id}");
            return "Jack";
        }

        // POST: api/People
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/People/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/People/5
        public void Delete(int id)
        {
        }
    }
}
