using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using DrawBitmap;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using MulticastNetWork;
using System.Data;
namespace Server
{
    class MySQL_Manager
    {
        private MySqlConnection conn; //用来连接数据库的对象--获取数据
        private MySqlConnection conn4Query; //用来连接数据库的对象---更新数据库
        private MySqlConnection conn4FriendCheck; //用来连接数据库的对象---查看好友关系

        /// <summary>
        /// 构造函数，基本上不用加参数，默认的就够用了
        /// </summary>
        /// <param name="DataSource"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public MySQL_Manager(String DataSource = "localhost", String username = "Asher", String password = "lalala")
        {
            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = "Data Source='" + DataSource + "';UserId='" + username + "';Password='" + password + "';";
                conn4Query = new MySqlConnection();
                conn4Query.ConnectionString = "Data Source='" + DataSource + "';UserId='" + username + "';Password='" + password + "';";
                conn4FriendCheck = new MySqlConnection();
                conn4FriendCheck.ConnectionString = "Data Source='" + DataSource + "';UserId='" + username + "';Password='" + password + "';";
                Open();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("构造函数中出现异常，请确认三个参数是否正确\n详细：" + ex.Message.ToString());
            }
        }

        /// <summary>
        /// 打开与数据库之间的连接，一般情况下需要获取数据都需要先打开连接
        /// </summary>
        private void Open()
        {
            try
            {
                conn.Open();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("开启连接时候出现异常\n详细：" + ex.Message.ToString());
            }
        }
        /// <summary>
        /// 关闭数据库的连接，到最后我竟然没用上
        /// </summary>
        private void Close()
        {
            conn.Close();
        }

        // 专门执行Query的连接的开启
        private void QueryOpen()
        {
            try
            {
                conn4Query.Open();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("开启连接时候出现异常\n详细：" + ex.Message.ToString());
            }
        }

        //专门执行Query的连接的关闭
        private void QueryClose()
        {
            this.conn4Query.Close();
        }


        //专门用于检测是否是朋友关系的连接的开启
        private void FriendCheckOpen()
        {
            try
            {
                conn4FriendCheck.Open();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("开启连接时候出现异常\n详细：" + ex.Message.ToString());
            }
        }

        //专门用于检测是否是朋友关系的连接的关闭
        private void FriendCheckClose()
        {
            this.conn4FriendCheck.Close();
        }

        /// <summary>
        /// 根据用户名得到用户id
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int getUserId(String username)
        {
            int id_return = 0;
            MySqlCommand mycm = new MySqlCommand("select id from drawtogether.user where name='" + username + "';", conn);
            MySqlDataReader s = mycm.ExecuteReader();
            while (s.Read())
            {
                if (s.HasRows)
                {
                    id_return = s.GetUInt16(0);
                }
            }
            s.Close();
            mycm.Dispose();
            return id_return;
        }

        /// <summary>
        /// 返回用户信息，索引从0开始,返回num个
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        //待修改
        public List<DrawBitmap.User> getUserList(int startIndex, int endIndex)
        {
            List<DrawBitmap.User> List_return = new List<DrawBitmap.User>();
            MySqlCommand mycm = new MySqlCommand("select id,name,nickname from drawtogether.user limit " + startIndex + "," + endIndex + ";", conn);
            MySqlDataReader s = mycm.ExecuteReader();
            while (s.Read())
            {
                if (s.HasRows)
                {
                    List_return.Add(new DrawBitmap.User(s.GetInt16(0), null, s.GetString(1), s.GetString(2)));
                }
            }
            s.Close();
            mycm.Dispose();
            return List_return;
        }

