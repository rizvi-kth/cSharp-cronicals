using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace Bus.Messages
{
    public class MySampleMessage :IMessage
    {
        public string NameProperty { get; set; }
        public string SpeedProperty { get; set; }
    }
}
