using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.MessageMutator;

namespace NSBEndpoint
{
    public class MutateIncomingMessages :
        IMutateIncomingMessages
    {
        public Task MutateIncoming(MutateIncomingMessageContext context)
        {
            // the incoming headers
            var headers = context.Headers;

            // the incoming message
            // optionally replace the message instance by setting context.Message
            var message = context.Message;

            return Task.FromResult(0);
        }
    }
}
