using System;

namespace AutoFac_Delegate_Factory
{
    public class Buyer
    {

        // 2. This the Factory-method
        public Func<string, ICar> FactoryCreateCar { get; set; }

        
        // 1. Client (Buyer) should depend on a Factory.
        public Buyer(Func<string, ICar> FactoryCreateCar)
        {
            this.FactoryCreateCar = FactoryCreateCar;
        }

        public void Show()
        {
            ICar car = FactoryCreateCar.Invoke("BMW");
            Console.WriteLine($"The car is : {car.Model}");
        }

    }
}