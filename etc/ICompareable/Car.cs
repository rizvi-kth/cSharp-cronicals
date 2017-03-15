using System;
using System.Collections;
using System.Collections.Generic;

namespace ICompareable
{
    internal class Car : IComparable<Car>
    {
        public Car()
        {

        }
        public string Name { get; set; }
        public double Price { get; set; }


        public int CompareTo(Car other)
        {
            // If you want to sort by Name
            return String.Compare(this.Name, other.Name, StringComparison.OrdinalIgnoreCase);

            // If you want to sort by Price
            //return this.Price.CompareTo(other.Price);
        }


        private class SortPriceAscendingComparer : IComparer<Car>
        {
            public int Compare(Car c1, Car c2)
            {
                if (c1.Price > c2.Price)
                    return 1;

                if (c1.Price < c2.Price)
                    return -1;

                else
                    return 0;
            }
        }

        // Method to return IComparer object for sort helper.
        public static IComparer<Car> SortPriceAscending()
        {
            return (IComparer<Car>) new SortPriceAscendingComparer();
        }

        private class SortPriceDescendingComparer : IComparer<Car>
        {
            public int Compare(Car c1, Car c2)
            {
                if (c1.Price < c2.Price)
                    return 1;
                if (c1.Price > c2.Price)
                    return -1;
                else
                    return 0;
            }
        }

        // Method to return IComparer object for sort helper.
        public static IComparer<Car> SortPriceDescending()
        {
            return (IComparer<Car>) new SortPriceDescendingComparer();
        }

    }
}