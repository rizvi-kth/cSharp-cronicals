using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using SimpleMessages;

namespace SimpleReciever
{
    public class OrderRecievedEventHandler : IConsumer<OrderRecievedEvent>
    {
        public async Task Consume(ConsumeContext<OrderRecievedEvent> context)
        {
            await Console.Out.WriteLineAsync("\n= = = = = = = = = = = = ");
            await Console.Out.WriteLineAsync($"[Handler] Order: {context.Message.OrderId}");
            var headers = context.Headers;
            await Console.Out.WriteLineAsync($"Headers count: {headers.GetAll().Count()} :");
            foreach (var keyValuePair in headers.GetAll())
            {
                await Console.Out.WriteLineAsync($"{keyValuePair.Key} : {keyValuePair.Value}");
            }
            await Console.Out.WriteLineAsync($"Context CorelationID: {context.CorrelationId?.ToString()} ");
            await Console.Out.WriteLineAsync($"Message corelationID: {context.Message.CorrelationId.ToString()} ");

            await Task.Delay(5000);

            await Console.Out.WriteLineAsync("\n>> >> >> >> >> >> >> >> >> >> >> >> ");
            await Console.Out.WriteLineAsync("Sending order cnformation ...");

            var conformation = new OrderRecievConformEvent(context.Message.CorrelationId);
            conformation.OrderId = context.Message.OrderId;
            conformation.ProductName = context.Message.ProductName;
            await context.Publish(conformation);
            
        }
    }
}
