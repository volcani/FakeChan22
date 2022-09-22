using FakeChan22.Tasks;
using System;
using System.Runtime.Serialization;

namespace FakeChan22
{
    [DataContract]
    public class ListenerConfigIpc : ListenerConfig
    {
        [DataMember] public string ChannelName { get; private set; }

        [DataMember] public string ObjURI { get; private set; }

        public ListenerConfigIpc(string channelName)
        {
            ChannelName = channelName;
            ObjURI = "Remoting";
            LabelName = String.Format(@"{0}/{1}", channelName, ObjURI);
            ServiceName = "IPC";
            LsnrType = ListenerType.ipc;
        }
    }
}
