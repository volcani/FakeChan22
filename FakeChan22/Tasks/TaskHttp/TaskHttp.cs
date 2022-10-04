using System;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using FakeChan22.Params;

namespace FakeChan22.Tasks
{
    public class TaskHttp : TaskBase, IDisposable
    {
        ListenerConfigHttp LsnrConfig = null;

        HttpListener httpListener = null;

        readonly string listFmt = @"""id"":{0}, ""kind"":""AquesTalk"", ""name"":""{1}"", ""alias"":""""";

        public TaskHttp(ref ListenerConfigHttp lsrCfg, ref MessageQueueWrapper que)
        {
            LsnrConfig = lsrCfg;
            MessQueue = que;
            based = false;
        }
        public void Dispose()
        {
            LsnrConfig = null;
            MessQueue = null;
            httpListener = null;
        }

        public override void TaskStart()
        {
            try
            {
                httpListener = new HttpListener();
                httpListener.Prefixes.Add(string.Format(@"http://{0}:{1}/", LsnrConfig.Host, LsnrConfig.Port));
                httpListener.Start();
                httpListener.BeginGetContext(new AsyncCallback(AcceptRequest), httpListener);
                IsRunning = true;
            }
            catch (Exception e)
            {
                Logging(String.Format(@"HTTP, {0}", e.Message));
                //throw new Exception(string.Format(@"Httpリスナ起動でエラー : {0}", e.Message));
            }
        }

        public override void TaskStop()
        {
            try
            {
                httpListener?.Stop();
            }
            catch (Exception)
            {
                //
            }

            IsRunning = false;
        }

        private void AcceptRequest(IAsyncResult result)
        {
            HttpListener listener = result.AsyncState as HttpListener;

            // EndGetContext()実行前にTcpListener.Stop()を実行するとコールバックされるのでトラップする
            try
            {
                listener.BeginGetContext(new AsyncCallback(AcceptRequest), listener);
            }
            catch (Exception)
            {
                return;
            }

            HttpListenerContext context = listener.EndGetContext(result);
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            int voice = -1;
            int volume = -1;
            int speed = -1;
            int tone = -1;
            string talktext = "";
            string callback = "";

            string UrlPath = request.Url.AbsolutePath.ToUpper();

            foreach (var item in Regex.Split(request.Url.Query, @"[&?]"))
            {
                if (item == "") continue;

                string[] s = Regex.Split(item, "=");
                if (s.Length < 2) s = new string[] { HttpUtility.UrlDecode(s[0]), "" };
                if (s.Length >= 2) s = new string[] { HttpUtility.UrlDecode(s[0]), HttpUtility.UrlDecode(s[1]) };

                switch (s[0])
                {
                    case "text":
                        talktext = s[1];
                        break;

                    case "voice":
                        int.TryParse(s[1], out voice);
                        break;

                    case "volume":
                        int.TryParse(s[1], out volume);
                        break;

                    case "speed":
                        int.TryParse(s[1], out speed);
                        break;

                    case "tone":
                        int.TryParse(s[1], out tone);
                        break;

                    case "callback":
                        callback = s[1];
                        break;

                    default:
                        break;
                }
            }

            response.ContentType = "application/json; charset=utf-8";
            StringBuilder sb = new StringBuilder();
            byte[] responseMessageBuff = null;

            switch (UrlPath)
            {
                case "/TALK":
                    MessageData talk = new MessageData()
                    {
                        LsnrCfg = LsnrConfig,
                        OrgMessage = talktext,
                        CompatSpeed = speed,
                        CompatTone = tone,
                        CompatVolume = volume,
                        CompatVType = voice,
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

                    if (callback != "") sb.AppendLine(string.Format(@"{0}(", callback));
                    sb.Append("{" + string.Format(@"""taskId"":{0}", talk.TaskId) + "}");
                    if (callback != "") sb.AppendLine(@");");
                    responseMessageBuff = Encoding.UTF8.GetBytes(sb.ToString());
                    break;

                case "/GETVOICELIST":
                    sb.Clear();
                    if (callback != "") sb.AppendLine( string.Format(@"{0}(",callback));
                    sb.AppendLine(@"{ ""voiceList"":[");
                    sb.Append(string.Join(",", LsnrConfig.SpeakerListDefault.ValidSpeakers.Select((v, i) => "{" + string.Format(listFmt, i, v.Value.Name) + "}").ToArray()));
                    sb.AppendLine(@"] }");
                    if (callback != "") sb.AppendLine(@")");
                    responseMessageBuff = Encoding.UTF8.GetBytes(sb.ToString());
                    Logging(@"HTTP, /GETVOICELIST");
                    break;

                case "/GETTALKTASKCOUNT":
                    sb.Clear();
                    if (callback != "") sb.AppendLine(string.Format(@"{0}(", callback));
                    sb.Append("{" + string.Format(@"""talkTaskCount"":{0}", MessQueue.count) + "}");
                    if (callback != "") sb.AppendLine(@")");
                    responseMessageBuff = Encoding.UTF8.GetBytes(sb.ToString());
                    break;

                case "/GETNOWTASKID":
                    sb.Clear();
                    if (callback != "") sb.AppendLine(string.Format(@"{0}(", callback));
                    sb.Append("{" + string.Format(@"""nowTaskId"":{0}", MessQueue.NowtaskId) + "}");
                    if (callback != "") sb.AppendLine(@")");
                    responseMessageBuff = Encoding.UTF8.GetBytes(sb.ToString());
                    break;

                case "/GETNOWPLAYING":
                    sb.Clear();
                    if (callback != "") sb.AppendLine(string.Format(@"{0}(", callback));
                    sb.Append("{" + string.Format(@"""nowPlaying"":{0}", MessQueue.IsSyncTaking) + "}");
                    if (callback != "") sb.AppendLine(@")");
                    responseMessageBuff = Encoding.UTF8.GetBytes(sb.ToString());
                    break;

                case "/CLEAR":
                    sb.Clear();
                    if (callback != "") sb.AppendLine(string.Format(@"{0}(", callback));
                    sb.Append(@"{}");
                    if (callback != "") sb.AppendLine(@")");
                    responseMessageBuff = Encoding.UTF8.GetBytes(sb.ToString());
                    break;

                default:
                    sb.Clear();
                    if (callback != "") sb.AppendLine(string.Format(@"{0}(", callback));
                    sb.Append(@"{ ""Message"":""content not found.""}");
                    if (callback != "") sb.AppendLine(@")");
                    responseMessageBuff = Encoding.UTF8.GetBytes(sb.ToString());
                    response.StatusCode = 404;
                    break;
            }

            response.OutputStream.Write(responseMessageBuff, 0, responseMessageBuff.Length);
            response.Close();
        }

    }
}
