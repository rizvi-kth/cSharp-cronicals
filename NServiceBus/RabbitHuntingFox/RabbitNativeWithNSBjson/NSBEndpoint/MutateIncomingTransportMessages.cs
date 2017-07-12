using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.MessageMutator;

namespace NSBEndpoint
{
    public class MutateIncomingTransportMessages :
        IMutateIncomingTransportMessages
    {
        public Task MutateIncoming(MutateIncomingTransportMessageContext context)
        {
            // the bytes containing the incoming messages.
            var bytes = context.Body;

            // optionally replace the Body
            //context.Body = ServiceThatChangesBody.Mutate(context.Body);

            // the incoming headers
            var headers = context.Headers;
            foreach (var pair in headers)
            {
                Console.WriteLine($"{pair.Key} : {pair.Value}");
            }
            
            return Task.FromResult(0);
        }
    }
}
