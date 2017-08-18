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
            //var bus = ServiceBusFactory.New(sbc =>
            //{
            //    sbc.UseRabbitMq(r =>
            //    {
            //        r.ConfigureHost(new Uri("rabbitmq://localhost/Tesla.Commands"), h =>
            //        {
            //            h.SetUsername("username");
            //            h.SetPassword("password");
            //        });
            //    });

            //});

            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://localhost/"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                
                cfg.ReceiveEndpoint(host, "Tesla.Commands", e =>
                {
                    
                    e.Consumer<EventValueEnteredHandler>();
                    e.PrefetchCount = 2;
                });
            });

            busControl.Start();
            
            Console.WriteLine("Listining on tesla");
            Console.ReadKey();
            busControl.Stop();

        }
    }
}
