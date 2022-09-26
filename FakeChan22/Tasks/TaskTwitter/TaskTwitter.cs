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
        ListenerConfigTwitter lsnrCfg = null;

        Tokens tokens = null;
        DispatcherTimer KickTweetRead = null;

        public TaskTwitter(ref ListenerConfigTwitter lsrCfg, ref MessageQueueWrapper que)
        {
            lsnrCfg = lsrCfg;
            MessQueue = que;
            based = false;
        }

        public void Dispose()
        {
            lsnrCfg = null;
            MessQueue = null;
            tokens = null;
            KickTweetRead = null;
        }

        public override void TaskStart()
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                tokens = Tokens.Create(lsnrCfg.ConsumerKey, lsnrCfg.ConsumerKeySecret, lsnrCfg.AccessToken, lsnrCfg.AccessTokenSecret);

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
                    {"q", lsnrCfg.SearchQuery },
                    {"lang", lsnrCfg.SearchLang },
                    {"count",lsnrCfg.SearchCount },
                    {"result_type",lsnrCfg.SearchResultType }
                };
                if (lsnrCfg.SearchSinceID != "") dict.Add("since_id", lsnrCfg.SearchSinceID);

                SearchResult result = tokens.Search.Tweets(dict);
                int count = result.SearchMetadata.Count;

                lsnrCfg.SearchSinceID = result.SearchMetadata.MaxId.ToString();

                for (int index = 0; index < count; index++)
                {
                    var item = result[index];

                    if (item.RetweetedStatus != null) continue;

                    var talk = new Params.MessageData()
                    {
                        LsnrCfg = lsnrCfg,
                        OrgMessage = Regex.Replace(item.Text, @"[\r\n]", ""),
                        CompatSpeed = -1,
                        CompatTone = -1,
                        CompatVolume = -1,
                        CompatVType = -1,
                        TaskId = MessQueue.count + 1
                    };

                    if (lsnrCfg.IsAsync)
                    {
                        AsyncTalk(talk);
                    }
                    else
                    {
                        SyncTalk(talk);
                    }
                }
            });
        }

    }
}
