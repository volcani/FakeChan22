using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace FakeChan22
{
    [DataContract]

    public class SpeakerFakeChan
    {
        [DataMember] public bool Apply { get; set; }

        [DataMember] public int Cid { get; set; }

        [DataMember] public string Name { get; set; }

        [DataMember] public string ProdName { get; set; }

        private string macroName;

        [DataMember]
        public string MacroName
        {
            get
            {
                return macroName;
            }

            set
            {
                if (value.Length > 2)
                {
                    throw new ArgumentException("識別子が2文字を越えている");
                }
                else if (!Regex.IsMatch(value, @"[\da-zA-Z]{0,2}"))
                {
                    throw new ArgumentException("識別子が1～2文字の英数字ではない");
                }

                macroName = value;
            }
        }

        [DataMember] public List<SpeakerAssistantSeikaParamSpec> Effects { get; set; }

        [DataMember] public List<SpeakerAssistantSeikaParamSpec> Emotions { get; set; }

        public SpeakerFakeChan Clone()
        {
            return (SpeakerFakeChan)MemberwiseClone();
        }

    }
}
