using EmojiTools;
using System.Runtime.Serialization;

namespace FakeChan22.Filters
{
    [DataContract]
    public class FilterProcEmojiCleaner : FilterProcBase
    {
        [DataMember] public string LabelName { get; set; } = "絵文字除去";

        [DataMember] public FilterConfigEmojiCleaner FilterConfig { get; set; }

        public FilterProcEmojiCleaner(ref FilterConfigEmojiCleaner filterCfg)
        {
            FilterConfig = filterCfg;
        }

        public override void Processing(ref FilterParams fp)
        {
            fp.Text = EmojiTool.StripEmoji(fp.Text);
        }

    }
}
