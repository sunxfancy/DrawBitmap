using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFDemo
{
    /// <summary>
    /// Panel1.xaml 的交互逻辑
    /// </summary>
    public partial class Panel1 : UserControl
    {
        private string datetime;
        UIElementCollection Father;
        Rotate3DContainer r3c;
        
        
        public Panel1(UIElementCollection ff,Rotate3DContainer f)
        {
            InitializeComponent();
            Father = ff;
            r3c = f;
            if (datetime ==null)
            {
                datetime = DateTime.Now.ToString();
                date.Content = datetime;
            }
            this.Height = 100;
            this.Width = 200;
            this.MouseDoubleClick += new MouseButtonEventHandler(Panel1_MouseDown);
        }

        void Panel1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Rotate3DContainer c = (Rotate3DContainer)ContainerUtils.GetNearestContainer(this);
            if (c != null)
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                    c.Turn(true);
                }
                else if (e.ChangedButton == MouseButton.Right)
                {
                    c.Turn(false);
                }
            }
            
        }

       

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            this.userc1.Width = 200;
            this.userc1.Height = 100;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Father.Remove(r3c);
           
        }

       
    }
}
