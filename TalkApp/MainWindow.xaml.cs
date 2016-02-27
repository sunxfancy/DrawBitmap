using Awesomium.Core;
using ColorCode;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace TalkApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private string[] StartupArgs;
        /// <summary>
        /// 目标主机ip
        /// </summary>
        private string ip;

        /// <summary>
        /// 目标主机端口号
        /// </summary>
        private int port = 9997;

        /// <summary>
        /// 判断是否为服务器，1为服务器，-1为客户端，0为单机模式
        /// </summary>
        public int isServer = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(string[] p)
        {
            // TODO: Complete member initialization
            this.StartupArgs = p;
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            MainData.text_show = Show;
           /* MainData.websession.DataPath = @".\Cache";
            var p = new WebPreferences();
            p.CanScriptsAccessClipboard = true;
            p.EnableGPUAcceleration = true;
            p.SmoothScrolling = true;
            p.WebGL = true;
            p.ShrinkStandaloneImagesToFit = false;
            MainData.websession.Preferences = p;*/
            ///test
            if (StartupArgs == null)
            {
                StartupArgs = new string[3];
                StartupArgs[0] = "Server";
                StartupArgs[1] = "sxf";
                StartupArgs[2] = "127.0.0.1";
            }

            if (StartupArgs != null)
            {

                if (StartupArgs.Length > 2)
                {
                    MainData.Me.name = StartupArgs[1];
                    ip = StartupArgs[2];
                }
                else
                {
                    return;
                }

                if (StartupArgs[0] == "Server")
                {
                    isServer = 1;
                    Server.ServerRun(ip, port);
                    Client.Connect(ip, port);
                }
                if (StartupArgs[0] == "Client")
                {

                    isServer = -1;
                    Client.Connect(ip, port);
                    Client.SendLogin();
                }

            }

          //  string sourceCode = File.ReadAllText(@"D:\workspace\C#\DreamingTest\MainWindow.xaml.cs");

          //  string colorizedSourceCode = new CodeColorizer().Colorize(sourceCode, Languages.CSharp);

          //  web.NavigateToString("<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"></head>" + colorizedSourceCode + "</html>");
        }



        private void TextEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            string SourceCode = MainData.m.Transform(TextEditor.Text);
          //  string SourceCode = new CodeColorizer().Colorize(TextEditor.Text, Languages.CSharp); 
            web.NavigateToString("<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"><style type=\"text/css\">"+
                "body{font-family:Microsoft YaHei Mono;font-size:18px;}</style>"
                +"</head>" + SourceCode + "</html>");
        }

        private void TextEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Send();
            }
        }

        private void Send()
        {
            string send_str = "<h2>" + MainData.Me.name + ":</h2>\r\n" + MainData.m.Transform(TextEditor.Text);
            MainData.Me.AddString(send_str);
            Client.SendText(send_str);
            TextEditor.Text = "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Send();
        }


    }
}
