using FakeChan22.Tasks;
using System.Runtime.Serialization;

namespace FakeChan22
{

    [DataContract]
    public class ListenerConfigBase
    {
        [DataMember] public string LabelName { get; set; }

        [DataMember] public SpeakerFakeChanList SpeakerListDefault { get; set; }

        [DataMember] public int SpeakerListDefaultIndex { get; set; }

        [DataMember] public ReplaceDefinitionList ReplaceListDefault { get; set; }

        [DataMember] public int ReplaceListDefaultIndex { get; set; }

        [DataMember] public bool IsNoJapanese { get; set; }

        [DataMember] public double NoJapaneseCharRate { get; set; }

        [DataMember] public SpeakerFakeChanList SpeakerListNoJapaneseJudge { get; set; }

        [DataMember] public int SpeakerListNoJapaneseJudgeIndex { get; set; }

        [DataMember] public ReplaceDefinitionList ReplaceListNoJapaneseJudge { get; set; }

        [DataMember] public int ReplaceListNoJapaneseJudgeIndex { get; set; }

        [DataMember] public string ServiceName { get; set; }

        [DataMember] public bool IsAsync { get; set; }

        [DataMember] public bool IsRandom { get; set; }

        [DataMember] public bool IsEnable { get; set; }

        [DataMember] public ListenerType LsnrType { get; set; }

        public ListenerConfigBase()
        {
            SpeakerListDefault = null;
            ReplaceListDefault = null;
            SpeakerListNoJapaneseJudge = null;
            ReplaceListNoJapaneseJudge = null;
            IsNoJapanese = false;
            IsAsync = false;
            IsRandom = false;
            IsEnable = false;
            NoJapaneseCharRate = 75.0;
        }
    }
}
