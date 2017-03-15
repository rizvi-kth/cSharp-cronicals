using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ICompareable
{

    internal class Bus: IComparable<Bus>
    {
        public string Name { get; set; }
        public double Price { get; set; }

        // A default sort without comparer parameter 
        int IComparable<Bus>.CompareTo(Bus other)
        {
            return this.Price.CompareTo(other.Price);
        }
    }


    // The preferad way of implementing the IComparer<T>.
    internal class BusComparer : IComparer<Bus>
    {
        private SortField SortBy; 
        public BusComparer(SortField SortBy)
        {
            this.SortBy = SortBy;
        }

        public enum SortField
        {
            Name_asc,
            Name_dsc,
            Price_asc,
            Price_dsc
        }

        public int Compare(Bus x, Bus y)
        {
            int res;
            switch (SortBy)
            {
                case SortField.Name_asc:
                    return x.Name.CompareTo(y.Name);
                case SortField.Name_dsc:
                    res = x.Name.CompareTo(y.Name);
                    return (res * -1); 
                case SortField.Price_asc:
                    return x.Price.CompareTo(y.Price);
                case SortField.Price_dsc:
                    res = x.Price.CompareTo(y.Price);
                    return res * -1;
                default:
                    return x.Name.CompareTo(y.Name);
            }
        }
    }
}