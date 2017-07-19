using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;

namespace SimpleMessages
{
    public class RegisterOrderCommand : CorrelatedBy<Guid>
    {
        public RegisterOrderCommand(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public int OrderId { get; set; }

        public Guid CorrelationId { get; }
    }
}
