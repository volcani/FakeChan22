using FakeChan22.Tasks;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
//using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;

namespace FakeChan22
{
    /// <summary>
    /// EditListenerConfig.xaml の相互作用ロジック
    /// </summary>
    public partial class EditListenerConfig : Window
    {
        ListenerConfigBase LsnrCfg = null;
        List<SpeakerFakeChanList> SpkrList = null;
        List<ReplaceDefinitionList> RepList = null;

        public EditListenerConfig(ref ListenerConfigBase lsnrCfg, ref List<SpeakerFakeChanList> spkrLsts, ref List<ReplaceDefinitionList> repLsts)
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

            ExtentPropertiesRendar();
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
            LsnrCfg.SpeakerListDefault = ComboBoxSpeakerList.SelectedItem as SpeakerFakeChanList;
            LsnrCfg.SpeakerListNoJapaneseJudgeIndex = ComboBoxSpeakerListNoJapanese.SelectedIndex;
            LsnrCfg.SpeakerListNoJapaneseJudge = ComboBoxSpeakerListNoJapanese.SelectedItem as SpeakerFakeChanList;
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

        // 拡張プロパティの入力欄生成
        private void ExtentPropertiesRendar()
        {
            Grid grid = GridExtentSetting;
            bool labSwitch = false;
            var items = LsnrCfg.GetType();
            var rowindex = 0;

            foreach(var item in items.GetProperties())
            {
                var propertyAttribute = GuiItemAttribute.Get(item);

                if (propertyAttribute == null) continue;

                labSwitch = true;

                grid.RowDefinitions.Add(new RowDefinition());

                UIElement component = null;
                Label lbh = new Label();

                lbh.Content = propertyAttribute.ParamName;
                lbh.Margin = new Thickness(2, 2, 2, 2);
                lbh.VerticalAlignment = VerticalAlignment.Center;
                grid.Children.Add(lbh);
                Grid.SetColumn(lbh, 0);
                Grid.SetRow(lbh, rowindex);

                if (item.PropertyType != typeof(bool))
                {
                    Label lbt = new Label();
                    lbt.Content = propertyAttribute.Description;
                    lbt.Margin = new Thickness(2, 2, 2, 2);
                    lbt.VerticalAlignment = VerticalAlignment.Center;
                    grid.Children.Add(lbt);
                    Grid.SetColumn(lbt, 2);
                    Grid.SetRow(lbt, rowindex);
                }

                switch (item.GetValue(LsnrCfg))
                {
                    case bool flag:
                        component = new CheckBox();
                        (component as CheckBox).IsChecked = flag;
                        (component as CheckBox).Content = propertyAttribute.Description;
                        (component as CheckBox).Margin = new Thickness(2, 2, 2, 2);
                        (component as CheckBox).VerticalAlignment = VerticalAlignment.Center;
                        (component as CheckBox).LostFocus += (object sender, RoutedEventArgs e) => {
                            var cb = sender as CheckBox;
                            item.SetValue(LsnrCfg, (bool)cb.IsChecked);
                        };
                        break;

                    case string str:
                        component = new TextBox();
                        (component as TextBox).Text = str;
                        (component as TextBox).Margin = new Thickness(2, 2, 2, 2);
                        (component as TextBox).VerticalAlignment = VerticalAlignment.Center;
                        (component as TextBox).LostFocus += (object sender, RoutedEventArgs e) => {
                            var tb = sender as TextBox;
                            item.SetValue(LsnrCfg, tb.Text);
                        };
                        break;

                    case int numInt:
                        component = new TextBox();
                        (component as TextBox).Text = numInt.ToString();
                        (component as TextBox).Margin = new Thickness(2, 2, 2, 2);
                        (component as TextBox).VerticalAlignment = VerticalAlignment.Center;
                        (component as TextBox).LostFocus += (object sender, RoutedEventArgs e) => {
                            var tb = sender as TextBox;
                            if(int.TryParse(tb.Text, out int i))
                            {
                                item.SetValue(LsnrCfg, i);
                            }
                            else
                            {
                                tb.Undo();
                            }
                        };
                        break;

                    case double numDouble:
                        component = new TextBox();
                        (component as TextBox).Text = numDouble.ToString();
                        (component as TextBox).Margin = new Thickness(2, 2, 2, 2);
                        (component as TextBox).VerticalAlignment = VerticalAlignment.Center;
                        (component as TextBox).LostFocus += (object sender, RoutedEventArgs e) => {
                            var tb = sender as TextBox;
                            if (double.TryParse(tb.Text, out double d))
                            {
                                item.SetValue(LsnrCfg, d);
                            }
                            else
                            {
                                tb.Undo();
                            }
                        };
                        break;
                }

                grid.Children.Add(component);
                Grid.SetColumn(component, 1);
                Grid.SetRow(component, rowindex);

                rowindex++;
            }

            if (labSwitch) LabelExtentSetting.Content = "固有設定";

        }
    }
}
