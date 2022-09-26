using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FakeChan22
{
    [DataContract]

    public class SpeakerFakeChanList
    {
        [DataMember] public List<SpeakerFakeChan> Speakers;

        [DataMember] public Dictionary<int, SpeakerFakeChan> ValidSpeakers;

        [DataMember] public Dictionary<string, SpeakerFakeChan> SpeakerMaps;

        private static string[] CompatMacros = { "y","b","h","d","a","r","t","g"};

        private string listname;
        [DataMember]
        public string Listname {
            get
            {
                return listname;
            }
            
            set
            {
                if (value==null) throw new ArgumentNullException("value");
                if (value.Length == 0) throw new ArgumentException("length");

                listname = value;
            }
        }

        private string uniqId;
        [DataMember]
        public string UniqId
        {
            get
            {
                return uniqId;
            }
            set
            {
                uniqId = value;
            }
        }

        public SpeakerFakeChanList()
        {
            Speakers = new List<SpeakerFakeChan>();
            ValidSpeakers = new Dictionary<int, SpeakerFakeChan>();
            SpeakerMaps = new Dictionary<string, SpeakerFakeChan>();

            listname = "話者リスト - " + DateTime.Now.ToString();
            UniqId = Guid.NewGuid().ToString();
        }

        public void MakeValidObjects()
        {
            MakeValidList();
            MakeMap();
        }

        public void MakeValidList()
        {
            ValidSpeakers.Clear();
            int idx = 0;

            for (int index = 0; index < Speakers.Count; index++)
            {
                if (Speakers[index].Apply)
                {
                    ValidSpeakers.Add(idx, Speakers[index]);
                    idx++;
                }
            }
        }

        private void MakeMap()
        {
            SpeakerMaps.Clear();

            // 棒読みちゃん互換の1文字識別子を登録
            int idx1 = 1;
            for (int i = 0; i < CompatMacros.Length; i++)
            {
                if (idx1 < ValidSpeakers.Count)
                {
                    SpeakerMaps.Add(CompatMacros[i], ValidSpeakers[idx1]);
                    idx1++;
                }
                else
                {
                    SpeakerMaps.Add(CompatMacros[i], ValidSpeakers[0]);
                }
            }

            // ユーザ定義の識別子を登録。同じ識別子が複数登録されている場合は後勝ちで登録される
            for (int idx2 = 0; idx2 < ValidSpeakers.Count; idx2++)
            {
                if ((ValidSpeakers[idx2].MacroName != "") && (ValidSpeakers[idx2].Apply))
                {
                    if(!SpeakerMaps.ContainsKey(ValidSpeakers[idx2].MacroName))
                    {
                        SpeakerMaps.Add(ValidSpeakers[idx2].MacroName, ValidSpeakers[idx2]);
                    }
                    else
                    {
                        SpeakerMaps[ValidSpeakers[idx2].MacroName] = ValidSpeakers[idx2];
                    }
                }
            }

        }


    }
}
