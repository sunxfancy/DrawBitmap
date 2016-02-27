using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using DrawBitmap;
using MulticastNetWork;

//为测试而引用的东西

namespace Server
{
    internal class Program
    {
        /// <summary>
        /// 服务器的ip
        /// </summary>
        private static string sip;

        /// <summary>
        /// 是否是debug模式
        /// </summary>
        private static  bool isdebug = false;

        /// <summary>
        /// 控制服务器线程
        /// </summary>
        private static Boolean run = true;

        /// <summary>
        /// 与数据库交互的方法类
        /// </summary>
        public static MySQL_Manager SqlManager;

        /// <summary>
        /// 最最开始的一个全局变量的初始化
        /// </summary>
        private static void initi()
        {
           // sip = "172.16.49.192";

           // sip = "192.168.113.168";
            isdebug = true;
            run = true;
            SqlManager = new MySQL_Manager();

        }
        private static void Main(string[] args)
        {
            if (args.Length != 0)
                sip = args[0];
            else
                sip = "127.0.0.1";
           sip = "172.16.49.192";
            initi();
            //服务器部分
            var Server = new ThreadPoolServer(sip, 9999);
            Server.accept += server_accept;
            Server.start();
            Console.WriteLine("ServerRun"+sip);
            //更新在线人数id列表
            //var sThread = new Thread(UserOnlineCheck);
            //sThread.Start();

            //开始....
            var sThread1 = new Thread(OnlineUserManager.UserOnlineCheck);
            sThread1.Start();
        }

