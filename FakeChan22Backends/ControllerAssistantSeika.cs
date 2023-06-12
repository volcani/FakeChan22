using System.Collections.Generic;
using System.Linq;
using AssistantSeika;


namespace FakeChan22.Backends
{
    internal class ControllerAssistantSeika
    {
        public string BackendName = "AssistantSeika";

        WCFClient api = null;

        public ControllerAssistantSeika()
        {
            api = new AssistantSeika.WCFClient();
        }

        public Dictionary<int, Dictionary<string, string>> AvatorList2()
        {
            return api.AvatorListDetail2().Where(v => v.Value["isalias"] == "False").ToDictionary(k => k.Key, v => v.Value);
        }

        public double Talk(int cid, string text, string filename, Dictionary<string, decimal> eff, Dictionary<string, decimal> emo)
        {
            return api.Talk(cid, text, "", eff, emo);
        }

        public void TalkAsync(int cid, string text, Dictionary<string,decimal> eff, Dictionary<string, decimal> emo)
        {
            api.TalkAsync(cid, text, eff, emo);
        }

        public Dictionary<string, Dictionary<string, Dictionary<string, decimal>>> GetDefaultParams2(int cid)
        {
            return api.GetDefaultParams2(cid);
        }

    }
}
