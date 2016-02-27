using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawBitmap;
using System.Threading;
using MulticastNetWork;


namespace Server
{
    [Serializable]
    public class OnlineUserMessge:UserMessage
    {
       public long timestamp;
        public OnlineUserMessge(int _id,byte _m, object _o,long _timestamp):base(_id,_m,_o)
        {
            timestamp = _timestamp;
        }

        public OnlineUserMessge( byte _m, object _o, long _timestamp): base( _m, _o)
        {
            timestamp = _timestamp;
        }
        public OnlineUserMessge(byte _m, object _o): base(_m, _o)
        {
            timestamp = MySQL_Manager.getTimestampOfNow();
        }

        public OnlineUserMessge()
        {
            // TODO: Complete member initialization
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public UserMessage ToUserMessage()
        {

            return new UserMessage(this.type, this.data);
        }
    }




    /// <summary>
    /// 在线的用户信息的用户类
    /// </summary>
   public class OnlineUser
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int ID { set; get; }
        /// <summary>
        /// 用户ip
        /// </summary>
        public byte[] IP{set;get;}
        public String Name { set; get; }
        public String Nickname { set; get; }

        public bool IsOnline { set; get; }
        /// <summary>
        /// 用户活力值，每次检测用户将会减掉一个活力值，当用户强服务端发出连接时，将恢复活力值，活力值为0视为下线并将其从在线列表中移除
        /// </summary>
        private byte vitality;

        public static byte Max_Vitality=5;
       /// <summary>
       /// 这个有点难搞，需要记录当前用户将其读到哪儿了,发送人的id，消息
       /// </summary>
        private List<pair<int, OnlineUserMessge>> msgList;//消息列表
        private int current_index=0;
        /// <summary>
        /// 构造函数，完成用户从数据库中提取所有相关信息(图片要不要呢？还是另外要？)
        /// </summary>
        /// <param name="id"></param>
        public OnlineUser(int user_id,byte[] user_ip,String name,String nickname)
        {
            ID = user_id;
            IP = user_ip;
            vitality = Max_Vitality;
            IsOnline = true;
            Name = name;
            Nickname = nickname;
            msgList = new List<pair<int, OnlineUserMessge>>();
            LoadMessageFromDatabase();
        }
       public User ToUser(bool IsImageNeed=false)
        {
            User ue = Program.SqlManager.getUserById(ID, IsImageNeed);
            ue.ip = IP;
            return ue;
        }
        /// <summary>
        /// 检测自己是否要下线，将返回一个可能是空的Message List
        /// </summary>
        /// <returns></returns>
        public List<pair<int, OnlineUserMessge>> OnlineCheck()
        {
            List<pair<int, OnlineUserMessge>> l_return = new List<pair<int, OnlineUserMessge>>();
            //活力值减为零
            if((--vitality)==0)
            {
                l_return.Add(new pair<int, OnlineUserMessge>(ID, Offline()));
                IsOnline = false;
            }
            return l_return;
        }

        public void VitalityRefresh()
        {
            vitality = Max_Vitality;
        }

