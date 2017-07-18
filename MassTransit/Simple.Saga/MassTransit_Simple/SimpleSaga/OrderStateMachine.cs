using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automatonymous;
using SimpleMessages;

namespace SimpleSaga
{
    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {
        public State OrderRecieved { get; set; }
        public State OrderConformed { get; set; }
        public Event<RegisterOrderCommand> RegisterOrderEvent { get; set; }
        public Event<OrderRecievConformEvent> OrderConformEvent { get; set; }

        public OrderStateMachine()
        {
            // Tell Saga which property in State-Object(OrderState) will it use to keep track of the STATE.
            InstanceState(x => x.CurrentState);

            // How the incoming message is associated/correlated with State-Object(ie. OrderState) : defined here with Event()
            // CorrelateBy(state => state.OrderId.ToString(), context => context.Message.OrderId.ToString()) : this function tells to
            // Correlate Saga-State-Object(ie. OrderState).SagaOrderId property with incomming message's (ie. RegisterOrderCommand).OrderId property
            // along with other properties.
            Event(() => RegisterOrderEvent, cc => cc.CorrelateBy(state => state.SagaOrderId.ToString(), context => context.Message.OrderId.ToString()).SelectId(context => Guid.NewGuid()));
            Event(()=> OrderConformEvent, cc => cc.CorrelateById(context => context.Message.CorrelationId));

            // Define what triggers a workflow
            Initially(
                When(RegisterOrderEvent)
                    .Then(context =>
                    {
                        // context.Instance -> Saga-State-Object(ie. OrderState)
                        // context.Data -> Message-object(ie. RegisterOrderCommand)
                        context.Instance.SagaOrderId = context.Data.OrderId;
                        context.Instance.SagaProductName = context.Data.ProductName;
                        context.Instance.SagaOrderRecieveTime = DateTime.Now;
                    })
                    .ThenAsync(async context =>
                    {
                        await Console.Out.WriteLineAsync($"Saga >> Order Id {context.Data.OrderId} RECIEVED with correlation {context.Data.CorrelationId}");
                    })
                    .TransitionTo(OrderRecieved)
                    .Publish(context =>
                    {
                        var ore = new OrderRecievedEvent(context.Instance.CorrelationId);
                        ore.OrderId = context.Data.OrderId;
                        ore.ProductName = context.Data.ProductName;
                        return ore;
                    })
                );

            During(OrderRecieved,
                When(OrderConformEvent)
                    .Then(context => context.Instance.SagaOrderConformTime = DateTime.Now)
                    .ThenAsync(context => Console.Out.WriteLineAsync($"Saga >> Order Id {context.Data.OrderId} CONFORMED with correlation {context.Data.CorrelationId}"))
                    .TransitionTo(OrderConformed)
                    .Finalize()
                );

            // Signals that the state machine instance should be deleted if it is finalized. 
            // This is used to tell Entity Framework to delete the row from the database.
            SetCompletedWhenFinalized();

        }



    }

    
}
