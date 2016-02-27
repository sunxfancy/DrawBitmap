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
using DrawBitmap.UserControls;
using DrawBitmap.MainClass;

namespace DrawBitmap.Windows
{




    /// <summary>
    /// MessageList.xaml 的交互逻辑
    /// </summary>
    public partial class MessageList : FatherWindow
    {
        Button min_button = new Button();
        Button close_button = new Button();
        public static bool isOpen = false;
        public MessageList()
        {
            InitializeComponent();
            isOpen = true;
            FatherWindow.TitleBarInit(min_button, close_button);
            this.titleBar.Children.Add(min_button);
            this.titleBar.Children.Add(close_button);
            min_button.HorizontalAlignment = HorizontalAlignment.Right;
            close_button.HorizontalAlignment = HorizontalAlignment.Right;
            min_button.Click += min_button_Click;
            close_button.Click += close_button_Click;
            this.Closing += MessageList_Closing;
        }

        void MessageList_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            isOpen = false;
        }

        void close_button_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility=Visibility.Hidden;
        }

        void min_button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }




        public UIElementCollection Children
        {
            get { return data.Children; }
        }

        private void Window_Initialized_1(object sender, EventArgs e)
        {
          //  var m = new MessageAddFriend(null);
           // m.message = string.Format("{0} 请求加您为好友", "sxf");
           // App.data.MessageList.Add(m);

            foreach (var item in App.data.MessageList)
            {
                MessageShow ms = new MessageShow(item);
                ms.ToolTip=item.id;
                Children.Add(ms);
            }
        }
    }
}
