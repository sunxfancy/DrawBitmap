using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Input;
using DrawBitmap.MainClass;

namespace DrawBitmap
{
    [Serializable]
    public class UserMessage
    {
        public UserMessage(byte _m,object _o)
        {
            type = _m;
            data = _o;
        }

        public UserMessage(int _id,byte _m,object _o)
        {
            id = _id;
            type = _m;
            data = _o;
        }
        public void setMsgId(int _id)
        {
            id = _id;
        }
        public UserMessage()
        {
            
        }
        public int id;
        public byte type;
        public object data;
    }

    [Serializable]
    public class RegisterData
    {
        public RegisterData(User _u,String pass)
        {
            user = _u;
            password = pass;
        }

        public RegisterData()
        {
            // TODO: Complete member initialization
        }
        public User user;
        public string password;
    }

    [Serializable]
    public class LoginData
    {

         public string username;
         public string password;

         public LoginData()
         {

         }
         public LoginData(string p1, string p2)
         {
             // TODO: Complete member initialization
             this.username = p1;
             this.password = p2;
         }
    }

    
    [Serializable]
    public class LoginReturn
    {
        /// <summary>
        /// 我自己的id和时间戳
        /// </summary>
        public pair<int,long> myTimeStamp;
        /// <summary>
        /// 所有好友的id和时间戳
        /// </summary>
        public List<pair<int, long>> FriendsTimeStamp;
        /// <summary>
        /// 群组的id和时间戳
        /// </summary>
        public List<pair<int, long>> GroupTimeStamp;
        /// <summary>
        /// 全部在线好友列表
        /// 好友的id和他的ip
        /// </summary>
        public List<pair<int,byte[]>> OnlineFriends;
        
        /// <summary>
        /// 所有自己离线期间未读取的Message列表
        /// </summary>
        public List<UserMessage> message;
    }

    
    [Serializable]
    public class FriendsData
    {
        public FriendsData()
        {

        }

        public List<User> UserList;
        public List<Group> GroupList;

        public FriendsData(List<User> list1, List<Group> list2)
        {
            // TODO: Complete member initialization
            this.UserList = list1;
            this.GroupList = list2;
        }
        
    }

    [Serializable]
    public class AddFriendData
    {
        public AddFriendData(int _id, int _f)
        {
            id = _id;
            f_id = _f;
        }

        /// <summary>
        /// 自己的id
        /// </summary>
        public int id;

        /// <summary>
        /// 好友的用户名
        /// </summary>
        public int f_id;
    }

    [Serializable]
    public class UpdatePasswordData
    {
        public UpdatePasswordData(int _id,string _old ,string _new)
        {
            id = _id;
            oldPassword = _old;
            newPassword = _new;
        }

        public int id;
        public string oldPassword;

        public string newPassword;
    }


    /// <summary>
    /// 获取对应id的人和群组的数据的类
    /// </summary>
    [Serializable]
    public class GetData
    {
        public int user_id;
        public List<int> FriendIDList;
        public List<int> GroupIDList;
    }


    /// <summary>
    /// 返回获取对应id的人和群组的数据的类
    /// </summary>
    [Serializable]
    public class ReturnData
    {
        public List<User> FriendList=new List<User>();
        public List<GroupData> GroupList=new List<GroupData>();
    }

 

    [Serializable]
    public struct pair<TKey,TValue> 
    {
        private TKey _key;
        private TValue _value;
        public pair(TKey key, TValue value)
        {
            _key = key;
            _value = value;
        }

        public TKey Key
        {
            get { return _key; }
        }

        public TValue Value
        {
            get { return _value; }
            set { _value = value; }
        }

    }
    
    
 
}
