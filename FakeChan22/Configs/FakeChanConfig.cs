using FakeChan22.Configs;
using FakeChan22.Filters;
using FakeChan22.Params;
using FakeChan22.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace FakeChan22.Config
{

    [DataContract]
    public class FakeChanConfig
    {
        [DataMember] public List<SpeakerFakeChanList> speakerLists = new List<SpeakerFakeChanList>();

        [DataMember] public List<ReplaceDefinitionList> replaceDefinitionLists = new List<ReplaceDefinitionList>();

        [DataMember] public SoloSpeechDefinitionList soloSpeechList = new SoloSpeechDefinitionList();

        [DataMember] public List<ListenerConfigBase> listenerConfigLists = new List<ListenerConfigBase>();

        [DataMember] public MessageQueueParam queueParam = new MessageQueueParam();

        [DataMember] public string fakeChan22WindowTitle = "偽装ちゃん22";

        [DataMember] public string versionStr = "0.0.0";

        [DataMember] public string commentXmGenlPath = @".\";

        public FakeChanConfig()
        {
            //
        }

        public void RebuildListenerConfig(FakeChanTypesCollector typeCollector)
        {
            // 話者リストが1つ以上作成されているなら継続
            if ((speakerLists == null) || (speakerLists.Count == 0)) throw new Exception("[inner] 話者リストの作成が無い");

            // 置換リストが1つ以上作成されているなら継続
            if ((replaceDefinitionLists == null) || (replaceDefinitionLists.Count == 0)) throw new Exception("[inner] 置換リストの作成が無い");

            // リスナ定義再構成
            if (listenerConfigLists == null) listenerConfigLists = new List<ListenerConfigBase>();
            if (listenerConfigLists.Count == 0)
            {
                listenerConfigLists.Add(new ListenerConfigIpc("BouyomiChan") { SpeakerListDefault = speakerLists[0], ReplaceListDefault = replaceDefinitionLists[0], SpeakerListNoJapaneseJudge = speakerLists[0], ReplaceListNoJapaneseJudge = replaceDefinitionLists[0] });
                listenerConfigLists.Add(new ListenerConfigSocket("127.0.0.1", 50001) { SpeakerListDefault = speakerLists[0], ReplaceListDefault = replaceDefinitionLists[0], SpeakerListNoJapaneseJudge = speakerLists[0], ReplaceListNoJapaneseJudge = replaceDefinitionLists[0] });
                listenerConfigLists.Add(new ListenerConfigSocket("127.0.0.1", 50002) { SpeakerListDefault = speakerLists[0], ReplaceListDefault = replaceDefinitionLists[0], SpeakerListNoJapaneseJudge = speakerLists[0], ReplaceListNoJapaneseJudge = replaceDefinitionLists[0] });
                //listenerConfigLists.Add(new ListenerConfigSocket("127.0.0.1", 50003) { SpeakerListDefault = speakerLists[0], ReplaceListDefault = replaceDefinitionLists[0], SpeakerListNoJapaneseJudge = speakerLists[0], ReplaceListNoJapaneseJudge = replaceDefinitionLists[0] });
                //listenerConfigLists.Add(new ListenerConfigSocket("127.0.0.1", 50004) { SpeakerListDefault = speakerLists[0], ReplaceListDefault = replaceDefinitionLists[0], SpeakerListNoJapaneseJudge = speakerLists[0], ReplaceListNoJapaneseJudge = replaceDefinitionLists[0] });

                listenerConfigLists.Add(new ListenerConfigHttp("127.0.0.1", 50080) { SpeakerListDefault = speakerLists[0], ReplaceListDefault = replaceDefinitionLists[0], SpeakerListNoJapaneseJudge = speakerLists[0], ReplaceListNoJapaneseJudge = replaceDefinitionLists[0] });
                listenerConfigLists.Add(new ListenerConfigHttp("127.0.0.1", 50081) { SpeakerListDefault = speakerLists[0], ReplaceListDefault = replaceDefinitionLists[0], SpeakerListNoJapaneseJudge = speakerLists[0], ReplaceListNoJapaneseJudge = replaceDefinitionLists[0] });
                //listenerConfigLists.Add(new ListenerConfigHttp("127.0.0.1", 50082) { SpeakerListDefault = speakerLists[0], ReplaceListDefault = replaceDefinitionLists[0], SpeakerListNoJapaneseJudge = speakerLists[0], ReplaceListNoJapaneseJudge = replaceDefinitionLists[0] });
                //listenerConfigLists.Add(new ListenerConfigHttp("127.0.0.1", 50083) { SpeakerListDefault = speakerLists[0], ReplaceListDefault = replaceDefinitionLists[0], SpeakerListNoJapaneseJudge = speakerLists[0], ReplaceListNoJapaneseJudge = replaceDefinitionLists[0] });

                listenerConfigLists.Add(new ListenerConfigClipboard() { SpeakerListDefault = speakerLists[0], ReplaceListDefault = replaceDefinitionLists[0], SpeakerListNoJapaneseJudge = speakerLists[0], ReplaceListNoJapaneseJudge = replaceDefinitionLists[0] });
            }
            else
            {
                // リスナ定義から除外対象を除外する処理
                var obj1 = listenerConfigLists.Find(v => v.GetType() == typeof(ListenerConfigSocket) && v.LabelName == "Socket127.0.0.1:50003");
                var obj2 = listenerConfigLists.Find(v => v.GetType() == typeof(ListenerConfigSocket) && v.LabelName == "Socket127.0.0.1:50004");
                var obj3 = listenerConfigLists.Find(v => v.GetType() == typeof(ListenerConfigHttp) && v.LabelName == "Http127.0.0.1:50082");
                var obj4 = listenerConfigLists.Find(v => v.GetType() == typeof(ListenerConfigHttp) && v.LabelName == "Http127.0.0.1:50083");

                var obj5 = listenerConfigLists.Find(v => v.GetType() == typeof(ListenerConfigClipboard));

                if (obj1 != null) listenerConfigLists.Remove(obj1);
                if (obj2 != null) listenerConfigLists.Remove(obj2);
                if (obj3 != null) listenerConfigLists.Remove(obj3);
                if (obj4 != null) listenerConfigLists.Remove(obj4);

                // リスナ定義に含まれていないリスナを追加する処理
                if (obj5 == null) listenerConfigLists.Add(new ListenerConfigClipboard() { SpeakerListDefault = speakerLists[0], ReplaceListDefault = replaceDefinitionLists[0], SpeakerListNoJapaneseJudge = speakerLists[0], ReplaceListNoJapaneseJudge = replaceDefinitionLists[0] });
            }

            // Extend フォルダの処理
            foreach (var item in typeCollector.ListenerConfigTypeDictionary)
            {
                if (listenerConfigLists.Find(v => v.GetType() == item.Value) == null)
                {
                    object lsnrObjx = Activator.CreateInstance(item.Value);

                    (lsnrObjx as ListenerConfigBase).SpeakerListDefault = speakerLists[0];
                    (lsnrObjx as ListenerConfigBase).ReplaceListDefault = replaceDefinitionLists[0];
                    (lsnrObjx as ListenerConfigBase).SpeakerListNoJapaneseJudge = speakerLists[0];
                    (lsnrObjx as ListenerConfigBase).ReplaceListNoJapaneseJudge = replaceDefinitionLists[0];

                    listenerConfigLists.Add(lsnrObjx as ListenerConfigBase);
                }
            }

        }

        public void RebuildReplaceDefinitionList(FakeChanTypesCollector typeCollector)
        {
            // 置換リスト再構成
            if (replaceDefinitionLists == null) replaceDefinitionLists = new List<ReplaceDefinitionList>();
            if (replaceDefinitionLists.Count == 0) replaceDefinitionLists.Add(new ReplaceDefinitionList());

            // 旧置換リストなら補正をかける（新形式への変換処理）
            foreach (var item in replaceDefinitionLists)
            {
                if (item.FilterProcs == null) item.FilterProcs = new List<Filters.FilterProcBase>();

                if (item.FilterProcs.Count == 0)
                {
                    var filterConfigSplitUser = new Filters.FilterConfigSplitUser();
                    var filterProcSplitUser = new Filters.FilterProcSplitUser(ref filterConfigSplitUser);
                    filterConfigSplitUser.IsUse = true;
                    filterConfigSplitUser.ApplyToSpeaker = true;
                    item.FilterProcs.Add(filterProcSplitUser);

                    var filterConfigCleanupURL = new Filters.FilterConfigCleanupURL();
                    var filterProcCleanupURL = new Filters.FilterProcCleanupURL(ref filterConfigCleanupURL);
                    filterConfigCleanupURL.IsUse = item.IsReplaceUrl;
                    item.FilterProcs.Add(filterProcCleanupURL);

                    var filterConfigGrassWord = new Filters.FilterConfigGrassWord();
                    var filterProcGrassWord = new Filters.FilterProcGrassWord(ref filterConfigGrassWord);
                    filterConfigGrassWord.IsUse = item.IsReplaceGrassWord;
                    item.FilterProcs.Add(filterProcGrassWord);

                    var filterConfigApplauseWord = new Filters.FilterConfigApplauseWord();
                    var filterProcApplauseWord = new Filters.FilterProcApplauseWord(ref filterConfigApplauseWord);
                    filterConfigApplauseWord.IsUse = item.IsReplaceApplauseWord;
                    item.FilterProcs.Add(filterProcApplauseWord);

                    var filterConfigEmojiReplace = new Filters.FilterConfigEmojiReplace();
                    var filterProcEmojiReplace = new Filters.FilterProcEmojiReplace(ref filterConfigEmojiReplace);
                    filterConfigEmojiReplace.IsUse = item.IsReplaceEmoji;
                    item.FilterProcs.Add(filterProcEmojiReplace);

                    var filterConfigEmojiCleaner = new Filters.FilterConfigEmojiCleaner();
                    var filterProcEmojiCleaner = new Filters.FilterProcEmojiCleaner(ref filterConfigEmojiCleaner);
                    filterConfigEmojiCleaner.IsUse = (item.IsRemovalEmojiBeforeReplace || item.IsRemovalEmojiAfterReplace);
                    item.FilterProcs.Add(filterProcEmojiCleaner);

                    var filterConfigZen2HanNotNum = new Filters.FilterConfigZen2HanChar();
                    var filterProcZen2HanNotNum = new Filters.FilterProcZen2HanChar(ref filterConfigZen2HanNotNum);
                    filterConfigZen2HanNotNum.IsUse = item.IsReplaceZentoHan1;
                    item.FilterProcs.Add(filterProcZen2HanNotNum);

                    var filterConfigZen2HanNum = new Filters.FilterConfigZen2HanNum();
                    var filterProcZen2HanNum = new Filters.FilterProcZen2HanNum(ref filterConfigZen2HanNum);
                    filterConfigZen2HanNum.IsUse = item.IsReplaceZentoHan2;
                    item.FilterProcs.Add(filterProcZen2HanNum);

                    var filterConfigReplaceText = new Filters.FilterConfigReplaceText();
                    var filterProcReplaceText = new Filters.FilterProcReplaceText(ref filterConfigReplaceText);
                    filterConfigReplaceText.IsUse = true;
                    item.FilterProcs.Add(filterProcReplaceText);
                    filterConfigReplaceText.Definitions = new List<ReplaceDefinition>(item.Definitions.ToList());
                    //item.Definitions.Clear();

                    var filterConfigCutString = new Filters.FilterConfigCutString();
                    var filterProcCutString = new Filters.FilterProcCutString(ref filterConfigCutString);
                    filterConfigCutString.IsUse = true;
                    item.FilterProcs.Add(filterProcCutString);

                    item.AppendStr = "";
                    item.CutLength = 1;
                    item.Definitions = null;
                    item.IsRemovalEmojiAfterReplace = false;
                    item.IsRemovalEmojiBeforeReplace = false;
                    item.IsReplaceApplauseWord = false;
                    item.IsReplaceEmoji = false;
                    item.IsReplaceGrassWord = false;
                    item.IsReplaceUrl = false;
                    item.IsReplaceZentoHan1 = false;
                    item.IsReplaceZentoHan2 = false;
                    item.ReplaceStrFromUrl = "";
                }
            }

            // Extend フォルダの処理
            foreach(var procItem in typeCollector.FilterProcTypeDictionary)
            {
                foreach(var repItem in replaceDefinitionLists)
                {
                    if (repItem.FilterProcs.Find(v => v.GetType() == procItem.Value) == null)
                    {
                        string cKey = Regex.Replace(procItem.Key, @"^FakeChan22\.Filters\.FilterProc", @"FakeChan22.Filters.FilterConfig");
                        object confObjx = Activator.CreateInstance(typeCollector.FilterConfigTypeDictionary[cKey]);
                        object procObjx = Activator.CreateInstance(procItem.Value, new object[] { confObjx });

                        repItem.FilterProcs.Add(procObjx as FilterProcBase);
                    }
                }
            }

        }

        public void RebuildSoloSpeechList()
        {
            // 呟き定義再構成
            if (soloSpeechList == null) soloSpeechList = new SoloSpeechDefinitionList();
            if (soloSpeechList.SpeechDefinitions == null) soloSpeechList.SpeechDefinitions = new Dictionary<int, SoloSpeechDefinition>();
        }

        public void RebuildQueueParam()
        {
            // キュー制御再構成
            if (queueParam == null) queueParam = new MessageQueueParam();

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
            foreach (var item in soloSpeechList.SpeechDefinitions)
            {
                guid = soloSpeechList.SpeechDefinitions[item.Key].speakerList.UniqId;
                name = soloSpeechList.SpeechDefinitions[item.Key].speakerList.Listname;
                soloSpeechList.SpeechDefinitions[item.Key].speakerList = guid != null ? RebuildSpeakerListGuid(guid) : RebuildSpeakerListListname(name);
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

        private SpeakerFakeChanList RebuildSpeakerListGuid(string guid)
        {
            return speakerLists.Find(v => v.UniqId == guid);
        }
        private SpeakerFakeChanList RebuildSpeakerListListname(string name)
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
