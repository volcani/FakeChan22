using FakeChan22.Params;
using System;
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

        [DataMember] public List<ListenerConfig> listenerConfigLists = new List<ListenerConfig>();

        [DataMember] public QueueParam queueParam = new QueueParam();

        [DataMember] public string fakeChan22WindowTitle = "偽装ちゃん22";

        [DataMember] public string versionStr = "0.0.0";

        [DataMember] public string commentXmGenlPath = @".\";

        public FakeChanConfig()
        {
            //
        }

        public void RebuildObjects()
        {
            string guid = "";
            string name = "";

            // リスナ定義のオブジェクト再構成（旧保存形式にも対応）

            for (int idx =0; idx < listenerConfigLists.Count; idx++)
            {
                guid = listenerConfigLists[idx].SpeakerListDefault.UniqId;
                name = listenerConfigLists[idx].SpeakerListDefault.Listname;
                listenerConfigLists[idx].SpeakerListDefault = guid != null ? RebuildSpeakerList(guid) : RebuildSpeakerListListname(name);

                guid = listenerConfigLists[idx].ReplaceListDefault.UniqId;
                name = listenerConfigLists[idx].ReplaceListDefault.Listname;
                listenerConfigLists[idx].ReplaceListDefault = guid != null ? RebuildReplaceDefinitionList(guid) : RebuildReplaceDefinitionListListname(name);

                guid = listenerConfigLists[idx].SpeakerListNoJapaneseJudge.UniqId;
                name = listenerConfigLists[idx].SpeakerListNoJapaneseJudge.Listname;
                listenerConfigLists[idx].SpeakerListNoJapaneseJudge = guid != null ? RebuildSpeakerList(guid) : RebuildSpeakerListListname(name);

                guid = listenerConfigLists[idx].ReplaceListNoJapaneseJudge.UniqId;
                name = listenerConfigLists[idx].ReplaceListNoJapaneseJudge.Listname;
                listenerConfigLists[idx].ReplaceListNoJapaneseJudge = guid != null ? RebuildReplaceDefinitionList(guid) : RebuildReplaceDefinitionListListname(name);
            }

            // 呟き定義のオブジェクト再構成（旧保存形式にも対応）

            foreach (var item in SoloSpeechList.SpeechDefinitions)
            {
                guid = SoloSpeechList.SpeechDefinitions[item.Key].speakerList.UniqId;
                name = SoloSpeechList.SpeechDefinitions[item.Key].speakerList.Listname;
                SoloSpeechList.SpeechDefinitions[item.Key].speakerList = guid != null ? RebuildSpeakerList(guid) : RebuildSpeakerListListname(name);
            }

            // 残りにguidを付与

            foreach (var item in speakerLists)
            {
                if (item.UniqId == null) item.UniqId = Guid.NewGuid().ToString();
            }

            foreach (var item in speakerLists)
            {
                if (item.UniqId == null) item.UniqId = Guid.NewGuid().ToString();
            }

        }

        private SpeakerList RebuildSpeakerList(string guid)
        {
            return speakerLists.Find(v => v.UniqId == guid);
        }
        private SpeakerList RebuildSpeakerListListname(string name)
        {
            var item = speakerLists.Find(v => v.Listname == name);

            if ((item != null) && (item.UniqId == null))
            {
                item.UniqId = Guid.NewGuid().ToString();
            }

            return item;
        }

        private ReplaceDefinitionList RebuildReplaceDefinitionList(string guid)
        {
            return replaceDefinitionLists.Find(v => v.UniqId == guid);
        }
        private ReplaceDefinitionList RebuildReplaceDefinitionListListname(string name)
        {
            var item = replaceDefinitionLists.Find(v => v.Listname == name);

            if ((item != null) && (item.UniqId == null))
            {
                item.UniqId = Guid.NewGuid().ToString();
            }

            return item;
        }
    }
}
