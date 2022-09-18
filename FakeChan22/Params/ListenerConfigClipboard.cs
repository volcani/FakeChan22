using FakeChan22.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FakeChan22
{
    [DataContract]
    public class ListenerConfigClipboard : ListenerConfig
    {
        public ListenerConfigClipboard()
        {
            LabelName = "ClipBoard";
            ServiceName = "ClipBoard";
            LsnrType = ListenerType.clipboard;
        }
    }
}
