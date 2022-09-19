using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Effects;
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

        public void AsyncTalk(MessageData talk)
        {
            int cid = 0;
            string text = "";
            string cname = "";
            Dictionary<string, decimal> eff;
            Dictionary<string, decimal> emo;

            (cid, cname, text, eff, emo) = ParseSpeakerAndParams(talk);

            api.TalkAsync(cid, text, eff, emo);
        }

        private void KickTalker_Tick(object sender, EventArgs e)
        {
            if ((!IsEntered)&& (messQueue.count != 0))
            {
                IsEntered = true;

                Task.Run(() =>
                {
                    messQueue.IsSyncTaking = true;

                    int cid = 0;
                    string text = "";
                    string cname = "";
                    Dictionary<string, decimal> eff;
                    Dictionary<string, decimal> emo;

                    foreach (var item in messQueue.QueueRef().GetConsumingEnumerable())
                    {
                        (cid, cname, text, eff, emo) = ParseSpeakerAndParams(item);

                        // 発声
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
            talk.Message = parsedText;

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
