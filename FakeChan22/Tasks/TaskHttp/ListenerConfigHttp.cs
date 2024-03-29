﻿using System;
using System.Runtime.Serialization;

namespace FakeChan22.Tasks
{
    [DataContract]
    public class ListenerConfigHttp : ListenerConfigBase
    {
        [DataMember] public string Host { get; private set; }

        [DataMember] public int Port { get; private set; }

        public ListenerConfigHttp(string host, int port)
        {
            Host = host;
            Port = port;
            LabelName = string.Format("Http{0}:{1}", Host, Port);
            ServiceName = string.Format("Http{0}", Port);
            TaskTypeFullName = typeof(TaskHttp).FullName;
        }
    }
}
