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
using System.Windows.Forms;
using MulticastNetWork;
using System.Net;
using System.Security.Policy;
using System.Drawing;
using System.IO;
using DrawBitmap.MainClass;
using ImageCropper;


namespace DrawBitmap
{
    /// <summary>
    /// RegExtPage.xaml 的交互逻辑
    /// </summary>
    public partial class RegExtPage2 : Page
    {
    
        
        public static BitmapImage tempImage;
        public RegExtPage2()
        {
            InitializeComponent();
            //userext.User_Image = UserExt.ImageToBase64(head.Source as BitmapImage);
            Friend me = App.data.Me;
            country.Text = me.Country;
           hometown.Text=me.Hometown;
            motto.Text=me.Motto;
            introduce.Text=me.Introduce;
            //age.Text = me.Age.ToString();
            head.Source = me.User_Image;
            DrawBitmap.Windows.FatherWindow.buttonWrap(confirm,System.Windows.Media.Brushes.Blue);
        }

        public void SelectAnBitmap()
        {
            var path = API.OpenImageFile();
            if (path != null)
            {
                this.imgCropper.ImageUrl = path;
                this.imgSecletCanvas.Visibility = Visibility.Visible;

            }
        }

        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
                //System.Windows.MessageBox.Show(ofd.FileName);
                SelectAnBitmap();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UserExt userext = new UserExt();
            int.TryParse(age.Text, out userext.Age);
            userext.User_Image = UserExt.ImageToBase64(head.Source as BitmapImage);
            userext.Country = country.Text;
            userext.Hometown = hometown.Text;
            userext.Motto = motto.Text;
            userext.Introduce = introduce.Text;
            User user = App.data.Me.toUser();
            user.ext = userext;
            if(ServerAPI.UpdataMyInfo(user))
            {
                App.data.Me.User_Image = head.Source;

                UserWindow.isNeedUpdate = true;
            }


        }


        /// <summary>
        /// 确认保存图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgsel_Button_Click_Confirm(object sender, RoutedEventArgs e)
        {
           this.imgCropper.SaveCroppedImage();
            if(this.imgCropper.imgSelected!=null)
                head.Source = this.imgCropper.imgSelected;
            this.imgSecletCanvas.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// 取消保存图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgsel_Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            this.imgSecletCanvas.Visibility = Visibility.Collapsed;
        }

        private bool p_extended = false;
        private void paswrd_detail_trigger(object sender, MouseButtonEventArgs e)
        {
            if(!p_extended)
            {
                p_extended = true;
                p_Canvas.Margin = new Thickness(0, 0, 0, 0);
                p_Canvas.Height = 400;
                this.p_label.Content = "修改密码↓↓";
                pswrd_detail.Visibility = Visibility.Visible;

            }
            else
            {
                p_extended = false;
                p_Canvas.Margin = new Thickness(0, 308, 0, 0);
                p_Canvas.Height = 30;
                this.p_label.Content = "修改密码↑↑";
                pswrd_detail.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void confirm_Click(object sender, RoutedEventArgs e)
        {
            if (p_p.Password == "")
            {
                System.Windows.MessageBox.Show("╭(╯^╰)╮ 4#:未填原密码");
                p_p.Focus();
                return;
            }
            if (p_n.Password == "")
            {
                System.Windows.MessageBox.Show("╭(╯^╰)╮ 5#:未填新密码");
                p_n.Focus();
                return;
            }

            if (p_n1.Password == "")
            {
                System.Windows.MessageBox.Show("╭(╯^╰)╮ 6#:请确认新密码");
                p_n1.Focus();
                return;
            }
            if(p_n.Password!=p_n1.Password)
            {
                System.Windows.MessageBox.Show("╭(╯^╰)╮ 7#:新密码两次输入不一致");
                p_n1.Focus();
                p_n1.SelectAll();
                return;
            }

            int result=ServerAPI.UpdataMyPassword(App.data.Me.user_id, p_p.Password, p_n1.Password);
            if(result==1)
            {
                System.Windows.MessageBox.Show("密码修改成功");               
            }
            else if(result==0)
            {
                System.Windows.MessageBox.Show("╭(╯^╰)╮ 8#:原密码错误,修改失败");
            }
            else
            {
                System.Windows.MessageBox.Show("ರ_ರ 一只bug粗线了");
            }

        }
    }
}
