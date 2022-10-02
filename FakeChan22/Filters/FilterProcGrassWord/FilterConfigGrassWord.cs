using System.Runtime.Serialization;

namespace FakeChan22.Filters
{
    [DataContract]
    public class FilterConfigGrassWord : FilterConfigBase
    {
        public FilterConfigGrassWord()
        {
            FilterProcTypeFullName = typeof(FilterProcGrassWord).FullName;
            LabelName = "ｗ 変換";
            Description = "テキスト中の「ｗ」を「わら」へ変換します ※英語話者用ならチェックを外した方がいいです";

        }
    }
}
