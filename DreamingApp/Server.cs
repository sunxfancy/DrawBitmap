using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MulticastNetWork;
using System.Windows;
using System.Diagnostics;
using System.Net.Sockets;

namespace DreamingApp
{
    public static class Server
    {
        static NetWorkServer server;
        static Dictionary<TcpClient, string> tcpmap = new Dictionary<TcpClient, string>();
        public static void ServerRun(string ip,int port)
        {
            try
            {
                server = new NetWorkServer(ip, port);
                server.answer += server_answer;
                server.offline += server_offline;
                server.BeginListener();
            }
            catch (Exception e2)
            {
                MessageBox.Show("Server.ServerRun" + e2.Message);
            }
        }

        static void server_offline(object sender, ServerAcceptEventArgs args)
        {
            try{
            Debug.WriteLine("OffLine!");
            var str = tcpmap[sender as TcpClient];
            server.ServerSendAll(new UserMessage(8, str, 0));
                }
            catch(Exception e2)
            {
                MessageBox.Show("Server.server_offline" + e2.Message);
            }
        }

        static void server_answer(object sender, AnswerEventArgs args)
        {
            try
            {
            Debug.WriteLine("Received!");
            var message = args.data as UserMessage;
            if (message.type == 2)
            {
                var s = sender as TcpClient;
                tcpmap.Add(s, message.name);
                var ns = s.GetStream();
                var data = new List<string>();
                data.Add(MainData.Me.name);
                data.AddRange(MainData.user_dic.Keys);
                var u = new UserMessage(3,"Server",data);
                server.formatter.Serialize(ns,u);
            }
            }
            catch(Exception e2)
            {
                MessageBox.Show("Server.server_answer" + e2.Message);
            }

        }
    }
}
