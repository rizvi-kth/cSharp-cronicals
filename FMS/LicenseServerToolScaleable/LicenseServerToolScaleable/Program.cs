using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonMessages;
using MassTransit;

namespace LicenseServerToolScaleable
{
    class Program
    {
        static void Main(string[] args)
        {
            var busControl = ConfigureBus();
            busControl.Start();
            Console.WriteLine("Enter to run stream");
            //Console.Write("Command > ");
            Console.ReadLine();
            for(int i=0;i<200;i++)
            {

                int value = i; //new Random(99).Next();

                //if ("quit".Equals(value, StringComparison.OrdinalIgnoreCase))
                //    break;

                // Command send 
                SendCommand(busControl, value).Wait();

                // Event publish 
                //busControl.Publish<EventValueEntered>(new
                //{
                //    Value = value
                //});
            }
            


            busControl.Stop();
        }

        private static async Task SendCommand(IBusControl busControl, int orderSequence)
        {
            var endpoint = await busControl.GetSendEndpoint(new Uri("rabbitmq://localhost/FMS_Commands"));

            var c2 = Guid.NewGuid();
            Console.WriteLine($"[For Handler] Message CorrelationId:{c2}");
            Task T1 = endpoint.Send<AcquireLicenseCommand>(
                new
                {
                    OrderSequence = orderSequence,
                    CorrelationId = c2
                },
                context =>
                {
                    context.Headers.Set("MyMessageType", "AcquireLicenseCommand");

                    // No need to set CorelationID when the message is implmenting the 'CorrelatedBy<Guid>' 
                    // and CorrelationId is assigned in constructor.
                    //context.CorrelationId = c2;
                }
            );

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
