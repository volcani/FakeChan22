using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace FakeChan22.Backends
{
    public class BackendBridge
    {
        Dictionary<int, Backends.BackendItem> Backends = null;
        int BackendSelector = 1;

        public int Selector {
            get
            {
                return BackendSelector;
            }
            set
            {
                if ((0 < value) && Backends.ContainsKey(value) )
                {
                    BackendSelector = value;
                }
            }
        }

        public bool IsConnect
        {
            get
            {
                return Backends[BackendSelector].Connected;
            }
        }

        public Dictionary<int, string> BackendList
        {
            get
            {
                return Backends.Select(v => new { K = v.Key, V = v.Value.Name + " : " + (v.Value.Connected ? "Connecting" : "No Connection") }).ToDictionary(k => k.K, v => v.V);
            }
        }

        public BackendBridge()
        {
            Backends = new Dictionary<int, Backends.BackendItem>();

            // AssistantSeikaとの通信を最初に行ってみる
            try
            {
                var obj = new ControllerAssistantSeika();
                Backends.Add(1, new Backends.BackendItem("AssistantSeika", true, new ControllerAssistantSeika()));
            }
            catch (Exception)
            {
                Backends.Add(1, new Backends.BackendItem("AssistantSeika", false, null));
            }

            // 以降は追加のバックエンド

            // 使えるよね？
            var count = Backends.Count(v => v.Value.Connected == true);
            if (Backends.Count(v => v.Value.Connected == true) == 0)
            {
                BackendSelector = 0;
            }

        }

        /// <summary>
        /// 話者一覧取得
        /// </summary>
        /// <returns>cidと付属情報の辞書</returns>
        public Dictionary<int, Dictionary<string, string>> AvatorList2()
        {
            dynamic conObj = Backends[Selector].BackendObject;

            return conObj?.AvatorList2();
        }

        /// <summary>
        /// デフォルトパラメタ取得
        /// </summary>
        /// <param name="cid">話者のcid</param>
        /// <returns>指定話者のパラメタ辞書</returns>
        public Dictionary<string, Dictionary<string, Dictionary<string, decimal>>> GetDefaultParams2(int cid)
        {
            dynamic conObj = Backends[Selector].BackendObject;
            
            return conObj?.GetDefaultParams2(cid);
        }

        /// <summary>
        /// 発声
        /// </summary>
        /// <param name="cid">話者のcid</param>
        /// <param name="talktext">発声させるテキスト</param>
        /// <param name="effects">音声効果パラメタ</param>
        /// <param name="emotions">感情系パラメタ</param>
        /// <returns>大体の再生時間</returns>
        public double Talk(int cid, string talktext, Dictionary<string, decimal> effects, Dictionary<string, decimal> emotions)
        {
            dynamic conObj = Backends[Selector].BackendObject;

            return conObj?.Talk(cid, talktext, "", effects, emotions);
        }

        /// <summary>
        /// 非同期発声
        /// </summary>
        /// <param name="cid">話者のcid</param>
        /// <param name="talktext">発声させるテキスト</param>
        /// <param name="effects">音声効果パラメタ</param>
        /// <param name="emotions">感情系パラメタ</param>
        public void TalkAsync(int cid, string talktext, Dictionary<string, decimal> effects, Dictionary<string, decimal> emotions)
        {
            dynamic conObj = Backends[Selector].BackendObject;

            conObj?.TalkAsync(cid, talktext, effects, emotions);
        }

    }
}
