using Awesomium.Core;
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

namespace TalkPlugin
{
    /// <summary>
    /// message_show.xaml 的交互逻辑
    /// </summary>
    public partial class message_show : UserControl
    {
        public message_show(string name,string text)
        {
            InitializeComponent();
            UserText = name;
            ShowHtml(text);
        }

        public string HtmlText
        {
            set
            {
               web.LoadHTML("<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"></head>" + value + "</html>");
            }
        }

        public string UserText
        {
            set
            {
                user.Content = value;
            }

            get
            {
                return user.Content as string;
            }
        }

        public ImageSource Source
        {
            set 
            {
                user_image.Source = value;
            }
            get
            {
                return user_image.Source;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        string htmltext;
        public void ShowHtml(string str)
        {
            htmltext = str.Clone() as string;
        }
        private void web_Loaded(object sender, RoutedEventArgs e)
        {
            this.HtmlText = htmltext;
            
        }

        private void web_LoadingFrameComplete(object sender, FrameEventArgs e)
        {
           // MessageBox.Show("Hello");
            var h = web.ExecuteJavascriptWithResult("document.body.scrollHeight");
            if(h.IsDouble||h.IsInteger||h.IsNumber)
            {
                web.Height = (double)h;

            }
            else
            {
                MessageBox.Show("unblivable");
            }
        }
    }
}
