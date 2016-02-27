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
using System.Windows.Shapes;
using System.Threading;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using MulticastNetWork;
using WPF.Themes;
using System.Windows.Navigation;
using DrawBitmap.MainClass;
using DrawBitmap.Windows;
using GifImageLib;

namespace DrawBitmap
{
    /// <summary>
    /// UserWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UserWindow : FatherWindow
    {
        public static bool isNeedUpdate = true;
        System.Windows.Forms.Timer getUserinfo;
        Button min_button;
        Button close_button;
        AddFriends add=null;  //加好友窗口
        private SetingWindow setWindow = null; //设置
        private MessageList mlistwindow = null;  //信息框
        private UserInfoModify userinfoset = null;
        private NewAGroup newagroup = null;

        public UserWindow()
        {
            InitializeComponent();
            this.Loaded += UserWindow_Loaded;
            ThemeManager.ApplyTheme(App.Current, "BubbleCreme");
            min_button = new Button();
            close_button = new Button();
            FatherWindow.TitleBarInit(min_button, close_button);
            this.TitleBar.Children.Add(min_button);
            this.TitleBar.Children.Add(close_button);
            min_button.Margin = new Thickness(0, 0, -5, 0);
            close_button.Click+=close_button_Click;
            min_button.Click+=min_button_Click;
            FatherWindow.buttonWrap(this.addFriendBtn,null);
            FatherWindow.buttonWrap(this.delFriendBtn, null);
            FatherWindow.buttonWrap(this.addFroupBtn, null);
            FatherWindow.buttonWrap(this.MsgListBtn, null);
            FatherWindow.buttonWrap(this.SetBtn, null);
            FatherWindow.buttonWrap(this.UserInfoSet,null);
            
            this.Closing += UserWindow_Closing;
            getUserinfo = new System.Windows.Forms.Timer();
            getUserinfo.Interval = 3000;
            getUserinfo.Tick += getUserinfo_Tick;
            getUserinfo.Start();

        }

        void getUserinfo_Tick(object sender, EventArgs e)
        {
            if (!UserWindow.isNeedUpdate) return;
            if (App.data.Me.User_Image != null)
           {
               this.userImg2.Source = App.data.Me.User_Image;
               this.userImg.Visibility = Visibility.Collapsed;
               this.userImg2.Visibility = Visibility.Visible;
               this.nickName.Text = App.data.Me.nickname;
           }

            UserWindow.isNeedUpdate = false;
        }

        void UserWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CloseAll();
        }

        void CloseAll()
        {
            if (add != null) add.Close();
            if (setWindow != null) setWindow.Close();
            if (mlistwindow != null) mlistwindow.Close();
            if (userinfoset != null) userinfoset.Close();
            if (newagroup != null) newagroup.Close();

        }

        public Friend AddFriend(int id)
        {
            Friend f = new Friend();
            MyFriends.Dispatcher.BeginInvoke(new Action(() =>
                {
                    //这里不明原因的要将更新放入UI线程。。。
                    f.Updata(id);
                    SwingImage s = new SwingImage(f);
                    MyFriends.DataChildren.Add(s); 
                }
            ));


            return f;
        }

        void UserWindow_Loaded(object sender, RoutedEventArgs e)
        {

            Initialize();
            //this.Hide();
        }
        /// <summary>
        /// 主应用程序初始化方法
        /// </summary>
        private void Initialize()
        {
            //尼玛，测试测试

            App.data.InitPluginSystem();
            //经测试，非UI线程是不能初始化UI系统的
          //  Thread InitThread = new Thread(InitPlugin);
           // InitThread.Start(); //新开一个线程来启动各个插件，减少UI响应延迟

            
           // var e = new ExtendFriendList();
            //e.Header = "五湖四海皆兄弟";
          //  MyFriends.Children.Insert(0, e);
          ////  var ee = new ExtendFriendList();
           // ee.Header = "五湖四海皆兄弟";
          //   MyFriends.Children.Insert(0,ee);


            //////////////////////////////////////////////////////////////////////////
            //呈现所有好友
            foreach (var item in App.data.FriendList.Values)
            {
                var ctrl = new SwingImage(item);
                item.Father = ctrl;
                this.MyFriends.DataChildren.Add(ctrl);
            }


           //////////////////////////////////////////////////////////////////////////////
            //呈现群组
            foreach(var item in App.data.GroupList)
            {
                ExtendFriendList temp_c = new ExtendFriendList();
                temp_c.Header = item.GroupName;
                this.groupList.Children.Add(temp_c);

                /*
                foreach (var subitem in item.UserSet)
                {
                    var ctrl = new SwingImage(subitem.Value.user);
                   // item.Father = ctrl;
                    this.MyFriends.DataChildren.Add(ctrl);
                }
                */

            }



        }

      

       

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (add == null||!AddFriends.isOpen)
            {
                //Friend f=  App.data.Me;
                add = new AddFriends();
                add.Show();
            }
            else
            {
                if(add.WindowState==WindowState.Minimized)
                {
                    add.WindowState = WindowState.Normal;
                    return;
                }

                if (add.Visibility == Visibility.Visible)
                    add.Visibility = Visibility.Hidden;
                else
                    add.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            if (setWindow == null || !SetingWindow.isOpen)
            {
                setWindow = new SetingWindow();
                setWindow.Show();
            }
            else
            {
                if (setWindow.WindowState == WindowState.Minimized)
                {
                    setWindow.WindowState = WindowState.Normal;
                    return;
                }
                if (setWindow.Visibility == Visibility.Visible)
                    setWindow.Visibility = Visibility.Hidden;
                else
                    setWindow.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// 查看当前的信息列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (mlistwindow == null||!MessageList.isOpen)
            {
                mlistwindow = new MessageList();
                mlistwindow.Show();
            }
            else
            {
                if (mlistwindow.WindowState == WindowState.Minimized)
                {
                    mlistwindow.WindowState = WindowState.Normal;
                    return;
                }
                if (mlistwindow.Visibility == Visibility.Visible)
                    mlistwindow.Visibility = Visibility.Hidden;
                else
                    mlistwindow.Visibility = Visibility.Visible;
            }

        }

        private void close_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void min_button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void UserInfoSet_Click(object sender, RoutedEventArgs e)
        {
            
            if (userinfoset == null || !UserInfoModify.isOpen)
            {
                userinfoset = new UserInfoModify();
                userinfoset.Show();
            }
            else
            {
                if (userinfoset.WindowState == WindowState.Minimized)
                {
                    userinfoset.WindowState = WindowState.Normal;
                    return;
                }
                if (userinfoset.Visibility == Visibility.Visible)
                    userinfoset.Visibility = Visibility.Hidden;
                else
                    userinfoset.Visibility = Visibility.Visible;
            }
        }

        private void AddNewGroup(object sender, RoutedEventArgs e)
        {
            if (newagroup == null || !NewAGroup.isOpen)
            {
                newagroup = new NewAGroup();
                newagroup.Show();
            }
            else
            {
                if (newagroup.WindowState == WindowState.Minimized)
                {
                    newagroup.WindowState = WindowState.Normal;
                    return;
                }
                if (newagroup.Visibility == Visibility.Visible)
                    newagroup.Visibility = Visibility.Hidden;
                else
                    newagroup.Visibility = Visibility.Visible;
            }
        }



    }
}
