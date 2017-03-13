using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using ThreadState = System.Diagnostics.ThreadState;

namespace ThreadedList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<string> listWord = new ObservableCollection<string>();
        private int count = 0;
        private int threadCount = 0;
        public MainWindow()
        {
            InitializeComponent();
            listBox.ItemsSource = listWord;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            
            // THIS APPROACH(PREFERED) DOES NOT USES THE UI-THREAD-CONTEXT IN THE WORKER THREAD; 
            // THATS WHY THE WORKER THREAD IS ASYNCHRONOS AS EXPECTED. 

            threadCount++;
            button.Content = string.Format("Thread Count: {0}" ,threadCount);
            Task<int> t1 = Task<int>.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return count++;
            });


            t1.ContinueWith((antecedent) =>
            {
                listWord.Add(string.Format("Item-{0}", antecedent.Result));
                threadCount--;
                button.Content = string.Format("Thread Count: {0}", threadCount);

            }, TaskScheduler.FromCurrentSynchronizationContext());
            
        }


        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //THIS APPROACH USES THE UI - THREAD - CONTEXT IN THE WORKER THREAD;
            //THATS WHY THE WORKER THREAD BECOMES SYNCHRONOUS(BLOCKS UI) INSTADE OF BEING ASYNCHRONOS.
            threadCount++;
            button2.Content = string.Format("Thread Count: {0}", threadCount);
            Task.Factory.StartNew(() =>
            {
               Thread.Sleep(5000);
               listWord.Add(string.Format("Item-{0}", count++));

               threadCount--;
               button2.Content = string.Format("Thread Count: {0}", threadCount);

            }, CancellationToken.None, TaskCreationOptions.HideScheduler, TaskScheduler.FromCurrentSynchronizationContext());

        }
    }
}
