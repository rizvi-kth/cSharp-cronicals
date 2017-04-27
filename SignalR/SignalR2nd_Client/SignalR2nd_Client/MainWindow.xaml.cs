using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Microsoft.AspNet.SignalR.Client;

namespace SignalR2nd_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IHubProxy stockTickerHubProxy;
        public MainWindow()
        {
            InitializeComponent();

        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var hubConnection = new HubConnection("http://localhost:37319/");
            stockTickerHubProxy = hubConnection.CreateHubProxy("MyChat");
            stockTickerHubProxy.On<string>("RecieveNotify", msg => Debug.WriteLine($">> Recieved message: {msg}"));
            stockTickerHubProxy.On<StockInfo>("handleStockInfo", stock => Dispatcher.Invoke( ()=> handleStockInfo(stock)));
            
            hubConnection.Start();
        }

        private void handleStockInfo(StockInfo stock)
        {
            listBox.Items.Add($"Stock:{stock.StockName } with value {stock.StockValue}");
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            stockTickerHubProxy.Invoke("ProcessChatMsg", "Rizvi", "Hello!");
        }
    }

    [DebuggerDisplay("Stock info : {StockName}")]
    public class StockInfo
    {
        public string StockName { get; set; }
        public int StockValue { get; set; }

    }
}
