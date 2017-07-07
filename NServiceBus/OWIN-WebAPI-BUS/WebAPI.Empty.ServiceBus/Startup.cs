using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.Owin.Cors;
using Owin;
using WebAPI.Empty.ServiceBus.OWIN.BusMiddleware;

namespace WebAPI.Empty.ServiceBus
{



    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var webApiConfig = new HttpConfiguration();
            webApiConfig.MapHttpAttributeRoutes();
            
            app.UseWebApi(webApiConfig);
            app.UseCors(CorsOptions.AllowAll);

            MapToMsmq(app);

            app.Map("/owinstatus", _app => {
                _app.Run(async context =>
                {
                    await context.Response.WriteAsync(" ** App is running ** ");
                });
            });

        }

        static void MapToMsmq(IAppBuilder builder)
        {
            var queue = Environment.MachineName + @"\private$\webapi.owin.msmq";
            var owinToMsmq = new OwinToMsmq(queue);
            builder.Map("/to-msmq", app => { app.Use(owinToMsmq.Middleware()); });
        }
    }
}