using System.Runtime.Serialization;

namespace FakeChan22.Filters
{
    [DataContract]
    public class FilterConfigApplauseWord : FilterConfigBase
    {
        public FilterConfigApplauseWord()
        {
            FilterProcTypeFullName = typeof(FilterProcApplauseWord).FullName;
            LabelName = "８ 変換";
            Description = "テキスト中の「８８８」を「パチパチパチ」へ変換します ※英語話者用ならチェックを外した方がいいです";
        }

    }
}
