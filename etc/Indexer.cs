using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

class Program
{
    public static void Main()
    {
        Employee e1 = new Employee(1,"Saman", "Hasb");
        Employee e2 = new Employee(4,"Max", "Malgrin");
        Employee e3 = new Employee(10);

        Office<Employee> office = new Office<Employee>();
        office.EmployeeList.Add(e1);
        office.EmployeeList.Add(e2);
        office.EmployeeList.Add(e3);

        foreach (Employee employee in office.EmployeeList)
        {
            Console.WriteLine($"Employe {employee}");
        }

        int id = 4;
        Console.WriteLine($"Get employe by id indexer: {id}");
        Console.WriteLine($"Employe : {office[id]}");
        




    }

    public class Office<T>  
        where T: class, new()
    {
        public IList<T> EmployeeList { get; } = new List<T>();

        // Implementing a indexer
        public T this[int id_Index]
        {
            get { return EmployeeList.SingleOrDefault((p) =>
                {
                    var employee = p as Employee;
                    if (employee?.Id == id_Index)
                        return true;

                    return false;
                });
            }
            
        }
    }

    public class Employee
    {
        public Employee() : this(0, "<No-Name>", string.Empty)
        { }

        // Constructors should be chained to a single entry point
        public Employee(int id) : this(id, "<No-Name>", string.Empty)
        {}

        // Entry point constructor
        public Employee(int id,string e, string f)
        {
            this.Id = id;
            this.Ename = e;
            this.Fname = f;
        }


        public int Id { get; set; }
        public string Ename { get; set; }
        public string Fname { get; set; }

        public override string ToString()
        {
            return Ename + " " + Fname;

        }
    }
    
}

