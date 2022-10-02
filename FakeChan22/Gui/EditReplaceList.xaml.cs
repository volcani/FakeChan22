using FakeChan22.Filters;
using FakeChan22.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
//using System.Windows.Forms;
using System.Windows.Media;

namespace FakeChan22
{
    /// <summary>
    /// EditReplaceList.xaml の相互作用ロジック
    /// </summary>
    public partial class EditReplaceList : Window
    {
        ReplaceDefinitionList Replaces = null;

        public EditReplaceList(ref ReplaceDefinitionList replaces)
        {
            Replaces = replaces;

            InitializeComponent();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataGridRepProcs.ItemsSource = null;
            DataGridRepProcs.ItemsSource = Replaces.FilterProcs;

            TextBoxListName.Text = Replaces.Listname;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!CloseCheck()) e.Cancel = true;
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

        private void ButtonMoveProcUp_Click(object sender, RoutedEventArgs e)
        {
            var idx = DataGridRepProcs.SelectedIndex;

            if (idx == -1) return;

            if ((idx > 0) && (Replaces.FilterProcs.Count > idx))
            {
                FilterProcBase x1 = DataGridRepProcs.SelectedItem as FilterProcBase;
                FilterProcBase x2 = x1.Clone();

                Replaces.FilterProcs.Insert(idx - 1, x2);
                Replaces.FilterProcs.Remove(x1);

                DataGridRepProcs.Items.Refresh();
                DataGridRepProcs.SelectedItem = x2;
                DataGridRepProcs.ScrollIntoView(DataGridRepProcs.SelectedItem);
            }
        }

