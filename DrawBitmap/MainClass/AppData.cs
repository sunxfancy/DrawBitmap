using DrawBitmap.MainClass;
using MulticastNetWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;
using System.Threading;

namespace DrawBitmap
{
    public class AppData
    {
        public static bool isPluginReady = false;

        [ImportMany(typeof(Plugin))]
        public IEnumerable<Plugin> plugin_list;

        public static SortedDictionary<int, Plugin> plugin_dic = new SortedDictionary<int, Plugin>();

        public List<FriendClass> FriendClassList = new List<FriendClass>();
        public SortedDictionary<int, Friend> FriendList = new SortedDictionary<int, Friend>();
        public List<Group> GroupList = new List<Group>();
        public List<Friends> RecentContacts = new List<Friends>();

        public SendMessage sending = new SendMessage();
        public List<MessageDone> MessageList = new List<MessageDone>();
        public Friend Me;

        /// <summary>
        /// 初始化所有插件，将其实例化后存入
        /// </summary>
        public void InitPlugin()
        {
            if (isPluginReady) return;
            var catalog = new DirectoryCatalog(System.Environment.CurrentDirectory+@"\plugin");
            var container = new CompositionContainer(catalog);

            try
            {
                container.ComposeParts(this);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return;
            }

        }
        /// <summary>
        /// 初始化数据，先加载自己的数据，然后是好友列表，然后是群组表，插件表
        /// </summary>
        /// <param name="data"></param>
        public void InitLoginData(LoginReturn data)
        {
            InitMe(data.myTimeStamp.Key, data.myTimeStamp.Value);
            //从磁盘加载好友数据
            //联网获取时间戳
            // 校验版本
            // 若不对要联网更新数据
            InitFriend(data.FriendsTimeStamp);

            //更新在线好友，更新ip
            foreach (var item in data.OnlineFriends)
            {
                Friend f = null;
                if (FriendList.TryGetValue(item.Key, out f) && f != null)
                {
                    f.isOnline = true;
                    //ToDo:
                    f.ip = new System.Net.IPAddress(item.Value);
                }
            }

            //////////////////////////////////////////////////////////////////////////
            //解析离线消息
            foreach (var item in data.message)
            {
                GetReturn.ParseMessage(item);
            }

            InitGroup(data.GroupTimeStamp);
            InitSending();
        }

        private void InitGroup(List<pair<int, long>> list)
        {
            foreach(var item in list)
            {
                var temp_g = new Group();
                GroupData t_data = ServerAPI.GetGroupData(item.Key);

                temp_g = t_data.toGroup(); 
                this.GroupList.Add(temp_g);
            }
        }


        public void InitMe(int id,long mytimestamp)
        {
            //从磁盘读取自身数据
            //联网获取自身数据版本号 long型时间戳
            //校验失败要联网获取自身数据
            if (Me==null)
            {
                Me = new Friend();
                Me.Updata(id);
            }
            else
            {
                if (Me.TimeStamp < mytimestamp)
                {
                    Me.Updata();
                }
            }
        }

        public void InitPluginSystem()
        {
            var dic = plugin_dic;
            
            if (!isPluginReady)
            {
                InitPlugin();
                if (plugin_list == null) return;
              
                foreach (var item in plugin_list)
                {
                    dic.Add(item.ID, item);
                }
            }

            foreach (var item in plugin_dic.Values)
            {
                item.Init();
                item.runhandle += item_runhandle;
                if (item.Dependencies != null)
                    foreach (var id in item.Dependencies)
                    {
                        if (!dic.ContainsKey(id))
                        {
                            //TODO:
                            //download...
                        }
                    }
            }
           
            isPluginReady = true;
        }

        /// <summary>
        /// 插件的按键回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void item_runhandle(object sender, PluginRunEvent.PluginRunEventArgs args)
        {
            var p = args.plugin;
            var list = new List<Friend>();
            var f_list=new SortedSet<int>();
            foreach (var item in FriendList.Values)
            {
                if (item.isSelected)
                {
                    list.Add(item);
                    f_list.Add( item.user_id);
                }
            }
            try
            {
                p.user_list = list;
                p.isServer = true;
                p.ip = Me.ip;
                p.Me = Me;

                p.RunPlugin();
            }catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            new Thread(() => {
                if (f_list.Count != 0)
                    ServerAPI.InviteFriends(p.ID, Me.user_id, f_list);
            }).Start();
                 //  MessageBox.Show("Hello");
        }

        public void InitFriend(List<pair<int, long>> FriendsTimeStamp)
        {
            //TODO:从磁盘加载好友信息

            //联网获取好友信息
            foreach (var item in FriendsTimeStamp)
            {
                Friend f  = null;

                if (FriendList.TryGetValue(item.Key, out f))
                {
                    if (item.Value > f.TimeStamp)
                    {
                        f.Updata();
                    }
                }
                else
                {
                    f = new Friend();
                    f.nickname = "xxx";
                    FriendList.Add(item.Key, f);
                    f.Updata(item.Key);
                  //  f.Father = App.mainWindow.AddFriend(f);
                }
            }
        }



        public void InitSending()
        {
            sending.StartSending();
        }

    }
}
