using FakeChan22.Params;
using FakeChan22.Tasks;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace FakeChan22.Filters
{
    [DataContract]
    public class FilterProcCleanupURL : FilterProcBase
    {
        static Regex RepUrlReg = new Regex(@"[hH]{0,1}[tT][tT][pP][sS]{0,1}:\/\/[^\t 　]{1,}");

        [DataMember] public string LabelName { get; set; } = "URL置換";

        [DataMember] public FilterConfigCleanupURL FilterConfig { get; set; }

        public FilterProcCleanupURL(ref FilterConfigCleanupURL filterCfg)
        {
            FilterConfig = filterCfg;
        }

        public override void Processing(ref FilterParams fp)
        {
            fp.Text = RepUrlReg.Replace(fp.Text, FilterConfig.ReplaceStrFromUrl);
        }

    }
}
