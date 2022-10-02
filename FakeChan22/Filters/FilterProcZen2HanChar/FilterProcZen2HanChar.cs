using FakeChan22.Plugins;
using System.Runtime.Serialization;

namespace FakeChan22.Filters
{
    [DataContract]
    public class FilterProcZen2HanChar : FilterProcBase
    {
        [DataMember] public string LabelName { get; set; } = "全角英字記号置換";

        [DataMember] public FilterConfigZen2HanChar FilterConfig { get; set; }

        public FilterProcZen2HanChar(ref FilterConfigZen2HanChar filterCfg)
        {
            FilterConfig = filterCfg;
        }

        public override void Processing(ref FilterParams fp)
        {
            fp.Text = ReplaceText.Zen2HanChar(fp.Text);
        }

    }
}
