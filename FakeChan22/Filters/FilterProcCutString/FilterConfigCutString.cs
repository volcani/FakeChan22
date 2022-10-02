using System.Runtime.Serialization;

namespace FakeChan22.Filters
{
    [DataContract]
    public class FilterConfigCutString : FilterConfigBase
    {
        [GuiItem(ParamName = "最大長", Description = "文字列がこの長さを越えたら切捨てます")]
        [DataMember]
        public int CutLength { get; set; }

        [GuiItem(ParamName = "追加文字列", Description = "切捨てが発生したら最後に付与する文字列です")]
        [DataMember]
        public string AppendStr { get; set; }

        public FilterConfigCutString()
        {
            FilterProcTypeFullName = typeof(FilterProcCutString).FullName;
            LabelName = "切捨て";
            Description = "テキストの文字数が指定値を越えたら切捨てます";

            CutLength = 96;
            AppendStr = @"(以下略";
        }
    }
}