        //判断这俩id是不是好友，是则返回true
        public bool isFriend(int from_id, int to_id)
        {
            bool needColse = false;
            if (conn4FriendCheck.State == ConnectionState.Closed)
            {
                conn4FriendCheck.Open();
                needColse = true;
            }
            bool result = false;
            String sql = "select count(*) from drawtogether.friendship where   from_id =" + from_id + " and to_id =" + to_id + " ;";
            //Console.WriteLine(sql);
            MySqlCommand mycm = new MySqlCommand(sql, this.conn4FriendCheck);
            MySqlDataReader s = mycm.ExecuteReader();
            if (s.Read())
                if (s.HasRows)
                    if (s.GetInt16(0) != 0)
                        result = true;
            s.Close();
            mycm.Dispose();
            if (needColse)
            {
                conn4FriendCheck.Close();
            }
            return result;
        }

        //返回好友列表，从0开始，若endIndex给的很大，那么就是所有好友了，可用
        public List<DrawBitmap.User> getFriendList(int user_id, int startIndex, int endIndex)
        {
            List<DrawBitmap.User> List_return = new List<DrawBitmap.User>();
            this.FriendCheckOpen();
            MySqlCommand mycm = new MySqlCommand("select id,name,nickname from drawtogether.user limit " + startIndex + "," + endIndex + ";", conn);
            MySqlDataReader s = mycm.ExecuteReader();
            while (s.Read())
            {
                if (s.HasRows)
                {
                    if (isFriend(user_id, s.GetInt16(0)))
                        List_return.Add(new DrawBitmap.User(s.GetInt16(0), null, s.GetString(1), s.GetString(2)));
                }
            }
            s.Close();
            mycm.Dispose();
            this.FriendCheckClose();
            return List_return;
        }

        //备注要稍后改么？，接受一个数组，里边放俩id
        public bool makeFriend(int[] kk)
        {
            try
            {
                int from_id = kk[0];
                int to_id = kk[1];
                if(isFriend(from_id,to_id))
                {
                    return false;
                }
                string sql = "INSERT INTO  `drawtogether`.`friendship` (`from_id` ,`to_id` ,`to_id_remark`)" +
                    "VALUES ('" + from_id + "',  '" + to_id + "',  ''), ('" + to_id + "',  '" + from_id + "',  '');";
                runSQL(sql);
            }
            catch (Exception e)
            {
                Console.WriteLine("error in {0},{1}", e.TargetSite, e.Message);
                return false;
            }
            return true;
        }

        //创建一个新用户，返回新用户id,可用,
        public int CreateNewUser(DrawBitmap.User user, string paswd)
        {
            return CreateNewUser(user.name, paswd, user.nickname);
        }

        //重载一下而已，新建用户的两个必需数据，返回新用户id
        public int CreateNewUser(String username, String paswd, String nickname)
        {
            String sql = "INSERT INTO  `drawtogether`.`user` (`name`,`password`,`nickname`)"
                                                                    + "VALUES ('" + username + "','" + paswd + "','" + nickname + "');";
            runSQL(sql);
            int id = getUserId(username);
            return id;
        }

