using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeChan22.Filters
{
    public class FilterParams
    {
        /// <summary>
        /// 処理されるテキスト
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// ユーザ指定（切り出しした場合）
        /// </summary>
        public string UserSpecifier { get; set; }

        /// <summary>
        /// 言語
        /// 現時点では"ja" or "noneja"
        /// </summary>
        public string Lang { get; set; }

        /// <summary>
        /// 非日本語判定計算値
        /// </summary>
        public double JudgeCalculatedValue { get; set; }


        public FilterParams()
        {
            Text = "";
            UserSpecifier = "";
            Lang = "ja";
        }
    }
}
