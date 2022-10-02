using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FakeChan22.Filters
{
    [DataContract]

    public class FilterConfigReplaceText : FilterConfigBase
    {
        [GuiItem(ParamName = "置換リスト", Description = "テキスト置換の正規表現リストです")]
        [DataMember]
        public List<ReplaceDefinition> Definitions { get; set; }

        public FilterConfigReplaceText()
        {
            FilterProcTypeFullName = typeof(FilterProcReplaceText).FullName;
            LabelName = "テキスト置換";
            Description = "置換リストに従ってテキストを置換します";

        }

    }
}
