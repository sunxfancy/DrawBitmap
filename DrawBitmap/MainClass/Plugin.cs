using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace DrawBitmap
{
    public interface Plugin
    {
        /// <summary>
        /// 这是一个用户结构体的list
        /// </summary>
        List<Friend> user_list
        {
            get;
            set;
        }

        /// <summary>
        /// 是否是服务器
        /// </summary>
        bool isServer
        {
            set;
            get;
        }

        IPAddress ip
        {
            set;
            get;
        }

        /// <summary>
        /// 插件名
        /// </summary>
        string Name
        {
            get;
        }

        int ID
        {
            get;
        }

        /// <summary>
        /// 记录所有依赖项
        /// </summary>
        SortedSet<int> Dependencies
        {
            get;
        }

        event PluginRunEvent.PluginRunEventHandler runhandle;

        /// <summary>
        /// 版本号，从小到大，依次存1.0.0.0四个版本号
        /// </summary>
        byte[] Versions
        {
            get;
        }

        /// <summary>
        /// Plugin的启动方法
        /// </summary>
        void RunPlugin();

        /// <summary>
        /// Plugin的初始化方法
        /// </summary>
        void Init();

        Friend Me { get; set; }
    }
}
