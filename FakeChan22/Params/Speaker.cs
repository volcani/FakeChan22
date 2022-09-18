using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FakeChan22
{
    [DataContract]

    public class Speaker
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

        [DataMember] public List<AvatorParamSpec> Effects { get; set; }

        [DataMember] public List<AvatorParamSpec> Emotions { get; set; }

        public Speaker Clone()
        {
            return (Speaker)MemberwiseClone();
        }

    }
}
