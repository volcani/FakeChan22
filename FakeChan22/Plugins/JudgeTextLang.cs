using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FakeChan22
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
            string s1 = Vrir1.Replace(text, "");

            if (s1 == "") return (false, 0.0);

            string s1a = s1.Substring(0, 1).ToUpper();
            string s1b = s1a.ToLower();

            Regex Vrir1_la = new Regex(string.Format(@"^[{0}{1}]+[ \t]*$", s1a, s1b));

            if (Vrir1_la.IsMatch(s1)) return (false, 0.0);

            MatchCollection s2 = Vrir2.Matches(s1);

            float le = s2.Count;
            float ri = s1.Length;

            return ( (le / ri)> Rate, (le / ri) );
        }

    }
}
