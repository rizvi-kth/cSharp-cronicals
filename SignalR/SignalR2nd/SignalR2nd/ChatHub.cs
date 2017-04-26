using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SignalR2nd
{
    public class ChatHub : Hub
    {
        public void ProcessChatMsg(string name, string message)
        {

            var contextUser = Context.User;
            // Call the broadcastMessage method to update clients.
            Debug.WriteLine($">> Message from {name} is {message} with connection id {Context.ConnectionId}");
            Clients.All.RecieveNotify(message);
        }



        public override Task OnConnected()
        {
            // Add your own code here.
            // For example: in a chat application, record the association between
            // the current connection ID and user name, and mark the user as online.
            // After the code in this method completes, the client is informed that
            // the connection is established; for example, in a JavaScript client,
            // the start().done callback is executed.
            Debug.WriteLine($"Connection opened : {Context.ConnectionId}");
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            // Add your own code here.
            // For example: in a chat application, mark the user as offline, 
            // delete the association between the current connection id and user name.
            Debug.WriteLine($"Connection cloased : {Context.ConnectionId}" );

            return base.OnDisconnected(false);
        }


    }
}