using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace RabbitNativeWithNSB
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Samples.RabbitMQ.NativeIntegration.Sender";
            var connectionFactory = new ConnectionFactory();

            using (var connection = connectionFactory.CreateConnection(Console.Title))
            {
                Console.WriteLine("Press enter to send a message");
                Console.WriteLine("Press any key to exit");

                while (true)
                {
                    var key = Console.ReadKey();
                    Console.WriteLine();
                    if (key.Key != ConsoleKey.Enter)
                    {
                        return;
                    }
                    using (var channel = connection.CreateModel())
                    {
                        var properties = channel.CreateBasicProperties();
                        #region GenerateUniqueMessageId
                        var messageId = Guid.NewGuid().ToString();

                        properties.MessageId = messageId;

                        #endregion

                        #region CreateNativePayload
                        var payload = "<Messages.MyMessage><SampleQuota>Hello from native sender</SampleQuota></Messages.MyMessage>";
                        #endregion

                        #region SendMessage

                        channel.BasicPublish(string.Empty, "Samples.RabbitMQ.NativeIntegration", false, properties, Encoding.UTF8.GetBytes(payload));
                        #endregion

                        Console.WriteLine($"Message with id {messageId} sent to queue {"Samples.RabbitMQ.NativeIntegration"}");
                    }
                }
            }
        }
    }
}
