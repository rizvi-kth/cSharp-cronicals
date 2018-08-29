using System;

					
public class Program
{
	public static void Main1()
	{
		var run = new Runner();
		
		Console.WriteLine("Hello World {0} : {1}", run.GetCordinates().Item1, run.GetCordinates().Item2);
		
	}
	
/*	public static (int x, int y) GetCordinates()
	{
		return (3,6)
	}
*/
}


public class Runner
{
	public (int,int) GetCordinates()
	{
		return (6,3);
	}
}
