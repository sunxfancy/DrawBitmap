using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MulticastNetWork
{
    public class ThreadPoolServer
    {
        public ThreadPoolServer(string _ip,int _port)
        {
            ip = _ip;
            port = _port;
        }

        private String command;
        public int port;
        public String ip = "127.0.0.1";
        private Thread listenerThread;

        public event ServerAcceptEventHandler accept;
        private int p;
        private void worker(object state)
        {
            TcpClient client = state as TcpClient;
            try
            {
                accept.Invoke(this, new ServerAcceptEventArgs(client));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (client != null)
                {
                    client.Close();
                }
            }
        }

        private void listener()
        {
            TcpListener listener = new TcpListener(IPAddress.Parse(ip), port);
            
            listener.Start();  // 开始监听客户端请求
            TcpClient client = null;

            while (true)
            {
                client = listener.AcceptTcpClient();
                client.ReceiveTimeout = 2000; //设置一个等待延时
                ThreadPool.QueueUserWorkItem(worker, client);  // 从线程池中获得一个线程来处理客户端请求
            }
        }

        public void start()
        {
            listenerThread = new Thread(listener);
            listenerThread.IsBackground = true;
            listenerThread.Start();  // 开始运行监听线程
        }
    }
}
