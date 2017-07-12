using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace NSBEndpoint
{
    public class MyHandler :
        IHandleMessages<MyMessage>
    {
        static ILog log = LogManager.GetLogger<MyHandler>();

        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            log.Info($"Got `MyMessage` with id: {context.MessageId}, property value: {message.SampleQuota}");
            return Task.FromResult(0);
        }
    }
}
