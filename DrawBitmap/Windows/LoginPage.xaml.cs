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
using WPF.Themes;
using DrawBitmap.MainClass;
using DrawBitmap.Windows;
using System.Threading;

namespace DrawBitmap
{

    /// <summary>
    /// LoginPage.xaml 的交互逻辑
    /// </summary>
    public partial class LoginPage : Page
    {
        public static Object loginInitiData="lala";
        public static String islogin = "已登录";
        public static String threadisrun = "是";
        bool issubmit = false;
        Button min_button;
        Button close_button;
        System.Windows.Forms.Timer getResultTimer;
        public LoginPage()
        {
            InitializeComponent();
            min_button = new Button();
            close_button = new Button();
            this.titleBar.Children.Add(min_button);
            this.titleBar.Children.Add(close_button);
            FatherWindow.TitleBarInit(min_button, close_button);
            min_button.Margin = new Thickness(0, 0, -5, 0);
            min_button.HorizontalAlignment = HorizontalAlignment.Right;
            close_button.HorizontalAlignment = HorizontalAlignment.Right;
            close_button.Click+=close_button_Click;
            min_button.Click+=min_button_Click;
          //  this.loginGif.Source = @"Resource/login.gif";
            getResultTimer = new System.Windows.Forms.Timer();
            getResultTimer.Interval = 3000;
            getResultTimer.Tick += getResultTimer_Tick;
            getResultTimer.Start();

        }

        void getResultTimer_Tick(object sender, EventArgs e)
        {
            if (issubmit) return;
           if (LoginPage.threadisrun.Equals( "否"))
            {
            if(LoginPage.islogin.Equals("未登陆"))
            {
                issubmit = true;
                MessageBox.Show("╭(╯^╰)╮  3#用户名密码不正确,难道是没注册么");
                this.loginP.Visibility = Visibility.Hidden;
            }
            else if (LoginPage.islogin.Equals("已登陆"))
            {
                     issubmit = true;
                     this.loginGif.Visibility = Visibility.Hidden;
                    App.data = new AppData();
                    UserWindow window = new UserWindow();
                    App.mainWindow = window;
                    App.data.InitLoginData(LoginPage.loginInitiData as LoginReturn);
                    App.mainWindow = window;
                    //这是什么道理= =，反正是好使了
                    window.Show();
                    App.Current.MainWindow.Close();
            }
            }

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
          
            if (Username.Text == "")
            {
                MessageBox.Show("╭(╯^╰)╮ 1#:用户名怎么可能为空");
                Username.Focus();
                return;
            }
            if (Password.Password == "")
            {
                MessageBox.Show("╭(╯^╰)╮ 2#:密码怎么可能为空");
                Password.Focus();
                return;
            }
            this.loginP.Visibility = Visibility.Visible;
            Thread temp_thread = new Thread(new ParameterizedThreadStart(thelogin));
            LoginPage.threadisrun = "是";
            temp_thread.Start(new String[] { Username.Text, Password.Password });
              issubmit = false;
        }

        public void thelogin(object o)
        {
            lock (LoginPage.islogin)
            {
                lock(LoginPage.loginInitiData){
              String[] s = (String[])o;
               LoginPage.loginInitiData=ServerAPI.Login(s[0],s[1]);
                if (LoginPage.loginInitiData!=null)
                {
                    LoginPage.islogin = "已登陆";
                }
                else
                {
                    LoginPage.loginInitiData = "as";
                    LoginPage.islogin = "未登陆";
                }
              }
            }
            lock(LoginPage.threadisrun)
            {
                LoginPage.threadisrun = "否";
            }
        }
        public void getFocus()
        {
            this.Username.Focus();
        }
        private void RegisterButtonDown(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Register());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

     
          API.TestLogin();
           
            
          // new UserWindow().Show();
           //App.Current.MainWindow.Close();

        }

        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            login_button.BorderThickness = new Thickness(2.0);
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            login_button.BorderThickness = new Thickness(0);
        }

        private void Button_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            register_button.BorderThickness = new Thickness(2.0);
        }

        private void Button_MouseLeave_1(object sender, MouseEventArgs e)
        {
            register_button.BorderThickness = new Thickness(0);
        }

        private void close_button_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void min_button_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            new SetingWindow().Show();
        }

 


    }
}
