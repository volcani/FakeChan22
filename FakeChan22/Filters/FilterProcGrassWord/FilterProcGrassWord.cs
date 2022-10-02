using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace FakeChan22.Filters
{
    [DataContract]
    public class FilterProcGrassWord : FilterProcBase
    {
        static Regex RepGrass01 = new Regex(@"^[WwＷｗ]{2,}$");
        static Regex RepGrass02 = new Regex(@"^[WwＷｗ]{2,}([^a-zA-Zａ-ｚＡ-Ｚ]{1,})");
        static Regex RepGrass03 = new Regex(@"([^a-zA-Zａ-ｚＡ-Ｚ]{1,})[WwＷｗ]{2,}([^a-zA-Zａ-ｚＡ-Ｚ]{1,})");
        static Regex RepGrass04 = new Regex(@"([^a-zA-Zａ-ｚＡ-Ｚ]{1,})[WwＷｗ]{2,}$");
        static Regex RepGrass11 = new Regex(@"^[WwＷｗ]{1,1}$");
        static Regex RepGrass12 = new Regex(@"^[WwＷｗ]{1,1}([^a-zA-Zａ-ｚＡ-Ｚ]{1,})");
        static Regex RepGrass13 = new Regex(@"([^a-zA-Zａ-ｚＡ-Ｚ]{1,})[WwＷｗ]{1,1}([^a-zA-Zａ-ｚＡ-Ｚ]{1,})");
        static Regex RepGrass14 = new Regex(@"([^a-zA-Zａ-ｚＡ-Ｚ]{1,})[WwＷｗ]{1,1}$");

        static string Words01w = @"わらわら";
        static string Words02w = @"わらわら $1";
        static string Words03w = @"$1 わらわら $2";
        static string Words04w = @"$1 わらわら";
        static string Words11w = @"わら";
        static string Words12w = @"わら $1";
        static string Words13w = @"$1 わら $2";
        static string Words14w = @"$1 わら";

        [DataMember] public string LabelName { get; set; } = "ｗ 変換";

        [DataMember] public FilterConfigGrassWord FilterConfig { get; set; }

        public FilterProcGrassWord(ref FilterConfigGrassWord filterCfg)
        {
            FilterConfig = filterCfg;
        }

        public override void Processing(ref FilterParams fp)
        {
            fp.Text = RepGrass04.Replace(RepGrass03.Replace(RepGrass02.Replace(RepGrass01.Replace(fp.Text, Words01w), Words02w), Words03w), Words04w);
            fp.Text = RepGrass14.Replace(RepGrass13.Replace(RepGrass12.Replace(RepGrass11.Replace(fp.Text, Words11w), Words12w), Words13w), Words14w);
        }
    }
}
