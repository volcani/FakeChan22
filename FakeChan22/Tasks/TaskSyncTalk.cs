using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace FakeChan22.Tasks
{
    public class TaskSyncTalk
    {
        MessageQueueWrapper messQueue;
        FakeChanConfig config;
        Random r = new Random();
        DispatcherTimer KickTalker;
        SubTaskCommentGen CommentGen;

        ScAPIs api;

        bool IsEntered = false;

        public TaskSyncTalk(ref MessageQueueWrapper que, ref FakeChanConfig cnfg)
        {
            messQueue = que;
            config = cnfg;

            api = new ScAPIs();
            KickTalker = new DispatcherTimer();
            KickTalker.Tick += new EventHandler(KickTalker_Tick);
            KickTalker.Interval = new TimeSpan(0, 0, 1);
            KickTalker.Start();

            CommentGen = new SubTaskCommentGen();
            CommentGen.TaskStart(config.commentXmGenlPath);
        }

        private void KickTalker_Tick(object sender, EventArgs e)
        {
            if ((messQueue.count != 0)&&(!IsEntered))
            {
                IsEntered = true;

                Task.Run(() =>
                {
                    messQueue.IsSyncTaking = true;

                    SpeakerList splist = null;
                    ReplaceDefinitionList replist = null;
                    string sepText = "";
                    string sepMap = "";
                    string parsedText = "";
                    int cid = 0;
                    string cname = "";
                    int splistIndex = -1;

                    foreach (var item in messQueue.QueueRef().GetConsumingEnumerable())
                    {
                        // 話者のキーとテキストを分離
                        (sepMap, sepText) = ReplaceText.SeparateMapKey(item.OrgMessage);

                        // 非日本語判定の実施判定
                        splist = item.LsnrCfg.SpeakerListDefault;
                        replist = item.LsnrCfg.ReplaceListDefault;

                        // 非日本語判定の実施判定
                        if (item.LsnrCfg.IsNoJapanese)
                        {
                            (bool judge, double rate) = JudgeTextLang.JudgeNoJapanese(item.OrgMessage, item.LsnrCfg.NoJapaneseCharRate);
                            if (judge)
                            {
                                splist = item.LsnrCfg.SpeakerListNoJapaneseJudge;
                                replist = item.LsnrCfg.ReplaceListNoJapaneseJudge;
                            }
                        }

                        // 置換処理
                        parsedText = ReplaceText.ParseText(sepText, replist);
                        item.Message = parsedText;

                        // 話者選定
                        if ((sepMap != "") && (splist.SpeakerMaps.ContainsKey(sepMap)))
                        {
                            // テキスト先頭に話者の識別子が定義されている場合はその話者とする
                            cid = splist.SpeakerMaps[sepMap].Cid;
                            cname = splist.SpeakerMaps[sepMap].Name;
                        }
                        else
                        {
                            if (item.LsnrCfg.IsRandom)
                            {
                                // ランダム指定があれば話者リストの範囲内でランダムに話者を選択する
                                splistIndex = r.Next(0, splist.ValidSpeakers.Count);
                                cid = splist.ValidSpeakers[splistIndex].Cid;
                                cname = splist.ValidSpeakers[splistIndex].Name;
                            }
                            else
                            {
                                if ((item.CompatVType != -1) &&(item.CompatVType < splist.ValidSpeakers.Count))
                                {
                                    // 声質指定がされている場合は話者リスト1番目～9番目に割り当てる
                                    splistIndex = item.CompatVType;
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
                            CommentGen.AddComment(item.Message, item.LsnrCfg.ServiceName, "", string.Format(@"{0}:{1}", cid, cname));

                            api.Talk(cid, item.Message, "", eff, emo);
                        }
                        catch(Exception)
                        {
                            //
                        }

                    }

                    messQueue.IsSyncTaking = false;

                    IsEntered = false;
                });

            }
        }

    }
}
