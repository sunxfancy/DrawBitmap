using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.IO;
using Awesomium.Windows.Controls;

namespace TalkApp
{
    public static class MainData
    {
        public static string go_down_js = @"scroll(0,document.body.scrollHeight);";
        public static Custom Me = new Custom();
        public static Dictionary<string, Custom> user_dic = new Dictionary<string, Custom>();

        public static WebControl text_show;
        public static List<message_show> show_list;
        public static List<string> raw_text = new List<string>();
        public static List<string> orgin_text = new List<string>();
        public static MarkdownSharp.Markdown m = new MarkdownSharp.Markdown();
         //feawf
     //   public static void Show_New_Message(string name,string str)
     //   {
          //  message_show show = new message_show(name + ":", str);
          //  var datashow = App.main.DataShow;
         //   datashow.Dispatcher.Invoke(new Action(
           //     () => { datashow.Children.Add(show); }));
     //   }
       // public static WebSessionProvider websession = new WebSessionProvider();
    }
    public class Custom
    {
        public string name;
        public List<string> str_list = new List<string>(); 

        private void ShowString()
        {
            int len = 0;
            foreach (var item in MainData.orgin_text)
            {
                len += item.Length;
            }
            var sb = new StringBuilder(len);
            foreach (var item in MainData.orgin_text)
            {
                sb.Append(item);
            }

            MainData.text_show.Dispatcher.Invoke(new Action(
                () =>
                {
                    MainData.text_show.LoadHTML(
                    "<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"><style type=\"text/css\">" +
                    "body{font-family:Microsoft YaHei Mono;font-size:18px;}</style>"
                    + "</head>" + sb.ToString() + "</html>"
                    );
                    MainData.text_show.LoadingFrameComplete += text_show_LoadingFrameComplete;
                }));

        }

        void text_show_LoadingFrameComplete(object sender, Awesomium.Core.FrameEventArgs e)
        {
            MainData.text_show.ExecuteJavascriptWithResult(MainData.go_down_js);
        }
        public void AddString(string str)
        {
            str_list.Add(str);
            MainData.orgin_text.Add(str);
            ShowString();
       //     MainData.Show_New_Message(MainData.Me.name, str);
        }

        public void ChangeString(int i,string str)
        {
            if (i < 0 || i >= str_list.Count) return;
            str_list[i] = str;
            ShowString();
        }

        
    }
 
}
