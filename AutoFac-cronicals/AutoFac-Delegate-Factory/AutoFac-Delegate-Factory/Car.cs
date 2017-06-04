using System;

namespace AutoFac_Delegate_Factory
{
    public class Car : ICar
    {
        public Car()
        {

        }
        public string Model { get; set; }

        
    }

    public interface ICar
    {
        string Model { get; set; }

    }
}