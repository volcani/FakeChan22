using FakeChan22.Plugins;
using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace FakeChan22.Filters
{
    [DataContract]
    public class FilterProcSplitUser : FilterProcBase
    {
        [DataMember] public string LabelName { get; set; } = "話者指定検出";

        [DataMember] public FilterConfigSplitUser FilterConfig { get; set; }

        public FilterProcSplitUser(ref FilterConfigSplitUser filterCfg)
        {
            FilterConfig = filterCfg;
        }

        public override void Processing(ref FilterParams fp)
        {
            string key = "";
            string text = "";

            if(FilterConfig.FixUserSpecifier)
            {
                (key, text) = ReplaceText.SplitUserSpecifierFullwide (fp.Text);
            }
            else
            {
                (key, text) = ReplaceText.SplitUserSpecifier(fp.Text);
            }

            fp.UserSpecifier = FilterConfig.ApplyToSpeaker ? key : "";
            fp.Text = text;
        }
    }
}
