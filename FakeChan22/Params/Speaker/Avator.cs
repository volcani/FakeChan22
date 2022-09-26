using System.Runtime.Serialization;

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
