using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;

namespace CommonMessages
{
    public class AcquireLicenseCommand : CorrelatedBy<Guid>
    {
        public AcquireLicenseCommand(Guid correlationId)
        {
            this.CorrelationId = correlationId;
        }
        public int OrderSequence { get; set; }

        public Guid CorrelationId { get; }
    }
}
