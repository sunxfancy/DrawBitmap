using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawBitmap;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows;

namespace MyWork_D
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
        Canvas4All mainWindow;
        public string Name
        {
            get;
            set;
        }

        public byte[] Versions
        {
            get;
            set;
        }

        public void RunPlugin()
        {
            mainWindow.Show();
        }

        public void Init()
        {
           mainWindow = new Canvas4All();
           mainWindow.Show();
           Button btn =   new Button();
            btn.Content= "畅想器";
            btn.PreviewMouseLeftButtonDown += btn_MouseLeftButtonDown;
            btn.Loaded += btn_Loaded;
            //btn.MouseLeftButtonDown += btn_MouseLeftButtonDown;
           API.AddUserWindowChild("通信类",btn);
        }

        void btn_Loaded(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hello");
        }


        void btn_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            mainWindow.Show();
            MessageBox.Show("Hello");
            runhandle.Invoke(this, new DrawBitmap.PluginRunEvent.PluginRunEventArgs(this));
        }


        public event DrawBitmap.PluginRunEvent.PluginRunEventHandler runhandle;
    }
}
