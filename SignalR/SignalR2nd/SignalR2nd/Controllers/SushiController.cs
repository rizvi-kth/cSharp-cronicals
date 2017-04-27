using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Antlr.Runtime.Tree;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR;

namespace SignalR2nd.Controllers
{
    public class SushiController : ApiController
    {
        // GET: api/Sushi
        public StockInfo Get()
        {
            return new StockInfo() {StockName = "Facebook stock", StockValue = 65};
        }

        // GET: api/Sushi/5
        public string Get(int id)
        {
            return $"value is : {id}";
        }

        // POST: api/Sushi
        /* (REQUEST SPECIFICATION FOR FIDDLER REQUEST COMPOSER)
        POST http://localhost:37319/api/sushi HTTP/1.1
        User-Agent: Fiddler
        Host: localhost:37319
        Content-Length: 47

        { "StockName":"Google stock", "StockValue":95 }
        */
        public HttpResponseMessage Post([FromBody]StockInfo stock)
        {
            Debug.WriteLine("Got value:", stock?.StockName ?? "<No Stock>");

            var hubContext = GlobalHost.ConnectionManager.GetHubContext("MyChat");
            if (Online.ConnectedIds.Count > 0)
            {
                hubContext.Clients.All.handleStockInfo(stock);
                return Request.CreateResponse(HttpStatusCode.OK); // 200
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,"Check you have connected from Client first!!"); // 400
            }

        }

        // PUT: api/Sushi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Sushi/5
        public void Delete(int id)
        {
        }

        [DebuggerDisplay("Stock info : {StockName}")]
        public class StockInfo
        {
            public string StockName { get; set; }
            public int StockValue { get; set; }

        }
    }
}
