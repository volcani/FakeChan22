using System;
using System.Runtime.Serialization;

namespace FakeChan22.Filters
{
    [DataContract]
    public class FilterConfigBase
    {
        [DataMember] public string LabelName { get; set; }

        [DataMember] public string Description { get; set; }

        [DataMember] public bool IsUse { get; set; }

        [DataMember] public string UniqId { get; set; }

        [DataMember] public string FilterProcTypeFullName { get; set; }


        public FilterConfigBase()
        {
            UniqId = Guid.NewGuid().ToString();
            IsUse = false;
        }

        public FilterConfigBase Clone()
        {
            return (FilterConfigBase)MemberwiseClone();
        }

    }
}
