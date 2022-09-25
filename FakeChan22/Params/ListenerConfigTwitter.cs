using CoreTweet;
using FakeChan22.Plugins;
using FakeChan22.Tasks;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace FakeChan22
{
    [DataContract]
    public class ListenerConfigTwitter : ListenerConfig
    {
        [GuiItem(ParamName = "API Key",Description = "Developer Platform で取得したAPIキー")]
        [DataMember]
        public string ConsumerKey { get; set; } = "";

        [GuiItem(ParamName = "API Key Secret", Description = "Developer Platform で取得したAPIシークレットキー")]
        [DataMember]
        public string ConsumerKeySecret { get; set; } = "";

        [GuiItem(ParamName = "Access Token", Description = "Developer Platform で取得したAPIトークン")]
        [DataMember]
        public string AccessToken { get; set; } = "";

        [GuiItem(ParamName = "Access Token Secret", Description = "Developer Platform で取得したAPIシークレットトークン")]
        [DataMember]
        public string AccessTokenSecret { get; set; } = "";

        [GuiItem(ParamName = "lang", Description = "検索時ツイート言語指定")]
        [DataMember]
        public string SearchLang { get; set; } = "ja";

        [GuiItem(ParamName = @"result__type", Description = "検索時対象ツイート")]
        [DataMember]
        public string SearchResultType { get; set; } = "recent";

        [GuiItem(ParamName = "count", Description = "検索で取得するツイート最大数")]
        [DataMember]
        public int SearchCount { get; set; } = 100;

        [GuiItem(ParamName = @"since__id", Description = "このIDは自動更新されます")]
        [DataMember]
        public string SearchSinceID { get; set; } = "";

        [GuiItem(ParamName = "q", Description = "検索時キーワード")]
        [DataMember]
        public string SearchQuery { get; set; } = @"#宝鐘マリン";

        public ListenerConfigTwitter()
        {
            LabelName = "Twitter";
            ServiceName = "Twitter";
            LsnrType = ListenerType.twitter;
        }
    }
}
