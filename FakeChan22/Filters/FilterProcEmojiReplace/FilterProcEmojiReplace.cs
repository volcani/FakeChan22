using EmojiTools;
using System.Runtime.Serialization;

namespace FakeChan22.Filters
{
    [DataContract]
    public class FilterProcEmojiReplace : FilterProcBase
    {
        [DataMember] public string LabelName { get; set; } = "絵文字置換";

        [DataMember] public FilterConfigEmojiReplace FilterConfig { get; set; }

        public FilterProcEmojiReplace(ref FilterConfigEmojiReplace filterCfg)
        {
            FilterConfig = filterCfg;
        }

        public override void Processing(ref FilterParams fp)
        {
            fp.Text = EmojiTool.ChangeEmoji(fp.Text);
        }

    }
}
