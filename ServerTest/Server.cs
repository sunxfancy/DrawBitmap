using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MulticastNetWork;
using System.Threading;
namespace ServerTest
{
    class Server
    {
        static NetWorkServer server;
        static void Main(string[] args)
        {
            server = new NetWorkServer("127.0.0.1", 10086); //监听9999端口
            server.answer += Answer; //如果收到信息，则执行Answer(托管)
            server.BeginListener();  //开始监听
            Console.WriteLine("Server Begin");
            while (true)
            {
                string command = Console.ReadLine();
                if (command == "exit")
                {
                    return;
                }
            }
        }

        private static void Answer(object sender, AnswerEventArgs args)
        {
            Console.WriteLine("Data Connection!"); //输出至控制台
      //      string s = args.data.ToString();   //将传递的信息转为字符串形式(不忒准确)
      //      Console.WriteLine(s);  //输出值控制台
        }

    }
}
