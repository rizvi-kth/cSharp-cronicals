using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bus.Messages;
using NServiceBus;
using NServiceBus.Logging;
using System.Threading;

namespace OWIN.WebAPI.SelfHostConsole
{
    class MySampleMessageHandler :
        IHandleMessages<MySampleMessage>
    {
        static ILog log = LogManager.GetLogger<MySampleMessage>();

        public Task Handle(MySampleMessage message, IMessageHandlerContext context)
        {
            string baseAddress = "http://localhost:4941";
            string owin_status = "/owinstatus";
            string webapi_status = "/api/products";

            HttpClient client = new HttpClient();
            Console.WriteLine("\n[Message Rcv Handler] >> Checking WebAPI status on " + baseAddress + webapi_status);
            var response = client.GetAsync(baseAddress + webapi_status).Result;
            //Console.WriteLine(response);
            Console.WriteLine("\n[Message Rcv Handler] >> " + response.Content.ReadAsStringAsync().Result);

            log.Info($"Received MyMessage. NameProperty:'{message.NameProperty}'. SpeedProperty:'{message.SpeedProperty}'");
            return Task.FromResult(0);
        }
    }
}
