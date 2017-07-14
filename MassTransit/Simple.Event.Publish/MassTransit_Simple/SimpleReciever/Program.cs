using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;


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

                cfg.ReceiveEndpoint(host, "tesla" , e =>
                {
                    e.Consumer<EventValueEnteredHandler>();
                });
            });

            busControl.Start();
            Console.WriteLine("Listining on tesla");
            Console.ReadKey();
            busControl.Stop();

        }
    }
}
