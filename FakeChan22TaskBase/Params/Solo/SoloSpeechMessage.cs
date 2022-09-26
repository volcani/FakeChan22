using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FakeChan22.Params
{
    [DataContract]
    public class SoloSpeechMessage
    {
        [DataMember] public bool IsUse { get; set; } = true;
        [DataMember] public string Message { get; set; }
    }
}
