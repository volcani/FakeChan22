using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FakeChan22.Filters
{
    [DataContract]

    public class FilterConfigSplitUser : FilterConfigBase
    {
        [GuiItem(ParamName = "適用", Description = "分離した話者指定を有効にします")]
        [DataMember]
        public bool ApplyToSpeaker { get; set; }

        [GuiItem(ParamName = "補正", Description = "全角記述の話者指定に対応します")]
        [DataMember]
        public bool FixUserSpecifier { get; set; }

        public FilterConfigSplitUser()
        {
            FilterProcTypeFullName = typeof(FilterProcSplitUser).FullName;
            LabelName = "話者指定検出";
            Description = "テキスト先頭の話者指定を分離します。OFFだと話者指定が有効になりません";

        }

    }
}
