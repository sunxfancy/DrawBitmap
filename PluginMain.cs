using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawBitmap;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Diagnostics;

namespace DreamingPlugin
{
    [Export(typeof(Plugin))]
    class PluginMain : Plugin
    {
        List<User> _user_list;
        public List<User> user_list
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
            get { return "畅想器"; }
        }

        public byte[] Versions
        {
            get;
            set;
        }

        public void RunPlugin()
        {
            Process p = new Process();
            p.StartInfo.FileName = "DreamingApp.exe";
            p.StartInfo.WorkingDirectory = "./App/";
           // p.StartInfo.Arguments = 
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;

            p.Start();
        }

        public void Init()
        {
            Button btn =   new Button();
            btn.Content= "畅想器";
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
            get { return 1; }
        }
    }
}
