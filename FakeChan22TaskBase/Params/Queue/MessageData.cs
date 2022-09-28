using FakeChan22.Tasks;

namespace FakeChan22.Params
{
    public class MessageData
    {
        public int TaskId { get; set; }

        public int CompatSpeed { get; set; }

        public int CompatTone { get; set; }

        public int CompatVolume { get; set; }

        public int CompatVType { get; set; }

        public ListenerConfigBase LsnrCfg { get; set; }

        public string Message { get; set; }

        private string orgmessage;
        public string OrgMessage
        {
            get { return orgmessage; }
            set { orgmessage = Message = value; }
        }
    }
}
