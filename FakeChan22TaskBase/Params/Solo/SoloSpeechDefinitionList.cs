using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FakeChan22.Params
{
    [DataContract]
    public class SoloSpeechDefinitionList
    {
        [DataMember] public bool IsUse { get; set; }

        [DataMember] public Dictionary<int, SoloSpeechDefinition> SpeechDefinitions;

        public SoloSpeechDefinitionList()
        {
            IsUse = false;
            SpeechDefinitions = new Dictionary<int, SoloSpeechDefinition>();
        }

    }
}
