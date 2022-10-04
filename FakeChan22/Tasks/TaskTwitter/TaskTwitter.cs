using System;
using System.Net;
using System.Text.RegularExpressions;
using CoreTweet;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeChan22.Params;

namespace FakeChan22.Tasks
{
    public class TaskTwitter : TaskBase, IDisposable
    {
        ListenerConfigTwitter LsnrConfig = null;

        Tokens tokens = null;
        DispatcherTimer KickTweetRead = null;

        public TaskTwitter(ref ListenerConfigTwitter lsrCfg, ref MessageQueueWrapper que)
        {
            LsnrConfig = lsrCfg;
            MessQueue = que;
            based = false;
        }

        public void Dispose()
        {
            LsnrConfig = null;
            MessQueue = null;
            tokens = null;
            KickTweetRead = null;
        }

        public override void TaskStart()
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                tokens = Tokens.Create(LsnrConfig.ConsumerKey, LsnrConfig.ConsumerKeySecret, LsnrConfig.AccessToken, LsnrConfig.AccessTokenSecret);

                KickTweetRead = new DispatcherTimer();
                KickTweetRead.Tick += new EventHandler(KickTweetRead_Tick);
                KickTweetRead.Interval = new TimeSpan(0, 0, 0, 40, 0);
                KickTweetRead.Start();

                IsRunning = true;
            }
            catch (Exception e)
            {
                Logging(String.Format(@"TWITTER, {0}", e.Message));
                //throw new Exception(string.Format(@"Twitterリスナ起動でエラー : {0}", e.Message));
            }
        }

        public override void TaskStop()
        {
            try
            {
                KickTweetRead?.Stop();
            }
            catch (Exception)
            {
                //
            }

            IsRunning = false;
        }

        private void KickTweetRead_Tick(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                Dictionary<string, object> dict = new Dictionary<string, object>()
                {
                    {"q", LsnrConfig.SearchQuery },
                    {"lang", LsnrConfig.SearchLang },
                    {"count",LsnrConfig.SearchCount },
                    {"result_type",LsnrConfig.SearchResultType }
                };
                if (LsnrConfig.SearchSinceID != "") dict.Add("since_id", LsnrConfig.SearchSinceID);

                SearchResult result = tokens.Search.Tweets(dict);
                int count = result.SearchMetadata.Count;

                LsnrConfig.SearchSinceID = result.SearchMetadata.MaxId.ToString();

                for (int index = 0; index < count; index++)
                {
                    var item = result[index];

                    if (item.RetweetedStatus != null) continue;

                    var talk = new Params.MessageData()
                    {
                        LsnrCfg = LsnrConfig,
                        OrgMessage = Regex.Replace(item.Text, @"[\r\n]", ""),
                        CompatSpeed = -1,
                        CompatTone = -1,
                        CompatVolume = -1,
                        CompatVType = -1,
                        TaskId = MessQueue.count + 1
                    };

                    AsTalk(talk);

                    //if (LsnrConfig.IsAsync)
                    //{
                    //    AsyncTalk(talk);
                    //}
                    //else
                    //{
                    //    SyncTalk(talk);
                    //}
                }
            });
        }

    }
}
