using System;

namespace AutoFac_Delegate_Factory
{
    public class CarFactory //: ICarFactory
    {
        public static Car CreateCar(string model)
        {
            return new Car() {
                Model = model
            };
            
        }
    }

    public interface ICarFactory
    {
        Car CreateCar(string model);

    }
}