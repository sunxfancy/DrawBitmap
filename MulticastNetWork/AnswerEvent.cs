using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MulticastNetWork
{
    public class AnswerEventArgs : EventArgs
    {
        public readonly object data;
        public AnswerEventArgs(object _data)
        {
            data = _data;
        }
    }

    public delegate void AnswerEventHandler(object sender, AnswerEventArgs args);
    public delegate void OffLineEventHandler(object sender, ServerAcceptEventArgs args);
    public class ServerAcceptEventArgs : EventArgs
    {
        public readonly TcpClient client;
        public ServerAcceptEventArgs(TcpClient _client)
        {
            client = _client;
        }
    }

    public delegate void ServerAcceptEventHandler(object sender, ServerAcceptEventArgs args);
}
