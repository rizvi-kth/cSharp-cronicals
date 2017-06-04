using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFac_Delegate_Factory
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Car>().As<ICar>();
            builder.RegisterType<CarFactory>();//.As<ICarFactory>();
            builder.Register<Func<string, ICar>>((context, parameters) => CarFactory.CreateCar);
            builder.RegisterType<Buyer>();

            //builder.RegisterType<BaseDependency>().As<IDependency>().SingleInstance();
            var Container = builder.Build();

            var buyer = Container.Resolve<Buyer>();

            //buyer.FactoryCreateCar.Invoke("BMW");
            buyer.Show();


            Console.ReadLine();
        }
    }
}