        /// <summary>
        /// 服务器监听函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void server_accept(object sender, ServerAcceptEventArgs args)
        {
            NetworkStream Ns = args.client.GetStream();
            var Formatter = new BinaryFormatter();
            Object obj = Formatter.Deserialize(Ns);
            var Message = (UserMessage) obj;
            byte[] current_client_ip = UserExt.IPStringToByte(((System.Net.IPEndPoint)(args.client).Client.RemoteEndPoint).Address.ToString());
            if(isdebug)
            {
                Console.WriteLine("Client accept!");
                Console.WriteLine("client ip：" + UserExt.IPByteToString(current_client_ip));
            }
            

            //获得数据，不同请求类型对应不同的数据
            Object Data = Message.data;

            //定义返回数据
            Object ReturnObject = null;
            switch (Message.type)
            {
                case 1://注册功能实现 //添加个人信息至数据库 //返回id给客户端done
                    ReturnObject =UserRegist((RegisterData) Data);
                    break;
                case 2://登陆功能实现，done
                    ReturnObject = UserLogin((LoginData)Data,current_client_ip);
                    break;
                case 3: //确认在线,//更新活力值接收在线消息，done
                    ReturnObject = HoldLineAndGetMessage((int)Data);
                    break;
                case 4://邀请好友使用某插件
                    ReturnObject = InviteFriendToUsePlugin((List<int>)Data);
                    break;
                case 5://注册用户名校验，done
                    ReturnObject = CheckUsernameRegistable((string)Data);
                    break;
                case 6://搜索符合条件的好友,目前只是按全名字匹配搜索 = =
                    List<User> list_u= SearchUserByName(Data as string);
                    foreach(var item in list_u)
                        if(isOnline(item.user_id))
                            item.ip = OnlineUserManager.user_list[item.user_id].IP;
                    ReturnObject = list_u;
                    break;
                case 7://添加好友
                    ReturnObject = AskToBeFriend((AddFriendData)Data);
                    break;
                case 8: //用户更新资料
                    ReturnObject = UserInfoSlefUpdate((User)Data);
                    break;
                case 9://修改密码 ,如果原密码正确才进行修改密码操作，否则返回-1
                    ReturnObject = UserPasswordModify((UpdatePasswordData)Data);
                    break;
                case 10://获取指定id用户的所有信息
                    User ur=SqlManager.getUserById((int)Data, true);
                    if(isOnline(ur.user_id))
                        ur.ip=OnlineUserManager.user_list[ur.user_id].IP;
                    ReturnObject = ur;
                    break;
                case 11://根据id获取群组的的数据信息
                    ReturnObject = SqlManager.getGroupById((int)Data);
                    break;
                case 12://批量获取用户和群组的信息
                    ReturnObject = "尼玛什么都没有行了吧";
                    break;
                case 13://处理同意加某人为好友 myid friendid
                    ReturnObject = OnlineUserManager.dealToMakeFriend((int[])Data);
                    break;
                case 14://确认已经读过某消息
                    ReturnObject=ConfirnUserMessage((int[])Data);
                    break;
                case 15://创建新群组
                    List<object> temp_pram = (List<object>)Data;
                    ReturnObject = SqlManager.createAGroup((int)(temp_pram[0]), (String)(temp_pram[1]), (String)(temp_pram[2]));
                    break;
            }
            try
            {
                Formatter.Serialize(Ns, ReturnObject);//将要返回的对象串行化并放入数据流
            }
            catch(Exception e)
            {
                Console.WriteLine("{0}-唉，{1}", Message.type, e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private static bool ConfirnUserMessage(int[] p)
        {
            bool result=false;
            try
            {
                Console.WriteLine("用户{0}的{1}消息", p[0], p[1]);
                OnlineUserManager.user_list[p[0]].removeMsgById(p[1]);
                result = true;
            }
            catch(Exception e)
            {
                Console.WriteLine("error in {0},{1}", e.TargetSite, e.Message);
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 根据用户名返回User，不存在则返回null
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private static List<User> SearchUserByName(string username)
        {
            if (isdebug)
            {
                Console.WriteLine("request for some info of user by name :" + username);
            }
            try
            {
                 return SqlManager.getUserByName(username);
            }
            catch(Exception e)
            {
                Console.WriteLine("error in SearchUserByName,{0}", e.Message);
                return null;
            }
        }


        /// <summary>
        /// 处理用户注册，成功则返回用户id，否则id为-1
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        private static int UserRegist(RegisterData Data)
        {
            int return_int = -1;
            try
            {
                RegisterData Rdata = (RegisterData)Data;
               
                return_int = CreateNewUser(Rdata.user, Rdata.password);
            }
            catch (Exception e)
            {
                return_int = -1;
                Console.WriteLine("error in UserRegist,{0}", e.Message);
            }
            if (isdebug)
            {
                Console.WriteLine("id of New User:{0}", return_int);
            }
            return return_int;
        }

        /// <summary>
        /// 处理用户登录的一些列操作，登陆不成功返回null，成功则返回一个LoginReturn
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static LoginReturn UserLogin(LoginData msg,byte[] login_ip)
        {
            //业务逻辑：
            //第一步，检测是否满足登陆条件，如果已经上线呵呵NULL，满足则进入第二步，否则返回NULL
            //第二步，将自己加入在线列表，并将自己在线的消息通知给在线好友
            //第三步获取对登陆者来说要进行更新的好友列表并返回LoginReturn
            if (isdebug) Console.WriteLine("{0}--{1}",msg.username,msg.password);
           LoginReturn info_return=new LoginReturn();
           User temp_user=new User();
           int login_id=SqlManager.checkUser(msg.username,msg.password,ref temp_user);
           
            if (login_id!=-1)//是否满足登陆条件
            {
                if(isOnline(login_id))
                {
                    if(isdebug)Console.WriteLine("已登录{0}", login_id);
                    return null;
                }
                if(isdebug)
                {
                    Console.WriteLine("登录验证成功,用户id是{0}",login_id);
                }
                try
                {
                //登记在线
                //Idonline_Dic.Add(login_id,init_Vitality);
                    OnlineUserManager.addOnlineUser(new OnlineUser(login_id, login_ip,temp_user.name,temp_user.nickname));
                //记录ip
               // Ip_Dic.Add(login_id,login_ip);
                //获取在线好友列表(用于返回给该用户),通知在线好友该用户在线了
                info_return.OnlineFriends = getOnlineFriends(login_id);

                //获取该用户的消息，一个用户刚刚上线，应该从数据库获取才对

                info_return.message = OnlineUserManager.user_list[login_id].getUserMessageList();
                info_return.FriendsTimeStamp = SqlManager.getFriendsTimestamp(login_id);
                info_return.GroupTimeStamp = SqlManager.getGroupTimeStamp(login_id);
                info_return.myTimeStamp = SqlManager.getUserTimestamp(login_id);
    
                }
                catch(Exception e)
                {
                    Console.WriteLine("构建返回值时出现问题,{0}", e.Message);
                }
           }
          else
          {
              if (isdebug)
              {
                  Console.WriteLine("if({0}<0) then 密码搞错了 else 已经登录过了", login_id);
              }
              info_return = null;
          }
          return info_return;
        }
        /// <summary>
        /// 返回id用户在线好友列表,这里的是否是好友的判断需要访问数据库
        /// </summary>
        /// <param name="login_id"></param>
        /// <returns></returns>
        private static List<pair<int,byte[]>> getOnlineFriends(int login_id)
        {
            List<pair<int, byte[]>> return_list = new List<pair<int, byte[]>>();
            foreach(var item in OnlineUserManager.user_list)
            {
                if(item.Value.IsOnline)
                {
                    if(SqlManager.isFriend(login_id,item.Key))
                        return_list.Add(new pair<int, byte[]>(item.Key,item.Value.IP));
                }
            }
            return return_list;
        }


        private static List<UserMessage> HoldLineAndGetMessage(int id)
        {
            List<UserMessage> return_list=null;
            if(isdebug)
            {
                Console.WriteLine("用户{0}请求获取在线消息",id);
            }
            try
            {
                OnlineUserManager.user_list[id].VitalityRefresh();
                return_list = OnlineUserManager.user_list[id].getUserMessageList();
                if (return_list.Count == 0)
                    Console.WriteLine("在估计本来就没有更新的了");
            }
            catch(Exception e)
            {
                Console.WriteLine("在HoldLineAnd...出错{0}", e.Message);
            }
            return return_list;
        }

        private static bool InviteFriendToUsePlugin(List<int> list)
        {
            bool result = false;
            try
            {
                int Plugin_id = list[0];
                int invitor_id = list[1];
                List<int> msg=new List<int>();
                msg.Add(Plugin_id);
                msg.Add(invitor_id);
                for (int i = 2; i < list.Count; i++)//遍历每个好友id,放入消息
                    {
                        if(isOnline(list[i]))
                        {
                            //Message_Dic[list[i]].Add(new UserMessage(3,msg));
                            OnlineUserManager.user_list[list[i]].ReciveMessage(invitor_id, new OnlineUserMessge(3,msg,MySQL_Manager.getTimestampOfNow()));
                            result = true;
                        }
                    }
            }
            catch(Exception e)
            {
                result = false;
                Console.WriteLine("error in InviteFriendToUsePlugin,{0}",e.Message);
            }
            return result;
         }

        /// <summary>
        /// 检测用户名是否为可注册的
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private static bool CheckUsernameRegistable(String username)
        {
            try
            {
                if (isdebug)
                {
                    Console.WriteLine("询问  '" + username + "' 是否可用");
                }
                return  SqlManager.isUsernameExist(username);
            }
            catch (Exception e)
           {
               Console.WriteLine("error in CheckUsernameRegistable in   case 5,details:{0}", e.Message);
                return false;
            }
         }
        /// <summary>
        /// 处理好友申请消息
        /// </summary>
        /// <param name="afd"></param>
        private static bool AskToBeFriend(AddFriendData afd)
        {
            bool result = false;
            if (isdebug)
            {
                Console.WriteLine("用户{0}要加用户{1}为好友",afd.id,afd.f_id);
             }
            try
            {
                OnlineUserManager.user_list[afd.id].VitalityRefresh();
                //用户afd.id请求加用户afd.f_id为好友，将给用户afd.f_id添加4号S-B消息
                lock (OnlineUserManager.user_list)
                {
                    Console.WriteLine("这里的在线人数{0}", OnlineUserManager.user_list.Count);
                    OnlineUserMessge afdMsg = new OnlineUserMessge(4, OnlineUserManager.user_list[afd.id].ToUser());
                    if (isOnline(afd.f_id))//在线就直接放到人家的消息列表，离线将消息串行化存入数据库
                    {
                        if (isdebug)
                        {
                            Console.WriteLine("用户{0}在线，Ta的消息容器", afd.f_id);
                        }
                        OnlineUserManager.user_list[afd.f_id].ReciveMessage(afd.id, afdMsg);
                    }
                    else
                    {
                        if (isdebug)
                        {
                            Console.WriteLine("用户{0}不在线，塞入数据库", afd.f_id);
                        }
                        List<pair<int, OnlineUserMessge>> ll = new List<pair<int, OnlineUserMessge>>();
                        ll.Add(new pair<int, OnlineUserMessge>(afd.id, afdMsg));
                        SqlManager.storeOnlineMessagesOfUser(afd.f_id, ll);
                        result = true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error in AskToBeFriend..{0}", e.Message);
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 用户信息自我更新
        /// </summary>
        /// <param name="user"></param>
        private static bool UserInfoSlefUpdate(User user)
        {
           // SqlManager.showUser(user);
            bool result=false;
            if(isdebug)
            {
                Console.WriteLine("用户{0}要更新Ta的信息", user.name);
            }
            try
            {
                 //把信息存入数据库
                 SqlManager.UpdateUserInfo(user);
                 result = true;
            }
            catch(Exception e)
            {
                Console.WriteLine("error in UserInfoSlefUpdate,{0}",e.Message);
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 响应用户密码更改请求
        /// </summary>
        /// <param name="upd"></param>
        /// <returns></returns>
        private static bool UserPasswordModify(UpdatePasswordData upd)
        {
            if(isdebug)
            {
                Console.WriteLine("用户(id:{0})要改密码", upd.id);
            }
            try
            {
                return SqlManager.modifyPassword(upd.id, upd.oldPassword, upd.newPassword);
            }
            catch(Exception e)
            {
                Console.WriteLine("error in UserPasswordModify{0}", e.Message);
                return false;
            }
             
        }


        /// <summary>
        /// 确认用户是否在线
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static Boolean isOnline(int id)
        {

            return OnlineUserManager.user_list.ContainsKey(id);
        }

        /// <summary>
        /// 响应用户注册，返回用户id
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private static int CreateNewUser(User user, string password)
        {
            int id;
            //操作数据库，添加一个用户的数据
            try
            {
                id = SqlManager.CreateNewUser(user,password);
            }
            catch(Exception e)
            {
                Console.WriteLine("error in addNewPerson,{0}", e.Message);
                id = -1;
            }
            return id;
        }
    }
}