using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automatonymous;

namespace SimpleSaga
{
    public class OrderState : SagaStateMachineInstance
    {
        // Automatonymous Interface Properties
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }

        // Properties for message object
        public int SagaOrderId { get; set; }
        public string SagaProductName { get; set; }   

        // Extra property for Saga State
        public DateTime SagaOrderRecieveTime { get; set; }
        public DateTime SagaOrderConformTime { get; set; }


    }
}
