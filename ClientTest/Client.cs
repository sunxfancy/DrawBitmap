using MulticastNetWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Configuration;

namespace ClientTest
{
    class Client
    {
        static NetWorkClient client;
        static void Main(string[] args)
        {
            string name = ConfigurationManager.AppSettings["name"];
            string ip = ConfigurationManager.AppSettings["ip"];
            int port = int.Parse(ConfigurationManager.AppSettings["port"]);
            
            client = new NetWorkClient(ip, port);  //链接服务器，端口为9999
            client.answer += GetAnswers;  //托管(回调)
            client.Connect();  //开始连接
            client.BeginListener();
            string command;
            Console.WriteLine("welcome to the chating room");
            while (true)
            {
                //不断获取指令 
                command = Console.ReadLine();

                String str = name + ":\n  " + command + "\n";
               // Console.WriteLine(str);
                client.Send(str);  //发过去
                /*
                if (command == "send")//若输入'send'
                {
                    //新构建Image对象
                    Image bitmap = Image.FromFile("ysn.gif");
                    client.Send(bitmap);  //发过去
                }

                if (command == "message")//若输入'send'
                {
                    string hello = "HelloWorld";
                    client.Send(hello);
                }
                */
            }
        }

        private static void GetAnswers(object sender, AnswerEventArgs args)
        {
            string hello = args.data as string;
            Console.WriteLine(hello);
       //     Image a = args.data as Image;  //强制转换为一个Image对象
        //    a.Save("yyy.gif");  //将此文件保存为yyy.gif，保存在本地了(这是何意)，呃，测试保存性能？
        //    Console.WriteLine("saved! yyy.gif");  //输出至控制台
        }
    }
}
