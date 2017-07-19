using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using SimpleMessages;

namespace SimpleReciever
{
    public class CommandChangeValueHandler: IConsumer<CommandChangeValue>
    {
        public async Task Consume(ConsumeContext<CommandChangeValue> context)
        {
            if (context.Message.Value.Contains("fail"))
                throw new Exception("Very bad thing happend when you sand FAIL!");

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

    public class CommandChangeValueFaultHandler : IConsumer<Fault<CommandChangeValue>>
    {
        public async Task Consume(ConsumeContext<Fault<CommandChangeValue>> context)
        {
            await Console.Out.WriteLineAsync("\nX X X X X X X X X X X X X X X X X X X X ");
            await Console.Out.WriteLineAsync($"[Customer] Message: {context.Message.Message.Value}");
            await Console.Out.WriteLineAsync($"[Customer] Exception: {context.Message.Exceptions?.FirstOrDefault()?.Message}");
            
        }

    }


}
