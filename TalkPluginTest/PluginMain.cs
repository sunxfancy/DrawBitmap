using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawBitmap;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows;
using System.Net;

namespace DreamingPlugin
{
    [Export(typeof(Plugin))]
    class PluginMain : Plugin
    {
        List<Friend> _user_list;
        public List<Friend> user_list
        {
            get
            {
                return _user_list;
            }
            set
            {
                _user_list = value;
            }
        }
        public string Name
        {
            get { return "聊天"; }
        }

        public byte[] Versions
        {
            get;
            set;
        }

        public void RunPlugin()
        {
            Process p = new Process();
            p.StartInfo.WorkingDirectory = Environment.CurrentDirectory + @"\plugin\Talk\";
            p.StartInfo.FileName = p.StartInfo.WorkingDirectory + "TalkApp.exe";
            string args = "";
            if (_isServer)
                args += "Server ";
            else
                args += "Client ";
            args += (Me.nickname +" "+ ip.ToString());
            //MessageBox.Show(args);
            p.StartInfo.Arguments = args;
            p.StartInfo.UseShellExecute = true;
      //     p.StartInfo.RedirectStandardInput = true;
       //    p.StartInfo.RedirectStandardOutput = true;
            MessageBox.Show(args);
            try
            {
                p.Start();
            }
            catch (System.ComponentModel.Win32Exception we)
            {
                MessageBox.Show("ERROR:"+we.ToString());
                return;
            }
        }

        public void Init()
        {
            Button btn = new Button();
            btn.Content= "聊天";
            btn.PreviewMouseLeftButtonDown += btn_PreviewMouseLeftButtonDown;
            API.AddUserWindowChild("通信类",btn);

        }


        void btn_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var func = runhandle;
            if (func != null)
            {
                func(this, new PluginRunEvent.PluginRunEventArgs(this));
            }
        }

        public event PluginRunEvent.PluginRunEventHandler runhandle;


        public SortedSet<int> Dependencies
        {
            get { return null; }
        }


        public int ID
        {
            get { return 8; }
        }

        bool _isServer;
        public bool isServer
        {
            get
            {
                return _isServer;
            }
            set
            {
                _isServer = value;
            }
        }

        IPAddress _ip;
        public IPAddress ip
        {
            get
            {
                return _ip;
            }
            set
            {
                _ip = value;
            }
        }

        public Friend Me { get; set; }
    }
}
