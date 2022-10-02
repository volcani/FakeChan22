using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace FakeChan22.Filters
{
    [DataContract]
    public class FilterProcApplauseWord : FilterProcBase
    {
        static Regex RepApplause01 = new Regex(@"^[8８]{3,}$");
        static Regex RepApplause02 = new Regex(@"^[8８]{3,}([^0-9０-９]{1,})");
        static Regex RepApplause03 = new Regex(@"([^0-9０-９]{1,})[8８]{3,}([^0-9０-９]{1,})");
        static Regex RepApplause04 = new Regex(@"([^0-9０-９]{1,})[8８]{3,}");

        static string Words01p = @"パチパチパチ";
        static string Words02p = @"パチパチパチ $1";
        static string Words03p = @"$1 パチパチパチ $2";
        static string Words04p = @"$1 パチパチパチ";

        [DataMember] public string LabelName { get; set; } = "８ 変換";

        [DataMember] public FilterConfigApplauseWord FilterConfig { get; set; }

        public FilterProcApplauseWord(ref FilterConfigApplauseWord filterCfg)
        {
            FilterConfig = filterCfg;
        }

        public override void Processing(ref FilterParams fp)
        {
            fp.Text = RepApplause04.Replace(RepApplause03.Replace(RepApplause02.Replace(RepApplause01.Replace(fp.Text, Words01p), Words02p), Words03p), Words04p);
        }

    }
}
