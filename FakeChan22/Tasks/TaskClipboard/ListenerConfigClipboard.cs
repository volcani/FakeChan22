using System;
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
            TaskTypeFullName = typeof(TaskClipboard).FullName;
        }
    }
}
