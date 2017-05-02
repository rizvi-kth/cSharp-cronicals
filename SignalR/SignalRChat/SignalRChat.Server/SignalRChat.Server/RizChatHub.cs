using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hosting;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalRChat.Server
{
    [HubName("RizChat")]
    public class RizChatHub:Hub
    {

        // Method called by Client.
        [HubMethodName("TransportMethod")]
        public string GetUsedTransportMechanism()
        {
            string transportMethod = Context.QueryString["transport"];
            Debug.WriteLine($"Chat Transport : {transportMethod}");
            

            return transportMethod;
        }


        public override Task OnConnected()
        {
            var version = Context.QueryString["chatversion"];
            Debug.WriteLine($"Chat Version : {version}");
            Clients.Caller.ConformConnection(Context.ConnectionId, QueryStringConvert(Context.QueryString)); //, 

            return base.OnConnected();
        }

        private Dictionary<string, object> QueryStringConvert(INameValueCollection nameValueCollection)
        {
            var queryStrings = new Dictionary<string, object>();
            foreach (var item in nameValueCollection)
            {
                queryStrings.Add(item.Key,item.Value);
            }
            return queryStrings;
        }
    }
}