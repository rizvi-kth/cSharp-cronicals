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
            Console.WriteLine("Enter message (or quit to exit)");
            Console.Write("> ");

            string value = Console.ReadLine();

            if ("quit".Equals(value, StringComparison.OrdinalIgnoreCase))
                return;

            var busControl = ConfigureBus();
            busControl.Start();

            // Command send 
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine("Sending ...");
                SendCommand(busControl, i.ToString()).Wait();

            }

            busControl.Stop();
            return;
        }

        private static async Task SendCommand(IBusControl busControl, string valueToSend)
        {
            var endpoint = await busControl.GetSendEndpoint(new Uri("rabbitmq://localhost/Tesla.Commands"));
            var T1 = endpoint.Send<EventValueEntered>(
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

            //var c2 = Guid.NewGuid();
            //Console.WriteLine($"[For Handler] Message CorrelationId:{c2}");
            //Task T2 = endpoint.Send<RegisterOrderCommand>(
            //    new
            //    {
            //        OrderId = 000,
            //        CorrelationId = c2
            //    },
            //    context =>
            //    {
            //        context.Headers.Set("MyMessageType", "RegisterOrderCommand");

            //        // No need to set CorelationID when the message is implmenting the 'CorrelatedBy<Guid>' 
            //        // and CorrelationId is assigned in constructor.
            //        //context.CorrelationId = c2;
            //    }
            //);

            Task.WhenAll(new List<Task>() { T1 }).Wait();
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
