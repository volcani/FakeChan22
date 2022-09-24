using FakeChan22.Params;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace FakeChan22
{
    /// <summary>
    /// EditSoloMessageList.xaml の相互作用ロジック
    /// </summary>
    public partial class EditSoloMessageList : Window
    {
        FakeChanConfig cfg = null;

        public EditSoloMessageList(ref FakeChanConfig config)
        {
            cfg = config;

            InitializeComponent();

            ComboBoxUseSpeakerList.ItemsSource = null;
            ComboBoxUseSpeakerList.ItemsSource = cfg.speakerLists;

            ListBoxSoloSpeechMessages.ItemsSource = null;
            ListBoxSoloSpeechMessages.ItemsSource = cfg.SoloSpeechList.SpeechDefinitions;
            ListBoxSoloSpeechMessages.Items.SortDescriptions.Add( new SortDescription("Key", ListSortDirection.Ascending));

            CheckBoxUseSoloSpeech.IsChecked = cfg.SoloSpeechList.IsUse;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //var target = (KeyValuePair<int, SoloSpeechDefinition>)ListBoxSoloSpeechMessages.SelectedItem;

            //ComboBoxUseSpeakerList.SelectedItem = target.Value.speakerList;
            //ComboBoxUseSpeakerList.Items.Refresh();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (cfg.SoloSpeechList.SpeechDefinitions.Count == 0) cfg.SoloSpeechList.IsUse = false;
        }

        private void TextBoxPastTime_LostFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            int time = -1;

            if (tb.Text == "") return;
            if (!int.TryParse(tb.Text, out time))
            {
                tb.Text = "";
                return;
            }

            if ((time < 60) || (time > 172800))
            {
                tb.Text = "";
                return;
            }
        }

        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            int time = -1;

            if (TextBoxPastTime.Text == "") return;

            if(int.TryParse(TextBoxPastTime.Text, out time))
            {
                if (!cfg.SoloSpeechList.SpeechDefinitions.ContainsKey(time))
                {
                    var sl = cfg.speakerLists[0];
                    cfg.SoloSpeechList.SpeechDefinitions.Add(time, new SoloSpeechDefinition(time, ref sl));
                    ListBoxSoloSpeechMessages.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("指定秒数の定義は既に存在しています", "呟き定義");
                }
            }

        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            int timeOld = -1;
            int timeNew = -1;

            if (TextBoxUpdatePastTime.Text == "") return;
            if (!int.TryParse(TextBoxUpdatePastTime.Text, out timeNew)) return;

            var target = (KeyValuePair<int, SoloSpeechDefinition>)ListBoxSoloSpeechMessages.SelectedItem;
            timeOld = target.Value.PastTime;

            if (!cfg.SoloSpeechList.SpeechDefinitions.ContainsKey(timeNew))
            {
                var item = cfg.SoloSpeechList.SpeechDefinitions[timeOld];

                item.PastTime = timeNew;

                cfg.SoloSpeechList.SpeechDefinitions.Add(timeNew, item);
                cfg.SoloSpeechList.SpeechDefinitions.Remove(timeOld);
                ListBoxSoloSpeechMessages.Items.Refresh();
            }
            else
            {
                MessageBox.Show("指定秒数の定義は既に存在するので変更できません", "呟き定義");
            }
        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxSoloSpeechMessages.SelectedIndex == -1) return;

            var target = (KeyValuePair<int, SoloSpeechDefinition>)ListBoxSoloSpeechMessages.SelectedItem;
            int idx = ListBoxSoloSpeechMessages.SelectedIndex;

            cfg.SoloSpeechList.SpeechDefinitions.Remove(target.Key);
            ListBoxSoloSpeechMessages.Items.Refresh();
            if (ListBoxSoloSpeechMessages.Items.Count == idx)
            {
                ListBoxSoloSpeechMessages.SelectedIndex = idx - 1;
            }
            else if (ListBoxSoloSpeechMessages.Items.Count > idx)
            {
                ListBoxSoloSpeechMessages.SelectedIndex = idx;
            }

            if(ListBoxSoloSpeechMessages.SelectedIndex == -1)
            {
                DataGridMessages.ItemsSource = null;
                ComboBoxUseSpeakerList.SelectedIndex = -1;
            }
        }

        private void ListBoxSoloSpeechMessages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lb = sender as ListBox;

            if (lb.SelectedIndex == -1) return;

            var target = (KeyValuePair<int, SoloSpeechDefinition>)lb.SelectedItem;

            DataGridMessages.ItemsSource = null;
            DataGridMessages.ItemsSource = target.Value.Messages;

            TextBoxUpdatePastTime.Text = target.Key.ToString();

            ComboBoxUseSpeakerList.SelectedItem = target.Value.speakerList;
            //ComboBoxUseSpeakerList.Items.Refresh();
        }

        private void ComboBoxUseSpeakerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ComboBox;

            if (cb.SelectedIndex == -1) return;
            if (ListBoxSoloSpeechMessages.SelectedIndex == -1) return;

            var target = (KeyValuePair<int, SoloSpeechDefinition>)ListBoxSoloSpeechMessages.SelectedItem;

            target.Value.speakerList = cb.SelectedItem as SpeakerList;

        }

        private void CheckBoxUseSoloSpeech_Click(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            cfg.SoloSpeechList.IsUse = (bool)cb.IsChecked;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
