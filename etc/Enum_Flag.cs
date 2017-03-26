using System;
using System.CodeDom;
using System.IO;
using System.Net.Sockets;
using System.Text;

class Program
{

    // You can use an enumeration type to define bit flags, which enables an instance of the enumeration type 
    // to store any combination of the values that are defined in the enumerator list. 
    // (Of course, some combinations may not be meaningful or allowed in your program code.)

    // You create a bit flags enum by applying the System.
    // FlagsAttribute attribute and defining the values appropriately so that AND, OR, NOT and XOR bitwise operations 
    // can be performed on them.In a bit flags enum, include a named constant with a value of zero that means 
    // "no flags are set." Do not give a flag a value of zero if it does not mean "no flags are set".

    [Flags]
    public enum DaysOfWeek : short
    {
        None = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 4,
        Thursday = 8,
        Friday = 16,
        Saturday = 32,
        Sunday = 64
    }
    
    public static void Main()
    {
        // Initialize with two flags using bitwise OR.
        DaysOfWeek meetingDays = DaysOfWeek.Monday | DaysOfWeek.Wednesday;
        Console.WriteLine($"Meeting days are : {meetingDays}");

        // Set an additional flag using bitwise OR.
        meetingDays = meetingDays | DaysOfWeek.Tuesday;
        Console.WriteLine($"New meeting days are : {meetingDays}");

        // Remove a flag using bitwise XOR.
        meetingDays = meetingDays ^ DaysOfWeek.Wednesday;
        Console.WriteLine($"New meeting days are : {meetingDays}");


        // Test value of flags using bitwise AND.
        bool isMeetingDay = (meetingDays & DaysOfWeek.Tuesday) == DaysOfWeek.Tuesday;
        Console.WriteLine("{0} {1} a meeting day", DaysOfWeek.Monday ,isMeetingDay == true ? "is" : "is not");

    }
    
}