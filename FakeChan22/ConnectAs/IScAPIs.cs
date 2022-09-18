using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FakeChan22
{
    [ServiceContract(SessionMode = SessionMode.Required)]
    internal interface IScAPIs
    {
        [OperationContract]
        string Verson();

        [OperationContract]
        Dictionary<int, string> AvatorList();

        [OperationContract]
        Dictionary<int, Dictionary<string, string>> AvatorList2();

        [OperationContract]
        Dictionary<int, Dictionary<string, string>> AvatorListDetail2();

        [OperationContract]
        Dictionary<string, Dictionary<string, Dictionary<string, decimal>>> GetDefaultParams2(int cid);

        [OperationContract]
        Dictionary<string, Dictionary<string, Dictionary<string, decimal>>> GetCurrentParams2(int cid);

        [OperationContract]
        double Talk(int cid, string talktext, string filepath, Dictionary<string, decimal> effects, Dictionary<string, decimal> emotions);

        [OperationContract]
        void TalkAsync(int cid, string talktext, Dictionary<string, decimal> effects, Dictionary<string, decimal> emotions);

        [OperationContract]
        double Save(int cid, string talktext, string filepath, Dictionary<string, decimal> effects, Dictionary<string, decimal> emotions);
    }
}
