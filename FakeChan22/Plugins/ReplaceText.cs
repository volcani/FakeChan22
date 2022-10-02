using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using EmojiTools;
using FakeChan22.Filters;

namespace FakeChan22.Plugins
{
    public class ReplaceText
    {
        /// <summary>
        /// 置換リストを使ってテキストをパースする
        /// </summary>
        /// <param name="text">処理対象テキスト</param>
        /// <param name="repDefs">処理に使用する置換リスト</param>
        /// <returns>変換結果テキスト</returns>
        public static FilterParams ParseText(string text, ReplaceDefinitionList repDefs)
        {
            FilterParams fp = new FilterParams();

            fp.Text = text;

            foreach (var proc in repDefs.FilterProcs)
            {
                dynamic convproc = proc;
                if(convproc.FilterConfig.IsUse) proc.Processing(ref fp);
            }

            return fp;
        }

        /// <summary>
        /// テキスト先頭の話者指定を分離する
        /// </summary>
        /// <param name="text">処理対象テキスト</param>
        /// <returns>(話者, 分離したテキスト)</returns>
        public static (string key, string splittedText) SplitUserSpecifier(string text,  bool ext = false)
        {
            string UserSpecifier = "";
            string Text = text;

            if (Regex.IsMatch(text, @"^([\da-zA-Z]{1,2})\).*$"))
            {
                UserSpecifier = Regex.Replace(text, @"^([\da-zA-Z]{1,2})\).*$", "$1");
                Text = Regex.Replace(text, @"^[\da-zA-Z]{1,2}\)(.*)$", "$1");
            }

            return (UserSpecifier, Text);
        }

        /// <summary>
        /// テキスト先頭の話者指定を分離する
        /// </summary>
        /// <param name="text">処理対象テキスト</param>
        /// <returns>(話者, 分離したテキスト)</returns>
        public static (string key, string splittedText) SplitUserSpecifierFullwide(string text, bool ext = false)
        {
            string UserSpecifier = "";
            string Text = text;

            if (Regex.IsMatch(text, @"^([0-9０-９a-zA-Zａ-ｚＡ-Ｚ]{1,2})[\)）].*$"))
            {
                UserSpecifier = Regex.Replace(text, @"^([0-9０-９a-zA-Zａ-ｚＡ-Ｚ]{1,2})[\)）].*$", "$1");
                Text = Regex.Replace(text, @"^[0-9０-９a-zA-Zａ-ｚＡ-Ｚ]{1,2}[\)）](.*)$", "$1");
            }

            return (UserSpecifier, Text);
        }

        public static string Zen2HanChar(string text)
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

        public static string Zen2HanNumChar(string text)
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
