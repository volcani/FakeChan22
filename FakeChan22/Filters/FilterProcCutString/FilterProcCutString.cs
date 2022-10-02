using System.Runtime.Serialization;

namespace FakeChan22.Filters
{
    [DataContract]
    public class FilterProcCutString : FilterProcBase
    {
        [DataMember] public string LabelName { get; set; } = "切捨て";

        [DataMember] public FilterConfigCutString FilterConfig { get; set; }

        public FilterProcCutString(ref FilterConfigCutString filterCfg)
        {
            FilterConfig = filterCfg;
        }

        public override void Processing(ref FilterParams fp)
        {
            if (fp.Text.Length > FilterConfig.CutLength)
            {
                fp.Text = fp.Text.Substring(0, FilterConfig.CutLength) + FilterConfig.AppendStr;
            }
        }

    }
}
