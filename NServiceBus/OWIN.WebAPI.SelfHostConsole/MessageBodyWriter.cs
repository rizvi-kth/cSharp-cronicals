using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.Logging;
using NServiceBus.MessageMutator;

namespace OWIN.WebAPI.SelfHostConsole
{
    class MessageBodyWriter :
        IMutateIncomingTransportMessages
    {
        static ILog log = LogManager.GetLogger<MessageBodyWriter>();

        public Task MutateIncoming(MutateIncomingTransportMessageContext context)
        {
            var bodyAsString = Encoding.UTF8
                .GetString(context.Body);
            log.Info($"Incoming Serialized Message { bodyAsString } ");
            //log.Info(bodyAsString);
            return Task.FromResult(0);
        }
    }
}
