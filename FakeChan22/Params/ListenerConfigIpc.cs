using FakeChan22.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FakeChan22
{
    [DataContract]
    public class ListenerConfigIpc : ListenerConfig
    {
        [DataMember] public string ChannelName { get; private set; }

        [DataMember] public string Name { get; private set; }

        public ListenerConfigIpc(string channelName)
        {
            ChannelName = channelName;
            Name = "Remoting";
            LabelName = String.Format(@"{0}/{1}", channelName, Name);
            ServiceName = "IPC";
            LsnrType = ListenerType.ipc;
        }
    }
}
