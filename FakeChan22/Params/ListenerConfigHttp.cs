using FakeChan22.Tasks;
using System.Runtime.Serialization;

namespace FakeChan22
{
    [DataContract]
    public class ListenerConfigHttp : ListenerConfig
    {
        public string Host { get; private set; }

        public int Port { get; private set; }

        public ListenerConfigHttp(string host, int port)
        {
            Host = host;
            Port = port;
            LabelName = string.Format("Http{0}:{1}", Host, Port);
            ServiceName = string.Format("Http{0}", Port);
            LsnrType = ListenerType.http;
        }
    }
}
