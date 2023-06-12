using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Text.RegularExpressions;
using FakeChan22.Params;
using FakeChan22.Plugins;
using FakeChan22.Config;
using FakeChan22.Filters;
using FakeChan22.Backends;
using static System.Net.Mime.MediaTypeNames;

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
        BackendBridge Bridge;
        Task backgroundTalker;
        BlockingCollection<string> logQue = new BlockingCollection<string>();

        public delegate void CallEventHandlerLogging(string logText);
        public event CallEventHandlerLogging OnLogging;

        private int SoloTimeCount = 0;

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

            Bridge = App.TTSBridge;

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

                    OnLogging?.Invoke(string.Format(@"TALK, {0}", txt));
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
            string sname = talk.LsnrCfg == null ? "----" : talk.LsnrCfg.ServiceName;

            var (cid, cname, text, eff, emo) = ParseSpeakerAndParams(talk);

            Log(string.Format(@"{0}, {1}, async, [{2}]", sname, cid, talk.OrgMessage));
            Bridge.TalkAsync(cid, text, eff, emo);
        }

        private void KickTalker_Tick(object sender, EventArgs e)
        {
            SoloTimeCount++;
            if (SoloTimeCount > 172800) SoloTimeCount = 0;

            if (messQueue.count == 0)
            {
                if ((config.soloSpeechList.IsUse) && (config.soloSpeechList.SpeechDefinitions.ContainsKey(SoloTimeCount)))
                {
                    Task.Run(() => {
                        var messs = config.soloSpeechList.SpeechDefinitions[SoloTimeCount].Messages.Where(v => v.IsUse).ToList();
                        var spkrs = config.soloSpeechList.SpeechDefinitions[SoloTimeCount].speakerList.ValidSpeakers;
                        var idx1 = r.Next(0, spkrs.Count);
                        var text = messs.Count == 0 ? "" : messs[r.Next(0, messs.Count)].Message;

                        var cid = spkrs[idx1].Cid;
                        var eff = spkrs[idx1].Effects.ToDictionary(k => k.ParamName, v => v.Value);
                        var emo = spkrs[idx1].Emotions.ToDictionary(k => k.ParamName, v => v.Value);

                        Log(string.Format(@"SOLO, {0}, past {1}sec, [{2}]", cid, SoloTimeCount, text));
                        Bridge.TalkAsync(cid, text, eff, emo);
                    });
                }

                if ((config.soloSpeechList.SpeechDefinitions.Count != 0) && (config.soloSpeechList.SpeechDefinitions.Max(k => k.Key) < SoloTimeCount)) SoloTimeCount = 0;

                return;
            }

            SoloTimeCount = 0;

            if ((backgroundTalker != null) && (!backgroundTalker.IsCompleted)) return;

            backgroundTalker = Task.Run(() => {

                if (messQueue.count == 0) return;
                if (messQueue.IsSyncTaking) return;

                messQueue.IsSyncTaking = true;

                MessageData item;
                while ((item = messQueue.TakeQueue()) != null)
                {
                    string sname;

                    messQueue.NowtaskId = item.TaskId;

                    sname = item.LsnrCfg == null ? "----" : item.LsnrCfg.ServiceName;

                    var (cid, cname, text, eff, emo) = ParseSpeakerAndParams(item);
                    int mode = config.queueParam.QueueMode(messQueue.count);

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
                        CommentGen.AddComment(item.OrgMessage, sname, "", string.Format(@"{0}:{1}", cid, cname));
                        Log(string.Format(@"{0}, {1}, mode{2}, [{3}]", sname, cid, mode, item.OrgMessage));
                        Bridge.Talk(cid, text, eff, emo);
                    }
                    catch (Exception)
                    {
                        //
                    }

                    messQueue.NowtaskId = 0;
                }

                messQueue.IsSyncTaking = false;
            });


        }

        private (int cid, string spkrName, string text, Dictionary<string,decimal> eff, Dictionary<string, decimal> emo) ParseSpeakerAndParams(MessageData talk)
        {
            int cid = 0;
            string spkrname = "";
            int splistIndex = -1;
            SpeakerFakeChanList splist = null;
            ReplaceDefinitionList replist = null;
            Dictionary<string, decimal> eff = null;
            Dictionary<string, decimal> emo = null;

            splist = talk.LsnrCfg.SpeakerListDefault;
            replist = talk.LsnrCfg.ReplaceListDefault;

            talk.OrgMessage = Regex.Replace(talk.OrgMessage, @"(\r\n|\r|\n)", "");

            var (sepMap, sepText) = ReplaceText.SplitUserSpecifier(talk.OrgMessage);

            // 非日本語判定の実施
            if (talk.LsnrCfg.IsNoJapanese)
            {
                talk.SelectedLang = "noneJa";

                (bool judge, double rate) = JudgeTextLang.JudgeNoJapanese(sepText, talk.LsnrCfg.NoJapaneseCharRate);
                if (judge)
                {
                    splist = talk.LsnrCfg.SpeakerListNoJapaneseJudge;
                    replist = talk.LsnrCfg.ReplaceListNoJapaneseJudge;
                }
            }
            else
            {
                talk.SelectedLang = "Ja";
            }

            // 置換処理
            FilterParams fp = ReplaceText.ParseText(talk.OrgMessage, replist);
            talk.Message = fp.Text;
            talk.OverrideAsync = fp.UseAsync;

            // 話者選定
            if ((fp.UserSpecifier != "") && (splist.SpeakerMaps.ContainsKey(fp.UserSpecifier)))
            {
                // テキスト先頭に話者の識別子が定義されている場合はその話者とする
                cid = splist.SpeakerMaps[fp.UserSpecifier].Cid;
                spkrname = splist.SpeakerMaps[fp.UserSpecifier].Name;

                // 音声パラメタ
                eff = splist.SpeakerMaps[fp.UserSpecifier].Effects.ToDictionary(k => k.ParamName, v => v.Value);
                emo = splist.SpeakerMaps[fp.UserSpecifier].Emotions.ToDictionary(k => k.ParamName, v => v.Value);
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

                // 音声パラメタ
                eff = splist.ValidSpeakers[splistIndex].Effects.ToDictionary(k => k.ParamName, v => v.Value);
                emo = splist.ValidSpeakers[splistIndex].Emotions.ToDictionary(k => k.ParamName, v => v.Value);
            }

            return (cid, spkrname, fp.Text, eff, emo);
        }

    }
}
