using DrawBitmap.MainClass;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace DrawBitmap
{
    /// <summary>
    /// 传入控件内的主程序API类
    /// </summary>
    public class API
    {
        public static Friend Me
        {
            get { return App.data.Me; }
            set { App.data.Me = value; }
        }

        /// <summary>
        /// 得到主界面上的你可以控制的WPF节点对象
        /// </summary>
        public static void AddUserWindowChild(string str,UIElement obj)
        {
            foreach (var item in App.mainWindow.Data.Children)
	        {
                var expander = item as ExtendList;
                if (expander != null)
                {
                    if (string.Equals(expander.Header as string,str))
                    {
                        expander.Children.Add(obj);
                    }
                }
	        }
        }

        /// <summary>
        /// 发送请求，给好友弹出一个MessageBox
        /// </summary>
        public static int SendMessageBox(User user, string message, int type)
        {
            throw new System.NotImplementedException();
        }

        public static void SendAllUserData(object data)
        {
            throw new System.NotImplementedException();
        }

       
        public static bool Login(string username,string password)
        {
            LoginReturn m = ServerAPI.Login(username, password);
            return m!=null;
        }

        public static void TestLogin()
        {
            App.data = new AppData();
            var data = App.data;
            UserWindow window = new UserWindow();
            App.mainWindow = window;
            window.Show();
        }


        public static string OpenImageFile()
        {
           // BitmapImage bitmap = new BitmapImage();
            OpenFileDialog ofd = new OpenFileDialog();
            // ofd.Filter= "*.jpg|*.jpg|*.jpeg|*.jpeg|*.bmp|*.bmp|*.gif|*.gif|*.png|*.png|*.Tiff|*.Tiff|*.Wmf|*.Wmf";
            ofd.Filter = "Images|*.png;*.jpg;*.jpeg;*.bmp;*.gif";
            List<string> allowableFileTypes = new List<string>();
            allowableFileTypes.AddRange(new string[] { ".png", ".jpg", ".jpeg", ".bmp", ".gif" });

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (ofd.FileName == "")
                {
                    return null;
                }
                if (!ofd.FileName.Equals(String.Empty))
                {
                    FileInfo f = new FileInfo(ofd.FileName);
                    if (allowableFileTypes.Contains(f.Extension.ToLower()))
                    {
                        return f.FullName;
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Invalid file type");
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("You did pick a file to use");
                }
            }
            return null;
        }
    }
}
