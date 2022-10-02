using FakeChan22.Filters;
using FakeChan22.Plugins;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
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
        List<SpeakerAssistantSeika> avatorList = new List<SpeakerAssistantSeika>();

        SpeakerFakeChanList speakerList = null;

        ScAPIs api = null;

        public EditSpeakerList(ref SpeakerFakeChanList spkrs)
        {
            speakerList = spkrs;

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            api = new ScAPIs();

            TextBoxListName.Text = speakerList.Listname;

            avatorList = api.AvatorList2().Where(v => v.Value["isalias"] == "False").Select(v=> new SpeakerAssistantSeika() { Cid = v.Key, Name = v.Value["name"], ProdName=v.Value["prod"] }).ToList();
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

            foreach (SpeakerAssistantSeika av in avatorList)
            {
                SpeakerFakeChan sp = new SpeakerFakeChan() { Apply = true, Cid = av.Cid, MacroName = "", Name = av.Name, ProdName = av.ProdName, Effects = null, Emotions = null };
                var prm = api.GetDefaultParams2(av.Cid);
                var eft = prm["effect"].Select(v=>new SpeakerAssistantSeikaParamSpec() { ParamName=v.Key, Value = v.Value["value"], Max_value = v.Value["max"], Min_value = v.Value["min"], Step = v.Value["step"] }).ToList();
                var emo = prm["emotion"].Select(v => new SpeakerAssistantSeikaParamSpec() { ParamName = v.Key, Value = v.Value["value"], Max_value = v.Value["max"], Min_value = v.Value["min"], Step = v.Value["step"] }).ToList();

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
            SpeakerAssistantSeika av = ListBoxAvators.SelectedItem as SpeakerAssistantSeika;

            if (av is null) return;

            DataGrid dg = DataGridAvators as DataGrid;
            SpeakerFakeChan sp = new SpeakerFakeChan() { Apply = true, Cid = av.Cid, MacroName = "", Name = av.Name, ProdName = av.ProdName, Effects = null, Emotions = null };
            int idx = dg.SelectedIndex;
            var prm = api.GetDefaultParams2(av.Cid);
            var eft = prm["effect"].Select(v => new SpeakerAssistantSeikaParamSpec() { ParamName = v.Key, Value = v.Value["value"], Max_value = v.Value["max"], Min_value = v.Value["min"], Step = v.Value["step"] }).ToList();
            var emo = prm["emotion"].Select(v => new SpeakerAssistantSeikaParamSpec() { ParamName = v.Key, Value = v.Value["value"], Max_value = v.Value["max"], Min_value = v.Value["min"], Step = v.Value["step"] }).ToList();

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

            speakerList.Speakers.Remove(dg.SelectedItem as SpeakerFakeChan);

            dg.Items.Refresh();
            dg.SelectedIndex = dg.Items.Count > idx + 1 ? idx : idx - 1;
        }


        private void ButtonMoveUp_Click(object sender, RoutedEventArgs e)
        {
            DataGrid dg = DataGridAvators;
            int idx = dg.SelectedIndex;

            if ((idx > 0) && (speakerList.Speakers.Count > idx))
            {
                SpeakerFakeChan x1 = (dg.SelectedItem as SpeakerFakeChan);
                SpeakerFakeChan x2 = x1.Clone();

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
                SpeakerFakeChan x1 = (dg.SelectedItem as SpeakerFakeChan);
                SpeakerFakeChan x2 = x1.Clone();

                speakerList.Speakers.Remove(x1);
                speakerList.Speakers.Insert(idx + 1, x2);

                dg.Items.Refresh();
                dg.SelectedItem = x2;
                dg.ScrollIntoView(dg.SelectedItem);
            }
        }

        private void DataGridAvators_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var spkr = (sender as DataGrid).SelectedItem as SpeakerFakeChan;

            if (spkr is null) return;

            StackPanelEffectParams.Children.Clear();
            StackPanelEmotionParams.Children.Clear();

            MakeSliders(StackPanelEffectParams.Children, spkr.Effects);
            MakeSliders(StackPanelEmotionParams.Children, spkr.Emotions);
        }

        private void MakeSliders(UIElementCollection panel, List<SpeakerAssistantSeikaParamSpec> @params)
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

        private void ButtonImport_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new System.Windows.Forms.OpenFileDialog();

            dlg.Filter = "Jsonファイル(*.json)|*.json";
            dlg.FileName = "FakeChan22Speakers.json";
            dlg.FilterIndex = 0;
            dlg.Title = "取込む話者リストの保存ファイルを指定";

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(List<SpeakerFakeChan>));
                    List<SpeakerFakeChan> lst = new List<SpeakerFakeChan>();
                    using (var fs = File.Open(dlg.FileName, FileMode.Open, FileAccess.Read))
                    {
                        lst = (List<SpeakerFakeChan>)js.ReadObject(fs);
                    }
                    speakerList.Speakers = lst;
                    DataGridAvators.ItemsSource = null;
                    DataGridAvators.ItemsSource = speakerList.Speakers;
                    DataGridAvators.SelectedIndex = 0;
                    speakerList.MakeValidObjects();
                }
                catch (Exception e1)
                {
                    MessageBox.Show(string.Format("エラー: {0}", e1.Message));
                }
            }
        }

        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new System.Windows.Forms.SaveFileDialog();

            dlg.Filter = "Jsonファイル(*.json)|*.json";
            dlg.FileName = "FakeChan22Speakers.json";
            dlg.FilterIndex = 0;
            dlg.Title = "書出す話者リストの保存ファイルを指定";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(List<SpeakerFakeChan>));
                    using (var fs = File.Open(dlg.FileName, FileMode.Create, FileAccess.Write))
                    using (var writer = JsonReaderWriterFactory.CreateJsonWriter(fs, Encoding.UTF8, true, true, "    "))
                    {
                        js.WriteObject(writer, speakerList.Speakers);
                    }
                }
                catch (Exception e1)
                {
                    MessageBox.Show(string.Format("エラー: {0}", e1.Message));
                }
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (CloseCheck()) this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridAvators.SelectedIndex == -1) return;

            speakerList.MakeValidObjects();

            var (key, text) =  ReplaceText.SplitUserSpecifier(TextboxSampleText.Text);

            var sp = DataGridAvators.SelectedItem as SpeakerFakeChan;

            if ((key != "") && speakerList.SpeakerMaps.ContainsKey(key))
            {
                sp = speakerList.SpeakerMaps[key];
            }

            var eff = sp.Effects.ToDictionary(k => k.ParamName, v => v.Value);
            var emo = sp.Emotions.ToDictionary(k => k.ParamName, v => v.Value);

            api.Talk(sp.Cid, text, "", eff, emo);
        }

    }
}
