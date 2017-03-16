using System;
using System.IO;
using System.Text;

class Program
{
    public static void Main()
    {
        
        // This loop will continuously create a lot of unmanaged objects.
        while (true)
        {

            // Using stement will automatically dispose the unmanaged objects. 
            // So the memory usage will NOT rise by time in the while loop.
            using (FileStream fs = File.Open(Path.Combine(@"C:\temp\testfile.txt"), FileMode.Open,FileAccess.Read,FileShare.Read))
            {

                // Without Using stement the unmanaged objects will NOT BE automatically disposed. 
                // So the memory usage will rise by time in the while loop.
                //FileStream fs = File.Open(Path.Combine(@"C:\temp\testfile.txt"), FileMode.Open, FileAccess.Read, FileShare.Read);

                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);

                while (fs.Read(b, 0, b.Length) > 0)
                {
                    Console.WriteLine(temp.GetString(b).Trim());
                }

                // Every time a new object will be created with a new Hash-Code.
                Console.WriteLine("Object Hashcode:{0}",fs.GetHashCode());
            }
        }

    }
}