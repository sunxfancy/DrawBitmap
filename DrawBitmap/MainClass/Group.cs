using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using DrawBitmap.MainClass;

namespace DrawBitmap
{
    public class Group : CanSelect
    {
        public int GroupID;
        public string GroupName;
        public string GroupCreatorName;
        public DateTime CreateDate;
        public ImageSource GroupImage;

        public SortedDictionary<int, GroupMember> UserSet;

        public override System.Windows.Media.ImageSource GetImage()
        {
            return GroupImage;
        }
        public override string GetName()
        {
            return GroupName;
        }
    }

    /// <summary>
    /// 和服务器端通信时使用的数据类
    /// </summary>
    [Serializable]
    public class GroupData 
    {
        public GroupData(int id,string groupname, string creatorname,string src)
        {
            GroupID = id;
            GroupName = groupname;
            GroupCreatorName = creatorname;
            GroupImage = src;
        }
        public GroupData()
        {

        }
        public int GroupID;
        public string GroupName;
        public string GroupCreatorName;
        public long CreateDate;
        public string GroupImage;
        
        public SortedDictionary<int,GroupMember> UserSet;
        public Group toGroup()
        {
            Group re=new Group();
            //re.CreateDate=CreateDate;
            re.GroupCreatorName=GroupCreatorName;
            re.GroupID=GroupID;
            re.GroupName=GroupName;


            //re.GroupImage=UserExt.Base64ToImage(GroupImage);
            
            re.UserSet=UserSet;
            return re;
        }
    }
    
    public class GroupMember
    {
        public GroupMember(Friend _user)
        {
            user = _user;
        }
        public GroupCard card;
        public Friend user;  //不必要获取所有人的数据，只获取最必要的部分已节约流量
        public bool isManager = false;
        public bool isCreator = false ;
        public bool isOnline = false;
    }
}
