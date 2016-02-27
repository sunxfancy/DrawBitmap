using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkPlugin
{
    class App
    {
        public MainWindow main;
        public MainData data;
        public void Run()
        {
            main = new MainWindow();
            data = new MainData();
            data.app = this;
            main.app = this;
            main.Show();
        }
    }
}
