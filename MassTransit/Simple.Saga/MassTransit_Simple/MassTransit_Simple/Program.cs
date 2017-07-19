using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using SimpleMessages;

namespace MassTransit_Simple
{
        public class Program
    {
        public static void Main()
        {
            var busControl = ConfigureBus();
            busControl.Start();
            

            do
            {
                Console.WriteLine("Enter message (or quit to exit)");
                Console.Write("Command > ");
                string value = Console.ReadLine();

                if ("quit".Equals(value, StringComparison.OrdinalIgnoreCase))
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
            var endpoint = await busControl.GetSendEndpoint(new Uri("rabbitmq://localhost/Tesla.Saga.Commands"));
            var T1 = endpoint.Send<RegisterOrderCommand>(
                new
                {
                    OrderId = new Random().Next(100,1000)
                },
                context =>
                {
                    context.Headers.Set("MyCustomHeader", "A Special Value");
                    var c = Guid.NewGuid();
                    Console.WriteLine($"[For Customer]Context CorrelationId:{c}");
                    context.CorrelationId = c;
                }
            );
            

            Task.WhenAll(new List<Task>() {T1}).Wait();
        }

        

        static IBusControl ConfigureBus()
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri("rabbitmq://localhost/"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });


            });
        }
    }
}
