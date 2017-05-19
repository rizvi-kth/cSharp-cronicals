using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

//  This is the approach most developers will specify the startup class. 
//[assembly: OwinStartup(typeof(OWIN.Test.Startup))]

// The appSetting element overrides the OwinStartup attribute and naming convention.
/*
 <configuration>
  <appSettings>
    <add key="owin:appStartup" value="StartupDemo.ProductionStartup, StartupDemo" />
  </appSettings>
  ...
 </configuration>
     */

namespace OWIN.Test
{
    // Katana looks for a class named Startup in namespace matching the assembly name or 
    // the global namespace.
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Good to read
            // https://www.codeproject.com/tips/1069790/understand-run-use-map-and-mapwhen-to-hook-middlew
            
            // Add the error page middleware to the pipeline. 
            app.UseErrorPage();
            
            // The middleware class can be easily added to the OWIN pipeline in the application startup code.
            // In case of Use extension, there is a chance to pass next invoker, so that HTTP request will be 
            // transferred to the next middleware after execution of current Use if there next invoker is present.
            app.Use<LoggerMiddleware>(new TraceLogger());
            // Next middleware 
            app.Use<LoggerMiddleware>(new TraceLoggerNext());
            // Another Middleware
            app.Use(async (environment, next) => {
                Console.WriteLine($">> Requesting {environment.Request.Path}");
                // Everything before next() call is request processing.
                await next();
                // Everything after next() call is response processing.
                Console.WriteLine($">> Responsing {environment.Response.StatusCode}");

            });


            app.UseWelcomePage("/");

            // Map extensions are used as convention for branching the pipeline. 
            // We can hook delegate to Map extension to push it to HTTP pipeline. 
            app.Map("/mapping", _app =>
            {
                _app.Run(async context => { await context.Response.WriteAsync("Now its mapping."); });
            });

            // The nature of Run extension is to short circuit the HTTP pipeline immediately.
            // So, it’s recommended to use Run extension to hook middleware at last in HTTP pipeline. 
            app.Run(async context =>
            {
                // Test OWIN error page: Throw an exception for this URI path.
                if (context.Request.Path.Value.EndsWith("/fail") )
                {
                    throw new Exception("Random exception");
                }

                if (context.Request.Path.Value.EndsWith("/hello"))
                {
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync("Hello, OWIN.");
                }
                await Task.FromResult<IOwinContext>(null);
            });
        }

       
    }
}
