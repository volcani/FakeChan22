using System.Runtime.Serialization;

namespace FakeChan22.Filters
{
    [DataContract]
    public class FilterConfigEmojiReplace : FilterConfigBase
    {
        public FilterConfigEmojiReplace()
        {
            FilterProcTypeFullName = typeof(FilterProcEmojiReplace).FullName;
            LabelName = "絵文字置換";
            Description = "テキスト中の絵文字の一部を通常文字へ置換します";

        }
    }
}
