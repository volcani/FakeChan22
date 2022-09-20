using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FakeChan22.Params
{
    [DataContract]
    public class SoloSpeachDefinition
    {
        [DataMember] public string Title { get; private set; }

        [DataMember] public bool IsUse { get; set; }

        [DataMember] public int PastTime { get; set; }

        [DataMember] public SpeakerList SpeechSpeakerList { get; set; }

        [DataMember] public List<string> Messages { get; set; }

        [DataMember] public List<SoloSpeachDefinition> Childs { get; set; }

        public SoloSpeachDefinition(string title, bool isuse)
        {
            IsUse = isuse;
            Title = title;
            Childs = new List<SoloSpeachDefinition>();
        }

        public SoloSpeachDefinition(string title)
        {
            IsUse = true;
            Title = title;
            Childs = new List<SoloSpeachDefinition>();
        }
    }
}
