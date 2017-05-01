using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SignalRChat.Server.Startup))]

namespace SignalRChat.Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            var hubConfig = new HubConfiguration();
            hubConfig.EnableDetailedErrors = true;
            hubConfig.EnableJavaScriptProxies = false;
            //hubConfig.Resolver
            app.MapSignalR("/rzSignalr", hubConfig);

        }
    }
}
