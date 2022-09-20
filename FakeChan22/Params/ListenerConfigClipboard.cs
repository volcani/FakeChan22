using FakeChan22.Tasks;
using System.Runtime.Serialization;

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
