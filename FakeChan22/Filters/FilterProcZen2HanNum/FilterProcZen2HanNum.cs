using FakeChan22.Plugins;
using System.Runtime.Serialization;

namespace FakeChan22.Filters
{
    [DataContract]
    public class FilterProcZen2HanNum : FilterProcBase
    {
        [DataMember] public string LabelName { get; set; } = "全角数字置換";

        [DataMember] public FilterConfigZen2HanNum FilterConfig { get; set; }

        public FilterProcZen2HanNum(ref FilterConfigZen2HanNum filterCfg)
        {
            FilterConfig = filterCfg;
        }

        public override void Processing(ref FilterParams fp)
        {
            fp.Text = ReplaceText.Zen2HanNumChar(fp.Text);
        }

    }
}
