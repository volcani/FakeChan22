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

        public void RebuildListenerConfig()
        {
            // 話者リストが1つ以上作成されているなら継続
            if ((speakerLists == null) || (speakerLists.Count == 0)) throw new Exception("[inner] 話者リストの作成が無い");

            // 置換リストが1つ以上作成されているなら継続
            if ((replaceDefinitionLists == null) || (replaceDefinitionLists.Count == 0)) throw new Exception("[inner] 置換リストの作成が無い");

            // リスナ定義再構成
            if (listenerConfigLists == null) listenerConfigLists = new List<ListenerConfig>();
            if (listenerConfigLists.Count == 0)
            {
                listenerConfigLists.Add(new ListenerConfigIpc("BouyomiChan") { SpeakerListDefault = speakerLists[0], ReplaceListDefault = replaceDefinitionLists[0], SpeakerListNoJapaneseJudge = speakerLists[0], ReplaceListNoJapaneseJudge = replaceDefinitionLists[0] });
                listenerConfigLists.Add(new ListenerConfigSocket("127.0.0.1", 50001) { SpeakerListDefault = speakerLists[0], ReplaceListDefault = replaceDefinitionLists[0], SpeakerListNoJapaneseJudge = speakerLists[0], ReplaceListNoJapaneseJudge = replaceDefinitionLists[0] });
                listenerConfigLists.Add(new ListenerConfigSocket("127.0.0.1", 50002) { SpeakerListDefault = speakerLists[0], ReplaceListDefault = replaceDefinitionLists[0], SpeakerListNoJapaneseJudge = speakerLists[0], ReplaceListNoJapaneseJudge = replaceDefinitionLists[0] });
                listenerConfigLists.Add(new ListenerConfigSocket("127.0.0.1", 50003) { SpeakerListDefault = speakerLists[0], ReplaceListDefault = replaceDefinitionLists[0], SpeakerListNoJapaneseJudge = speakerLists[0], ReplaceListNoJapaneseJudge = replaceDefinitionLists[0] });
                listenerConfigLists.Add(new ListenerConfigSocket("127.0.0.1", 50004) { SpeakerListDefault = speakerLists[0], ReplaceListDefault = replaceDefinitionLists[0], SpeakerListNoJapaneseJudge = speakerLists[0], ReplaceListNoJapaneseJudge = replaceDefinitionLists[0] });
                listenerConfigLists.Add(new ListenerConfigHttp("127.0.0.1", 50080) { SpeakerListDefault = speakerLists[0], ReplaceListDefault = replaceDefinitionLists[0], SpeakerListNoJapaneseJudge = speakerLists[0], ReplaceListNoJapaneseJudge = replaceDefinitionLists[0] });
                listenerConfigLists.Add(new ListenerConfigHttp("127.0.0.1", 50081) { SpeakerListDefault = speakerLists[0], ReplaceListDefault = replaceDefinitionLists[0], SpeakerListNoJapaneseJudge = speakerLists[0], ReplaceListNoJapaneseJudge = replaceDefinitionLists[0] });
                listenerConfigLists.Add(new ListenerConfigHttp("127.0.0.1", 50082) { SpeakerListDefault = speakerLists[0], ReplaceListDefault = replaceDefinitionLists[0], SpeakerListNoJapaneseJudge = speakerLists[0], ReplaceListNoJapaneseJudge = replaceDefinitionLists[0] });
                listenerConfigLists.Add(new ListenerConfigHttp("127.0.0.1", 50083) { SpeakerListDefault = speakerLists[0], ReplaceListDefault = replaceDefinitionLists[0], SpeakerListNoJapaneseJudge = speakerLists[0], ReplaceListNoJapaneseJudge = replaceDefinitionLists[0] });
                listenerConfigLists.Add(new ListenerConfigClipboard() { SpeakerListDefault = speakerLists[0], ReplaceListDefault = replaceDefinitionLists[0], SpeakerListNoJapaneseJudge = speakerLists[0], ReplaceListNoJapaneseJudge = replaceDefinitionLists[0] });
                listenerConfigLists.Add(new ListenerConfigTwitter() { SpeakerListDefault = speakerLists[0], ReplaceListDefault = replaceDefinitionLists[0], SpeakerListNoJapaneseJudge = speakerLists[0], ReplaceListNoJapaneseJudge = replaceDefinitionLists[0] });
            }
            else
            {
                // 旧リスナ定義に含まれていないリスナを追加する処理

                if (listenerConfigLists.Find(v => v.LsnrType == Tasks.ListenerType.clipboard) == null)
                {
                    listenerConfigLists.Add(new ListenerConfigClipboard() { SpeakerListDefault = speakerLists[0], ReplaceListDefault = replaceDefinitionLists[0], SpeakerListNoJapaneseJudge = speakerLists[0], ReplaceListNoJapaneseJudge = replaceDefinitionLists[0] });
                }

                if (listenerConfigLists.Find(v => v.LsnrType == Tasks.ListenerType.twitter) == null)
                {
                    listenerConfigLists.Add(new ListenerConfigTwitter() { SpeakerListDefault = speakerLists[0], ReplaceListDefault = replaceDefinitionLists[0], SpeakerListNoJapaneseJudge = speakerLists[0], ReplaceListNoJapaneseJudge = replaceDefinitionLists[0] });
                }
            }
        }

        public void RebuildReplaceDefinitionList()
        {
            // 置換リスト再構成
            if (replaceDefinitionLists == null) replaceDefinitionLists = new List<ReplaceDefinitionList>();
            if (replaceDefinitionLists.Count == 0) replaceDefinitionLists.Add(new ReplaceDefinitionList());
        }

        public void RebuildSoloSpeechList()
        {
            // 呟き定義再構成
            if (SoloSpeechList == null) SoloSpeechList = new SoloSpeechDefinitionList();
            if (SoloSpeechList.SpeechDefinitions == null) SoloSpeechList.SpeechDefinitions = new Dictionary<int, SoloSpeechDefinition>();
        }

        public void RebuildQueueParam()
        {
            // キュー制御再構成
            if (queueParam == null) queueParam = new QueueParam();

            // キュー設定補正
            if (queueParam.Mode5QueueLimit < queueParam.Mode4QueueLimit)
            {
                queueParam.Mode5QueueLimit = queueParam.Mode4QueueLimit + 10;
            }
        }

        public void RebuildMappingObjects()
        {
            string guid;
            string name;

            // リスナ定義のオブジェクト再構成（旧保存形式にも対応）
            for (int idx = 0; idx < listenerConfigLists.Count; idx++)
            {
                guid = listenerConfigLists[idx].SpeakerListDefault.UniqId;
                name = listenerConfigLists[idx].SpeakerListDefault.Listname;
                listenerConfigLists[idx].SpeakerListDefault = guid != null ? RebuildSpeakerListGuid(guid) : RebuildSpeakerListListname(name);

                guid = listenerConfigLists[idx].ReplaceListDefault.UniqId;
                name = listenerConfigLists[idx].ReplaceListDefault.Listname;
                listenerConfigLists[idx].ReplaceListDefault = guid != null ? RebuildReplaceDefinitionListGuid(guid) : RebuildReplaceDefinitionListListname(name);

                guid = listenerConfigLists[idx].SpeakerListNoJapaneseJudge.UniqId;
                name = listenerConfigLists[idx].SpeakerListNoJapaneseJudge.Listname;
                listenerConfigLists[idx].SpeakerListNoJapaneseJudge = guid != null ? RebuildSpeakerListGuid(guid) : RebuildSpeakerListListname(name);

                guid = listenerConfigLists[idx].ReplaceListNoJapaneseJudge.UniqId;
                name = listenerConfigLists[idx].ReplaceListNoJapaneseJudge.Listname;
                listenerConfigLists[idx].ReplaceListNoJapaneseJudge = guid != null ? RebuildReplaceDefinitionListGuid(guid) : RebuildReplaceDefinitionListListname(name);
            }

            // 呟き定義のオブジェクト再構成（旧保存形式にも対応）
            foreach (var item in SoloSpeechList.SpeechDefinitions)
            {
                guid = SoloSpeechList.SpeechDefinitions[item.Key].speakerList.UniqId;
                name = SoloSpeechList.SpeechDefinitions[item.Key].speakerList.Listname;
                SoloSpeechList.SpeechDefinitions[item.Key].speakerList = guid != null ? RebuildSpeakerListGuid(guid) : RebuildSpeakerListListname(name);
            }

            // 残りにguidを付与

            foreach (var item in speakerLists)
            {
                if (item.UniqId == null) item.UniqId = Guid.NewGuid().ToString();
            }

            foreach (var item in replaceDefinitionLists)
            {
                if (item.UniqId == null) item.UniqId = Guid.NewGuid().ToString();
            }

        }

        private SpeakerList RebuildSpeakerListGuid(string guid)
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

        private ReplaceDefinitionList RebuildReplaceDefinitionListGuid(string guid)
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
