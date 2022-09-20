using FakeChan22.Tasks;
using System.Runtime.Serialization;

namespace FakeChan22
{
    [DataContract]
    public class ListenerConfigSocket : ListenerConfig
    {
        public string Host { get; private set; }

        public int Port { get; private set; }

        public ListenerConfigSocket(string host, int port)
        {
            Host = host;
            Port = port;

            LabelName = string.Format("Socket{0}:{1}", Host, Port);
            ServiceName = string.Format("Socket{0}", Port);
            LsnrType = ListenerType.socket;
        }

    }
}
