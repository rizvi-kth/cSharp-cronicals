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
            // SignalR enables you to inject your own code into the Hub pipeline.
            // LoggingPipelineModule - a custom Hub pipeline module that logs each incoming/Outgoing method call
            GlobalHost.HubPipeline.AddModule(new LoggingPipelineModule());

            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888

            //Hub configuration 
            var hubConfig = new HubConfiguration();
            hubConfig.EnableDetailedErrors = true;
            hubConfig.EnableJavaScriptProxies = false;
            //hubConfig.Resolver
            app.MapSignalR("/rzSignalr", hubConfig);

        }
    }
}
