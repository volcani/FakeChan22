using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FakeChan22
{
    [DataContract]
    public class ReplaceDefinitionList
    {
        [DataMember] public List<ReplaceDefinition> Definitions;

        private string listname;
        [DataMember]
        public string Listname
        {
            get
            {
                return listname;
            }

            set
            {
                if (value == null) throw new ArgumentNullException("value");
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

        [DataMember] public bool IsReplaceUrl { get; set; }

        [DataMember] public string ReplaceStrFromUrl { get; set; }

        [DataMember] public bool IsReplaceZentoHan1 { get; set; }

        [DataMember] public bool IsReplaceZentoHan2 { get; set; }

        [DataMember] public bool IsReplaceGrassWord { get; set; }

        [DataMember] public bool IsReplaceApplauseWord { get; set; }

        [DataMember] public bool IsReplaceEmoji { get; set; }

        [DataMember] public bool IsRemovalEmojiBeforeReplace { get; set; }

        [DataMember] public bool IsRemovalEmojiAfterReplace { get; set; }

        private int cutLength;
        
        [DataMember]
        public int CutLength
        {
            get
            {
                return cutLength;
            }

            set
            {
                if (value < 1) throw new ArgumentOutOfRangeException("CutLength");

                cutLength = value;
            }
        }

        [DataMember] public string AppendStr { get; set; }

        public ReplaceDefinitionList()
        {
            Listname = "置換リスト - " + DateTime.Now.ToString();
            UniqId = Guid.NewGuid().ToString();

            Definitions = new List<ReplaceDefinition>();

            IsReplaceGrassWord = true;
            IsReplaceApplauseWord = true;
            IsReplaceEmoji = false;
            IsRemovalEmojiAfterReplace = false;

            ReplaceStrFromUrl = @"URL省略";
            CutLength = 96;
            AppendStr = @"(以下略";
        }
    }
}
