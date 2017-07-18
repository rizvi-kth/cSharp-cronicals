using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automatonymous;
using GreenPipes;
using GreenPipes.Introspection;
using log4net.Config;
using MassTransit;
using MassTransit.Log4NetIntegration;
using MassTransit.Saga;
using Newtonsoft.Json;
using SimpleMessages;
using SimpleSaga;


namespace SimpleReciever
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            // To Enable Log4Net
            //XmlConfigurator.Configure();
            // Or add the following line to AssemblyInfo.cs
            //[assembly: log4net.Config.XmlConfigurator(Watch = true)]
            // and add the following line with log decleraton 
            log.Info("Application Started");

            var saga = new OrderStateMachine();
            var repo = new InMemorySagaRepository<OrderState>();

            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://localhost/"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                // Recieve endpoint needs this extra configuratioin:
                // REGISTER A RECIEVE-ENDPOINT(attached to a queue) ON THE BUS. 
                cfg.ReceiveEndpoint(host, "Tesla.Saga.Commands", e =>
                {
                    // Enable Log4Net
                    cfg.UseLog4Net();

                    //e.Consumer<RegisterOrderCommandHandler>();
                    e.StateMachineSaga(saga,repo);
                    
                });

            });

            busControl.Start();
            // To probe bus configuration and return an object graph of the bus.
            ProbeResult result = busControl.GetProbeResult();
            //Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            System.IO.File.WriteAllText(@"C:\Temp\BusSagaSerializedJson.txt", JsonConvert.SerializeObject(result, Formatting.Indented));

            Console.WriteLine("Listining Saga on Tesla.Saga.Commands for RegisterOrderCommand");
            Console.ReadLine();
            busControl.Stop();

        }
    }
}

