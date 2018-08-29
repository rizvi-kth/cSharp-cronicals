
using System;
using System.Collections.Generic;

public class ToupleTest
{
    
    internal void run()
    {
        Console.WriteLine("Returning touple> {0} : {1}", this.GetTouple().Item1, this.GetTouple().Item2);           
        (int s, int t) = this.GetTouple();
	    Console.WriteLine("Deconstructing touple> {0} : {1}", s, t);
	    Console.WriteLine("Dict Value at > {0}", this.TestToupleDict(this.GetTouple()));
    }
    
	public (int,int) GetTouple()
	{
		return (6,3);
	}

    internal string TestToupleDict((int, int) p)
    {
        var myDict = new Dictionary<(int, int), string>();
        myDict.Add((6,3), "Room 6 floor 3");
        myDict.Add((7,4), "Room 7 floor 4");

        return myDict[p];
    }

}