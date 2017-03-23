using System;
using System.CodeDom;
using System.IO;
using System.Net.Sockets;
using System.Text;

class Program
{
    public static void Main()
    {
        var num = new FullPhoneNumber() {Digit = "1234567890"};
        Console.WriteLine($"Unformatted string: {num.Digit}" );

        // Do a formatting with a formatter
        var formatted = string.Format(new PhoneNumberFormetter(), "{0}", num);
        Console.WriteLine($"Formatted string: {formatted}");


    }
}

// Seperation of Concern Principle: 
// Business logic should be seperated from the Presentation Logic.


// Business logic
public class FullPhoneNumber
{
    private string _digit;
    public string Digit {
        get { return this._digit; }
        set
        {
            if (value.Length != 10 )
                throw new ArgumentException();
            this._digit = value;
        }
    }
}

//Presentation Logic
public class PhoneNumberFormetter : ICustomFormatter, IFormatProvider
{
    public string Format(string format, object arg, IFormatProvider formatProvider)
    {
        if (arg is FullPhoneNumber)
        {
            var num = (FullPhoneNumber) arg;
            return "[" + num.Digit + "]";
        }
        else if(string.IsNullOrEmpty(format))
        {
            return arg.ToString();
        }
        else
        {
            return String.Format("{0:" + format + "}", arg);
        }
    }

    public object GetFormat(Type formatType)
    {
        if (formatType == typeof(ICustomFormatter))
            return this;

        return null;
    }
}