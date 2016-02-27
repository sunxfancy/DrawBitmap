using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DrawBitmap
{
    /// <summary>
    /// 用户数据结构体
    /// </summary>
    [Serializable]
    public class User
    {
        public User()
        {

        }
        public User(int _id, byte[] _ip, string _name, string _nickname, UserExt _ext = null)
        {
            this.user_id = _id;
            this.ip = _ip;
            this.name = _name;
            this.ext = _ext;
            this.nickname = _nickname;
        }

        public User(Friend data)
        {
            // TODO: Complete member initialization
            UpdataFromFriend(data);
        }

        public int user_id;
        public string nickname;
        public byte[] ip;
        public string name;
        public UserExt ext;


        public void UpdataFromFriend(Friend f)
        {
            user_id = f.user_id;
            if (f.ip != null)
                ip = f.ip.GetAddressBytes();
            else
                ip = null;
            nickname = f.nickname;
            name = f.name;
            if (ext == null)
            {
                ext = new UserExt();
                ext.Age = f.Age;
                ext.Country = f.Country;
                ext.GroupSet = f.GroupSet;
                ext.Hometown = f.Hometown;
                ext.Introduce = f.Introduce;
                ext.Level = f.Level;
                ext.Motto = f.Motto;
                ext.Telephone = f.Telephone;
                ext.User_Image = UserExt.ImageToBase64(f.User_Image as BitmapImage);
            }
        }

    }

    /// <summary>
    /// 用户扩展数据结构体
    /// </summary>
    [Serializable]
    public class UserExt
    {
        
        public string User_Image;
        public int Level;
        public string Introduce;
        public int Age;
        public string Motto;
        public string Hometown;
        public string Telephone;
        public string Country;

        public SortedSet<int> GroupSet;

        public static string ImageToBase64(BitmapImage bitmap)
        {
            if (bitmap == null) return null;
            byte[] imageData = new byte[bitmap.StreamSource.Length];

            // now, you have get the image bytes array, and you can store it to SQL Server

            bitmap.StreamSource.Seek(0, System.IO.SeekOrigin.Begin);//very important, it should be set to the start of the stream
            bitmap.StreamSource.Read(imageData, 0, imageData.Length);
           // return imageData;
            return Convert.ToBase64String(imageData);
            
        }

        public static BitmapImage Base64ToImage(string base64)
        {
            if (base64 == null) return null;
            byte[] bytes = Convert.FromBase64String(base64);
            var stream = new MemoryStream(bytes);

            stream.Seek(0, System.IO.SeekOrigin.Begin);

            BitmapImage newBitmapImage = new BitmapImage();

            newBitmapImage.BeginInit();

            newBitmapImage.StreamSource = stream;

            newBitmapImage.EndInit();

            return newBitmapImage;
        }


        public static string IPByteToString(byte[] b)
        {
            var ip = new IPAddress(b);
            return ip.ToString();
        }

        public static byte[] IPStringToByte(string str)
        {
            return IPAddress.Parse(str).GetAddressBytes();
        }


    }
}
