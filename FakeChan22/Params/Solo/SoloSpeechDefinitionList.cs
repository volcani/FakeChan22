using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Animation;

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
