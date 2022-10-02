using System.Runtime.Serialization;

namespace FakeChan22.Filters
{
    [DataContract]
    public class FilterConfigZen2HanNum : FilterConfigBase
    {
        public FilterConfigZen2HanNum()
        {
            FilterProcTypeFullName = typeof(FilterProcZen2HanNum).FullName;
            LabelName = "全角数字置換";
            Description = "テキスト中の全角数字を半角に変換します";

        }

    }
}
