using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace FakeChan22.Filters
{
    [DataContract]
    public class FilterProcReplaceText : FilterProcBase
    {
        [DataMember] public string LabelName { get; set; } = "テキスト置換";

        [DataMember] public FilterConfigReplaceText FilterConfig { get; set; }

        public FilterProcReplaceText(ref FilterConfigReplaceText filterCfg)
        {
            FilterConfig = filterCfg;
        }

        public override void Processing(ref FilterParams fp)
        {
            if (fp.Text.Length != 0)
            {
                for (int idx = 0; idx < FilterConfig.Definitions.Count; idx++)
                {
                    if (FilterConfig.Definitions[idx].IsUse)
                    {
                        try
                        {
                            fp.Text = Regex.Replace(fp.Text, FilterConfig.Definitions[idx].MatchingPattern, FilterConfig.Definitions[idx].ReplaceText);
                        }
                        catch (Exception)
                        {
                            //
                        }
                    }
                }
            }

        }
    }
}
