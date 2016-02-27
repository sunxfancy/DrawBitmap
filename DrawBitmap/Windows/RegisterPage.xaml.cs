using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MulticastNetWork;
using System.Net;
using System.Threading;
using DrawBitmap.MainClass;

namespace DrawBitmap
{
    /// <summary>
    /// Register.xaml 的交互逻辑
    /// </summary>
    public partial class Register : Page
    {
        public Register()
        {
            InitializeComponent();  
        }

        public static User user = new User();
        public int RegisterResult;
        public Thread t1;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string infomessage=null;
            user.name =  id_text.Text;
          //  byte[] ip = IPAddress.Parse(SendingClient.GetLocalIP()).GetAddressBytes();
            user.nickname = name_text.Text;
            string password = null;
            if (user.name=="")
            {
                infomessage +="用户名不能为空 ";
                ChangeInfo(Brushes.Red,infomessage);
                return;
            }
            

            if (user.nickname=="")
            {
                infomessage += "昵称不能为空 ";
                ChangeInfo(Brushes.Red, infomessage);
                return;
            }
           else if(user.nickname.Length>16)
            {
                infomessage += "昵称过长，不超过16个字符/8个汉字 ";
                ChangeInfo(Brushes.Red, infomessage);
                return;
            }
            
            if (pword1.Password=="")
            {
                infomessage += "密码不能为空 ";
                ChangeInfo(Brushes.Red, infomessage);
                return;
            }
            else if(pword1.Password.Length<6||pword1.Password.Length>12)
            {
                infomessage += "密码长度不符合要求，6到12个字符 ";
                ChangeInfo(Brushes.Red, infomessage);
                return;
            }


            if (pword1.Password.Equals(pword2.Password))
            {
                password = pword1.Password;
            }
            if (!pword1.Password.Equals(pword2.Password))
            {
                infomessage += "两次输入密码不一致";
                ChangeInfo(Brushes.Red, infomessage);
                return;
            }
          
            if(infomessage==null)
            {
                        int result = ServerAPI.Register(user.name,password,user.nickname);
                        if (result == -1)
                        {
                            info.Foreground = Brushes.Red;
                            info.Content = "注册失败，请重试！";
                        }
                        else if (result == 0)
                        {
                            info.Foreground = Brushes.Red;
                            info.Content = "网络不通，请检查网络后重试！";
                        }
                        else
                        {
                            user.user_id = result;
                            info.Foreground = Brushes.Green;
                            info.Content = "注册成功！";
                            NavigationService.Navigate(new RegExtPage());
                        }
                    
            }
            else
            {
                info.Foreground = Brushes.Red;
                info.Content = infomessage;
            }
            
        }

        
        public void thread()
        {

            RegisterResult = ServerAPI.TestName(user.name);

            if (RegisterResult == 0)
            {
                ChangeInfo(Brushes.Red,"网络不通，请检查网络后重试！");
            }
            if (RegisterResult == -1)
            {
                ChangeInfo(Brushes.Red, "用户名已被注册");
                ChangeImage(state_wrong, Visibility.Visible);
            }
            else
            {
                ChangeImage(state_right, Visibility.Visible);
            }

        }

       

        private void ChangeInfo(Brush brush, string data)
        {
            info.Dispatcher.Invoke(new Action(delegate { info.Foreground = brush; info.Content = data; }));
        }

        private void ChangeImage(Image i,Visibility state)
        {
            i.Dispatcher.Invoke(new Action(() => { i.Visibility = state; }));
        }



        /// <summary>
        /// 此处是用户名控件失去焦点时触发，验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void id_text_LostFocus(object sender, RoutedEventArgs e)
        {
            if (id_text.Text.Length == 0) return;
            user.name = id_text.Text;
            t1 = new Thread(new ThreadStart(thread));
            t1.IsBackground = true;
          //  t1.SetApartmentState(ApartmentState.STA);
            t1.Start();
        }

        private void back_Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }

        private void id_text_GotFocus(object sender, RoutedEventArgs e)
        {
            ChangeImage(state_wrong, Visibility.Hidden);
            ChangeImage(state_right, Visibility.Hidden);
            ChangeInfo(Brushes.Red,"");
        }
       
    }
}
