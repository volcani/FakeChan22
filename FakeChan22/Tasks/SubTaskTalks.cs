using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Runtime.Remoting.Contexts;
using System.Text.RegularExpressions;

namespace FakeChan22.Tasks
{
    public class SubTaskTalks
    {
        MessageQueueWrapper messQueue;
        FakeChanConfig config;
        Random r = new Random();
        DispatcherTimer KickTalker;
        DispatcherTimer KickLogQue;
        SubTaskCommentGen CommentGen;
        ScAPIs api;
        Task backgroundTalker;
        BlockingCollection<string> logQue = new BlockingCollection<string>();

        public delegate void CallEventHandlerLogging(string logText);
        public event CallEventHandlerLogging OnLogging;

        public SubTaskCommentGen CommentGenSubTask
        {
            get
            {
                return CommentGen;
            }
        }

        public SubTaskTalks(ref MessageQueueWrapper que, ref FakeChanConfig cnfg)
        {
            messQueue = que;
            config = cnfg;

            api = new ScAPIs();

            KickTalker = new DispatcherTimer();
            KickTalker.Tick += new EventHandler(KickTalker_Tick);
            KickTalker.Interval = new TimeSpan(0, 0, 0, 1, 0);
            KickTalker.Start();

            KickLogQue = new DispatcherTimer();
            KickLogQue.Tick += new EventHandler(Logging);
            KickLogQue.Interval = new TimeSpan(0, 0, 0, 1, 0);
            KickLogQue.Start();

            CommentGen = new SubTaskCommentGen();
            CommentGen.TaskStart(config.commentXmGenlPath);
        }

        private void Logging(object sender, EventArgs e)
        {
            if (logQue.Count!=0)
            {
                try
                {
                    string txt = logQue.Take();

                    OnLogging?.Invoke(string.Format(@"ASCALL, {0}", txt));
                }
                catch(Exception)
                {
                    //
                }
            }
        }

        private void Log(string logText)
        {
            logQue.Add(logText);
        }

        public void AsyncTalk(MessageData talk)
        {
            int cid;
            string text;
            string cname;
            Dictionary<string, decimal> eff;
            Dictionary<string, decimal> emo;

            (cid, cname, text, eff, emo) = ParseSpeakerAndParams(talk);

            api.TalkAsync(cid, text, eff, emo);
        }

        private void KickTalker_Tick(object sender, EventArgs e)
        {
            if (messQueue.count == 0) return;

            if (backgroundTalker != null)
            {
                if (!backgroundTalker.IsCompleted) return;
            }


            backgroundTalker = Task.Run(() => {

                messQueue.IsSyncTaking = true;

                MessageData item;
                while ((item = messQueue.TakeQueue()) != null)
                {
                    int mode;
                    int cid;
                    string text;
                    string orgtext;
                    string cname;
                    string sname;
                    Dictionary<string, decimal> eff;
                    Dictionary<string, decimal> emo;

                    sname = item.LsnrCfg.ServiceName;
                    orgtext = Regex.Replace(item.OrgMessage, @"(\r\n|\r|\n)", "");
                    (cid, cname, text, eff, emo) = ParseSpeakerAndParams(item);
                    text = Regex.Replace(text, @"(\r\n|\r|\n)", "");

                    mode = config.queueParam.QueueMode(messQueue.count);
                    switch (mode)
                    {
                        case 1:
                            eff["speed"] = eff["speed"] * 1.3m;
                            break;

                        case 2:
                            eff["speed"] = eff["speed"] * 1.5m;
                            break;

                        case 3:
                            eff["speed"] = eff["speed"] * 1.7m;
                            text = text.Length > 24 ? text.Substring(0, 24) + "(以下略" : text;
                            break;

                        case 4:
                            eff["speed"] = eff["speed"] * 1.9m;
                            text = text.Length > 12 ? text.Substring(0, 12) + "(以下略" : text;
                            break;

                        case 5:
                            messQueue.ClearQueue();
                            eff["speed"] = eff["speed"] * 1.7m;
                            text = "キュークリアします";
                            break;

                        case 0:
                        default:
                            break;
                    }

                    item.Message = text;

                    try
                    {
                        CommentGen.AddComment(text, sname, "", string.Format(@"{0}:{1}", cid, cname));
                        api.Talk(cid, text, "", eff, emo);
                        Log(string.Format(@"{0}, {1}, mode{2}, [{3}]", sname, cid, mode, text));
                    }
                    catch (Exception)
                    {
                        //
                    }
                }

                messQueue.IsSyncTaking = false;
            });


        }

        private (int cid, string spkrName, string text, Dictionary<string,decimal> eff, Dictionary<string, decimal> emo) ParseSpeakerAndParams(MessageData talk)
        {
            int cid = 0;
            string sepText = "";
            string sepMap = "";
            string parsedText = "";
            string spkrname = "";
            int splistIndex = -1;
            SpeakerList splist = null;
            ReplaceDefinitionList replist = null;

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

            // 話者選定
            if ((sepMap != "") && (splist.SpeakerMaps.ContainsKey(sepMap)))
            {
                // テキスト先頭に話者の識別子が定義されている場合はその話者とする
                cid = splist.SpeakerMaps[sepMap].Cid;
                spkrname = splist.SpeakerMaps[sepMap].Name;
            }
            else
            {
                if (talk.LsnrCfg.IsRandom)
                {
                    // ランダム指定があれば話者リストの範囲内でランダムに話者を選択する
                    splistIndex = r.Next(0, splist.ValidSpeakers.Count);
                    cid = splist.ValidSpeakers[splistIndex].Cid;
                    spkrname = splist.ValidSpeakers[splistIndex].Name;
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
                    spkrname = splist.ValidSpeakers[splistIndex].Name;
                }
            }

            // 音声パラメタ
            var eff = splist.ValidSpeakers[splistIndex].Effects.ToDictionary(k => k.ParamName, v => v.Value);
            var emo = splist.ValidSpeakers[splistIndex].Emotions.ToDictionary(k => k.ParamName, v => v.Value);

            return (cid, spkrname, parsedText, eff, emo);
        }

    }
}
