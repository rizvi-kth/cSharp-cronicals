using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using NServiceBus.Transport.Msmq;


namespace WebAPI.Empty.ServiceBus.OWIN.BusMiddleware
{
    static class MsmqHeaderSerializer
    {
        static XmlSerializer serializer = new XmlSerializer(typeof(List<HeaderInfo>));
        public static byte[] CreateHeaders(string messageType)
        {
            var headerInfos = new List<HeaderInfo>
            {
                new HeaderInfo
                {
                    Key = "NServiceBus.EnclosedMessageTypes",
                    Value = messageType
                }
            };
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, headerInfos);
                return stream.ToArray();
            }
        }
    }

}