        public static int getTimestampOfNow()
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(DateTime.Now - startTime).TotalSeconds;
        }


        //更新用户信息()不包括xUserExt,返回当前新创建用户的id，可用
        public void UpdateUserInfo(DrawBitmap.User user)
        {
            String sql = "UPDATE  `drawtogether`.`user` SET "
                 + " `name` =  '" + user.name + "',"
                 + " `nickname` =  '" + user.nickname + "',"
                 + "`Level` =  '" + user.ext.Level + "',"
                 + "`Introduce` =  '" + user.ext.Introduce + "',"
                 + "`birthday` =  '" + user.ext.Age + "',"  //这里需要对userExt进行修改
                 + "`Motto` =  '" + user.ext.Motto + "',"
                 + "`Hometown` =  '" + user.ext.Hometown + "',"
                 + "`Telephone` =   '" + user.ext.Telephone + "',"
                 + "`Country` =   '" + user.ext.Country + "',"
                 + "`timestamp` ='" + getTimestampOfNow() + "'"
                 + "WHERE  `user`.`id` =" + user.user_id + " ;";
            runSQL(sql);
            UpdateUserExtInfoImage(user.user_id, user.ext);
        }

        //更新用户扩展信息头像,这个好说，已经测试了
        public void UpdateUserExtInfoImage(int user_id, DrawBitmap.UserExt userExt)//
        {
            if (userExt.User_Image == "No Change" || userExt.User_Image == "NoChange")
                return;
            String sql = "UPDATE  `drawtogether`.`user` SET  "
                + "`User_Image` =   '" + userExt.User_Image + "'"
                + " WHERE  `user`.`id` =" + user_id + ";";
            //Console.WriteLine("更改头像");
            runSQL(sql);
        }


        //删号,通过用户id删号，可用
        public void DeleteUser(int user_id)
        {
            string[] sqls = new string[2];
            sqls[0] = "delete from `drawtogether`.`user` where id='" + user_id + "';";
            runSQL(sqls);
        }

        //删号，通过用户名删号,可用
        public void DeleteUser(String username)
        {
            DeleteUser(getUserId(username));
        }


        /// <summary>
        /// 根据用户名或者昵称得到满足条件的User List,可用
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<DrawBitmap.User> getUserByName(String name)
        {
            List<DrawBitmap.User> user_list = new List<User>();
            MySqlCommand mycm = new MySqlCommand("select id,name,nickname,User_Image,Country,Hometown,Introduce,Motto,Telephone,Level from drawtogether.user where ( name='" + name + "' or nickname='" + name + "' );", conn);
            // Console.WriteLine(mycm.CommandText);
            MySqlDataReader s = mycm.ExecuteReader();

            try
            {
                while (s.Read())
                {
                    if (s.HasRows)
                    {
                        User u = new User(s.GetInt16(0), null, s.GetString(1), s.GetString(2));
                        u.ext = new UserExt();
                        u.ext.User_Image = s.GetString(3);
                        u.ext.Age = 1;
                        u.ext.Country = s.GetString(4);
                        u.ext.Hometown = s.GetString(5);
                        u.ext.Introduce = s.GetString(6);
                        u.ext.Motto = s.GetString(7);
                        u.ext.Telephone = s.GetString(8);
                        u.ext.Level = s.GetInt16(9);

                        user_list.Add(u);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error in getUserByName {0}", e.Message);
                s.Close();
                mycm.Dispose();
            }
            s.Close();
            mycm.Dispose();
            return user_list;
        }

        //根据用户名得到用户User(DrawBitmap.User),可用
        public DrawBitmap.User getUserById(int user_id, bool includeExt = false)
        {
            DrawBitmap.User user = null;
            MySqlCommand mycm =null;
            if(!includeExt)
            {
                mycm = new MySqlCommand("select name,nickname,User_Image from drawtogether.user where id='" + user_id + "';", conn);
            }
            else
            {
                mycm = new MySqlCommand("select * from drawtogether.user where id='" + user_id + "';", conn);
            }
            
            MySqlDataReader s = mycm.ExecuteReader();
            while (s.Read())
            {
                if (s.HasRows)
                {
                    
                    if (includeExt)
                    {
                        user = new User(user_id, null, s.GetString(1), s.GetString(10));
                        user.ext = new UserExt();
                        user.ext.User_Image = s.GetString(11);
                        //user.ext.Age=s.GetInt32()
                        user.ext.Country = s.GetString(9);
                        user.ext.Hometown = s.GetString(7);
                        user.ext.Telephone = s.GetString(8);
                        user.ext.Motto= s.GetString(6);
                        user.ext.Introduce = s.GetString(4);
                    }
                    else
                    {
                        user = new User(user_id, null, s.GetString(0), s.GetString(1));
                        user.ext = new UserExt();
                        user.ext.User_Image = s.GetString(2);
                    }

                    break;
                }
            }
            s.Close();
            mycm.Dispose();

            if(includeExt)
            {
                user.ext.GroupSet = new SortedSet<int>(getGroupListOfUser(user_id));
            }
            return user;
        }

        /// <summary>
        /// 检测用户名是否存在
        /// </summary>
        /// <param name="username"></param>
        /// <returns>bool值</returns>
        public bool isUsernameExist(String username)
        {
            return (getUserId(username) == 0);
        }

        /// <summary>
        ///检测用户名和密码是否匹配， -1表示不合法,合法将会返回用户id,
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>

        public int checkUser(string username, string password, ref User temp_user)
        {
            int result = -1;
            MySqlCommand mycm = new MySqlCommand("select id,name,nickname from drawtogether.user where name='" + username + "' and password='" + password + "';", conn);
            MySqlDataReader s = mycm.ExecuteReader();
            while (s.Read())
            {
                if (s.HasRows)
                {
                    result = s.GetInt16(0);
                    temp_user.user_id = result;
                    temp_user.name = s.GetString(1);
                    temp_user.nickname = s.GetString(2);
                }
            }
            s.Close();
            mycm.Dispose();
            return result;
        }

        //		true表示合法     false表示不合法
        public bool checkUser(int user_id, string password)
        {
            bool result = false;
            MySqlCommand mycm = new MySqlCommand("select id from drawtogether.user where id='" + user_id + "' and password='" + password + "';", conn);
            MySqlDataReader s = mycm.ExecuteReader();
            while (s.Read())
            {
                if (s.HasRows)
                {
                    result = true;
                }
            }
            s.Close();
            mycm.Dispose();
            return result;
        }

        public List<int> getGroupListOfUser(int user_id)
        {
            List<int> gl = new List<int>();
            MySqlCommand mycm = new MySqlCommand("select group_id from drawtogether.user2group where user_id='" + user_id + "';", conn);
            MySqlDataReader s = mycm.ExecuteReader();
            while (s.Read())
            {
                if (s.HasRows)
                {
                    gl.Add(s.GetInt16(0));
                }
            }
            s.Close();
            mycm.Dispose();
            return gl;
        }

        /// <summary>
        /// 退群
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="group_id"></param>
        public void exitAGoup(int user_id, int group_id)
        {
            string sql = "delete from `drawtogether`.`user2group` where user_id=" + user_id + " and group_id=" + group_id + ";";
            runSQL(sql);
        }
        /// <summary>
        /// 删好友
        /// </summary>
        /// <param name="from_id"></param>
        /// <param name="to_id"></param>
        public void deleteAFriend(int from_id, int to_id)
        {
            string sql = "delete from `drawtogether`.`friendship` where ( from_id=" + from_id + " and to_id=" + to_id + " ) or ( from_id=" + to_id + " and to_id=" + from_id + " );";
            runSQL(sql);
        }
        //将byte[] 类型的ip值转换为string
        private string byteToString(byte[] ip)
        {
            return ip[0].ToString() + "." + ip[1].ToString() + "." + ip[2].ToString() + "." + ip[3].ToString();
        }


        public bool createAGroup(int user_id, String groupname, String groupdesc)
        {
            String sql = "INSERT INTO  `drawtogether`.`group` (`id` ,`name` ,`creator_id` ,`desc` ,`zone` ,`createtime`)"
                                                                         + "VALUES (NULL ,  '" + groupname + "',  " + user_id + ", '" + groupdesc + "' , NULL , ' current_timestamp()');";
            runSQL(sql);
            return true;
            
        }

        /// <summary>
        /// 未完成，获得群信息
        /// </summary>
        /// <param name="group_id"></param>
        /// <returns></returns>
        public DrawBitmap.GroupData getGroupById(int group_id)
        {
            DrawBitmap.GroupData group = null;
            MySqlCommand mycm = new MySqlCommand("select id,name,creator_id,image,createtime from drawtogether.group where id='" + group_id + "';", conn);
            MySqlDataReader s = mycm.ExecuteReader();
            while (s.Read())
            {
                if (s.HasRows)
                {
                    group = new DrawBitmap.GroupData();

                    //group.CreateDate =DateTime.ParseExact(s.GetString(4), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    group.CreateDate = s.GetInt64(4);
                    group.GroupID = group_id;
                    //group.GroupImage = s.GetString(3);
                    group.GroupName = s.GetString(1);
                    group.GroupCreatorName = s.GetString(2);
                    group.UserSet = new SortedDictionary<int, GroupMember>();
                }
            }
            s.Close();
            mycm.CommandText = "select * from  drawtogether.user2group where id=" + group_id + " and user_id=" + 2 + ";";
            mycm.Dispose();

            //GroupMember
            //group.UserSet
            //DrawBitmap.GroupMember ss = new GroupMember();
            // group.UserSet
            return group;
        }

        public Dictionary<int, GroupMember> getGroupMembersInAGroup(int group_id)
        {
            Dictionary<int, GroupMember> result_list = new Dictionary<int, GroupMember>();
            List<int> mem_list = getGroupMemberIdInAGroup(group_id);
            foreach (var item in mem_list)
            {
                // string sql = "select * from ";
            }
            return result_list;
        }

        private List<int> getGroupMemberIdInAGroup(int group_id)
        {
            throw new NotImplementedException();
        }








        /// <summary>
        /// 将mysql语句运行，可用于数据库的增删改，不包括查询数据
        /// </summary>
        /// <param name="sql"></param>
        public void runSQL(string sql)
        {
            this.QueryOpen();
            MySqlCommand cmd = new MySqlCommand(sql, this.conn4Query);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("error in runSQL(single){0}", e.Message);
                cmd.Dispose();
            }
            cmd.Dispose();
            this.QueryClose();
        }

        /// <summary>
        /// 将一堆mysql语句运行，可用于数据库的增删改，不包括查询数据
        /// </summary>
        /// <param name="sql"></param>
        public void runSQL(string[] sqls)
        {
            this.QueryOpen();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = this.conn4Query;
            try
            {
                foreach (string sql in sqls)
                {
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error in runSQL(multiple){0}", e.Message);
                cmd.Dispose();
            }
            cmd.Dispose();
            this.QueryClose();
        }

        //为调试测试而生
        public void showUser(DrawBitmap.User user)
        {
            Console.WriteLine("用户信息：\nid:{0}\nname:{1}\nnickname:{2}\nmotto:{3}", user.user_id, user.name, user.nickname, user.ext.Motto);
            Console.ReadLine();
        }

        //为调试测试而生
        public void showUserExt(DrawBitmap.UserExt userExt)
        {
            Console.WriteLine(userExt.Level + "   " + userExt.Hometown + "   " + userExt.Telephone);
        }
        /// <summary>
        /// 将用户from_id向用户to_id发出的消息存入数据库
        /// </summary>
        /// <param name="from_id"></param>
        /// <param name="to_id"></param>
        /// <param name="mdata"></param>
        internal void SaveUserMessage(int from_id, int to_id, string mdata)
        {
            string sql = "INSERT INTO  `drawtogether`.`systemmsg` (`from_id` ,`to_id` ,`time` ,`content`)" +
                                                            "VALUES ('" + from_id + "',  '" + to_id + "' ,'" + getTimestampOfNow() + "', '" + mdata + "');";

            Console.WriteLine(sql);
            runSQL(sql);
        }
        /// <summary>
        /// 在数据库中添加消息，使得id为recviver_id的用户上线后能够获取updater_id已经更新的信息
        /// </summary>
        /// <param name="updater_id"></param>
        /// <param name="recviver_id"></param>
        internal void addUpdate(int updater_id, int recviver_id)
        {
            bool undo = true;
            MySqlCommand mycm = new MySqlCommand("select updater_id from drawtogether.group where updater_id='" + updater_id + "' and recviver_id='" + recviver_id + "' ;", conn);
            MySqlDataReader s = mycm.ExecuteReader();
            while (s.Read())
            {
                if (s.HasRows)
                {
                    undo = false;
                }
            }
            s.Close();
            mycm.Dispose();
            if (undo)
            {
                string sql = "insert into `drawtogether`.`userwhoupdate` values(" + updater_id + ",'" + recviver_id + "');";
                runSQL(sql);
            }

        }

        /// <summary>
        /// 更新id用户的密码，返回啥啊这是？object？
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        internal int UpdatePassword(int id, string newPassword)
        {
            String sql = "update `drawtogether`.`user` set password = '" + newPassword + "' where id=" + id + ";";
            runSQL(sql);
            return 1;
        }

        public bool modifyPassword(int id, string oldPassword, string newPassword)
        {
            if (checkUser(id, oldPassword))
            {
                UpdatePassword(id, newPassword);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取Lid用户所有的已经更新过的用户的信息，返回一个用户列表,这个返回值好不好呢？
        /// </summary>
        /// <param name="recviver_id"></param>
        /// <returns></returns>
        /// 待修改
        internal List<User> getUpdateFriendList(int recviver_id)
        {
            List<DrawBitmap.User> List_return = new List<DrawBitmap.User>();
            MySqlCommand mycm = new MySqlCommand("select * from drawtogether.userwhoupdate where recviver_id= '" + recviver_id + "';", conn);
            MySqlDataReader s = mycm.ExecuteReader();
            while (s.Read())
            {
                if (s.HasRows)
                {
                    //List_return.Add(new DrawBitmap.User(s.GetInt16(0), null, s.GetString(2)));
                }
            }
            s.Close();
            mycm.Dispose();
            String sql = "delete from drawtogether.userwhoupdate where recviver_id='" + recviver_id + "';";
            runSQL(sql);
            return List_return;
        }
        /// <summary>
        /// 获取Lid用户所有的已经更新过的用户的信息，返回一个用户列表
        /// </summary>
        /// <param name="recviver_id"></param>
        /// <returns></returns>
        internal List<int> getUpdateFriendIdList(int recviver_id)
        {
            List<int> List_return = new List<int>();
            MySqlCommand mycm = new MySqlCommand("select updater_id from drawtogether.userwhoupdate where recviver_id= '" + recviver_id + "';", conn);
            MySqlDataReader s = mycm.ExecuteReader();
            while (s.Read())
            {
                if (s.HasRows)
                {
                    List_return.Add(s.GetInt16(0));
                }
            }
            s.Close();
            mycm.Dispose();
            String sql = "delete from drawtogether.userwhoupdate where recviver_id='" + recviver_id + "';";
            runSQL(sql);
            return List_return;
        }

        internal List<int> getFriendIdList(int user_id)
        {
            List<int> List_return = new List<int>();
            MySqlCommand mycm = new MySqlCommand("select to_id from drawtogether.friendship where from_id= '" + user_id + "';", conn);
            MySqlDataReader s = mycm.ExecuteReader();
            while (s.Read())
            {
                if (s.HasRows)
                {
                    List_return.Add(s.GetInt16(0));
                }
            }
            s.Close();
            mycm.Dispose();
            return List_return;
        }
        public List<pair<int, long>> getFriendsTimestamp(int id)
        {
            List<pair<int, long>> list_return = new List<pair<int, long>>();
            string sql = "select distinct f.to_id,u.timestamp from `drawtogether`.`user` u, `drawtogether`.`friendship` f where u.id=f.to_id and f.from_id=" + id + ";";
            MySqlCommand mycm = new MySqlCommand(sql, conn);
            MySqlDataReader s = mycm.ExecuteReader();
            while (s.Read())
            {
                if (s.HasRows)
                {
                    try
                    {
                        //Console.WriteLine("时间：：：{0}", (long)(s.GetInt32(1)));
                        list_return.Add(new pair<int, long>(s.GetInt32(0), (long)(s.GetInt32(1))));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("获取好友时间{0}", e.Message);
                    }
                }
            }
            s.Close();
            mycm.Dispose();
            return list_return;
        }

        internal List<pair<int, long>> getGroupTimeStamp(int login_id)
        {
            List<pair<int, long>> list_return = new List<pair<int, long>>();
            string sql = "select g.id, g.timestamp from `drawtogether`.`user` u,`drawtogether`.`user2group` ug,`drawtogether`.`group` g where u.id=ug.user_id and ug.group_id=g.id and u.id=" + login_id + ";";
            MySqlCommand mycm = new MySqlCommand(sql, conn);
            MySqlDataReader s = mycm.ExecuteReader();
            while (s.Read())
            {
                if (s.HasRows)
                {
                    try
                    {
                        //Console.WriteLine("时间：：：{0}", (long)(s.GetInt32(0)));
                        list_return.Add(new pair<int, long>(s.GetInt16(0), (long)(s.GetInt32(1))));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("获取好友时间{0}", e.Message);
                    }

                }
            }
            s.Close();
            mycm.Dispose();
            return list_return;
        }

        internal pair<int, long> getUserTimestamp(int login_id)
        {
            pair<int, long> return_pair = new pair<int, long>(login_id, 0);
            MySqlCommand mycm = new MySqlCommand("select timestamp from drawtogether.user where id= '" + login_id + "';", conn);
            MySqlDataReader s = mycm.ExecuteReader();
            while (s.Read())
            {
                if (s.HasRows)
                {
                    return_pair.Value = (long)(s.GetInt64(0));
                }
            }
            s.Close();
            mycm.Dispose();
            return return_pair;
        }

        /// <summary>
        /// 将用户下线前没处理的消息再次放入数据库
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="msgList"></param>
        public void storeOnlineMessagesOfUser(int user_id, List<pair<int, OnlineUserMessge>> msgList)
        {
            foreach (var item in msgList)
            {
                storeSingleOnlineMessage(user_id, item.Key, item.Value);
            }
        }


        public static void KKKKK(OnlineUserMessge image)
        {
            MySqlConnection m_conn = new MySqlConnection("Data Source='127.0.0.1';UserId='Asher';Password='lalala';");
            m_conn.Open();


            byte[] bytBLOBData = otob(image);

            MySqlCommand cmd2 = new MySqlCommand();
            //cmd2.CommandText = "INSERT INTO drawtogether.TESTTABLE (id,NAME,IMG) VALUES ('4','test',?alfa)";
            cmd2.CommandText = "INSERT INTO drawtogether.systemmsg (id,content) VALUES ('',?alfa)";

            MySqlParameter prm = new MySqlParameter("?alfa", MySqlDbType.Blob, bytBLOBData.Length, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Current, bytBLOBData);
            cmd2.Parameters.Add(prm);
            cmd2.Connection = m_conn;
            cmd2.ExecuteNonQuery();
            cmd2.Dispose();
            m_conn.Close();
        }
        public static OnlineUserMessge OOOOO()
        {
            OnlineUserMessge ss = null;
            MySqlConnection m_conn = new MySqlConnection("Data Source='127.0.0.1';UserId='Asher';Password='lalala';");
            m_conn.Open();
            //MySqlCommand cmd = new MySqlCommand("SELECT IMG,name FROM drawtogether.TESTTABLE WHERE id=18", m_conn);
            MySqlCommand cmd = new MySqlCommand("SELECT content,id FROM drawtogether.systemmsg WHERE id=41", m_conn);
            try
            {
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds, "BLOBTest");
                int c = ds.Tables["BLOBTest"].Rows.Count;
                if (c > 0)
                {
                    Byte[] byteBLOBData = new Byte[0];
                    byteBLOBData = (Byte[])(ds.Tables["BLOBTest"].Rows[c - 1]["content"]);
                    ss = (OnlineUserMessge)btoo(byteBLOBData);
                    string k = ds.Tables["BLOBTest"].Rows[c - 1]["id"].ToString();
                    Console.WriteLine(k);
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("error in {0},{1}", ex.TargetSite, ex.Message);

            }
            m_conn.Close();
            return ss;
        }


        //将对象转换为字节数组 
        public static byte[] otob(object o)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream rems = new MemoryStream();
            formatter.Serialize(rems, o);
            return rems.GetBuffer();
        }

        //将字节数组转换为对象 
        public static object btoo(byte[] b)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream rems = new MemoryStream(b);
            return formatter.Deserialize(rems);
        }

        /// <summary>
        /// 从数据库中获取未处理的消息，可用
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public List<pair<int, OnlineUserMessge>> getMessageToUser(int user_id)
        {
            List<pair<int, OnlineUserMessge>> List_return = new List<pair<int, OnlineUserMessge>>();
            MySqlCommand cmd = new MySqlCommand("SELECT content,from_id FROM drawtogether.systemmsg WHERE to_id=" + user_id + ";", conn);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            int Msgindex = 0;
            try
            {
                da.Fill(ds, "BLOBTest");
                int c = ds.Tables["BLOBTest"].Rows.Count;
                for (int index = 0; index < c; ++index)
                {
                    Byte[] byteBLOBData = new Byte[0];
                    byteBLOBData = (Byte[])(ds.Tables["BLOBTest"].Rows[index]["content"]);

                    OnlineUserMessge ts = (OnlineUserMessge)btoo(byteBLOBData);
                    //ts.id = (int)ds.Tables["BLOBTest"].Rows[index]["id"];
                    ts.id = Msgindex;
                    ++Msgindex;
                    List_return.Add(new pair<int, OnlineUserMessge>((int)ds.Tables["BLOBTest"].Rows[index]["from_id"], ts));
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("error in getMessageToUser,{0}", ex.Message);
            }
            finally
            {
                ds.Dispose();
                da.Dispose();
            }
            runSQL("delete from drawtogether.systemmsg WHERE to_id=" + user_id + ";");
            return List_return;
        }




        public OnlineUserMessge getSingleMessage(int user_id)
        {
            OnlineUserMessge t_return = new OnlineUserMessge();
            MySqlCommand cmd = new MySqlCommand("SELECT id,content,from_id FROM drawtogether.systemmsg WHERE to_id=" + user_id + ";", conn);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds, "BLOBTest");
                int c = ds.Tables["BLOBTest"].Rows.Count;
                //for (int index = 0; index < c; ++index)
                int index = c - 1;
                {
                    Byte[] byteBLOBData = new Byte[0];
                    byteBLOBData = (Byte[])(ds.Tables["BLOBTest"].Rows[index]["content"]);

                    t_return = (OnlineUserMessge)btoo(byteBLOBData);
                    t_return.id = (int)ds.Tables["BLOBTest"].Rows[index]["id"];

                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("error in getMessageToUser,{0}", ex.Message);
            }
            finally
            {
                ds.Dispose();
                da.Dispose();
            }

            return t_return;
        }


        public void storeSingleOnlineMessage(int reciver_id, int sender_id, OnlineUserMessge msg)
        {
            byte[] bytBLOBData = otob(msg);
            MySqlCommand cmd2 = new MySqlCommand();
            cmd2.CommandText = "INSERT INTO drawtogether.systemmsg (from_id,to_id,content) VALUES (" + sender_id + "," + reciver_id + ",?alfa)";
            MySqlParameter prm = new MySqlParameter("?alfa", MySqlDbType.Blob, bytBLOBData.Length, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Current, bytBLOBData);
            cmd2.Parameters.Add(prm);
            cmd2.Connection = conn;
            cmd2.ExecuteNonQuery();
            cmd2.Dispose();
        }
    }
}
