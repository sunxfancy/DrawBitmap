using MulticastNetWork;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace DreamingApp
{
    public static class Client
    {
        private static NetWorkClient client;
        public static bool isClient = true;


        public static bool Connect(string ip, int port)
        {
            bool ans = false;
            try
            {

                client = new NetWorkClient(ip, port);
                ans = client.Connect();
                if (isClient)
                {
                    client.answer += client_answer;
                    client.BeginListener();

                    var t = new Thread(Send);
                    t.IsBackground = true;
                    t.Start();
                }
            }
            catch(Exception e1)
            {
                MessageBox.Show("Client.Connect" + e1.Message);
            }
            return ans;
        }

        static void client_answer(object sender, AnswerEventArgs args)
        {
            var message = args.data as UserMessage;

            try{

            switch (message.type)
            {
                case 1://传输线条
                    {
                      //  MessageBox.Show(String.Format("线条来自{0}", message.name));
                        MyStroke p = message.data as MyStroke;
                        Custom c;
                        MainData.user_dic.TryGetValue(message.name,out c);
                        if (c!=null && p !=null)
                            c.AddStroke(p);
                    }
                    break;
                case 2://登录，表示有某个人登录了
                    {
                      //  MessageBox.Show(String.Format("登录{0}", message.name));
                        if (MainData.user_dic.ContainsKey(message.name))
                        {
                            MessageBox.Show("Error 请不要重复登录");
                        }
                        var p =  new Custom();
                        p.name = message.name;
                        
                        MainData.user_dic.Add(message.name,p);
                    }
                    break;
                case 3://登录反馈，收到的服务器发过来的在线人的名字列表
                    {
                        List<string> s = message.data as List<string>;
                        string str = "";
                        foreach (var item in s)
	                    {
                            str = str + item + " ";
	                    }
                      //  if (s != null) MessageBox.Show(str);
                       // else MessageBox.Show("Get But No Data");
                        foreach (var item in s)
                        {
                            var p = new Custom();
                            p.name = item;
                            MainData.user_dic.Add(item, p);
                        }
                    }
                    break;
                case 4://线宽变化
                    {
                        var d = (double)message.data;
                        Custom c;
                        MainData.user_dic.TryGetValue(message.name, out c);
                        if (c != null)
                            c.ChangeWidth(d);
                    }
                    break;
                case 5://颜色变化
                    {
                        var cc = message.data as string;
                      //  MessageBox.Show(cc);
                        Custom c;
                        MainData.user_dic.TryGetValue(message.name, out c);
                        if (c != null)
                            c.ChangeColor(cc);
                    }
                    break;
                case 6:
                    {
                        Custom c;
                        MainData.user_dic.TryGetValue(message.name, out c);
                        if (c != null)
                            c.Undo();
                    }
                    break;
                case 7:
                    {
                        Custom c;
                        MainData.user_dic.TryGetValue(message.name, out c);
                        if (c != null)
                            c.Clear();
                    }
                    break;
                case 8://下线通知 此时name是下线者
                    {
                      //  MessageBox.Show("Offline");
                        Custom c;
                        MainData.user_dic.TryGetValue(message.name, out c);
                        if (c != null)
                            c.Remove();
                        MainData.user_dic.Remove(message.name);
                    }
                    break;
                default:
                    break;
            }
                 }
            catch(Exception e1)
            {
                MessageBox.Show("Client.client_answer" + e1.Message);
            }


        }

        /// <summary>
        /// 发送一个用户信息结构体
        /// 用户名会自动附上当前自己的名字
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="obj">数据</param>
        /// <returns></returns>
        internal static void SendMessage(byte type, object obj)
        {
            var u = new UserMessage(type, MainData.Me.name, obj);
            lock (lock_obj)
            {
                sending_list.Enqueue(u);
            }
        }
        static object lock_obj = new object();
        /// <summary>
        /// 用户信息发送队列
        /// </summary>
        public static Queue<object> sending_list = new Queue<object>();

        private static void Send()
        {
            object obj = null;
            while(true)
            {
                if (sending_list.Count > 0)
                {
                    lock (lock_obj)
                    {
                        obj = sending_list.Dequeue();
                    }
                }
                if (obj != null)
                {
                    try
                    {
                        client.Send(obj);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.ToString());
                    }
                    finally
                    {
                        obj = null;
                    }
                }
                Thread.Sleep(1);
            }
           
        }

        internal static void SendLogin()
        {
            SendMessage(2, 0);
        }

        internal static void SendStroke(Stroke stroke)
        {
            MyStroke my = new MyStroke(stroke);
            SendMessage(1, my);
        }

        internal static void SendLineWidth(Double d)
        {
            SendMessage(4, d);
        }

        internal static void SendColor(Color c)
        {
            ColorConverter cc = new ColorConverter();
            string str = cc.ConvertToString(c);
            SendMessage(5,str);
        }

        internal static void SendUndo()
        {
            SendMessage(6, 0);
        }

        internal static void SendClear()
        {
            SendMessage(7, 0);
        }

    }
}
