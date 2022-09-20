using System.Runtime.Serialization;

namespace FakeChan22
{
    [DataContract]
    public class QueueParam
    {
        [DataMember]
        public int Mode5QueueLimit { get; set; }

        [DataMember]
        public int Mode4QueueLimit { get; set; }

        [DataMember]
        public int Mode3QueueLimit { get; set; }

        [DataMember]
        public int Mode2QueueLimit { get; set; }

        [DataMember]
        public int Mode1QueueLimit { get; set; }

        [DataMember]
        public int Mode0QueueLimit { get; set; }


        public QueueParam()
        {
            Mode5QueueLimit = 30;
            Mode4QueueLimit = 21;
            Mode3QueueLimit = 16;
            Mode2QueueLimit = 10;
            Mode1QueueLimit =  7;
            Mode0QueueLimit =  6;
        }

        public int QueueMode(int queCount)
        {
                 if (queCount > Mode5QueueLimit) return 5;
            else if (queCount > Mode4QueueLimit) return 4;
            else if (queCount > Mode3QueueLimit) return 3;
            else if (queCount > Mode2QueueLimit) return 2;
            else if (queCount > Mode1QueueLimit) return 1;
            else return 0;
        }

    }
}
