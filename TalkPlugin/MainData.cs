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

namespace TalkPlugin
{
    public class MainData
    {
        public Custom Me = new Custom();
        public Dictionary<string, Custom> user_dic = new Dictionary<string, Custom>();

        public WebBrowser text_show;
        public List<message_show> show_list;
        public List<string> raw_text = new List<string>();
        public List<string> orgin_text = new List<string>();
        public MarkdownSharp.Markdown m = new MarkdownSharp.Markdown();
        internal App app;

        public void Show_New_Message(string name,string str)
        {
            message_show show = new message_show(name + ":", str);
            var datashow = app.main.DataShow;
            datashow.Dispatcher.Invoke(new Action(
                () => { datashow.Children.Add(show); }));
        }
       // public static WebSessionProvider websession = new WebSessionProvider();
    }
    public class Custom
    {
        public string name;
        public List<string> str_list = new List<string>(); 
        public App app;
        private void ShowString()
        {
            int len = 0;
            foreach (var item in app.data.orgin_text)
            {
                len += item.Length;
            }
            var sb = new StringBuilder(len);
            foreach (var item in app.data.orgin_text)
            {
                sb.Append(item);
            }
            app.data.text_show.Dispatcher.Invoke(new Action(
                () =>
                {
                    app.data.text_show.NavigateToString(
                    "<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"></head>"
                    + sb.ToString() + "</html>"
                    ); }));
        }
        public void AddString(string str)
        {
            str_list.Add(str);
            app.data.orgin_text.Add(str);
            ShowString();
            app.data.Show_New_Message(app.data.Me.name, str);
        }

        public void ChangeString(int i,string str)
        {
            if (i < 0 || i >= str_list.Count) return;
            str_list[i] = str;
            ShowString();
        }
    }
 
}
