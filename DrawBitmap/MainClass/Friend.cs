using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrawBitmap;
using System.Windows.Media;
using System.Net;
using DrawBitmap.MainClass;
using System.Windows.Media.Imaging;

namespace DrawBitmap
{
    /// <summary>
    /// 描述一个用户好友本地存储的底层结构
    /// </summary>
    public class Friend : CanSelect
    {
        public SwingImage Father
        {
            set;
            get;
        }
        public bool isOnline{set;get;}
        public int user_id{set;get;}
        public IPAddress ip{set;get;}
        public string name{set;get;}
        public string nickname{set;get;}
        public ImageSource User_Image{set;get;}
        public ImageSource Black_Image{set;get;}
        public int Level{set;get;}
        public string Introduce{set;get;}
        public int Age{set;get;}
        public string Motto{set;get;}
        public string Hometown{set;get;}
        public string Telephone{set;get;}
        public string Country{set;get;}
        public SortedSet<int> GroupSet{set;get;}
        public long TimeStamp{set;get;}

        /// <summary>
        /// 更新整个好友信息
        /// 注意！调用前一定要确保id正确，如果是新构建的，可直接在此附上id号
        /// </summary>
        public void Updata(int id = 0)
        {
             
            if (id != 0)
            {
                user_id = id;
            }
            var user = ServerAPI.GetFriendData(user_id);
            if (user != null)
                UpdatabyUser(user);
        }

        /// <summary>
        /// 不含ext部分
        /// </summary>
        /// <returns></returns>
        public User toUser()
        {
            User re = new User();
            re.user_id = user_id;
            re.name = name;
            re.nickname=nickname;
            return re;
        }

        public static void UpdataSomeOne(int id)
        {
            Friend f ;
            if (App.data.FriendList.TryGetValue(id, out f))
            {
                f.Updata();
            }
        }

        public override System.Windows.Media.ImageSource GetImage()
        {
            if (isOnline) return User_Image;
            else return Black_Image;
        }
        public override string GetName()
        {
            return nickname;
        }


        public Friend()
        {
        }

        public Friend(User user)
        {
            UpdatabyUser(user);
        }

        public void UpdatabyUser(User user)
        {
            user_id = user.user_id;
            name = user.name;
            if (user.ip == null)
                ip = null;
            else
                ip = new IPAddress(user.ip);
            nickname = user.nickname;
            var ex = user.ext;
            if (ex == null) return;
            User_Image = UserExt.Base64ToImage(ex.User_Image);
            Black_Image = ChangeToBlackBitmap(User_Image as BitmapImage);
            Level = ex.Level;
            Introduce = ex.Introduce;
            Age = ex.Age;
            Motto = ex.Motto;
            Hometown = ex.Hometown;
            Telephone = ex.Telephone;
            Country = ex.Country;
            GroupSet = ex.GroupSet;
        }

        public static ImageSource ChangeToBlackBitmap(BitmapSource image)
        {
            if (image == null) return null;
            ////////// Convert the BitmapSource to a new format ////////////
            // Use the BitmapImage created above as the source for a new BitmapSource object
            // which is set to a gray scale format using the FormatConvertedBitmap BitmapSource.                                               
            // Note: New BitmapSource does not cache. It is always pulled when required.

            FormatConvertedBitmap newFormatedBitmapSource = new FormatConvertedBitmap();

            // BitmapSource objects like FormatConvertedBitmap can only have their properties
            // changed within a BeginInit/EndInit block.
            newFormatedBitmapSource.BeginInit();

            // Use the BitmapSource object defined above as the source for this new 
            // BitmapSource (chain the BitmapSource objects together).
            newFormatedBitmapSource.Source = image;

            // Set the new format to Gray32Float (grayscale).
            newFormatedBitmapSource.DestinationFormat = PixelFormats.Gray32Float;
            newFormatedBitmapSource.EndInit();

            return newFormatedBitmapSource;
        }
    }
}
