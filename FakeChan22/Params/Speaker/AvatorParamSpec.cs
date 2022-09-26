using System.Runtime.Serialization;

namespace FakeChan22
{
    [DataContract]
    public class AvatorParamSpec
    {
        [DataMember] public string ParamName { get; set; }

        [DataMember] public decimal Value { get; set; }

        [DataMember] public decimal Min_value { get; set; }

        [DataMember] public decimal Max_value { get; set; }

        [DataMember] public decimal Step { get; set; }
    }
}