        private void ButtonMoveProcDn_Click(object sender, RoutedEventArgs e)
        {
            var idx = DataGridRepProcs.SelectedIndex;

            if (idx == -1) return;

            if (DataGridRepProcs.Items.Count > (idx + 1))
            {
                FilterProcBase x1 = DataGridRepProcs.SelectedItem as FilterProcBase;
                FilterProcBase x2 = x1.Clone();

                Replaces.FilterProcs.Remove(x1);
                Replaces.FilterProcs.Insert(idx + 1, x2);

                DataGridRepProcs.Items.Refresh();
                DataGridRepProcs.SelectedItem = x2;
                DataGridRepProcs.ScrollIntoView(DataGridRepProcs.SelectedItem);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TextBoxAfterStr.Text = ReplaceText.ParseText(TextBoxBeforeStr.Text, Replaces).Text;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DataGridRepProcs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dg = sender as DataGrid;
            dynamic item = dg.SelectedItem;

            if (dg.SelectedIndex == -1) return;

            LabelProcDescription.Content = item.FilterConfig.Description;
            ExtentPropertiesRendar();
        }

        private void ExtentPropertiesRendar()
        {
            if (DataGridRepProcs.SelectedIndex == -1) return;

            Grid grid = GridExtentSetting;
            dynamic cnvproc = DataGridRepProcs.SelectedItem;
            var FilterConfig = cnvproc.FilterConfig;
            var items = FilterConfig.GetType();
            var rowindex = 0;

            grid.Children.Clear();

            foreach (var item in items.GetProperties())
            {
                var propertyAttribute = GuiItemAttribute.Get(item);

                if (propertyAttribute == null) continue;

                grid.RowDefinitions.Add(new RowDefinition());

                UIElement component = null;

                if (item.PropertyType != typeof(List<ReplaceDefinition>))
                {
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
                }

                switch (item.GetValue(FilterConfig))
                {
                    case bool flag:
                        component = new CheckBox();
                        (component as CheckBox).IsChecked = flag;
                        (component as CheckBox).Content = propertyAttribute.Description;
                        (component as CheckBox).Margin = new Thickness(2, 2, 2, 2);
                        (component as CheckBox).VerticalAlignment = VerticalAlignment.Center;
                        (component as CheckBox).LostFocus += (object sender, RoutedEventArgs e) =>
                        {
                            var cb = sender as CheckBox;
                            item.SetValue(FilterConfig, (bool)cb.IsChecked);
                        };
                        grid.Children.Add(component);
                        Grid.SetColumn(component, 1);
                        Grid.SetRow(component, rowindex);
                        break;

                    case string str:
                        component = new TextBox();
                        (component as TextBox).Text = str;
                        (component as TextBox).Margin = new Thickness(2, 2, 2, 2);
                        (component as TextBox).VerticalAlignment = VerticalAlignment.Center;
                        (component as TextBox).LostFocus += (object sender, RoutedEventArgs e) =>
                        {
                            var tb = sender as TextBox;
                            item.SetValue(FilterConfig, tb.Text);
                        };
                        grid.Children.Add(component);
                        Grid.SetColumn(component, 1);
                        Grid.SetRow(component, rowindex);
                        break;

                    case int numInt:
                        component = new TextBox();
                        (component as TextBox).Text = numInt.ToString();
                        (component as TextBox).Margin = new Thickness(2, 2, 2, 2);
                        (component as TextBox).VerticalAlignment = VerticalAlignment.Center;
                        (component as TextBox).LostFocus += (object sender, RoutedEventArgs e) =>
                        {
                            var tb = sender as TextBox;
                            if (int.TryParse(tb.Text, out int i))
                            {
                                item.SetValue(FilterConfig, i);
                            }
                            else
                            {
                                tb.Undo();
                            }
                        };
                        grid.Children.Add(component);
                        Grid.SetColumn(component, 1);
                        Grid.SetRow(component, rowindex);
                        break;

                    case double numDouble:
                        component = new TextBox();
                        (component as TextBox).Text = numDouble.ToString();
                        (component as TextBox).Margin = new Thickness(2, 2, 2, 2);
                        (component as TextBox).VerticalAlignment = VerticalAlignment.Center;
                        (component as TextBox).LostFocus += (object sender, RoutedEventArgs e) =>
                        {
                            var tb = sender as TextBox;
                            if (double.TryParse(tb.Text, out double d))
                            {
                                item.SetValue(FilterConfig, d);
                            }
                            else
                            {
                                tb.Undo();
                            }
                        };
                        grid.Children.Add(component);
                        Grid.SetColumn(component, 1);
                        Grid.SetRow(component, rowindex);
                        break;

                    case List<ReplaceDefinition> replaceDefList:
                        component = CreateDefinitionEdit(ref replaceDefList);
                        grid.Children.Add(component);
                        Grid.SetColumn(component, 0);
                        Grid.SetColumnSpan(component, 3);
                        Grid.SetRow(component, rowindex);
                        break;

                }

                rowindex++;
            }

        }

        private UIElement CreateDefinitionEdit(ref List<ReplaceDefinition> repList)
        {
            Grid grid = new Grid();
            DataGrid dgrid = new DataGrid();
            List<ReplaceDefinition> RepList = repList;

            // グリッド配置

            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());

            // ボタン配置

            var ButtonInsert = new Button();
            var ButtonDelete = new Button();
            var ButtonMoveUp = new Button();
            var ButtonMoveDn = new Button();

            var ButtonImport = new Button();
            var ButtonExport = new Button();

            ButtonInsert.Margin = new Thickness(2, 2, 2, 2);
            ButtonInsert.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(ButtonInsert);
            Grid.SetColumn(ButtonInsert, 0);
            Grid.SetRow(ButtonInsert, 0);

            ButtonDelete.Margin = new Thickness(2, 2, 2, 2);
            ButtonDelete.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(ButtonDelete);
            Grid.SetColumn(ButtonDelete, 1);
            Grid.SetRow(ButtonDelete, 0);

            ButtonMoveUp.Margin = new Thickness(2, 2, 2, 2);
            ButtonMoveUp.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(ButtonMoveUp);
            Grid.SetColumn(ButtonMoveUp, 2);
            Grid.SetRow(ButtonMoveUp, 0);

            ButtonMoveDn.Margin = new Thickness(2, 2, 2, 2);
            ButtonMoveDn.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(ButtonMoveDn);
            Grid.SetColumn(ButtonMoveDn, 3);
            Grid.SetRow(ButtonMoveDn, 0);

            ButtonImport.Margin = new Thickness(2, 2, 2, 2);
            ButtonImport.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(ButtonImport);
            Grid.SetColumn(ButtonImport, 4);
            Grid.SetRow(ButtonImport, 0);

            ButtonExport.Margin = new Thickness(2, 2, 2, 2);
            ButtonExport.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(ButtonExport);
            Grid.SetColumn(ButtonExport, 5);
            Grid.SetRow(ButtonExport, 0);

            ButtonInsert.Content = "追加";
            ButtonDelete.Content = "削除";
            ButtonMoveUp.Content = "↑移動";
            ButtonMoveDn.Content = "↓移動";
            ButtonExport.Content = "エクスポート";
            ButtonImport.Content = "インポート";

            ButtonInsert.Click += (object sender, RoutedEventArgs e) => {
                var idx = dgrid.SelectedIndex;

                if (idx == -1)
                {
                    RepList.Add(new ReplaceDefinition());
                    idx = 0;
                }
                else
                {
                    RepList.Insert(dgrid.SelectedIndex, new ReplaceDefinition());
                }

                dgrid.Items.Refresh();
                dgrid.SelectedIndex = idx;
            };
            
            ButtonDelete.Click += (object sender, RoutedEventArgs e) => {
                var idx = dgrid.SelectedIndex;

                if (idx == -1) return;

                RepList.Remove(dgrid.SelectedItem as ReplaceDefinition);
                dgrid.Items.Refresh();
                dgrid.SelectedIndex = dgrid.Items.Count > idx + 1 ? idx : idx - 1;
            }; 
            
            ButtonMoveUp.Click += (object sender, RoutedEventArgs e) => {
                var idx = dgrid.SelectedIndex;

                if ((idx > 0) && (RepList.Count > idx))
                {
                    ReplaceDefinition x1 = dgrid.SelectedItem as ReplaceDefinition;
                    ReplaceDefinition x2 = x1.Clone();

                    RepList.Insert(idx - 1, x2);
                    RepList.Remove(x1);

                    dgrid.Items.Refresh();
                    dgrid.SelectedItem = x2;
                    dgrid.ScrollIntoView(dgrid.SelectedItem);
                }
            };

            ButtonMoveDn.Click += (object sender, RoutedEventArgs e) => {
                var idx = dgrid.SelectedIndex;

                if (idx == -1) return;

                if (dgrid.Items.Count > (idx + 1))
                {
                    ReplaceDefinition x1 = dgrid.SelectedItem as ReplaceDefinition;
                    ReplaceDefinition x2 = x1.Clone();

                    RepList.Remove(x1);
                    RepList.Insert(idx + 1, x2);

                    dgrid.Items.Refresh();
                    dgrid.SelectedItem = x2;
                    dgrid.ScrollIntoView(dgrid.SelectedItem);
                }
            };

            ButtonExport.Click += (object sender, RoutedEventArgs e) => {
                var dlg = new System.Windows.Forms.SaveFileDialog();

                dlg.Filter = "Jsonファイル(*.json)|*.json";
                dlg.FileName = "FakeChan22Replaces.json";
                dlg.FilterIndex = 0;
                dlg.Title = "書出す置換定義の保存ファイルを指定";
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(List<ReplaceDefinition>));
                        using (var fs = File.Open(dlg.FileName, FileMode.Create, FileAccess.Write))
                        using (var writer = JsonReaderWriterFactory.CreateJsonWriter(fs, Encoding.UTF8, true, true, "    "))
                        {
                            js.WriteObject(writer, RepList);
                        }
                    }
                    catch (Exception e1)
                    {
                        MessageBox.Show(string.Format("エラー: {0}", e1.Message));
                    }
                }
            };

            ButtonImport.Click += (object sender, RoutedEventArgs e) => {
                var dlg = new System.Windows.Forms.OpenFileDialog();

                dlg.Filter = "Jsonファイル(*.json)|*.json";
                dlg.FileName = "FakeChan22Replaces.json";
                dlg.FilterIndex = 0;
                dlg.Title = "取込む置換定義の保存ファイルを指定";

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(List<ReplaceDefinition>));
                        List<ReplaceDefinition> lst = new List<ReplaceDefinition>();
                        using (var fs = File.Open(dlg.FileName, FileMode.Open, FileAccess.Read))
                        {
                            lst = (List<ReplaceDefinition>)js.ReadObject(fs);
                        }
                        RepList.Clear();
                        RepList.AddRange(lst);
                        dgrid.ItemsSource = null;
                        dgrid.ItemsSource = RepList;
                        dgrid.SelectedIndex = 0;
                    }
                    catch (Exception e1)
                    {
                        MessageBox.Show(string.Format("エラー: {0}", e1.Message));
                    }
                }
            };

            // データグリッド配置

            dgrid.Margin = new Thickness(3, 3, 3, 3);
            dgrid.VerticalAlignment = VerticalAlignment.Stretch;
            dgrid.VerticalContentAlignment = VerticalAlignment.Center;
            dgrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            dgrid.AutoGenerateColumns = false;
            dgrid.IsManipulationEnabled = true;
            dgrid.AreRowDetailsFrozen = true;
            dgrid.CanUserReorderColumns = false;
            dgrid.CanUserResizeRows = false;
            dgrid.CanUserSortColumns = false;
            dgrid.RowHeaderWidth = 15;
            dgrid.HeadersVisibility = DataGridHeadersVisibility.Column;
            dgrid.CanUserAddRows = false;
            dgrid.CanUserDeleteRows = false;
            dgrid.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            dgrid.HorizontalScrollBarVisibility= ScrollBarVisibility.Disabled;
            dgrid.Height = 302;

            grid.Children.Add(dgrid);
            Grid.SetColumn(dgrid, 0);
            Grid.SetRow(dgrid, 1);
            Grid.SetColumnSpan(dgrid, 7);

            var c1 = new DataGridTemplateColumn();
            var c2 = new DataGridTextColumn();
            var c3 = new DataGridTextColumn();
            var c4 = new DataGridTextColumn();

            c1.Header = "適用";
            c1.CanUserResize = false;
            c1.CanUserReorder = false;
            c1.CellTemplate = new DataTemplate();

            var binding = new Binding();
            binding.Mode= BindingMode.TwoWay;
            binding.Path = new PropertyPath("IsUse");
            binding.UpdateSourceTrigger= UpdateSourceTrigger.PropertyChanged;

            // See https://tnakamura.hatenablog.com/entry/20081014/1225084186
            FrameworkElementFactory cbTemplate = new FrameworkElementFactory(typeof(CheckBox));
            cbTemplate.SetBinding(CheckBox.IsCheckedProperty, binding);
            cbTemplate.SetValue(CheckBox.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            cbTemplate.SetValue(CheckBox.VerticalAlignmentProperty, VerticalAlignment.Center);

            c1.CellTemplate.VisualTree = cbTemplate;

            c2.Header = "dummy";
            c2.Visibility = Visibility.Hidden;

            c3.Header = "マッチングパターン";
            c3.Width = new DataGridLength(45, DataGridLengthUnitType.Star);
            c3.Binding = new Binding("MatchingPattern");

            c4.Header = "置換内容";
            c4.Width = new DataGridLength(45, DataGridLengthUnitType.Star);
            c4.Binding = new Binding("ReplaceText");

            dgrid.Columns.Add(c1);
            dgrid.Columns.Add(c2);
            dgrid.Columns.Add(c3);
            dgrid.Columns.Add(c4);

            dgrid.ItemsSource = null;
            dgrid.ItemsSource = RepList;
            
            return grid;
        }

    }
}