       public void first_login_max_vitality()
        {
            vitality =150;
        }
        /// <summary>
        /// 完成对在线用户的离线操作,返回一个该用户下线的UserMessage
        /// 离线操作包括将自己的消息盒子消息选择性的存入数据库，并清空自己的消息，
        /// </summary>
        private OnlineUserMessge Offline()
        {
            msgList.Clear();
            UploadMessageToDatabase();
            return new OnlineUserMessge(2, ID,MySQL_Manager.getTimestampOfNow());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userMessage"></param>
        internal void ReciveMessage(int sender_id, OnlineUserMessge userMessage)
        {
            msgList.Add(new pair<int,OnlineUserMessge>(sender_id,userMessage));
        }

        
/// <summary>
        /// 返回列表中的一条UserMessage,估计要改
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public UserMessage getMessage(int index)
        {
            return msgList[index].Value;
        }
       /// <summary>
       /// 删除列表中指定消息id的消息
       /// </summary>
       /// <param name="_id"></param>
       /// <returns></returns>
       public bool removeMsgById(int _id)
        {
            bool result = false;
            pair<int, OnlineUserMessge> t=new pair<int,OnlineUserMessge>();
            foreach(var item in msgList)
            {
                if(item.Value.id==_id)
                {
                    msgList.Remove(t);
                    result = true;
                    --current_index;
                    break;
                }
            }
           if(result)
           {
                //msgList.Remove(t);
                //--current_index;
           }
                
            return result;
        }


        /// <summary>
        /// 根据传入的用户id从数据库中载入消息列表
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns>是否成功</returns>
        public void LoadMessageFromDatabase()
        {
            try
            {
                msgList.AddRange(Program.SqlManager.getMessageToUser(ID));
                int index = 0;
                //重新排id
                foreach(var item in msgList)
                {
                    item.Value.id = index;
                    ++index;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("error in LoadMessageFromDatabase,{0}", e.Message);
            }
        }
        /// <summary>
        /// 将
        /// </summary>
        /// <param name="user_id"></param>
        public void UploadMessageToDatabase()
        {
            try
            {
                //在此对msglist做一个过滤，滤掉一些不需要存在数据库中的消息
                //。。。。

                Program.SqlManager.storeOnlineMessagesOfUser(ID, msgList);
                msgList.Clear();
            }
            catch (Exception e)
            {
                Console.WriteLine("error in {1},{0}", e.Message, e.TargetSite.ToString());
            }
        }
       /// <summary>
       /// 返回用户请求的消息，并将当前消息索引更新
       /// </summary>
       /// <returns></returns>
        internal List<UserMessage> getUserMessageList()
        {
            List<UserMessage> re=new List<UserMessage>();
            for (;current_index < msgList.Count; ++current_index)
            {
                re.Add(msgList[current_index].Value.ToUserMessage());
            }
            return re;
        }

        
    }


    /// <summary>
    /// 静态类，完成对在线用户及其消息的管理
    /// </summary>
    public  class OnlineUserManager
    {

         static OnlineUserManager()
        {
            initi();
        }
        /// <summary>
        /// 在线用户列表
        /// </summary>
         public static Dictionary<int, OnlineUser> user_list = new Dictionary<int, OnlineUser>();
        /// <summary>
        /// 检测在线的函数运行周期（ms）
        /// </summary>
        public static int IntervalOfCheck { set; get; }

        /// <summary>
        /// 某些初始化
        /// </summary>
        public static void initi()
        {
            IntervalOfCheck = 1000;
            OnlineUser.Max_Vitality = 15; //十五秒无交流视为离线
        }

        /// <summary>
        /// 检测在线与否，活力值递减,离线之后将会分发消息
        /// </summary>
        public static void UserOnlineCheck(object o)
        {
            while (true)
            {
                Thread.Sleep(IntervalOfCheck);
                try
                {
                    Console.WriteLine("当前在线人数{0}", user_list.Count);


                     //锁定用户列表，防止线程访问冲突
                    lock (user_list)
                    {
                        //要下线的用户索引
                        List<int> index_l = new List<int>();
                        //临时消息盛放列表
                        List<pair<int, OnlineUserMessge>> m_l = new List<pair<int, OnlineUserMessge>>();
                        foreach(var item in user_list.Values)
                        {
                            m_l.AddRange(item.OnlineCheck());
                            if (!item.IsOnline)
                            {
                                index_l.Add(item.ID);
                            }
                        }
                        //将临时消息列表分配
                        foreach(var item in m_l)
                        {
                            foreach(var item1 in user_list.Values)
                            {
                                if(Program.SqlManager.isFriend(item1.ID,item.Key))
                                {
                                    user_list[item1.ID].ReciveMessage(item.Key, item.Value);
                                }
                            }
                        }
                        //从user_list中删去离线用户
                        foreach (var item in index_l)
                        {
                            Console.WriteLine("用户{0}下线",item);
                            user_list.Remove(item);
                        }
                        //清空临时消息列表
                        m_l.Clear();

                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("处理类UserOnlineCheck函数中出现异常,{0}", e.Message);
                }
            }
        }
        /// <summary>
        /// data[3]{请求人id,被请求人id,0或1(拒绝与否)},返回值表示请求人在线与否
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool dealToMakeFriend(int[] data)
        {
            bool result = false;
            if(data[2]==1)//同意
            {
                Program.SqlManager.makeFriend(data);
            }

            List<object> re1 = new List<object>(2);
            re1.Add(Program.SqlManager.getUserById(data[1],true));
            re1.Add(data[2]);
            OnlineUserMessge re = new OnlineUserMessge(6, re1);
            if(user_list.ContainsKey(data[0]))//请求人在线
            {
                user_list[data[0]].ReciveMessage(data[1],re);
                result = true;
            }
            else //不在
            {
                List<pair<int, OnlineUserMessge>> ll = new List<pair<int, OnlineUserMessge>>();
                ll.Add(new pair<int, OnlineUserMessge>(data[1], re));
                Program.SqlManager.storeOnlineMessagesOfUser(data[0], ll);
            }
            return result;
        }

        public static void addOnlineUser(User user)
        {
            addOnlineUser(new OnlineUser(user.user_id, user.ip,user.name,user.nickname));
        }
        /// <summary>
        /// 上线之后通知所有在线好友
        /// </summary>
        /// <param name="user"></param>
        public static void addOnlineUser(OnlineUser user)
        {
            
            foreach(var item in user_list.Values)
            {
                if(Program.SqlManager.isFriend(item.ID,user.ID))
                {
                    item.ReciveMessage(user.ID, new OnlineUserMessge(1, new pair<int, byte[]>(user.ID,user.IP), MySQL_Manager.getTimestampOfNow()));
                }
            }
            user.first_login_max_vitality();
            user_list.Add(user.ID,user);

        }

        /// <summary>
        /// 返回很多用户的信息和群组的信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ReturnData getManyData(GetData data)
        {
            ReturnData r_d = new ReturnData();
            foreach(var item in data.FriendIDList)
            {
                r_d.FriendList.Add(Program.SqlManager.getUserById(item, true));
            }
            foreach (var item in data.GroupIDList)
            {
                r_d.GroupList.Add(Program.SqlManager.getGroupById(item));
            }
            return r_d;
        }

    }
}
