using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using SimpleMessages;

namespace SimpleReciever
{
    public class CommandChangeValueHandler:
        IConsumer<CommandChangeValue>
    {
        public async Task Consume(ConsumeContext<CommandChangeValue> context)
        {
            await Console.Out.WriteLineAsync("\n===========================");
            await Console.Out.WriteLineAsync($"[Customer] Message: {context.Message.Value}");
            var headers = context.Headers;
            await Console.Out.WriteLineAsync($"Headers count: {headers.GetAll().Count()} :");
            foreach (var keyValuePair in headers.GetAll())
            {
                await Console.Out.WriteLineAsync($"{keyValuePair.Key} : {keyValuePair.Value}");
            }
            await Console.Out.WriteLineAsync($"Context corelationID: {context.CorrelationId?.ToString()} :");
            await Console.Out.WriteLineAsync($"Message corelationID: <Not implemented> :");



        }
    }
}
