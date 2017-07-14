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
            await Console.Out.WriteLineAsync($"Updating customer: {context.Message.Value}");

            // update the customer address
        }
    }
}
