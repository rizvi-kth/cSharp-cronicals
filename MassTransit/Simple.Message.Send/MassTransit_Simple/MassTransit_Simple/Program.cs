using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using SimpleMessages;
using System.Configuration;
using System.Threading;

namespace MassTransit_Simple
{
        public class Program
    {
        public static void Main()
        {
            Console.WriteLine("RABBITMQ Host : " + Environment.GetEnvironmentVariable("RABBITMQ_HOST").ToString());

            var busControl = ConfigureBus();
            busControl.Start();

            do
            {
                Console.WriteLine("Enter message (or quit to exit)");
                Console.Write("Command (start with ::)> ");
                // string value = Console.ReadLine();
                string value = new Random(9999).Next().ToString();
                Thread.Sleep(3000);

                if ("quit".Equals(value, StringComparison.OrdinalIgnoreCase) ||
                    "exit".Equals(value, StringComparison.OrdinalIgnoreCase))
                    break;

                // Command send 
                SendCommand(busControl, value).Wait();
                
                // Event publish 
                //busControl.Publish<EventValueEntered>(new
                //{
                //    Value = value
                //});
            }
            while (true);
            
            busControl.Stop();
            return;
        }

        private static async Task SendCommand(IBusControl busControl, string valueToSend )
        {
            var endpoint = await busControl.GetSendEndpoint(new Uri("rabbitmq://"+ Environment.GetEnvironmentVariable("RABBITMQ_HOST").ToString() + "/Tesla.Commands"));
            var T1 = endpoint.Send<CommandChangeValue>(
                new
                {
                    Value = $"Message writen as : {valueToSend}"
                },
                context =>
                {
                    context.Headers.Set("MyCustomHeader", "A Special Value");
                    var c = Guid.NewGuid();
                    Console.WriteLine($"[For Customer]Context CorrelationId:{c}");
                    context.CorrelationId = c;
                }
            );

            var c2 = Guid.NewGuid();
            Console.WriteLine($"[For Handler] Message CorrelationId:{c2}");
            Task T2 = endpoint.Send<RegisterOrderCommand>(
                new
                {
                    OrderId =  000,
                    CorrelationId = c2
                },
                context =>
                {
                    context.Headers.Set("MyMessageType", "RegisterOrderCommand");
                    
                    // No need to set CorelationID when the message is implmenting the 'CorrelatedBy<Guid>' 
                    // and CorrelationId is assigned in constructor.
                    //context.CorrelationId = c2;
                }
            );

            Task.WhenAll(new List<Task>() {T1,T2}).Wait();
        }

        

        static IBusControl ConfigureBus()
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri("rabbitmq://" + Environment.GetEnvironmentVariable("RABBITMQ_HOST") + "/"), h =>
                {
                    h.Username(Environment.GetEnvironmentVariable("RABBITMQ_USER"));
                    h.Password(Environment.GetEnvironmentVariable("RABBITMQ_PASS"));
                });


            });
        }
    }
}
