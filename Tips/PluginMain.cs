using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawBitmap;
using System.Windows.Controls;
using System.ComponentModel.Composition;
using System.Net;

namespace Tips
{

    [Export(typeof(Plugin))]
    public class PluginMain : Plugin
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
            get { return "交流墙"; }
        }

        public int ID
        {
            get { return 3; }
        }

        public SortedSet<int> Dependencies
        {
            get { return null; }
        }

        public event PluginRunEvent.PluginRunEventHandler runhandle;

        public byte[] Versions
        {
            get;
            set;
        }

        public void RunPlugin()
        {
            
        }

        private void btn_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            WPFDemo.MainWindow mainWindow = new WPFDemo.MainWindow();
            mainWindow.Show();
            var func = runhandle;
            if (func != null)
            {
                func(this, new PluginRunEvent.PluginRunEventArgs(this));
            }
        }

        public void Init()
        {
            Button btn = new Button();
            btn.Content = "交流墙";
            btn.PreviewMouseLeftButtonDown += btn_PreviewMouseLeftButtonDown;
            API.AddUserWindowChild("工作类", btn);
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
