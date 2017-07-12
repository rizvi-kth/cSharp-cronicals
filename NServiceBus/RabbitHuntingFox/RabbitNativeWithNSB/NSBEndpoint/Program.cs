using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using Messages;

namespace NSBEndpoint
{
    class Program
    {
        static void Main()
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            Console.Title = "Samples.RabbitMQ.NativeIntegration.Receiver";

            #region ConfigureRabbitQueueName
            var endpointConfiguration = new EndpointConfiguration("Samples.RabbitMQ.NativeIntegration");
            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.ConnectionString("host=localhost");
            #endregion
            //var conventions = endpointConfiguration.Conventions();
            //conventions.DefiningMessagesAs(
            //    type =>
            //    {
            //        return type.Namespace == "Messages";
            //    });

            endpointConfiguration.SendFailedMessagesTo("error");
            //endpointConfiguration.UseSerialization<XmlSerializer>();
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            var myMessage = new MyMessage("Message from NSB ");
            await endpointInstance.SendLocal(myMessage).ConfigureAwait(false);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}
