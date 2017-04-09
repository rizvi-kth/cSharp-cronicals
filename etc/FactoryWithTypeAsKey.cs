using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Experiments
{

/*
    Advantages:
        As the factory class remains unchanged even when new products are added, this implementation doesn't violate the open and closed principle.
        This implementation provides the loose coupling between Factory class and concrete products.This is because product objects are created using reflection so the factory need not know the instantiation details of product class.
        The client doesn't need to be aware of all the available products. Only the required products need to be registered before use. Moreover product creation is abstracted from the client.
   
    Disadvantages
        The client has to register required product classes with the factory, before use.                
        This implementation is more difficult to implement.
*/
    class Program
    {
        static void Main(string[] args)
        {

            ProductFactory pf = new ProductFactory();
            // Products registration
            pf.RegisterProduct(typeof(ProductA), new ProductA());
            pf.RegisterProduct(typeof(ProductB), new ProductB());
            pf.RegisterProduct(typeof(ProductC), new ProductC());

            // Factory client consuming products by key
            Console.WriteLine($"Query for A brings {pf.GetProductByKey(typeof(ProductA)).Name}");
            Console.WriteLine($"Query for B brings {pf.GetProductByKey(typeof(ProductB)).Name}");
            Console.WriteLine($"Query for C brings {pf.GetProductByKey(typeof(ProductC)).Name}");
        }
    }

    public class ProductFactory
    {
        public IDictionary<Type, IProduct> productCatalogue = new Dictionary<Type, IProduct>();

        public void RegisterProduct(Type key, IProduct value)
        {
            productCatalogue.Add(key,value);
        }

        public IProduct GetProductByKey(Type key)
        {
            IProduct product;
            if (productCatalogue.TryGetValue(key, out product))
            {
                return product;
            }

            throw new ArgumentOutOfRangeException();
        }

    }

    internal class ProductC : IProduct
    {
        public string Name => "ProductC";
        public IProduct GetProduct()
        {
            return new ProductC();
        }


    }

    internal class ProductB : IProduct
    {
        public string Name => "ProductB";
        public IProduct GetProduct()
        {
            return new ProductB();
        }
    }

    internal class ProductA : IProduct
    {
        public string Name => "ProductA";
        public IProduct GetProduct()
        {
            return new ProductA();
        }
    }

    public interface IProduct
    {
        string Name { get;  }
        IProduct GetProduct();


    }
}
