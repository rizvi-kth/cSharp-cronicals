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
            Lion ,
            Bird
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

                        // A jump statement is required at every case/default block
                        // jump statements: return, break, goto, goto case, continue and throw
                        switch (choice)
                        {
                            case AnimalEnum.Donkey:
                                Console.WriteLine("This is Donkye");
                                goto case AnimalEnum.Monkey;                        // A jump statement is required
                            case AnimalEnum.Monkey:
                                Console.WriteLine("This is Monkey");
                                goto default;                                       // A jump statement is required
                            case AnimalEnum.Lion:
                                Console.WriteLine("This is Lion");
                                throw new Exception("This lion caused exception!"); // A jump statement is required
                            case AnimalEnum.Bird:
                                Console.WriteLine("This is Bird");
                                break;                                              // A jump statement is required
                            default:
                                Console.WriteLine("Not a valid choice");
                                continue;                                           // A jump statement is required
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
