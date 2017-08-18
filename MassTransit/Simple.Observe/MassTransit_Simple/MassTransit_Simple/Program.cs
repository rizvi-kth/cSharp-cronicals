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
                Console.Write("> ");
                string value = Console.ReadLine();

                if ("quit".Equals(value, StringComparison.OrdinalIgnoreCase))
                    break;

                busControl.Publish<EventValueEntered>(new
                {
                    Value = value
                });
            }
            while (true);
            
            busControl.Stop();
            return;
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
