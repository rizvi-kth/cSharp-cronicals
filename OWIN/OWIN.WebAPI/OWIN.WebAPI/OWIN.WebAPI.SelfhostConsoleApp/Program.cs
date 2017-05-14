using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using OWIN.WebAPI.Empty;

namespace OWIN.WebAPI.SelfhostConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Install Package   : Microsoft.AspNet.WebApi.OwinSelfHost
            // Dependent Packages : Microsoft.Owin.Hosting
            //                      Microsoft.Owin.Host.HttpListener
            string baseAddress = "http://localhost:9002";
            string owin_status = "/owinstatus";
            string webapi_status = "/api/products";

            using (WebApp.Start<Startup>(baseAddress + owin_status))
            {
                // Create HttpCient and make a request to /owinstatus 
                HttpClient client = new HttpClient();
                Console.WriteLine("\nChecking OWIN status on " + baseAddress + owin_status);
                var response = client.GetAsync(baseAddress + owin_status).Result;
                Console.WriteLine(response);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);

                // Create HttpCient and make a request to /api/products
                Console.WriteLine("\nChecking WebAPI status on " + baseAddress + webapi_status);
                response = client.GetAsync(baseAddress + webapi_status).Result;
                Console.WriteLine(response);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                
                Console.ReadLine();
            }
        }
    }
}
