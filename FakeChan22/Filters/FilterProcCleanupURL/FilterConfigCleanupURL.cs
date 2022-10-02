using System.Runtime.Serialization;

namespace FakeChan22.Filters
{
    [DataContract]
    public class FilterConfigCleanupURL : FilterConfigBase
    {
        [GuiItem(ParamName = "置換後文字", Description = "URLを置換える文字列です")]
        [DataMember]
        public string ReplaceStrFromUrl { get; set; }

        public FilterConfigCleanupURL()
        {
            FilterProcTypeFullName = typeof(FilterProcCleanupURL).FullName;
            LabelName = "URL置換";
            Description = "テキスト中のURLらしい文字を指定の文字列へ置換します";

            ReplaceStrFromUrl = "URL省略";
        }
    }
}
