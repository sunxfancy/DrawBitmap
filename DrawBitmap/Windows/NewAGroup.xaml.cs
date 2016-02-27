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
using System.Windows.Shapes;

namespace DrawBitmap.Windows
{
    /// <summary>
    /// NewAGroup.xaml 的交互逻辑
    /// </summary>
    public partial class NewAGroup : FatherWindow
    {
        public static bool isOpen = false;
        Button min_button = new Button();
        Button close_button = new Button();
        public NewAGroup()
        {
            InitializeComponent();
            isOpen = true;
            FatherWindow.TitleBarInit(min_button, close_button);
            this.titleBar.Children.Add(min_button);
            this.titleBar.Children.Add(close_button);
            min_button.Click += min_button_Click;
            close_button.Click += close_button_Click;

            min_button.HorizontalAlignment = HorizontalAlignment.Right;
            close_button.HorizontalAlignment = HorizontalAlignment.Right;
            this.Closing += SetingWindow_Closing;
        }

        private void min_button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void close_button_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }
        void SetingWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            isOpen = false;
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            if (this.groupName.Text == "")
            {
                System.Windows.MessageBox.Show("╭(╯^╰)╮ 8#:请填写群组名");
                this.groupName.Focus();
                return;
            }

            if (this.groupdetail.Text == "")
            {
                System.Windows.MessageBox.Show("╭(╯^╰)╮ 8#:请填写群组描述");
                this.groupdetail.Focus();
                return;
            }

            List<object>  pramsToSend=new List<object>(3);
            pramsToSend.Add(App.data.Me.user_id);
            pramsToSend.Add(this.groupName.Text);
            pramsToSend.Add(this.groupdetail.Text);

            if(ServerAPI.newAGroup(pramsToSend))
            {
                System.Windows.MessageBox.Show("群组"+groupName.Name+"创建成功");
            }
            else
            {
                System.Windows.MessageBox.Show("╭(╯^╰)╮ 9#:失败，群名字重复了");
            }
            

        }

    }
}
