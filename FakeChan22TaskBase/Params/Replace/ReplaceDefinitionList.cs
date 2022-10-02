using FakeChan22.Filters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FakeChan22
{
    [DataContract]
    public class ReplaceDefinitionList
    {
        [DataMember] public List<FilterProcBase> FilterProcs;

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

        #region // 互換性のために残しているプロパティ

        /// <summary>
        /// 互換のために残している
        /// </summary>
        [DataMember] public List<ReplaceDefinition> Definitions;

        /// <summary>
        /// 互換のために残している
        /// </summary>
        [DataMember] public bool IsReplaceUrl { get; set; }

        /// <summary>
        /// 互換のために残している
        /// </summary>
        [DataMember] public string ReplaceStrFromUrl { get; set; }

        /// <summary>
        /// 互換のために残している
        /// </summary>
        [DataMember] public bool IsReplaceZentoHan1 { get; set; }

        /// <summary>
        /// 互換のために残している
        /// </summary>
        [DataMember] public bool IsReplaceZentoHan2 { get; set; }

        /// <summary>
        /// 互換のために残している
        /// </summary>
        [DataMember] public bool IsReplaceGrassWord { get; set; }

        /// <summary>
        /// 互換のために残している
        /// </summary>
        [DataMember] public bool IsReplaceApplauseWord { get; set; }

        /// <summary>
        /// 互換のために残している
        /// </summary>
        [DataMember] public bool IsReplaceEmoji { get; set; }

        /// <summary>
        /// 互換のために残している
        /// </summary>
        [DataMember] public bool IsRemovalEmojiBeforeReplace { get; set; }

        /// <summary>
        /// 互換のために残している
        /// </summary>
        [DataMember] public bool IsRemovalEmojiAfterReplace { get; set; }

        private int cutLength;

        /// <summary>
        /// 互換のために残している
        /// </summary>
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

        /// <summary>
        /// 互換のために残している
        /// </summary>
        [DataMember] public string AppendStr { get; set; }

        #endregion

        public ReplaceDefinitionList()
        {
            Listname = "置換リスト - " + DateTime.Now.ToString();
            UniqId = Guid.NewGuid().ToString();

            FilterProcs = new List<FilterProcBase>();

            // 互換のための設定
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
