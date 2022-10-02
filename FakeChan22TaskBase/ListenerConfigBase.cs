using System;
using System.Runtime.Serialization;

namespace FakeChan22.Tasks
{
    [DataContract]
    public class ListenerConfigBase
    {
        /// <summary>
        /// 名称表示に使われます。
        /// </summary>
        [DataMember] public string LabelName { get; set; }

        /// <summary>
        /// 通常時に適用する話者リストです。エディタで変更されます。
        /// </summary>
        [DataMember] public SpeakerFakeChanList SpeakerListDefault { get; set; }

        /// <summary>
        /// 内部処理用です。
        /// </summary>
        [DataMember] public int SpeakerListDefaultIndex { get; set; }

        /// <summary>
        /// 通常時に適用する置換リストです。エディタで変更されます。
        /// </summary>
        [DataMember] public ReplaceDefinitionList ReplaceListDefault { get; set; }

        /// <summary>
        /// 内部処理用です。
        /// </summary>
        [DataMember] public int ReplaceListDefaultIndex { get; set; }

        /// <summary>
        /// 非日本語判定の有無です。true時に非日本語判定を行います。エディタで変更されます。
        /// </summary>
        [DataMember] public bool IsNoJapanese { get; set; }

        /// <summary>
        /// テキスト中の非日本語文字が、この占有率を越えると非日本語と判定されます。エディタで変更されます。
        /// </summary>
        [DataMember] public double NoJapaneseCharRate { get; set; }

        /// <summary>
        /// 非日本語判定時に適用する話者リストです。エディタで変更されます。
        /// </summary>
        [DataMember] public SpeakerFakeChanList SpeakerListNoJapaneseJudge { get; set; }

        [DataMember] public int SpeakerListNoJapaneseJudgeIndex { get; set; }

        /// <summary>
        /// 非日本語判定時に適用する置換リストです。エディタで変更されます。
        /// </summary>
        [DataMember] public ReplaceDefinitionList ReplaceListNoJapaneseJudge { get; set; }

        [DataMember] public int ReplaceListNoJapaneseJudgeIndex { get; set; }

        /// <summary>
        /// comment.xmlのService属性に設定する値です。エディタで変更されます。
        /// </summary>
        [DataMember] public string ServiceName { get; set; }

        /// <summary>
        /// テキスト(メッセージ)をキューに登録せず非同期に行う場合はtrueにします。エディタで変更されます。
        /// </summary>
        [DataMember] public bool IsAsync { get; set; }

        /// <summary>
        /// 話者リスト中の話者をランダムに選択する場合はtrueにします。エディタで変更されます。
        /// </summary>
        [DataMember] public bool IsRandom { get; set; }

        /// <summary>
        /// リスナ起動を許可する場合にtrueを設定します。エディタで変更されます。
        /// </summary>
        [DataMember] public bool IsEnable { get; set; }

        /// <summary>
        /// 内部処理用です。
        /// </summary>
        [DataMember] public string TaskTypeFullName { get; set; }

        public ListenerConfigBase()
        {
            SpeakerListDefault = null;
            ReplaceListDefault = null;
            SpeakerListNoJapaneseJudge = null;
            ReplaceListNoJapaneseJudge = null;
            IsNoJapanese = false;
            IsAsync = false;
            IsRandom = false;
            IsEnable = false;
            NoJapaneseCharRate = 75.0;
        }
    }
}
