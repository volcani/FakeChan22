using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FakeChan22
{
    [DataContract]
    public class Avator
    {
        [DataMember] public int Cid { get; set; }

        [DataMember] public string Name { get; set; }

        [DataMember] public string ProdName { get; set; }

        [DataMember]
        public string DispName
        {
            get
            {
                return string.Format(@"{0} : {1}_{2}", Cid, Name, ProdName);
            }

        }
    }
}
