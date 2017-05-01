using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Microsoft.AspNet.SignalR.Client.Transports;

namespace SignalRChat.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HubConnection hubConnection;
        private IHubProxy hubProxy;
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            var querystringData = new Dictionary<string, string>();
            querystringData.Add("chatversion", "1.0");
            hubConnection = new HubConnection("http://localhost:17968/rzSignalr", querystringData, useDefaultUrl: false);
            hubProxy = hubConnection.CreateHubProxy("RizChat");
            hubProxy.On<string, Dictionary<string, object>>("ConformConnection", (connectionId, queryStrings) => ConformConnectionStatus(connectionId, queryStrings));

            ServicePointManager.DefaultConnectionLimit = 10;
            
            //TODO: Get the connection status from s (TaskStatus)
            await hubConnection.Start().ContinueWith((s) => StatusUpdate(s) , TaskScheduler.FromCurrentSynchronizationContext());
            //await hubConnection.Start(new LongPollingTransport());
             
        }
        // Callback for hubConnection.Start() async method.
        private void StatusUpdate(Task t)
        {
            listBox.Items.Add("Status : " + (t.Status.Equals(TaskStatus.RanToCompletion) ? "Connected!" : "Dis..Connected"));
        }

        // Client-Method that is called by server
        private void ConformConnectionStatus(string connectionId, Dictionary<string,object> queryStrings)
        {
            Dispatcher.InvokeAsync(() =>
            {
                listBox.Items.Add($"Connection ID: {connectionId}");
                foreach (var item in queryStrings)
                {
                    listBox.Items.Add($"{item.Key} : {item.Value}");
                }
            });
            //Console.WriteLine($"Connection : {connectionId}");
        }

        private async void button1_Click(object sender, RoutedEventArgs e)
        {
            if (hubConnection != null && hubProxy != null)
            {
                // Call a server method-'TransportMethod' which returns <String>
                var transMethod = await hubProxy.Invoke<string>("TransportMethod");
                listBox.Items.Add($"Final Transport Method : {transMethod}");
            }
            else
            {
                listBox.Items.Add("No connection established.");
            }
        }
    }
}
