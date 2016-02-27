using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawBitmap
{
    public class PluginRunEvent
    {
        public class PluginRunEventArgs : EventArgs
        {
            public readonly Plugin plugin;
            public PluginRunEventArgs(Plugin _data)
            {
                plugin = _data;
            }
        }

        public delegate void PluginRunEventHandler(object sender, PluginRunEventArgs args);

    }
}
