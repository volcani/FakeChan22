using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EmojiTools;

namespace FakeChan22
{
    public class ReplaceText
    {
        static Regex RepUrlReg = new Regex(@"[hH]{0,1}[tT][tT][pP][sS]{0,1}:\/\/[^\t 　]{1,}");
        static Regex RepMapReg = new Regex(@"^[\da-zA-Z]{1,2}\)");

        static Regex RepGrass01 = new Regex(@"^[WwＷｗ]{2,}$");
        static Regex RepGrass02 = new Regex(@"^[WwＷｗ]{2,}([^a-zA-Zａ-ｚＡ-Ｚ]{1,})");
        static Regex RepGrass03 = new Regex(@"([^a-zA-Zａ-ｚＡ-Ｚ]{1,})[WwＷｗ]{2,}([^a-zA-Zａ-ｚＡ-Ｚ]{1,})");
        static Regex RepGrass04 = new Regex(@"([^a-zA-Zａ-ｚＡ-Ｚ]{1,})[WwＷｗ]{2,}$");

        static Regex RepGrass11 = new Regex(@"^[WwＷｗ]{1,1}$");
        static Regex RepGrass12 = new Regex(@"^[WwＷｗ]{1,1}([^a-zA-Zａ-ｚＡ-Ｚ]{1,})");
        static Regex RepGrass13 = new Regex(@"([^a-zA-Zａ-ｚＡ-Ｚ]{1,})[WwＷｗ]{1,1}([^a-zA-Zａ-ｚＡ-Ｚ]{1,})");
        static Regex RepGrass14 = new Regex(@"([^a-zA-Zａ-ｚＡ-Ｚ]{1,})[WwＷｗ]{1,1}$");

        static Regex RepApplause01 = new Regex(@"^[8８]{3,}$");
        static Regex RepApplause02 = new Regex(@"^[8８]{3,}([^0-9０-９]{1,})");
        static Regex RepApplause03 = new Regex(@"([^0-9０-９]{1,})[8８]{3,}([^0-9０-９]{1,})");
        static Regex RepApplause04 = new Regex(@"([^0-9０-９]{1,})[8８]{3,}");

        static string Words01w = @"わらわら";
        static string Words02w = @"わらわら $1";
        static string Words03w = @"$1 わらわら $2";
        static string Words04w = @"$1 わらわら";
        static string Words11w = @"わら";
        static string Words12w = @"わら $1";
        static string Words13w = @"$1 わら $2";
        static string Words14w = @"$1 わら";

        static string Words01p = @"パチパチパチ";
        static string Words02p = @"パチパチパチ $1";
        static string Words03p = @"$1 パチパチパチ $2";
        static string Words04p = @"$1 パチパチパチ";


        public static string ParseText(string text, ReplaceDefinitionList defs, bool demomode=false)
        {
            string s = text;

            // demo mode

            if (demomode)
            {
                s = RepMapReg.Replace(s, "");
            }

            // before

            if (defs.IsReplaceUrl)
            {
                s = RepUrlReg.Replace(s, defs.ReplaceStrFromUrl);
            }

            if(defs.IsReplaceGrassWord)
            {
                s = ReplaceGrass(s);
            }

            if (defs.IsReplaceApplauseWord)
            {
                s = ReplaceApplause(s);
            }

            if (defs.IsReplaceEmoji)
            {
                s = EmojiTool.ChangeEmoji(s);
            }

            if (defs.IsRemovalEmojiAfterReplace)
            {
                s = EmojiTool.StripEmoji(s);
            }

            if (defs.IsReplaceZentoHan1)
            {
                s = TransZen2HanString(s);
            }

            if (defs.IsReplaceZentoHan2)
            {
                s = TransZen2HanNumString(s);
            }

            //replace

            if (s.Length != 0)
            {
                for (int idx = 0; idx < defs.Definitions.Count; idx++)
                {
                    if (defs.Definitions[idx].Apply)
                    {
                        try { s = Regex.Replace(s, defs.Definitions[idx].MatchingPattern, defs.Definitions[idx].ReplaceText); } catch (Exception) { }
                    }
                }
            }

            //after

            if (s.Length > defs.CutLength)
            {
                s = s.Substring(0, defs.CutLength) + defs.AppendStr;
            }

            return s;
        }

        public static (string key, string text) SeparateMapKey(string text)
        {
            string key = "";
            string txt = text;

            if (Regex.IsMatch(txt, @"^([\da-zA-Z]{1,2})\).*$"))
            {
                key = Regex.Replace(txt, @"^([\da-zA-Z]{1,2})\).*$", "$1");
                txt = Regex.Replace(txt, @"^[\da-zA-Z]{1,2}\)(.*)$", "$1");
            }

            return (key, txt);
        }

