using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(OWIN.WebAPI.Startup))]

namespace OWIN.WebAPI
{
    // Install for Host   : Microsoft.Owin.Host.SystemWeb
    // Install for WebAPI : Microsoft.AspNet.WebApi.Owin
    // Application_Start VS Startup.Configuration read:
    // http://stackoverflow.com/questions/20168978/do-i-need-a-global-asax-cs-file-at-all-if-im-using-an-owin-startup-cs-class-and
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var webApiConfig = new HttpConfiguration();
            webApiConfig.MapHttpAttributeRoutes();

            app.UseWebApi(webApiConfig);

        }
    }
}
