using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experiments
{
    class Program
    {
        static void Main(string[] args)
        {


            IProduct product = GetProduct(1);
            Console.WriteLine(product.Name);

            product = GetProduct(2);
            Console.WriteLine(product.Name);

            product = GetProduct(3);
            Console.WriteLine(product.Name);

        }

        static IProduct GetProduct(int id)
        {
            IDictionary<int, Func<IProduct>> products = new Dictionary<int, Func<IProduct>>
                {
                    {1, () => new ProductA() },
                    {2, () => new ProductB() },
                    {3, () => new ProductC() }
                };

            Func<IProduct> product;
            if (products.TryGetValue(id, out product))
                return product();
            else
                return null;
        }
    }

    internal class ProductC : IProduct
    {
        public string Name => "ProductC";
    }

    internal class ProductB : IProduct
    {
        public string Name => "ProductB";
    }

    internal class ProductA : IProduct
    {
        public string Name => "ProductA";
    }

    internal interface IProduct
    {
        string Name { get; }

    }
}
