using FakeChan22.Params;
using System;
using System.Windows;
using System.Windows.Controls;

namespace FakeChan22
{
    /// <summary>
    /// EditQueueControll.xaml の相互作用ロジック
    /// </summary>
    public partial class EditQueueControll : Window
    {
        MessageQueueParam queueParam;

        public EditQueueControll(ref MessageQueueParam cueuePms)
        {
            queueParam = cueuePms;
            this.DataContext = cueuePms;

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LabelMode0.Content = queueParam.Mode0QueueLimit;
            TextBoxMode1.Text = queueParam.Mode1QueueLimit.ToString();
            TextBoxMode2.Text = queueParam.Mode2QueueLimit.ToString();
            TextBoxMode3.Text = queueParam.Mode3QueueLimit.ToString();
            TextBoxMode4.Text = queueParam.Mode4QueueLimit.ToString();
            TextBoxMode5.Text = queueParam.Mode5QueueLimit.ToString();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // 今はまだ処理無し
        }

        private void TextBoxMode5_LostFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            int bklimit = queueParam.Mode5QueueLimit;

            try
            {
                int limit = int.Parse(tb.Text);
                if (limit > queueParam.Mode4QueueLimit)
                {
                    queueParam.Mode5QueueLimit = limit;
                }
            }
            catch (Exception)
            {
                queueParam.Mode5QueueLimit = bklimit;
            }

            tb.Text = queueParam.Mode5QueueLimit.ToString();
        }

        private void TextBoxMode4_LostFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            int bklimit = queueParam.Mode4QueueLimit;

            try
            {
                int limit = int.Parse(tb.Text);

                if ((limit > queueParam.Mode3QueueLimit) && (limit < queueParam.Mode5QueueLimit))
                {
                    queueParam.Mode4QueueLimit = limit;
                }
            }
            catch (Exception)
            {
                queueParam.Mode4QueueLimit = bklimit;
            }

            tb.Text = queueParam.Mode4QueueLimit.ToString();
        }

        private void TextBoxMode3_LostFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            int bklimit = queueParam.Mode3QueueLimit;

            try
            {
                int limit = int.Parse(tb.Text);

                if ((limit > queueParam.Mode2QueueLimit) && (limit < queueParam.Mode4QueueLimit))
                {
                    queueParam.Mode3QueueLimit = limit;
                }
            }
            catch (Exception)
            {
                queueParam.Mode3QueueLimit = bklimit;
            }

            tb.Text = queueParam.Mode3QueueLimit.ToString();
        }

        private void TextBoxMode2_LostFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            int bklimit = queueParam.Mode2QueueLimit;

            try
            {
                int limit = int.Parse(tb.Text);

                if ((limit > queueParam.Mode1QueueLimit) && (limit < queueParam.Mode3QueueLimit))
                {
                    queueParam.Mode2QueueLimit = limit;
                }
            }
            catch (Exception)
            {
                queueParam.Mode2QueueLimit = bklimit;
            }

            tb.Text = queueParam.Mode2QueueLimit.ToString();
        }

        private void TextBoxMode1_LostFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            int bklimit = queueParam.Mode1QueueLimit;

            try
            {
                int limit = int.Parse(tb.Text);

                if (limit < queueParam.Mode2QueueLimit)
                {
                    queueParam.Mode1QueueLimit = limit;
                }

            }
            catch (Exception)
            {
                queueParam.Mode1QueueLimit = bklimit;
            }

            tb.Text = queueParam.Mode1QueueLimit.ToString();

            queueParam.Mode0QueueLimit = queueParam.Mode1QueueLimit - 1;

            LabelMode0.Content = queueParam.Mode0QueueLimit;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
