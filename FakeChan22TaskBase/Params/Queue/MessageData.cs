using FakeChan22.Tasks;
using System.Collections.Generic;

namespace FakeChan22.Params
{
    public class MessageData
    {
        public int TaskId { get; set; }

        public int CompatSpeed { get; set; }

        public int CompatTone { get; set; }

        public int CompatVolume { get; set; }

        public int CompatVType { get; set; }

        public bool OverrideAsync { get; set; } = false;

        public string SelectedLang { get; set; } = "Ja";

        public ListenerConfigBase LsnrCfg { get; set; }

        public string Message { get; set; }

        public List<string> Messages { get; set; } = new List<string>();

        private string orgmessage;
        public string OrgMessage
        {
            get { return orgmessage; }
            set { orgmessage = Message = value; }
        }

        public MessageData()
        {
            Messages = new List<string>();
        }
    }
}
