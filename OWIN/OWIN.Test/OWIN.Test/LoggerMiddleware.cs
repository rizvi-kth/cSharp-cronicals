using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin;

namespace OWIN.Test
{

    /* 
     * This class derives from OwinMiddleware, implements a constructor that accepts 
     * an instance of the next middleware in the pipeline as one of its arguments, 
     * and then passes it to the base constructor.Additional arguments used to configure 
     * the middleware are also declared as constructor parameters after the next middleware parameter. */
    public class LoggerMiddleware : OwinMiddleware
    {
        private readonly ILog _logger;

        public LoggerMiddleware(OwinMiddleware next, ILog logger) : base(next)
        {
            _logger = logger;
        }

        public override async Task Invoke(IOwinContext context)
        {
            _logger.LogInfo(" >> Middleware on request line.");
            await this.Next.Invoke(context);
            _logger.LogInfo(" << Middleware on response line.");
        }
    }

    public interface ILog
    {
        void LogInfo(string msg);
    }

    internal class TraceLogger:ILog
    {
        public TraceLogger()
        {
        }

        public void LogInfo(string msg)
        {
            Debug.WriteLine($"LogMiddle {msg}");
        }
    }

    internal class TraceLoggerNext : ILog
    {
        public TraceLoggerNext()
        {
        }

        public void LogInfo(string msg)
        {
            Debug.WriteLine($"NextLogMiddle {msg}");
        }
    }
}