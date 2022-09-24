using FakeChan22.Params;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FakeChan22
{
    [DataContract]
    public class FakeChanConfig
    {
        [DataMember] public List<SpeakerList> speakerLists = new List<SpeakerList>();

        [DataMember] public List<ReplaceDefinitionList> replaceDefinitionLists = new List<ReplaceDefinitionList>();

        [DataMember] public SoloSpeechDefinitionList SoloSpeechList = new SoloSpeechDefinitionList();

        [DataMember] public QueueParam queueParam = new QueueParam();

        [DataMember] public List<ListenerConfig> listenerConfigLists = new List<ListenerConfig>();

        [DataMember] public string fakeChan22WindowTitle = "偽装ちゃん22";

        [DataMember] public string versionStr = "0.0.0";

        [DataMember] public string commentXmGenlPath = @".\";

        public FakeChanConfig()
        {
            //
        }

        public void RebuildObjects()
        {
            string name = "";

            // リスナ定義のオブジェクト再構成
            for(int idx =0; idx < listenerConfigLists.Count; idx++)
            {
                name = listenerConfigLists[idx].SpeakerListDefault.Listname;
                listenerConfigLists[idx].SpeakerListDefault = RebuildSpeakerList(name);

                name = listenerConfigLists[idx].ReplaceListDefault.Listname;
                listenerConfigLists[idx].ReplaceListDefault = RebuildReplaceDefinitionList(name);

                name = listenerConfigLists[idx].SpeakerListNoJapaneseJudge.Listname;
                listenerConfigLists[idx].SpeakerListNoJapaneseJudge = RebuildSpeakerList(name);

                name = listenerConfigLists[idx].ReplaceListNoJapaneseJudge.Listname;
                listenerConfigLists[idx].ReplaceListNoJapaneseJudge = RebuildReplaceDefinitionList(name);
            }

            // 呟き定義のオブジェクト再構成
            foreach(var item in SoloSpeechList.SpeechDefinitions)
            {
                name = SoloSpeechList.SpeechDefinitions[item.Key].speakerList.Listname;
                SoloSpeechList.SpeechDefinitions[item.Key].speakerList = RebuildSpeakerList(name);
            }
        }

        private SpeakerList RebuildSpeakerList(string name)
        {
            return speakerLists.Find(v => v.Listname == name);
        }

        private ReplaceDefinitionList RebuildReplaceDefinitionList(string name)
        {
            return replaceDefinitionLists.Find(v => v.Listname == name);
        }
    }
}
