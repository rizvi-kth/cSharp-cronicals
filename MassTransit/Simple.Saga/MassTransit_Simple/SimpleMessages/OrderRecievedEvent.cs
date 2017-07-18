using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;

namespace SimpleMessages
{
    public class OrderRecievedEvent: CorrelatedBy<Guid>
    {
        public OrderRecievedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
            //var registerOrderCommand = order as RegisterOrderCommand;
            //if (registerOrderCommand != null)
            //{
            //    this.OrderId = registerOrderCommand.OrderId;
            //    this.ProductName = registerOrderCommand.ProductName;
            //}
                

        }

        public int OrderId { get; set; }
        public string ProductName { get; set; }

        public Guid CorrelationId { get; }
    }
}
