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
        static int? GetNullableInt()
        {
            return null;
        }

        static string GetStringValue()
        {
            return null;
        }

        static void Main()
        {
            int? x = null;

            // Set y to the value of x if x is NOT null; otherwise,
            // if x = null, set y to -1.
            int y = x ?? -1;

            // Assign i to return value of the method if the method's result
            // is NOT null; otherwise, if the result is null, set i to the
            // default value of int.
            int i = GetNullableInt() ?? default(int);

            string s = GetStringValue();
            // Display the value of s if s is NOT null; otherwise, 
            // display the string "Unspecified".
            Console.WriteLine(s ?? "Unspecified");

            var someOne = new
            {
                Name = "Dina",
                id = x ?? 9999
            };

            Console.WriteLine($"Student  {someOne.Name}  with id  {someOne.id}");

        }


    }

    public class SomeStudent
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
