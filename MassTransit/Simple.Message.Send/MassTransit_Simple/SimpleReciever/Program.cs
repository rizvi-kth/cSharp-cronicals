using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using SimpleMessages;


namespace SimpleReciever
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
                cfg.ReceiveEndpoint(host, "Tesla.Commands", e =>
                {
                    e.Consumer<CommandChangeValueHandler>();

                    // Handling without a consumer class
                    e.Handler<RegisterOrderCommand>(context =>
                    {
                        Console.Out.WriteLineAsync("\n= = = = = = = = = = = = ");
                        Console.Out.WriteLineAsync($"[Handler] Order: {context.Message.OrderId}");
                        var headers = context.Headers;
                        Console.Out.WriteLineAsync($"Headers count: {headers.GetAll().Count()} :");
                        foreach (var keyValuePair in headers.GetAll())
                        {
                            Console.Out.WriteLineAsync($"{keyValuePair.Key} : {keyValuePair.Value}");
                        }
                        Console.Out.WriteLineAsync($"Context CorelationID: {context.CorrelationId?.ToString()} ");
                        Console.Out.WriteLineAsync($"Message corelationID: {context.Message.CorrelationId.ToString()} ");
                        Console.Out.WriteLineAsync($"Context ConversationId: {context.ConversationId?.ToString()} : (not a CorelationID)" );

                        return Task.FromResult(0);

                    });
                });



            });

            busControl.Start();
            Console.WriteLine("Listining on tesla");
            Console.ReadLine();
            busControl.Stop();

        }
    }
}

