using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DreamingApp
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static InkCanvas ink;
        public static MainWindow main;
        private void Application_Startup_1(object sender, StartupEventArgs e)
        {
            if (e.Args.Length > 0)
                main = new MainWindow(e.Args);
            else
                main = new MainWindow();

            main.Show();
        }
    }
}
