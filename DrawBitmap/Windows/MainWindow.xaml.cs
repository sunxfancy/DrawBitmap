using DrawBitmap.MainClass;
using DrawBitmap.Windows;
using MulticastNetWork;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF.Themes;

namespace DrawBitmap
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            //初始化
            this.frame.Focus();
        }

        private void InitAppData()
        {
             if(App.ip=="")  //非命令行启动时，ip将为""（空串）
                 App.ip = "172.16.49.192";  //此处手动设置ip
                 //App.ip = "111.195.198.161";  //此处手动设置ip
            ThemeManager.ApplyTheme(App.Current, "BubbleCreme");
            ServerAPI.InitServerAPI();
        }

        private void mainwindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitAppData();
        }

    }

}
