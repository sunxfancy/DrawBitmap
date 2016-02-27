using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
        

namespace DrawBitmap.MainClass
{
    public class GetReturn
    {
        public static void ParseMessage(UserMessage message)
        {
            switch (message.type)
            {
                case 1:
                    //上线
                    {
                        pair<int, byte[]> data = (pair<int, byte[]>)message.data;
                        Friend f = App.data.FriendList[data.Key];
                        f.ip = new System.Net.IPAddress(data.Value);
                        f.isOnline = true;
                        f.Father.Dispatcher.Invoke(() => { f.Father.Updata(); });
                    }
                    break;
                case 2:
                    //下线
                    {
                        int id = (int)message.data;
                        Friend f = App.data.FriendList[id];
                        f.isOnline = false;
                        f.Father.Dispatcher.Invoke(() => { f.Father.Updata(); });
                    }
                    break;
                case 3:
                    //邀请运行某插件
                    {
                        MessageBox.Show("收到邀请");
                        List<int> data = message.data as List<int>;
                        int plugin_id = data[0];
                        int user_id = data[1];
                        var m = new MessageInvite(user_id, plugin_id);
                        m.id = message.id;
                        string name =App.data.FriendList[user_id].nickname;
                        string p_name = AppData.plugin_dic[plugin_id].Name;
                        m.message = string.Format("{0} 邀请您一同使用 {1}", name, p_name);
                        App.data.MessageList.Add(m);
                    }
                    break;
                case 4:
                    //好友添加请求
                    {
                        User friend = message.data as User;
                        var m = new MessageAddFriend(friend);
                        m.type = 1;
                        m.id = message.id;
                        m.message = string.Format("{0} 请求加您为好友", friend.nickname);
                        App.data.MessageList.Add(m);
                    }
                    break;
                case 5:
                    //资料更新
                    {
                        User F = message.data as User;
                        Friend friend = App.data.FriendList[F.user_id];
                        friend.UpdatabyUser(F);
                        friend.Father.Dispatcher.Invoke(() => { friend.Father.Updata(); });
                    }
                    break;
                case 6:
                    //对好友申请的表态
                    {
                       List<object> data = (List<object>)message.data;
                        var m = new MessageInformShow(message.id);
                        m.id = message.id;
                        m.type = 2;
                        String temp="拒绝";
                        if((int)data[1]==1)
                        {
                            temp="同意";
                        }
                        m.message = string.Format("{0} {1}加您为好友",((User)data[0]).name,temp);
                        App.data.MessageList.Add(m);
                    }
                    break;


                default:
                    break;
            }
        }

        
    }



    /// <summary>
    /// 处理信息的类
    /// </summary>
    public class MessageDone
    {
        public MessageDone()
        {
            Status = 0;
        }
        public int id=-1;
        public byte type=1;  //何种消息，要不要显示按钮，还是显示什么按钮
        public string message;
        protected int status;
        public virtual int Status//0是还未处理 1是同意 -1是拒绝 2是已阅
        {
            set { status = value; }
            get { return status; }
        }
    }

    public class MessageInvite : MessageDone
    {
        public MessageInvite(int invite_id,int plugin_id)
        {
            type = 3;
            this.invite_id = invite_id;
            this.plugin_id = plugin_id;
        }
        public int invite_id;
        public int plugin_id;
        public override int Status
        {
            get
            {
                return base.Status;
            }
            set
            {
                base.Status = value;
                if (value == 1) RunPluginWithFriend();
            }
        }
        /// <summary>
        /// 启动插件
        /// </summary>
        private void RunPluginWithFriend()
        {

            Plugin p = AppData.plugin_dic[plugin_id];

            p.isServer = false;
            p.Me = App.data.Me;
            p.ip = App.data.FriendList[this.invite_id].ip;
            p.RunPlugin();

        }
    }

    public class MessageAddFriend : MessageDone
    {
        public MessageAddFriend(User u)
        {
            type = 4;
            friend = u;
        }
        public User friend;
        public override int Status
        {
            get
            {
                return base.Status;
            }
            set
            {
                base.Status = value;
                switch (value)
                {
                    case 1: //添加好友
                         new Thread(AddFriend).Start(value);
                         break;
                    case -1://拒绝好友
                         break;
                    default:
                         break;
                }
            }
        }


        private void AddFriend(object obj)
        {
            int attitude = 0;
            if((int)obj==1)
                attitude = 1;
            bool result = ServerAPI.AgreeAddFriend(friend.user_id,App.data.Me.user_id, attitude);
            if (attitude ==1)
            {
                Friend f = App.mainWindow.AddFriend(friend.user_id);
                App.data.FriendList.Add(friend.user_id, f);
            }
        }
    }

    /// <summary>
    /// 仅仅将内容显示出来
    /// </summary>
    public class MessageInformShow:MessageDone
    {
        String msg;
        int reciver_id;
        int msgId;
        public MessageInformShow(int MsgId)
        {
            type = 4;
            msgId = MsgId;
        }
        public override int Status
        {
            get
            {
                return base.Status;
            }
            set
            {
                base.Status = value;
                switch (value)
                {
                    case 2: //确认消息
                        new Thread(confirm).Start();
                        break;
                }
            }
        }
        private void confirm(object obj)
        {
            bool result = ServerAPI.confirmMessage(App.data.Me.user_id, msgId);
        }
    }






}
