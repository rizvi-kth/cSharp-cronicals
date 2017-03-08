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
    class Program
    {
        enum AnimalEnum
        {
            Donkey ,
            Monkey ,
            Lion
        }

        public static void Main()
        {
            try
            {
                while (true)
                {
                    string input = Console.ReadLine();
                    Console.WriteLine("You entered: {0}", input);
                    if (input == "exit")
                    {
                        /*
                        This method has two different overloads, 
                        one that only takes a string and another 
                        one that also takes an exception. 
                        When this method is called, the message (and optionally the exception) are written to the 
                        Windows application event log, and the application is terminated.
                        */
                        Environment.FailFast("Failing a test");
                    }
                    if (input != null)
                    {
                        AnimalEnum choice = (AnimalEnum) int.Parse(input);
                        switch (choice)
                        {
                            case AnimalEnum.Donkey:
                                Console.WriteLine("This is Donkye");
                                goto case AnimalEnum.Monkey; 
                            case AnimalEnum.Monkey:
                                Console.WriteLine("This is Monkey");
                                goto default;
                            case AnimalEnum.Lion:
                                Console.WriteLine("This is Lion");
                                break;
                            default:
                                Console.WriteLine("Not a valid choice");
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }



            Console.WriteLine("Test");

        }
    }
}
