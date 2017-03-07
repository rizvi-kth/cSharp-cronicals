using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ThrowException1(); // line 19
            }
            catch (Exception x)
            {
                Console.WriteLine("Exception 1:");
                Console.WriteLine(x.StackTrace);
            }
            try
            {
                ThrowException2(); // line 25
            }
            catch (Exception x)
            {
                Console.WriteLine("Exception 2:");
                Console.WriteLine(x.StackTrace);
            }
        }

        private static void ThrowException1()
        {
            try
            {
                DivByZero(); // line 34
            }
            catch
            {
                throw; // line 36
            }
        }
        private static void ThrowException2()
        {
            try
            {
                DivByZero(); // line 41
            }
            catch (Exception ex)
            {
                // With this 'throw ex' we will lose the info of function DivByZero()
                // in the exception call stack. 
                throw ex; // line 43
            }
        }

        private static void DivByZero()
        {
            int x = 0;
            int y = 1 / x; // line 49
        }
    }
}
