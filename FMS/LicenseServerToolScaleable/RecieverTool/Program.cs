using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreenPipes;
using GreenPipes.Introspection;
using MassTransit;
using Newtonsoft.Json;

namespace RecieverTool
{
    class Program
    {
        static void Main(string[] args)
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://localhost/"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                // Recieve endpoint needs this extra configuratioin:
                // REGISTER A RECIEVE-ENDPOINT(attached to a queue) ON THE BUS. 
                cfg.ReceiveEndpoint(host, "FMS_Commands", e =>
                {
                    
                    e.Consumer<AcquireLicenseCommandHandler>();
                    e.PrefetchCount = 1;

                    //// Handling without a consumer class
                    //e.Handler<RegisterOrderCommand>(context =>
                    //{
                    //    Console.Out.WriteLineAsync("\n= = = = = = = = = = = = ");
                    //    Console.Out.WriteLineAsync($"[Handler] Order: {context.Message.OrderId}");
                    //    var headers = context.Headers;
                    //    Console.Out.WriteLineAsync($"Headers count: {headers.GetAll().Count()} :");
                    //    foreach (var keyValuePair in headers.GetAll())
                    //    {
                    //        Console.Out.WriteLineAsync($"{keyValuePair.Key} : {keyValuePair.Value}");
                    //    }
                    //    Console.Out.WriteLineAsync($"Context CorelationID: {context.CorrelationId?.ToString()} ");
                    //    Console.Out.WriteLineAsync($"Message corelationID: {context.Message.CorrelationId.ToString()} ");
                    //    Console.Out.WriteLineAsync($"Context ConversationId: {context.ConversationId?.ToString()} : (not a CorelationID)");

                    //    return Task.FromResult(0);

                    //});
                });



            });

            busControl.Start();
            // To probe bus configuration and return an object graph of the bus.
            //ProbeResult result = busControl.GetProbeResult();
            //Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            //System.IO.File.WriteAllText(@"C:\Temp\BusSerializedJson.txt", JsonConvert.SerializeObject(result, Formatting.Indented));

            Console.WriteLine("\n\nListining on FMS_Commands");
            Console.ReadLine();
            busControl.Stop();

        }
    }

    
}
