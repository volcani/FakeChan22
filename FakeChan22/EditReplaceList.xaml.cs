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
    /// EditReplaceList.xaml の相互作用ロジック
    /// </summary>
    public partial class EditReplaceList : Window
    {
        ReplaceDefinitionList Replaces = null;

        List<ReplaceDefinition> RepList = null;

        public EditReplaceList(ref ReplaceDefinitionList replaces)
        {
            Replaces = replaces;
            RepList = replaces.Definitions;

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataGridRepDefs.ItemsSource = null;
            DataGridRepDefs.ItemsSource = RepList;

            TextBoxListName.Text = Replaces.Listname;
            CheckBoxIsURLReplace.IsChecked = Replaces.IsReplaceUrl;
            TextBoxUrlRepStr.Text = Replaces.ReplaceStrFromUrl;

            CheckBoxTransGrassWords.IsChecked = Replaces.IsReplaceGrassWord;
            CheckBoxTransApplauseWords.IsChecked = Replaces.IsReplaceApplauseWord;

            CheckBoxTransEmoji.IsChecked = Replaces.IsReplaceEmoji;
            CheckBoxRemovalEmoji.IsChecked = Replaces.IsRemovalEmojiBeforeReplace;
            CheckBoxRemovalEmojiafter.IsChecked = Replaces.IsRemovalEmojiAfterReplace;

            CheckBoxTransZen2Han.IsChecked = Replaces.IsReplaceZentoHan1;
            CheckBoxTransZen2HanNum.IsChecked = Replaces.IsReplaceZentoHan2;

            TextBoxTextLength.Text = Replaces.CutLength.ToString();
            TextBoxAddSuffixStr.Text = Replaces.AppendStr;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!CloseCheck()) e.Cancel = true;
        }

        private void ButtonInsert_Click(object sender, RoutedEventArgs e)
        {
            DataGrid dg = DataGridRepDefs;
            var idx = dg.SelectedIndex;

            if (idx == -1)
            {
                RepList.Add(new ReplaceDefinition());
                idx = 0;
            }
            else
            {
                RepList.Insert(dg.SelectedIndex, new ReplaceDefinition());
            }

            dg.Items.Refresh();
            dg.SelectedIndex = idx;
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            DataGrid dg = DataGridRepDefs;
            var idx = dg.SelectedIndex;

            if (idx == -1) return;

            RepList.Remove(dg.SelectedItem as ReplaceDefinition);
            dg.Items.Refresh();
            dg.SelectedIndex = dg.Items.Count > idx + 1 ? idx : idx - 1;
        }

        private void ButtonMoveUp_Click(object sender, RoutedEventArgs e)
        {
            DataGrid dg = DataGridRepDefs;
            var idx = dg.SelectedIndex;

            if ((idx > 0)&&(RepList.Count > idx))
            {
                ReplaceDefinition x1 = dg.SelectedItem as ReplaceDefinition;
                ReplaceDefinition x2 = x1.Clone();

                RepList.Insert(idx - 1, x2);
                RepList.Remove(x1);

                dg.Items.Refresh();
                dg.SelectedItem = x2;
                dg.ScrollIntoView(dg.SelectedItem);
            }
        }

        private void ButtonMoveDn_Click(object sender, RoutedEventArgs e)
        {
            DataGrid dg = DataGridRepDefs;
            var idx = dg.SelectedIndex;

            if (dg.Items.Count > (idx + 1))
            {
                ReplaceDefinition x1 = dg.SelectedItem as ReplaceDefinition;
                ReplaceDefinition x2 = x1.Clone();

                RepList.Remove(x1);
                RepList.Insert(idx + 1, x2);

                dg.Items.Refresh();
                dg.SelectedItem = x2;
                dg.ScrollIntoView(dg.SelectedItem);
            }
        }

        private void TextBox_MaxTextSizePreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !new Regex(@"[0-9]").IsMatch(e.Text);
        }

        private void TextBox_MaxTextSizePreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (e.Command == ApplicationCommands.Paste)
            {
                e.Handled = !new Regex(@"[0-9]").IsMatch(tb.Text);
            }
        }

        private void TextBox_MaxTextSizePreviewTextInput2(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !new Regex(@"[0-9\.]").IsMatch(e.Text);
        }

        private void TextBox_MaxTextSizePreviewExecuted2(object sender, ExecutedRoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (e.Command == ApplicationCommands.Paste)
            {
                e.Handled = !new Regex(@"[0-9\.]").IsMatch(tb.Text);
            }
        }

        private void TextBoxTextLength_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            int len;
            if (int.TryParse(tb.Text, out len))
            {
                if (len < 1) len = 1;
            }
            else
            {
                len = 1;
            }

            Replaces.CutLength = len;
        }

        private void TextBoxTextLength_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;

            if ((tb.Text == "0") || (tb.Text == "")) tb.Text = "1";
        }

        private void TextBoxAddSuffixStr_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;

            Replaces.AppendStr = tb.Text;
        }

        private void CheckBoxIsURLReplace_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;

            Replaces.IsReplaceUrl = (bool)cb.IsChecked;
        }

        private void TextBoxUrlRepStr_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;

            Replaces.ReplaceStrFromUrl = tb.Text;
        }

        private void CheckBoxTransZen2Han_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;

            Replaces.IsReplaceZentoHan1 = (bool)cb.IsChecked;
        }

        private void CheckBoxTransZen2HanNum_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;

            Replaces.IsReplaceZentoHan2 = (bool)cb.IsChecked;
        }

        private void TextBoxListName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            try
            {
                tb.Background = Brushes.White;
                Replaces.Listname = tb.Text.Trim();
            }
            catch (Exception)
            {
                tb.Background = Brushes.LightPink;
            }
        }


        private bool CloseCheck()
        {
            if (TextBoxListName.Text == "")
            {
                MessageBox.Show("置換定義名が設定されていません", "置換定義編集");
                return false;
            }

            return true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TextBoxAfterStr.Text = ReplaceText.ParseText(TextBoxBeforeStr.Text, Replaces, true);
        }

        private void CheckBoxTransGrassWords_Click(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;

            Replaces.IsReplaceGrassWord = (bool)cb.IsChecked;
        }

        private void CheckBoxTransApplauseWords_Click(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;

            Replaces.IsReplaceApplauseWord = (bool)cb.IsChecked;
        }

        private void CheckBoxTransEmoji_Click(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;

            Replaces.IsReplaceEmoji = (bool)cb.IsChecked;
        }

        private void CheckBoxRemovalEmoji_Click(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;

            Replaces.IsRemovalEmojiBeforeReplace = (bool)cb.IsChecked;
        }

        private void CheckBoxRemovalEmojiafter_Click(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;

            Replaces.IsRemovalEmojiAfterReplace = (bool)cb.IsChecked;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
