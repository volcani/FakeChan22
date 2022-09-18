using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FakeChan22
{
    /// <summary>
    /// EditListenerConfig.xaml の相互作用ロジック
    /// </summary>
    public partial class EditListenerConfig : Window
    {
        ListenerConfig LsnrCfg = null;
        List<SpeakerList> SpkrList = null;
        List<ReplaceDefinitionList> RepList = null;

        public EditListenerConfig(ref ListenerConfig lsnrCfg, ref List<SpeakerList> spkrLsts, ref List<ReplaceDefinitionList> repLsts)
        {
            LsnrCfg = lsnrCfg;
            SpkrList = spkrLsts;
            RepList = repLsts;

            InitializeComponent();

            ComboBoxSpeakerList.ItemsSource = null;
            ComboBoxSpeakerList.ItemsSource = SpkrList;

            ComboBoxSpeakerListNoJapanese.ItemsSource = null;
            ComboBoxSpeakerListNoJapanese.ItemsSource = SpkrList;

            ComboBoxReplaceTextList.ItemsSource = null;
            ComboBoxReplaceTextList.ItemsSource = RepList;

            ComboBoxReplaceTextListNoJapanese.ItemsSource = null;
            ComboBoxReplaceTextListNoJapanese.ItemsSource = RepList;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // 最初の表示の時だけSelectedItemが書き換わらない。暫定でSelectedIndexと併用

            ComboBoxSpeakerList.SelectedIndex = LsnrCfg.SpeakerListDefaultIndex;
            ComboBoxSpeakerList.SelectedItem = LsnrCfg.SpeakerListDefault;
            ComboBoxSpeakerListNoJapanese.SelectedIndex = LsnrCfg.SpeakerListNoJapaneseJudgeIndex;
            ComboBoxSpeakerListNoJapanese.SelectedItem = LsnrCfg.SpeakerListNoJapaneseJudge;
            ComboBoxReplaceTextList.SelectedIndex = LsnrCfg.ReplaceListDefaultIndex;
            ComboBoxReplaceTextList.SelectedItem = LsnrCfg.ReplaceListDefault;
            ComboBoxReplaceTextListNoJapanese.SelectedIndex = LsnrCfg.ReplaceListNoJapaneseJudgeIndex;
            ComboBoxReplaceTextListNoJapanese.SelectedItem = LsnrCfg.ReplaceListNoJapaneseJudge;

            CheckBoxIsAsync.IsChecked = LsnrCfg.IsAsync;
            CheckBoxIsEnable.IsChecked = LsnrCfg.IsEnable;
            CheckBoxIsRandom.IsChecked = LsnrCfg.IsRandom;
            CheckBoxIsNoJapanese.IsChecked = LsnrCfg.IsNoJapanese;

            TextBoxCharRate.Text = LsnrCfg.NoJapaneseCharRate.ToString();
            TextBoxServiceName.Text = LsnrCfg.ServiceName;

            LabelListenerName.Content = LsnrCfg.LabelName;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LsnrCfg.SpeakerListDefaultIndex = ComboBoxSpeakerList.SelectedIndex;
            LsnrCfg.SpeakerListDefault = ComboBoxSpeakerList.SelectedItem as SpeakerList;
            LsnrCfg.SpeakerListNoJapaneseJudgeIndex = ComboBoxSpeakerListNoJapanese.SelectedIndex;
            LsnrCfg.SpeakerListNoJapaneseJudge = ComboBoxSpeakerListNoJapanese.SelectedItem as SpeakerList;
            LsnrCfg.ReplaceListDefaultIndex = ComboBoxReplaceTextList.SelectedIndex;
            LsnrCfg.ReplaceListDefault = ComboBoxReplaceTextList.SelectedItem as ReplaceDefinitionList;
            LsnrCfg.ReplaceListNoJapaneseJudgeIndex = ComboBoxReplaceTextListNoJapanese.SelectedIndex;
            LsnrCfg.ReplaceListNoJapaneseJudge = ComboBoxReplaceTextListNoJapanese.SelectedItem as ReplaceDefinitionList;

            LsnrCfg.IsAsync = (bool)CheckBoxIsAsync.IsChecked;
            LsnrCfg.IsEnable = (bool)CheckBoxIsEnable.IsChecked;
            LsnrCfg.IsRandom = (bool)CheckBoxIsRandom.IsChecked;
            LsnrCfg.IsNoJapanese = (bool)CheckBoxIsNoJapanese.IsChecked;

            LsnrCfg.NoJapaneseCharRate = double.Parse(TextBoxCharRate.Text);
            LsnrCfg.ServiceName = TextBoxServiceName.Text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CheckBoxIsEnable_Click(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            LsnrCfg.IsEnable = (bool)cb.IsChecked;
        }

        private void CheckBoxIsAsync_Checked(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            LsnrCfg.IsAsync = (bool)cb.IsChecked;

        }

        private void CheckBoxIsRandom_Click(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            LsnrCfg.IsRandom = (bool)cb.IsChecked;
        }

        private void CheckBoxIsNoJapanese_Click(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            LsnrCfg.IsNoJapanese = (bool)cb.IsChecked;
        }

        private void TextBoxCharRate_LostFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;

            if (!Regex.IsMatch(tb.Text, @"^(\d{1,2}\.\d{1,2}|\d{1,2})$"))
            {
                tb.Text = LsnrCfg.NoJapaneseCharRate.ToString();
                return;
            }

            if (double.TryParse(tb.Text, out var rate))
            {
                if ((Math.Round(rate, 2) >= 0) && (Math.Round(rate, 2) <= 99.99))
                {
                    tb.Text = Math.Round(rate, 2).ToString();
                    LsnrCfg.NoJapaneseCharRate = Math.Round(rate, 2);
                }
            }
            else
            {
                tb.Text = LsnrCfg.NoJapaneseCharRate.ToString();
            }
        }

        private void ServiceName_LostFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;

            if (tb.Text == "")
            {
                tb.Text = LsnrCfg.ServiceName;
            }
            else
            {
                LsnrCfg.ServiceName = tb.Text;
            }
        }
    }
}
