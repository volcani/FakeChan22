﻿using System;
using System.Runtime.Serialization;

namespace FakeChan22.Tasks
{
    [DataContract]
    public class ListenerConfigIpc : ListenerConfig
    {
        [DataMember] public string ChannelName { get; private set; }

        [DataMember] public string ObjURI { get; private set; }

        public ListenerConfigIpc(string channelName, string serviceName = "IPC")
        {
            ChannelName = channelName;
            ObjURI = "Remoting";
            LabelName = String.Format(@"{0}/{1}", channelName, ObjURI);
            ServiceName = serviceName;
            LsnrType = ListenerType.ipc;
        }
    }
}