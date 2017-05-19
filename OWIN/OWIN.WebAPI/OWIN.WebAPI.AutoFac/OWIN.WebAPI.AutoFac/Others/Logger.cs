using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace OWIN.WebAPI.AutoFac.Others
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