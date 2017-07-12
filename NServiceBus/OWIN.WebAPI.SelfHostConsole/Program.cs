using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using NServiceBus;
using NServiceBus.Logging;
using Owin;
using WebAPI.Empty.ServiceBus;
using JsonSerializer = NServiceBus.JsonSerializer;

namespace OWIN.WebAPI.SelfHostConsole
{
    class Program
    {
        static void Main()
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            Console.Title = "WebAPI OWIN MSMQ TEST";
            LogManager.Use<DefaultFactory>().Level(LogLevel.Info);

            #region startbus

            var endpointConfiguration = new EndpointConfiguration("webapi.owin.msmq");
            //var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            //transport.ConnectionString("host=localhost");

            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.UseSerialization<JsonSerializer>();

            // register the mutator so the the message on the wire is written
            endpointConfiguration.RegisterComponents(components =>
            {
                components.ConfigureComponent<MessageBodyWriter>(DependencyLifecycle.InstancePerCall);
            });
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.EnableInstallers();

            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            // Startup Owin with Endpoint.
            using (StartOwinHost(endpointInstance))
            {
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
            await endpointInstance.Stop().ConfigureAwait(false);

            #endregion
            
        }


        static IDisposable StartOwinHost(IEndpointInstance endpointInstance)
        {
            
            string baseAddress = "http://localhost:4941";
            
            var app = WebApp.Start<Startup>(baseAddress);

            // Create HttpCient and make a GET request to /owinstatus 
            string owin_status = "/owinstatus";
            HttpClient client = new HttpClient();
            Console.WriteLine("\nChecking OWIN status on " + baseAddress + owin_status);
            var response = client.GetAsync(baseAddress + owin_status).Result;
            Console.WriteLine(response);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);

            // Make a GET request to /api/products
            string webapi_status = "/api/products";
            Console.WriteLine("\nChecking WebAPI status on " + baseAddress + webapi_status);
            response = client.GetAsync(baseAddress + webapi_status).Result;
            Console.WriteLine(response);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);

            Console.WriteLine("\n Press R to run 100 resuest for the MSMQ");

            string msmqLink = "/to-msmq"; // This URL is for OWIN midleware which send message to MSMQ.
            var input = Console.ReadKey();

            if (input.Key == ConsoleKey.R)
            {
                for (int i = 0; i < 100; i++)
                {
                    // Make a POST request to /api/products
                    //Console.WriteLine("\nChecking WebAPI status on " + baseAddress + msmqLink);
                    var payload = new
                    {
                        NameProperty = "Axel 500",
                        SpeedProperty = "50 rpm"
                    };
                    // Serialize our concrete class into a JSON String
                    var stringPayload = JsonConvert.SerializeObject(payload);

                    // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                    var myhttpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                    myhttpContent.Headers.Add("MessageType", "Bus.Messages.MySampleMessage");
                    var taskResult = Task.Run(() => client.PostAsync(baseAddress + msmqLink, myhttpContent));
                    
                    var response2 = taskResult.Result;

                    //Console.WriteLine(response2);
                    Console.WriteLine($"[Main] >> Post { i } result >> { response2.Content.ReadAsStringAsync().Result} " );
                }
            }


            Console.ReadLine();
            return app;

        }
    }
}