        private static string ReplaceGrass(string text)
        {
            string s = RepGrass04.Replace(RepGrass03.Replace(RepGrass02.Replace(RepGrass01.Replace(text, Words01w), Words02w), Words03w), Words04w);
            s = RepGrass14.Replace(RepGrass13.Replace(RepGrass12.Replace(RepGrass11.Replace(s, Words11w), Words12w), Words13w), Words14w);

            return s;
        }

        private static string ReplaceApplause(string text)
        {
            return RepApplause04.Replace(RepApplause03.Replace(RepApplause02.Replace(RepApplause01.Replace(text, Words01p), Words02p), Words03p), Words04p); ;
        }


        private static string TransZen2HanString(string text)
        {
            StringBuilder sb = new StringBuilder();

            sb.Clear();
            foreach (var item in text.ToCharArray())
            {
                if (Zen2Han.ContainsKey(item))
                {
                    sb.Append(Zen2Han[item]);
                }
                else
                {
                    sb.Append(item);
                }
            }

            return sb.ToString();
        }

        private static string TransZen2HanNumString(string text)
        {
            StringBuilder sb = new StringBuilder();

            sb.Clear();
            foreach (var item in text.ToCharArray())
            {
                if (Zen2HanNum.ContainsKey(item))
                {
                    sb.Append(Zen2HanNum[item]);
                }
                else
                {
                    sb.Append(item);
                }
            }

            return sb.ToString();
        }


        private static readonly Dictionary<char, char> Zen2Han = new Dictionary<char, char>()
        {
            {'！',     '!'},
            {'”',     '"'},
            {'＃',     '#'},
            {'＄',     '$'},
            {'％',     '%'},
            {'＆',     '&'},
            {'’',     '\''},
            {'（',     '('},
            {'）',     ')'},
            {'＊',     '*'},
            {'＋',     '+'},
            {'，',     ','},
            {'－',     '-'},
            {'．',     '.'},
            {'／',     '/'},
            {'：',     ':'},
            {'；',     ';'},
            {'＜',     '<'},
            {'＝',     '='},
            {'＞',     '>'},
            {'？',     '?'},
            {'＠',     '@'},
            {'Ａ',     'A'},
            {'Ｂ',     'B'},
            {'Ｃ',     'C'},
            {'Ｄ',     'D'},
            {'Ｅ',     'E'},
            {'Ｆ',     'F'},
            {'Ｇ',     'G'},
            {'Ｈ',     'H'},
            {'Ｉ',     'I'},
            {'Ｊ',     'J'},
            {'Ｋ',     'K'},
            {'Ｌ',     'L'},
            {'Ｍ',     'M'},
            {'Ｎ',     'N'},
            {'Ｏ',     'O'},
            {'Ｐ',     'P'},
            {'Ｑ',     'Q'},
            {'Ｒ',     'R'},
            {'Ｓ',     'S'},
            {'Ｔ',     'T'},
            {'Ｕ',     'U'},
            {'Ｖ',     'V'},
            {'Ｗ',     'W'},
            {'Ｘ',     'X'},
            {'Ｙ',     'Y'},
            {'Ｚ',     'Z'},
            {'［',     '['},
            {'￥',     '\\'},
            {'］',     ']'},
            {'＾',     '^'},
            {'＿',     '_'},
            {'｀',     '`'},
            {'ａ',     'a'},
            {'ｂ',     'b'},
            {'ｃ',     'c'},
            {'ｄ',     'd'},
            {'ｅ',     'e'},
            {'ｆ',     'f'},
            {'ｇ',     'g'},
            {'ｈ',     'h'},
            {'ｉ',     'i'},
            {'ｊ',     'j'},
            {'ｋ',     'k'},
            {'ｌ',     'l'},
            {'ｍ',     'm'},
            {'ｎ',     'n'},
            {'ｏ',     'o'},
            {'ｐ',     'p'},
            {'ｑ',     'q'},
            {'ｒ',     'r'},
            {'ｓ',     's'},
            {'ｔ',     't'},
            {'ｕ',     'u'},
            {'ｖ',     'v'},
            {'ｗ',     'w'},
            {'ｘ',     'x'},
            {'ｙ',     'y'},
            {'ｚ',     'z'},
            {'｛',     '{'},
            {'｜',     '|'},
            {'｝',     '}'},
            {'～',     '~'}
        };

        private static readonly Dictionary<char, char> Zen2HanNum = new Dictionary<char, char>()
        {
            {'０',     '0'},
            {'１',     '1'},
            {'２',     '2'},
            {'３',     '3'},
            {'４',     '4'},
            {'５',     '5'},
            {'６',     '6'},
            {'７',     '7'},
            {'８',     '8'},
            {'９',     '9'}
        };

    }
}
