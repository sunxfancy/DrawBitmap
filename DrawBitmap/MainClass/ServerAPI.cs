using MulticastNetWork;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawBitmap.MainClass
{
    public class ServerAPI
    {
        public static void InitServerAPI()
        {
            //不要在这里设置ip

            ServerAPI.ServerIP = App.ip;
            ServerAPI.ServerPort = 9999;
            ServerAPI.client = new SendingClient(ServerAPI.ServerIP, ServerAPI.ServerPort);
        }

        public static object _l = new object();
        /// <summary>
        ///  注册 
        ///  发送格式：UserMessage(1, RegisterData(User(1, null, username, nickname),password);)
        ///  接收格式：int，注册成功返回用户id，否则返回-1
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="nickname"></param>
        /// <returns></returns>
        public static int Register(string username,string password,string nickname)
        {
            User user = new User(1, null, username, nickname);
            RegisterData registerdata = new RegisterData(user,password);

            lock(_l){
            if (ServerAPI.client.Connect())
            {
                int result = (int)client.Sending(new UserMessage(1, registerdata));
                client.Close();
                return result;
            }
            else 
                return 0;
            }
        }

        /// <summary>
        /// 登录
        /// 发送格式：UserMessage(2, LoginData(username, password))
        /// 接收格式：LoginReturn,要求LoginReturn各成员变量不为null
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static LoginReturn Login(string username, string password)
        {
            LoginData ld = new LoginData(username, password);
            lock (_l)
            {
                if (client.Connect())
                {
                    LoginReturn result = client.Sending(new UserMessage(2, ld)) as LoginReturn;
                    client.Close();
                    return result;
                }
            }
            return null;
        }

        /// <summary>
        /// 确认在线操作，并获取自己在服务器端消息盒子里的新消息
        /// 发送格式：UserMessage(3, id)
        /// 接收格式：List&lt;UserMessage&gt;
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<UserMessage> Checking(int id)
        {
            try
            {
                var user = new UserMessage(3, id);
                lock (_l)
                {
                    if (client.Connect())
                    {
                        List<UserMessage> message = client.Sending(user) as List<UserMessage>;
                        return message;
                    }
                    else
                        return null;
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return null;
            }
            finally
            {
                client.Close();
            }
        }

        /// <summary>
        /// 邀请好友
        /// 发送格式：UserMessage(4, List&lt;int&gt;); List第一项为插件id，第二项为邀请人id，接下来的都是被邀请人id
        /// 接收格式：bool 消息成功发送true，消息发送失败false
        /// </summary>
        /// <param name="id_list"></param>
        /// <returns></returns>
        public static bool InviteFriends(int plugin_id,int my_id,SortedSet<int> id_list)
        {
            bool result = false;
            try { 
                var list =new List<int>();
                list.Add(plugin_id);
                list.Add(my_id);
                list.AddRange(id_list);
                var user = new UserMessage(4, list);
                lock (_l)
                {
                    if (client.Connect())
                    {
                        result = (bool)client.Sending(user);
                        client.Close();
                    }
                }
            }
            catch(Exception e){
                
                return false;
            }
            return result;
        }

        /// <summary>
        /// 测试名字是否可用
        /// 发送格式：UserMessage(5, name)
        /// 接收格式：bool 是true ，否false
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int TestName(string name)
        {
            try
            {
                lock (_l)
                {
                    if (client.Connect())
                    {
                        UserMessage m = new UserMessage(5, name);
                        bool ans = (bool)client.Sending(m);
                        client.Close();
                        if (ans)
                            return 1;
                        else
                            return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        /// <summary>
        /// 根据名字查找好友
        /// 发送格式：UserMessage(6,name);
        /// 接收格式：List&lt;User&gt;
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<User> SearchFriends(string name)
        {
            try {
                lock (_l)
                {
                    if (client.Connect())
                    {
                        UserMessage m = new UserMessage(6, name);
                        List<User> user_list = client.Sending(m) as List<User>;
                        client.Close();
                        return user_list;
                    }
                    else return null;
                }
            }catch(Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// 加好友（请求人id, 被请求人id）
        /// 发送格式：UserMessage(7, AddFriendData(请求人id, 被请求人id));
        /// 接收格式：bool 消息成功发送true，消息发送失败false
        /// </summary>
        /// <param name="my_id"></param>
        /// <param name="f_id"></param>
        /// <returns></returns>
        public static bool AddFriend(int my_id ,int f_id)
        {
            bool result = false;
            try
            {
                lock (_l)
                {
                    if (client.Connect())
                    {
                        UserMessage m = new UserMessage(7, new AddFriendData(my_id, f_id));
                        result = (bool)client.Sending(m);
                        client.Close();
                    }
                }
            }
            catch(Exception e)
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 更新我自身的数据，向服务器提交我的最新数据
        /// 发送格式：UserMessage(8, User);
        /// 接收格式：是否更改成功，true or false
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdataMyInfo(Friend data)
        {
            bool result = false;
            var user = new User(data);
            lock (_l)
            {
                if (client.Connect())
                {
                    UserMessage m = new UserMessage(8, user);
                    result = (bool)client.Sending(m);
                    client.Close();
                }
            }
            return result;
        }

        /// <summary>
        /// 更新我自身的数据，向服务器提交我的最新数据
        /// 发送格式：UserMessage(8, User);
        /// 接收格式：是否更改成功，true or false
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdataMyInfo(User data)
        {
            lock (_l)
            {
                if (client.Connect())
                {
                    UserMessage m = new UserMessage(8, data);
                    client.Sending(m);
                    client.Close();
                    return true;
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// 更新Password（用户id，旧密码，新密码）
        /// 发送格式：UserMessage(9, UpdatePasswordData(id, oldp, newp));
        /// 接收格式：
        /// </summary>
        /// <param name="id"></param>
        /// <param name="oldp"></param>
        /// <param name="newp"></param>
        /// <returns></returns>
        public static int UpdataMyPassword(int id,string oldp,string newp)
        {
            var data = new UpdatePasswordData(id, oldp, newp);
            lock (_l)
            {
                if (client.Connect())
                {
                    UserMessage m = new UserMessage(9, data);
                    int result;
                    if ((bool)client.Sending(m))
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                    client.Close();
                    return result;
                }
                else
                    return -1;
            }
        }


        /// <summary>
        /// 根据id获取指定人的全部数据
        /// type 10
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static User GetFriendData(int id)
        {
            try
            {
                lock (_l)
                {
                    if (client.Connect())
                    {
                        UserMessage m = new UserMessage(10, id);
                        User user = client.Sending(m) as User;
                        client.Close();
                        return user;
                    }
                    else return null;
                }
            }catch(Exception e)
            {
                return null;
            }
        }


        /// <summary>
        /// 获取指定群组的信息
        /// type 11
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static GroupData GetGroupData(int id)
        {
            try
            {
                lock (_l)
                {
                    if (client.Connect())
                    {
                        UserMessage m = new UserMessage(11, id);
                        GroupData user = client.Sending(m) as GroupData;
                        client.Close();
                        return user;
                    }
                    else return null;
                }
            }catch(Exception e)
            {
                return null;
            }
        }


        /// <summary>
        /// 获取一组人的数据
        /// 发送格式：UserMessage(12, GetData())
        /// 接收格式：ReturnData
        /// </summary>
        /// <param name="friend_list"></param>
        /// <param name="group_list"></param>
        /// <returns></returns>
        public static ReturnData GetGroupData(List<int> friend_list,List<int> group_list)
        {
            try
            {
                GetData data = new GetData();
                data.FriendIDList = friend_list;
                data.GroupIDList = group_list;
                lock (_l)
                {
                    if (client.Connect())
                    {
                        UserMessage m = new UserMessage(12, data);
                        ReturnData user = client.Sending(m) as ReturnData;
                        client.Close();
                        return user;
                    }
                    else return null;
                }
            }catch(Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// 回应一个增加好友的请求
        /// 发送格式：UserMessage(13, int[3]{请求人id,被请求人id,0或1(拒绝与否)});
        /// 接收格式：bool，成功或失败
        /// </summary>
        /// <param name="my_id"></param>
        /// <param name="friend_id"></param>
        /// <returns></returns>
        public static bool AgreeAddFriend(int my_id,int friend_id,int attitude)
        {
            
            bool result = false;
            try
            {
                lock (_l)
                {
                    if (client.Connect())
                    {
                        UserMessage m = new UserMessage(13, new int[] { my_id, friend_id, attitude });
                        result = (bool)client.Sending(m);
                        client.Close();
                    }
                    return result;
                }
            }catch (Exception e)
            {
                return false;
            }
        }
        /// <summary>
        /// 确认某条通知类信息已经处理过了
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="msg_id"></param>
        /// <returns></returns>
        public static bool confirmMessage(int user_id,int msg_id)
        {
            bool result = false;
            try
            {
                lock (_l)
                {
                    if (client.Connect())
                    {
                        UserMessage m = new UserMessage(14, new int[] { user_id, msg_id });
                        result = (bool)client.Sending(m);
                        client.Close();
                    }
                }
            }catch (Exception e)
            {
                return false;
            }
            return result;
        }


        public static SendingClient client;
        public static string ServerIP;
        public static int ServerPort;

        /// <summary>
        /// 创建新群组 p1{创建者}
        /// </summary>
        /// <param name="p1"></param>
        internal static bool newAGroup(List<object> p1)
        {
            bool result = false;
            try
            {
                lock (_l)
                {
                    if (client.Connect())
                    {
                        UserMessage m = new UserMessage(15,p1);
                        result = (bool)client.Sending(m);
                        client.Close();
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return result;
        }
    }
}
