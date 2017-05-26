using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Rz.OWIN.WebAPI.AutoFac.DI.Middlewares
{
    public class Logger : ILogger
    {
        //public void Write(string message, params object[] args)
        //{
        //    Debug.WriteLine(message, args);
        //}
        public void Write(string message)
        {
            Debug.WriteLine(message);
        }
    }
}