﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace Messages
{
    public class OrderBilled :IEvent
    {
        public string BilledOrderID{ get; set; } 
    }
}
