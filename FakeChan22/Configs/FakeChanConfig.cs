using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FakeChan22
{
    [DataContract]
    public class FakeChanConfig
    {
        [DataMember] public List<SpeakerList> speakerLists = new List<SpeakerList>();

        [DataMember] public List<ReplaceDefinitionList> replaceDefinitionLists = new List<ReplaceDefinitionList>();

        [DataMember] public QueueParam queueParam = new QueueParam();

        [DataMember] public List<ListenerConfig> listenerConfigLists = new List<ListenerConfig>();

        [DataMember] public string fakeChan22WindowTitle = "偽装ちゃん22";

        [DataMember] public string versionStr = "0.0.0";

        [DataMember] public string commentXmGenlPath = @".\";

        public FakeChanConfig()
        {
            //
        }
    }
}
