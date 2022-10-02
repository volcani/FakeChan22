using System;
using System.Text.RegularExpressions;

namespace FakeChan22.Plugins
{
    public class JudgeTextLang
    {
        static Regex Vrir1 = new Regex(@"[0-9,\.+\- \t]");
        static Regex Vrir2 = new Regex(@"[a-zA-ZàÀèÈùÙéÉâÂêÊîÎûÛôÔäÄëËïÏüÜÿŸöÖñÑãÃõÕœŒçÇẞßءآأؤإئابةتثٹپجحخچدذڈڐرزژڑسشصضطظعغفقكکگلمنںهھۀہۂۃوىيیےۓ!""#$%&'\(\)\-=^~|\\`@\{\[\]\};+:*,./?]");

        private static double rate = 0.75;
        public static double Rate
        {
            get
            {
                return rate;
            }

            set
            {
                if ((value >= 0.0) && (value < 100))
                {
                    rate = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Rate");
                }
            }
        }

        public static (bool judge, double rate) JudgeNoJapanese(string text, double rate)
        {
            // 半角数値は日本語でも使うから消し込んでおく
            string s1 = Vrir1.Replace(text, "");

            // 長さゼロ、長さがゼロになった、場合は判定しない
            if (s1 == "") return (false, 0.0);

            string s1a = s1.Substring(0, 1).ToUpper();
            string s1b = s1a.ToLower();
            Regex Vrir1_la = new Regex(string.Format(@"^[{0}{1}]+[ \t]*$", s1a, s1b));

            // 同じ文字が連続している場合は判定をしない
            if (Vrir1_la.IsMatch(s1)) return (false, 0.0);

            MatchCollection s2 = Vrir2.Matches(s1);

            float le = s2.Count;
            float ri = s1.Length;

            // Vrir2で定義する文字がrateより多く含まれていれば非日本語とする
            return ( (le / ri)> Rate, (le / ri) );
        }

    }
}
