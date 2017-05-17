using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;
using OWIN.WebAPI.AutoFac.Others;

[assembly: OwinStartup(typeof(OWIN.WebAPI.AutoFac.Startup))]

namespace OWIN.WebAPI.AutoFac
{
    public class Startup
    {
        // Owin integration #
        // Install : Microsoft.Owin.Host.SystemWeb - This package provides an OWIN server that runs in the ASP.NET request pipeline.
        // Install : Microsoft.AspNet.WebApi.Owin        
        // AutoFac integration # 
        // Install : Autofac.WebApi2.Owin

        public void Configuration(IAppBuilder app)
        {
            // In OWIN you create your own HttpConfiguration rather than re-using the GlobalConfiguration.
            var httpConfig = new HttpConfiguration();
            httpConfig.MapHttpAttributeRoutes();
            
            // Builder for AutoFac
            var builder = new ContainerBuilder();

            // Register Web API controller in executing assembly.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Register a logger service to be used by the controller and middleware as dependency.
            builder.Register(c => new Logger()).As<ILogger>().InstancePerRequest();

            // Create and assign a dependency resolver for Web API to use.
            var container = builder.Build();
            httpConfig.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // Make sure the Autofac lifetime scope is passed to Web API.
            app.UseAutofacWebApi(httpConfig);

            // Add WebAPI framework as OWIN middleware
            app.UseWebApi(httpConfig);

            app.Map("/owinstatus", _app => {
                _app.Run(async context =>
                {
                    await context.Response.WriteAsync("App is running");
                });
            });

        }
    }
}
