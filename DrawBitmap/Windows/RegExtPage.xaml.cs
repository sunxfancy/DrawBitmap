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
    public partial class RegExtPage : Page
    {
        public static BitmapImage tempImage;
        public RegExtPage()
        {
            InitializeComponent();
            
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
            User user = Register.user;
            user.ext = userext;
            ServerAPI.UpdataMyInfo(user);

            LoginPage loginpage = new LoginPage();
            loginpage.Username.Text = Register.user.name;
            NavigationService.Navigate(loginpage);
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
    }
}
