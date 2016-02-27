using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;

namespace TalkPlugin
{
    /// <summary>
    /// 网络通信时，用户信息结构体
    /// </summary>
    [Serializable]
    public class UserMessage
    {
        /// <summary>
        /// 构建一个用户信息结构体
        /// </summary>
        /// <param name="_m">消息类型</param>
        /// <param name="_n">发送者名字</param>
        /// <param name="_o">数据</param>
        public UserMessage(byte _m,string _n, object _o)
        {
            type = _m;
            name = _n;
            data = _o;
        }

        public UserMessage()
        {

        }

        public byte type;
        public object data;
        public string name;
    }
    
}
