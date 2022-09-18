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
    /// EditSpeakerList.xaml の相互作用ロジック
    /// </summary>
    public partial class EditSpeakerList : Window
    {
        List<Avator> avatorList = new List<Avator>();

        SpeakerList speakerList = null;

        ScAPIs api = null;

        public EditSpeakerList(ref SpeakerList spkrs)
        {
            speakerList = spkrs;

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            api = new ScAPIs();

            TextBoxListName.Text = speakerList.Listname;

            avatorList = api.AvatorList2().Where(v => v.Value["isalias"] == "False").Select(v=> new Avator() { Cid = v.Key, Name = v.Value["name"], ProdName=v.Value["prod"] }).ToList();
            ListBoxAvators.ItemsSource = null;
            ListBoxAvators.ItemsSource = avatorList;
            ListBoxAvators.DisplayMemberPath = "DispName";

            DataGridAvators.ItemsSource = null;
            DataGridAvators.ItemsSource = speakerList.Speakers;

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!CloseCheck()) e.Cancel = true;

            speakerList?.MakeValidObjects();
        }

        private void ButtonAddAll_Click(object sender, RoutedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            DataGrid dg = DataGridAvators as DataGrid;
            int idx = dg.SelectedIndex;

            if(idx==-1) idx=0;

            foreach (Avator av in avatorList)
            {
                Speaker sp = new Speaker() { Apply = true, Cid = av.Cid, MacroName = "", Name = av.Name, ProdName = av.ProdName, Effects = null, Emotions = null };
                var prm = api.GetDefaultParams2(av.Cid);
                var eft = prm["effect"].Select(v=>new AvatorParamSpec() { ParamName=v.Key, Value = v.Value["value"], Max_value = v.Value["max"], Min_value = v.Value["min"], Step = v.Value["step"] }).ToList();
                var emo = prm["emotion"].Select(v => new AvatorParamSpec() { ParamName = v.Key, Value = v.Value["value"], Max_value = v.Value["max"], Min_value = v.Value["min"], Step = v.Value["step"] }).ToList();

                sp.Effects = eft;
                sp.Emotions = emo;

                speakerList.Speakers.Insert(idx, sp);
                idx++;
            }

            dg.Items.Refresh();

            dg.SelectedIndex = dg.Items.Count - 1;
            dg.ScrollIntoView(dg.SelectedItem);
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            Avator av = ListBoxAvators.SelectedItem as Avator;

            if (av is null) return;

            DataGrid dg = DataGridAvators as DataGrid;
            Speaker sp = new Speaker() { Apply = true, Cid = av.Cid, MacroName = "", Name = av.Name, ProdName = av.ProdName, Effects = null, Emotions = null };
            int idx = dg.SelectedIndex;
            var prm = api.GetDefaultParams2(av.Cid);
            var eft = prm["effect"].Select(v => new AvatorParamSpec() { ParamName = v.Key, Value = v.Value["value"], Max_value = v.Value["max"], Min_value = v.Value["min"], Step = v.Value["step"] }).ToList();
            var emo = prm["emotion"].Select(v => new AvatorParamSpec() { ParamName = v.Key, Value = v.Value["value"], Max_value = v.Value["max"], Min_value = v.Value["min"], Step = v.Value["step"] }).ToList();

            sp.Effects = eft;
            sp.Emotions = emo;

            if (idx == -1) idx = 0;

            speakerList.Speakers.Insert(idx, sp);

            dg.Items.Refresh();

            dg.SelectedItem = sp;
            dg.ScrollIntoView(dg.SelectedItem);
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            DataGrid dg = DataGridAvators as DataGrid;
            int idx = dg.SelectedIndex;

            if (idx == -1) return;

            speakerList.Speakers.Remove(dg.SelectedItem as Speaker);

            dg.Items.Refresh();
            dg.SelectedIndex = dg.Items.Count > idx + 1 ? idx : idx - 1;
        }


        private void ButtonMoveUp_Click(object sender, RoutedEventArgs e)
        {
            DataGrid dg = DataGridAvators;
            int idx = dg.SelectedIndex;

            if ((idx > 0) && (speakerList.Speakers.Count > idx))
            {
                Speaker x1 = (dg.SelectedItem as Speaker);
                Speaker x2 = x1.Clone();

                speakerList.Speakers.Insert(idx - 1, x2);
                speakerList.Speakers.Remove(x1);

                dg.Items.Refresh();
                dg.SelectedItem = x2;
                dg.ScrollIntoView(dg.SelectedItem);
            }

        }

        private void ButtonMoveDn_Click(object sender, RoutedEventArgs e)
        {
            DataGrid dg = DataGridAvators;
            int idx = dg.SelectedIndex;

            if (speakerList.Speakers.Count > idx + 1)
            {
                Speaker x1 = (dg.SelectedItem as Speaker);
                Speaker x2 = x1.Clone();

                speakerList.Speakers.Remove(x1);
                speakerList.Speakers.Insert(idx + 1, x2);

                dg.Items.Refresh();
                dg.SelectedItem = x2;
                dg.ScrollIntoView(dg.SelectedItem);
            }
        }

        private void DataGridAvators_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var spkr = (sender as DataGrid).SelectedItem as Speaker;

            if (spkr is null) return;

            StackPanelEffectParams.Children.Clear();
            StackPanelEmotionParams.Children.Clear();

            MakeSliders(StackPanelEffectParams.Children, spkr.Effects);
            MakeSliders(StackPanelEmotionParams.Children, spkr.Emotions);
        }

        private void MakeSliders(UIElementCollection panel, List<AvatorParamSpec> @params)
        {
            foreach(var param in @params)
            {
                StackPanel sp = new StackPanel();
                Label lb = new Label();
                TextBlock tb = new TextBlock();

                sp.Orientation = Orientation.Vertical;
                sp.Margin = new Thickness(2, 0, 2, 2);
                sp.Background = new SolidColorBrush(Color.FromArgb(0xff, 253, 252, 227));

                // パラメタ名
                lb.HorizontalAlignment = HorizontalAlignment.Left;
                lb.Content = param.ParamName;

                // スライダー
                Slider sl = new Slider();
                sl.Name = param.ParamName;
                sl.Width = 100;
                sl.Minimum = Convert.ToDouble(param.Min_value);
                sl.Maximum = Convert.ToDouble(param.Max_value);
                sl.SelectionStart = Convert.ToDouble(param.Min_value);
                sl.SelectionEnd = Convert.ToDouble(param.Max_value);
                sl.Value = Convert.ToDouble(param.Value);
                sl.TickFrequency = Convert.ToDouble(param.Step);
                sl.LargeChange = Convert.ToDouble(param.Step);
                sl.SmallChange = Convert.ToDouble(param.Step);
                sl.IsSnapToTickEnabled = true;

                sl.ValueChanged += (sender, args) => {
                    param.Value = Convert.ToDecimal(sl.Value);
                };

                // 数値（表示）
                Binding myBinding = new Binding("Value");
                myBinding.Source = sl;
                tb.SetBinding(TextBlock.TextProperty, myBinding);
                tb.HorizontalAlignment = HorizontalAlignment.Center;

                sp.Children.Add(lb);
                sp.Children.Add(sl);
                sp.Children.Add(tb);

                panel.Add(sp);
            }
        }

        private void TextBoxListName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            try
            {
                tb.Background = Brushes.White;
                speakerList.Listname = tb.Text.Trim();
            }
            catch(Exception)
            {
                tb.Background = Brushes.LightPink;
            }
        }

        private bool CloseCheck()
        {
            if (TextBoxListName.Text == "")
            {
                MessageBox.Show("話者リスト名が設定されていません", "話者リスト編集");
                return false;
            }

            if (DataGridAvators.Items.Count == 0)
            {
                MessageBox.Show("話者リストに話者が登録されていません", "話者リスト編集");
                return false;
            }

            if (DataGridAvators.Items.Count == 0)
            {
                MessageBox.Show("話者リストに話者が登録されていません", "話者リスト編集");
                return false;
            }

            speakerList.MakeValidList();
            if (speakerList.ValidSpeakers.Count == 0)
            {
                MessageBox.Show("利用のチェックが入っている話者が最低1名必要です", "話者リスト編集");
                return false;
            }

            return true;
        }

        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {
            if (CloseCheck()) this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridAvators.SelectedIndex == -1) return;

            speakerList.MakeValidObjects();

            var (key, txt) = ReplaceText.SeparateMapKey(TextboxSampleText.Text);
            var sp = DataGridAvators.SelectedItem as Speaker;

            if ((key != "") && (speakerList.SpeakerMaps.ContainsKey(key)))
            {
                sp = speakerList.SpeakerMaps[key];
            }

            var eff = sp.Effects.ToDictionary(k => k.ParamName, v => v.Value);
            var emo = sp.Emotions.ToDictionary(k => k.ParamName, v => v.Value);

            api.Talk(sp.Cid, txt, "", eff, emo);
        }
    }
}
