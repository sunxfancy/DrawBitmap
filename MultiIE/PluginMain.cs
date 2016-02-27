using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawBitmap;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows;
using System.Net;

namespace MultiIE
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
            get { return "多人同步浏览器"; }

        }

        public byte[] Versions
        {
            get;
            set;
        }

        public void RunPlugin()
        {
            IEWindow mainWindow = new IEWindow();
            mainWindow.Show();
        }

        public void Init()
        {
            Button btn =   new Button();
            btn.Content= "多人同步浏览器";
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
            get { return 2; }
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
                _ip = ip;
            }
        }

        public Friend Me { get; set; }
    }
}
