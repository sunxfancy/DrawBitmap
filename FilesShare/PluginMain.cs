using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawBitmap;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows;
using MulticastNetWork;
using System.IO;
using System.Net;

namespace FilesShare
{
    [Export(typeof(Plugin))]
    class PluginMain : Plugin
    {
        public enum AcceptState
        {
            NotReady = 0,
            Ready,
            Done
        };
        static NetWorkServer server = null;
        static NetWorkClient client = null;
        static MainWindow mainWindow = null;

        public static DirectoryInfo di = null;
        public static AcceptState acceptData = AcceptState.NotReady;

        List<Friend> _user_list;
        public List<Friend> user_list
        {
            get
            {
                return _user_list;
            }
            set
            {
                _user_list = value;
            }
        }
     
        public string Name
        {
            get { return "文件互传"; }
        }

        public byte[] Versions
        {
            get;
            set;
        }

        public void RunPlugin()
        {

        }
        public void Init()
        {
            Button btn =   new Button();
            btn.Content= "文件互传";
            btn.PreviewMouseLeftButtonDown += btn_PreviewMouseLeftButtonDown;
            API.AddUserWindowChild("生活类",btn);
            
        }


        void btn_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
            Friend f = null;
            foreach (var item in App.data.FriendList)
            {
                if (item.Value.isSelected)
                {
                    f = (Friend)item.Value;
                    client = new NetWorkClient(f.ip.ToString(), 6537);
                    client.answer += GetAnswers;  //托管(回调)
                    client.Connect();  //开始连接
                    client.BeginListener();
                    break;
                }
            }
            if (f == null)
            {
                f = API.Me;
                server = new NetWorkServer(f.ip.ToString(), 6537);
                server.answer += Answer; //如果收到信息，则执行Answer(托管)
                server.BeginListener();  //开始监听
            }
            if (!f.Equals(API.Me))
            {

                    client.Send("Info");
                
            }
            mainWindow = new MainWindow(f);
            mainWindow.Show();
            mainWindow.Focus();
            
            var func = runhandle;
            if (func != null)
            {
                func(this, new PluginRunEvent.PluginRunEventArgs(this));
            }
            
        }

        public event PluginRunEvent.PluginRunEventHandler runhandle;


        public SortedSet<int> Dependencies
        {
            get { return null; }
        }


        public int ID
        {
            get { return 4; }
        }

        public static void Answer(object sender, AnswerEventArgs args)
        {
            if (args.data.ToString().Equals("Info"))
            {
                if(di!=null)
                server.ServerSendAll(di);
                
            }
            else if (args.data.ToString().Equals("Data"))
            {
                Queue<DirectoryInfo> qd =new Queue<DirectoryInfo>();
                if (di != null)
                {
                    DirectoryInfo tmp = di;
                    qd.Enqueue(tmp);
                    while (qd.Count > 0)
                    {
                        
                        server.ServerSendAll(qd.Peek());
                        foreach (FileInfo fi in qd.Peek().GetFiles())
                        {
                            
                            
                            FileStream fs = new FileStream(fi.FullName, FileMode.Open);
                            
                            server.ServerSendAll(fs);
                        }
                        foreach (DirectoryInfo d in qd.Peek().GetDirectories())
                        {
                            qd.Enqueue(d);
                        }
                        qd.Dequeue();
                    }
                    server.ServerSendAll("Done");
                }
            }
        }
        bool _isServer;
        public bool isServer
        {
            get
            {
                return _isServer;
            }
            set
            {
                _isServer = value;
            }
        }

        IPAddress _ip;
        public IPAddress ip
        {
            get
            {
                return _ip;
            }
            set
            {
                _ip = ip;
            }
        }

        public Friend Me { get; set; }

        public static void GetAnswers(object sender, AnswerEventArgs args)
        {
            if (acceptData==AcceptState.Ready)
            {
                if(args.data is DirectoryInfo)
                {
                       Directory.CreateDirectory(((DirectoryInfo)args.data).FullName);
                    

                }
                if(args.data is FileStream)
                {
                    FileStream f = (FileStream)args.data;
                    FileStream tag = new FileStream(f.Name, FileMode.Create);
                    f.CopyTo(tag);
                    

                }
                else if (args.data.Equals("Done"))
                {
                    acceptData = AcceptState.Done;
                }
                else
                {
                    client.Send("Data");
                }

            }
            else if(acceptData == AcceptState.NotReady)
            {
                if (args.data is DirectoryInfo)
                {

                    mainWindow.ShowShareFolder((DirectoryInfo)(args.data));
                }
            }
            else if (acceptData == AcceptState.Done)
            { 
               
            }
        }
    }


}
