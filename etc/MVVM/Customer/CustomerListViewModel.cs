using System.Collections.Generic;

namespace MVVM.Customer
{
    public class CustomerListViewModel:BaseViewModel
    {
        public CustomerListViewModel()
        {
            CustomerList = new List<Customer>()
            {
                new Customer() {Name = "Robin",Adress = "Stockholm"},
                new Customer() {Name = "Rifat",Adress = "New work"}
            };        
        }

        public List<Customer> CustomerList { get; set; }
        
    }

    public class Customer
    {
        public string Name { get; set; }
        public string Adress { get; set; }

    }
}