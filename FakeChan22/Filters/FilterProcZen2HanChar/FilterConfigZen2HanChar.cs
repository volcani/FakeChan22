using System.Runtime.Serialization;

namespace FakeChan22.Filters
{
    [DataContract]
    public class FilterConfigZen2HanChar : FilterConfigBase
    {
        public FilterConfigZen2HanChar()
        {
            FilterProcTypeFullName = typeof(FilterProcZen2HanChar).FullName;
            LabelName = "全角英字記号置換";
            Description = "テキスト中の全角英字と記号を半角に変換します";

        }
    }
}
