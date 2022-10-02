using System.Runtime.Serialization;

namespace FakeChan22.Filters
{
    [DataContract]
    public class FilterConfigEmojiCleaner : FilterConfigBase
    {
        public FilterConfigEmojiCleaner()
        {
            FilterProcTypeFullName = typeof(FilterProcEmojiCleaner).FullName;
            LabelName = "絵文字除去";
            Description = "テキスト中の絵文字を除去します";

        }

    }
}
