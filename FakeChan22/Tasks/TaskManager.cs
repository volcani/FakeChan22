using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace FakeChan22.Tasks
{
    public class TaskManager
    {
        Dictionary<ListenerConfig, TaskBase> tasks;
        MessageQueueWrapper messQue;
        FakeChanConfig config;
        ScAPIs api = new ScAPIs();
        Random r = new Random();

        ListenerConfigClipboard listenerConfigClipboard;
        TaskClipboard taskClipboard;

        SubTaskCommentGen taskCommentGen;

        TaskSyncTalk talkTask;

        public TaskManager(ref List<ListenerConfig> list, ref MessageQueueWrapper que, ref FakeChanConfig cfg)
        {
            tasks = new Dictionary<ListenerConfig, TaskBase>();
            messQue = que;
            config = cfg;

            foreach (var item in list)
            {
                switch (item.LsnrType)
                {
                    case ListenerType.ipc:
                        var lsnrIpc = item as ListenerConfigIpc;
                        tasks.Add(item, new TaskIpc(ref lsnrIpc, ref que));
                        break;

                    case ListenerType.socket:
                        var lsnrSocket = item as ListenerConfigSocket;
                        tasks.Add(item, new TaskSocket(ref lsnrSocket, ref que));
                        break;

                    case ListenerType.http:
                        var lsnrHttp = item as ListenerConfigHttp;
                        tasks.Add(item, new TaskHttp(ref lsnrHttp, ref que));
                        break;

                    case ListenerType.clipboard:
                        listenerConfigClipboard = item as ListenerConfigClipboard;
                        taskClipboard = new TaskClipboard(ref listenerConfigClipboard, ref que);
                        tasks.Add(item, taskClipboard);
                        break;
                }

                tasks[item].OnCallAsyncTalk += AsyncTalk;
            }

            // 常時稼働のタスク
            talkTask = new TaskSyncTalk(ref messQue, ref config);
        }

        public void TaskShutdown()
        {
            foreach (var item in tasks)
            {
                item.Value.TaskStop();
            }
        }

        public void TaskBoot()
        {
            foreach (var item in tasks)
            {
                if(item.Key.IsEnable) item.Value.TaskStart();
            }
        }

        public void TaskReBoot(ref ListenerConfig lsnr)
        {
            if(tasks.ContainsKey(lsnr))
            {
                tasks[lsnr].TaskStop();

                if (lsnr.IsEnable) tasks[lsnr].TaskStart();
            }
        }

        public TaskClipboard ClipboardTask
        {
            get
            {
                return taskClipboard;
            }
        }

        public void AsyncTalk(MessageData talk)
        {
            Task.Run(() =>
            {
                SpeakerList splist = null;
                ReplaceDefinitionList replist = null;
                string sepText = "";
                string sepMap = "";
                string parsedText = "";
                int cid = 0;
                string cname = "";
                int splistIndex = -1;

                // 話者のキーとテキストを分離
                (sepMap, sepText) = ReplaceText.SeparateMapKey(talk.OrgMessage);

                // 非日本語判定の実施判定
                splist = talk.LsnrCfg.SpeakerListDefault;
                replist = talk.LsnrCfg.ReplaceListDefault;

                // 非日本語判定の実施判定
                if (talk.LsnrCfg.IsNoJapanese)
                {
                    (bool judge, double rate) = JudgeTextLang.JudgeNoJapanese(talk.OrgMessage, talk.LsnrCfg.NoJapaneseCharRate);
                    if (judge)
                    {
                        splist = talk.LsnrCfg.SpeakerListNoJapaneseJudge;
                        replist = talk.LsnrCfg.ReplaceListNoJapaneseJudge;
                    }
                }

                // 置換処理
                parsedText = ReplaceText.ParseText(sepText, replist);
                talk.Message = parsedText;

                // 話者選定
                if ((sepMap != "") && (splist.SpeakerMaps.ContainsKey(sepMap)))
                {
                    // テキスト先頭に話者の識別子が定義されている場合はその話者とする
                    cid = splist.SpeakerMaps[sepMap].Cid;
                    cname = splist.SpeakerMaps[sepMap].Name;
                }
                else
                {
                    if (talk.LsnrCfg.IsRandom)
                    {
                        // ランダム指定があれば話者リストの範囲内でランダムに話者を選択する
                        splistIndex = r.Next(0, splist.ValidSpeakers.Count);
                        cid = splist.ValidSpeakers[splistIndex].Cid;
                        cname = splist.ValidSpeakers[splistIndex].Name;
                    }
                    else
                    {
                        if ((talk.CompatVType != -1) && (talk.CompatVType < splist.ValidSpeakers.Count))
                        {
                            // 声質指定がされている場合は話者リスト1番目～9番目に割り当てる
                            splistIndex = talk.CompatVType;
                        }
                        else
                        {
                            // 話者リストの範囲を超えていたら話者リスト1番目に割り当てる
                            splistIndex = 0;
                        }
                        cid = splist.ValidSpeakers[splistIndex].Cid;
                        cname = splist.ValidSpeakers[splistIndex].Name;
                    }
                }

                // 発声
                var eff = splist.ValidSpeakers[splistIndex].Effects.ToDictionary(k => k.ParamName, v => v.Value);
                var emo = splist.ValidSpeakers[splistIndex].Emotions.ToDictionary(k => k.ParamName, v => v.Value);
                try
                {
                    api.TalkAsync(cid, talk.Message, eff, emo);
                }
                catch (Exception)
                {
                    //
                }
            });

        }
    }
}
