using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;


namespace Messages
{
    public class MyMessage : IMessage
    {
        public MyMessage(string sampleQuota)
        {
            SampleQuota = sampleQuota;
        }

        public string SampleQuota { get; set; }


    }
}
