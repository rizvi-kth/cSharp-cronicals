using System;

public class HandleRef
{
    internal void run()
    {
        Console.WriteLine(" Test:2 Control array position by Ref");
        

        int[] someArray = {1,4,7,9};
        Console.Write(" Given array:");
        foreach (var item in someArray)
        {
            Console.Write(" {0}",item);
        }
        ref int position = ref getPosition(7, someArray);
        position = 888;
        
        Console.WriteLine(" \nReplacing 7 with 888 ");
        Console.Write("Result array:");

        foreach (var item in someArray)
        {
            Console.Write(" {0}",item);
        }
    }

    private ref int getPosition(int v, int[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] == v)
                return ref arr[i];
        }

        throw new IndexOutOfRangeException("Not found!");
    }
}