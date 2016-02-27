using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MulticastNetWork
{
    public class NetWorkServer
    {
        List<TcpClient> user_list = new List<TcpClient>();
        Queue<KeyValuePair<TcpClient, object>> Sending_List = new Queue<KeyValuePair<TcpClient, object>>();
        string ip = "127.0.0.1";
        public NetWorkServer(string _ip ,int _port)
        {
            ip = _ip;
            port = _port;
        }

        public BinaryFormatter formatter = new BinaryFormatter();
        public int port {private set; get; }

        Thread listenThread;
        Thread sendingThread;
        public void BeginListener()
        {
            listenThread = new Thread(StartListener);
            listenThread.IsBackground = true;
            listenThread.Start();
            sendingThread = new Thread(MessageSender);
            sendingThread.IsBackground = true;
            sendingThread.Start();
        }

        public void EndListener()
        {
            listenThread.Abort();
            sendingThread.Abort();
        }

        public void StartListener()
        {
            TcpListener listener = new TcpListener(IPAddress.Parse(ip), port);//ip为服务器IP地址，port为监听的端口

            listener.Start();//开启监听

            try
            {
                while (true)
                {
                    TcpClient tcp = listener.AcceptTcpClient();
                    Thread tcpThread = new Thread(Accpet);
                    tcpThread.IsBackground = true;
                    tcpThread.Start(tcp);
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
                Console.WriteLine(e.ToString());
            }
        }

        void Accpet(object t)
        {
            TcpClient tcp = t as TcpClient;
            try
            {
                user_list.Add(tcp);
              //  byte[] buffer = new byte[1024];
                while (true)
                {
                    NetworkStream ns = tcp.GetStream();
                    object data = formatter.Deserialize(ns);//第一版
                   // int intSize = 0;
                   // intSize = ns.Read(buffer, 0, 1024);
                   // while(intSize>0)
                   // {
                   //     SendAll(buffer,intSize);
                   //     intSize = ns.Read(buffer, 0, 1024);
                   // }
                    var send_data = new KeyValuePair<TcpClient, object>(tcp,data);
                    lock (Sending_List)
                    {
                        Sending_List.Enqueue(send_data);
                    }
                    //SendAll(tcp,data);
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
                Console.WriteLine(e.ToString());
                user_list.Remove(tcp);
                if (offline != null)
                    offline(tcp, new ServerAcceptEventArgs(tcp));
                tcp.Close();
            }
        }

        public event AnswerEventHandler answer;
        //TODO: 下线通知 event
        public event OffLineEventHandler offline;


        KeyValuePair<TcpClient, object>? sdata = null;
        void MessageSender()
        {
            while (true)
            {
                lock (Sending_List)
                {
                    if (Sending_List.Count != 0)
                    {
                        sdata = Sending_List.Dequeue();
                    }
                }
                if (sdata != null)
                {
                    SendAll(sdata.Value.Key, sdata.Value.Value);
                    sdata = null;
                }
                Thread.Sleep(1);
            }
        }



        void SendAll(TcpClient my,object data)
        {
            try
            {
                answer.Invoke(my, new AnswerEventArgs(data));
                foreach (var user in user_list)
                {
                    if (my.Equals(user)) continue;
                    var ns = user.GetStream();
                    //ns.Write(data, 0, 1024);
                    formatter.Serialize(ns, data);//第一版
                }
            }catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
                Console.WriteLine(e.ToString());
            }
        }
        public void ServerSendAll(object data)
        {
            try
            {
                //answer.Invoke(my, new AnswerEventArgs(data));
                foreach (var user in user_list)
                {
                    //if (my.Equals(user)) continue;
                    var ns = user.GetStream();
                    //ns.Write(data, 0, 1024);
                    formatter.Serialize(ns, data);//第一版
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                Console.WriteLine(e.ToString());
            }
        }

        void SendAll(byte[] data,int size) //经过测试，这种传输方式速度并不快，原因不明。
        {
            try
            {
                answer.Invoke(this, new AnswerEventArgs(data));
                foreach (var user in user_list)
                {
                    var ns = user.GetStream();
                    ns.Write(data, 0, size);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
