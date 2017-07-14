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
                SendCommand(busControl, value);
                

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

        private static async void SendCommand(IBusControl busControl, string valueToSend )
        {
            var endpoint = await busControl.GetSendEndpoint(new Uri("rabbitmq://localhost/Tesla.Commands"));
            await endpoint.Send<CommandChangeValue>(new { Value = $"Command Testing value : {valueToSend}" });
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
