using DrawBitmap.Windows;
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

namespace DrawBitmap
{
    /// <summary>
    /// AddFriends.xaml 的交互逻辑
    /// </summary>
    public partial class AddFriends : FatherWindow
    {
        Button min_button = new Button();
        Button close_button = new Button();
        public static bool isOpen = false;
        public AddFriends()
        {
            InitializeComponent();
            isOpen = true;
            FatherWindow.TitleBarInit(min_button, close_button);
            min_button.Click+=min_button_Click;
            close_button.Click+=close_button_Click;
            this.titleBar.Children.Add(min_button);
            this.titleBar.Children.Add(close_button);
            this.close_button.Margin = new Thickness(-5, 0, 5, 0);
            this.Closing += AddFriends_Closing;
        }

        void AddFriends_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            isOpen = false;
        }

        private void min_button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void close_button_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void DragWindowHandler(object sender, MouseButtonEventArgs e)
        {
            try
            { this.DragMove(); }
            catch { }
        }
    }
}
