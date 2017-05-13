using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using OWIN.Test;

namespace OWIN.Host.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Necessary Packages : Microsoft.Owin.Hosting
            //                      Microsoft.Owin.Host.HttpListener

            using (WebApp.Start<Startup>("http://localhost:9001"))
            {
                Console.WriteLine("Press [enter] to quit...");
                Console.ReadLine();
            }
        }
    }
}
