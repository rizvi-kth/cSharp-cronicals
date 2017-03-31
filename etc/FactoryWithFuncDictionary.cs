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
    FACTORY WITH DICTIONARY OF TYPE AND DELEGATE FUNCTION
    
        Advantages:
        As the factory class remains unchanged even when new products are added, this implementation doesn't violate the open and closed principle.
        This implementation provides the loose coupling between Factory class and concrete products. This is because product objects are created using a delegate passed to the factory dictionary.
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
            pf.RegisterProduct(typeof(ProductA), () => new ProductA() );
            pf.RegisterProduct(typeof(ProductB), () => new ProductB(new ProductA()));
            pf.RegisterProduct(typeof(ProductC), () => new ProductC(new ProductB(new ProductA())));

            // Factory client consuming products by key
            Console.WriteLine($"Query for A brings {pf.GetProductByKey(typeof(ProductA))().Name}");
            Console.WriteLine($"Query for B brings {pf.GetProductByKey(typeof(ProductB))().Name}");
            Console.WriteLine($"Query for C brings {pf.GetProductByKey(typeof(ProductC))().Name}");
        }
    }

    public class ProductFactory
    {
        public IDictionary<Type, Func<IProduct>> productCatalogue = new Dictionary<Type, Func<IProduct>>();

        public void RegisterProduct(Type key, Func<IProduct> value)
        {
            productCatalogue.Add(key, value);
        }

        public Func<IProduct> GetProductByKey(Type key)
        {
            Func<IProduct> product;
            if (productCatalogue.TryGetValue(key, out product))
            {
                return product;
            }

            throw new ArgumentOutOfRangeException();
        }

    }

    internal class ProductC : IProduct
    {
        private IProduct prodB;
        public ProductC(IProduct B)
        {
            prodB = B;
        }
        public string Name => prodB.Name +  " -> ProductC";
        

    }

    internal class ProductB : IProduct
    {
        private IProduct prodA;
        public ProductB(IProduct A)
        {
            prodA = A;
        }
        public string Name => prodA .Name +  " -> ProductB";
        
    }

    internal class ProductA : IProduct
    {
        public string Name => "ProductA";
        
    }

    public interface IProduct
    {
        string Name { get;  }
        //IProduct CreateProduct();


    }
}
