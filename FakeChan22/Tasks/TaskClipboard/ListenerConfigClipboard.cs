using System.Runtime.Serialization;

namespace FakeChan22.Tasks
{
    [DataContract]
    public class ListenerConfigClipboard : ListenerConfigBase
    {
        public ListenerConfigClipboard()
        {
            LabelName = "ClipBoard";
            ServiceName = "ClipBoard";
            LsnrType = ListenerType.clipboard;
        }
    }
}
