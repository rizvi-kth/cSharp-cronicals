using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ICompareable
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Car car = new Car();
        private List<Car> cars; 
        private List<Bus> Buses; 

        public MainWindow()
        {
            InitializeComponent();
        }


        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {

         


        }

        private void button0_Click(object sender, RoutedEventArgs e)
        {
            cars = new List<Car>()
            {
                new Car() { Name = "Crasler", Price = 300000.00},
                new Car() { Name = "Marcidiz", Price = 350000.00},
                new Car() { Name = "BMW", Price = 400000.00},
                new Car() { Name = "McLaren", Price = 700000.00}
            };
            listBox.ItemsSource = cars;
        }


        private void button_Click(object sender, RoutedEventArgs e)
        {
            listBox1.ItemsSource = null;
            cars.Sort();
            listBox1.ItemsSource = cars;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //var arrayCars = cars.ToArray();
            //Array.Sort(arrayCars, Car.SortPriceAscending());
            //listBox1.ItemsSource = arrayCars;
            listBox1.ItemsSource = null;
            cars.Sort(Car.SortPriceAscending());
            listBox1.ItemsSource = cars;


        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //var arrayCars = cars.ToArray();
            //Array.Sort(arrayCars, Car.SortPriceDescending());
            //listBox1.ItemsSource = arrayCars;
            listBox1.ItemsSource = null;
            cars.Sort(Car.SortPriceDescending());
            listBox1.ItemsSource = cars;
        }

        
        private void button1_Bus_Click(object sender, RoutedEventArgs e)
        {
            listBox.ItemsSource = null;
            IComparer<Bus> busComperar = new BusComparer(BusComparer.SortField.Price_asc);
            Buses.Sort(busComperar);
            listBox.ItemsSource = Buses;
        }

        private void button2_Bus_Click(object sender, RoutedEventArgs e)
        {
            listBox.ItemsSource = null;
            IComparer<Bus> busComperar = new BusComparer(BusComparer.SortField.Price_dsc);
            Buses.Sort(busComperar);
            listBox.ItemsSource = Buses;
        }

        private void button0_Bus_Click(object sender, RoutedEventArgs e)
        {
            Buses = new List<Bus>()
            {
                new Bus() { Name = "Scania", Price = 300000.00},
                new Bus() { Name = "Volvo", Price = 350000.00},
                new Bus() { Name = "Ford", Price = 400000.00},
                new Bus() { Name = "Man", Price = 700000.00}
            };

            Buses.Sort();
            listBox.ItemsSource = Buses;
        }

        private void buttonND_Bus_Click(object sender, RoutedEventArgs e)
        {
            listBox.ItemsSource = null;
            IComparer<Bus> busComperar = new BusComparer(BusComparer.SortField.Name_asc);
            Buses.Sort(busComperar);
            listBox.ItemsSource = Buses;
        }

        private void buttonNA_Bus_Click(object sender, RoutedEventArgs e)
        {
            listBox.ItemsSource = null;
            IComparer<Bus> busComperar = new BusComparer(BusComparer.SortField.Name_dsc);
            Buses.Sort(busComperar);
            listBox.ItemsSource = Buses;
        }
    }
}
