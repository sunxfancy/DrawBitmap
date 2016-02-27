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
using Sker.Controls;
using MulticastNetWork;
using DrawBitmap.MainClass;
using System.IO;

namespace DrawBitmap
{
    /// <summary>
    /// SearchFriendsByName.xaml 的交互逻辑
    /// </summary>
    public partial class SearchFriendsByName : Page
    {
        public SearchFriendsByName()
        {
            InitializeComponent();
        }

        

        private void SearchBox_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
            }
        }

        /// <summary>
        /// 将一个列表的内容添加到容器中显示出来
        /// </summary>
        /// <param name="showlist"></param>
        private void ShowUsers(List<Friend> showlist)
        {
            userlist.Items.Clear();
            foreach (var item in showlist)
	        {
                userlist.Items.Add(item);
	        }
            userlist.UpdateLayout();
        }

        /// <summary>
        /// 搜索按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var user = ServerAPI.SearchFriends(searcher.Text);
            if (user != null)
            {
                List<Friend> showlist = new List<Friend>();
                foreach(var item in user)
                {
                    showlist.Add(new Friend(item));
                }
                ShowUsers(showlist); 
            }
            else
            {
                MessageBox.Show("怎么可能");
            }
        }

        /// <summary>
        /// 加为好友按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            bool isSendAddFriend = false;
            foreach (var item in userlist.SelectedItems)
	        {
                var it = item as Friend;
                var id = it.user_id;
                ServerAPI.AddFriend(App.data.Me.user_id, id);
                isSendAddFriend = true;
	        }
            if (isSendAddFriend)
                MessageBox.Show("已经发送添加好友请求。");
            else
                MessageBox.Show("未发送任何请求。");
        }

    }


}
