using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FakeChan22
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        static Mutex mutex = new Mutex(false, "echoseika.fakechan22");

        [STAThread]
        public static void Main()
        {
            if (!mutex.WaitOne(0, false))
            {
                mutex.Close();
                mutex = null;
                return;
            }

            App app = new App();
            app.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
            app.InitializeComponent();
            app.Run();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //if (!mutex.WaitOne(0, false))
            //{
            //    mutex.Close();
            //    mutex = null;
            //    this.Shutdown();
            //}
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (mutex != null)
            {
                mutex.ReleaseMutex();
                mutex.Close();
                mutex = null;
            }
        }

    }
}
