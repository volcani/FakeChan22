using System.Runtime.Serialization;

namespace FakeChan22.Tasks
{
    [DataContract]
    public class ListenerConfigSocket : ListenerConfigBase
    {
        [DataMember] public string Host { get; private set; }

        [DataMember] public int Port { get; private set; }

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
