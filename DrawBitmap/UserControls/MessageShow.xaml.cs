using DrawBitmap.MainClass;
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

namespace DrawBitmap.UserControls
{
    /// <summary>
    /// MessageShow.xaml 的交互逻辑
    /// </summary>
    public partial class MessageShow : UserControl
    {
        /// <summary>
        /// 这个表示消息的类型，比如仅仅的通知类型消息就不必添加同意拒绝...
        /// </summary>
        int messageType = 1;  //默认会显示按钮

        public MessageShow(MessageDone m)
        {
            InitializeComponent();
            mm = m;
            if (mm.Status == 1||mm.Status==-1)
            {
                button_ok.Visibility = Visibility.Hidden;
                button_cancel.Visibility = Visibility.Hidden;

                showlabel.Visibility = Visibility.Visible;
                this.button_ok.Visibility = Visibility.Collapsed;
                if (mm.Status == -1)
                    showlabel.Content = "已拒绝";
                if (mm.Status == 1)
                    showlabel.Content = "已同意";
            }
            else if(mm.Status==2)
            {
                
                button_ok.Visibility = Visibility.Hidden;
                button_cancel.Visibility = Visibility.Hidden;
                button_check.Visibility = Visibility.Collapsed;
                showlabel.Visibility = Visibility.Visible;
                showlabel.Content = "已阅";
            }
        }

        MessageDone mm;
        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            mm.Status = -1;
            button_ok.Visibility = Visibility.Hidden;
            button_cancel.Visibility = Visibility.Hidden;
            showlabel.Content = "已拒绝";
            showlabel.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 同意按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            mm.Status = 1;
            button_ok.Visibility = Visibility.Hidden;
            button_cancel.Visibility = Visibility.Hidden;
            showlabel.Content = "已同意";
            showlabel.Visibility = Visibility.Visible;
        }

        private void Grid_Initialized_1(object sender, EventArgs e)
        {
            
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            Message.Content = mm.message;
            messageType = mm.type;
            if(messageType==1)
            {
                    showlabel.Visibility = Visibility.Collapsed;
                    this.button_cancel.Visibility = Visibility.Visible;
                    this.button_ok.Visibility = Visibility.Visible;
                    this.button_check.Visibility = Visibility.Collapsed;
            }
            else if(messageType==2)
            {
                showlabel.Visibility = Visibility.Collapsed;
                this.button_cancel.Visibility = Visibility.Collapsed;
                this.button_ok.Visibility = Visibility.Collapsed;
                this.button_check.Visibility = Visibility.Visible;
            }
            else if (messageType == 3)
            {
                showlabel.Visibility = Visibility.Collapsed;
                this.button_cancel.Visibility = Visibility.Visible;
                this.button_ok.Visibility = Visibility.Visible;
                this.button_check.Visibility = Visibility.Collapsed;
            }    
            else
            {
                showlabel.Visibility = Visibility.Collapsed;
                this.button_cancel.Visibility = Visibility.Visible;
                this.button_ok.Visibility = Visibility.Visible;
                this.button_check.Visibility = Visibility.Collapsed;
            }
        }

        private void Button_Click_check(object sender, RoutedEventArgs e)
        {
            mm.Status = 2;
            button_ok.Visibility = Visibility.Hidden;
            button_cancel.Visibility = Visibility.Hidden;
            button_ok.Visibility = Visibility.Collapsed;
            button_check.Visibility = Visibility.Collapsed;
            showlabel.Content = "已阅";
            showlabel.Visibility = Visibility.Visible;
        }
    }
}
