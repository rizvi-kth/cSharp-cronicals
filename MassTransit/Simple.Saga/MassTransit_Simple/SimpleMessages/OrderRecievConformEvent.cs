using System;
using MassTransit;

namespace SimpleMessages
{
    public class OrderRecievConformEvent : CorrelatedBy<Guid>
    {
        public OrderRecievConformEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        

        public int OrderId { get; set; }
        public string ProductName { get; set; }

        public Guid CorrelationId { get; }
    }
}