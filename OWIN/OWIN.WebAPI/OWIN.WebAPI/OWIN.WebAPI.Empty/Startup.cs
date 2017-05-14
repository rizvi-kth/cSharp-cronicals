using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(OWIN.WebAPI.Empty.Startup))]

namespace OWIN.WebAPI.Empty
{
    public class Startup
    {
        // NuGet package:
        // Install : Microsoft.Owin.Host.SystemWeb - This package provides an OWIN server that runs in the ASP.NET request pipeline.
        // Install : Microsoft.AspNet.WebApi.Owin
        public void Configuration(IAppBuilder app)
        {
            var webApiConfig = new HttpConfiguration();
            webApiConfig.MapHttpAttributeRoutes();
            
            app.UseWebApi(webApiConfig);

            app.Map("/owinstatus", _app => {
                _app.Run(async context =>
                {
                    await context.Response.WriteAsync("App is running");
                });
            });
            //app.Run(async context =>
            //{
            //    await context.Response.WriteAsync("App is running");
            //});
        }
    }
}
