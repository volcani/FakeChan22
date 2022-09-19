using System;
using System.Collections.Generic;
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

namespace FakeChan22
{
    /// <summary>
    /// EditQueueControll.xaml の相互作用ロジック
    /// </summary>
    public partial class EditQueueControll : Window
    {
        QueueParam queueParam;

        public EditQueueControll(ref QueueParam cueuePms)
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
            int bklimit = queueParam.Mode4QueueLimit;

            try
            {
                queueParam.Mode5QueueLimit = int.Parse(tb.Text);
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

                if (limit > queueParam.Mode3QueueLimit)
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
