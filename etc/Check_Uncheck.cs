using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TestConsole
{
    using System;

    class Program
    {
        static void Main()
        {
            // The first short will overflow after the second short does.
            short a = 0;
            short b = 100;
            try
            {
                //
                // Keep incrementing the shorts until an exception is thrown.
                // ... This is terrible program design.
                //
                while (true)
                {
                    checked
                    {
                        a++;
                    }
                    unchecked
                    {
                        b++;
                    }
                }
            }
            catch
            {
                // Display the value of the shorts when overflow exception occurs.
                Console.WriteLine(a);
                Console.WriteLine(b);
            }
        }
    }
}
