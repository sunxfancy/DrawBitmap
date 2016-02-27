
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

namespace MyWork_D
{
    /// <summary>
    /// Canvas4All.xaml 的交互逻辑
    /// </summary>
    public partial class Canvas4All 
    {
        
        System.Windows.Ink.DrawingAttributes da;  //墨迹

        int Span_current_index=0;
        Image[] Span_image;
        //Text_2S text_ss;
        public Canvas4All()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            Span_image = new Image[2];
            Span_image[0] = this.Chat_Span;
            Span_image[1] = this.Pen_Span;

            selectSpan();
            da = new System.Windows.Ink.DrawingAttributes();
            this.Canvas_p.DefaultDrawingAttributes = da;
        }

        private void selectSpan(int index=0)
        {
            if (index == Span_current_index)
                return;

            if (index < 2)
            {
                Span_image[Span_current_index].Height = 20;
                Span_image[Span_current_index].Width = 20;
                Span_image[index].Height = 28;
                Span_image[index].Width = 28;
            }
            else return;
            if (Span_current_index == 0)
            {
                this.Chat_Edit.Visibility = Visibility.Collapsed;
                this.Chat_Show.Visibility = Visibility.Collapsed;
            }
            else if (Span_current_index == 1)
            {
                this.Pen_p.Visibility = Visibility.Collapsed;
            }
            if (index == 0)
            {
                this.Chat_Edit.Visibility = Visibility.Visible;
                this.Chat_Show.Visibility = Visibility.Visible;
            }
            else if(index==1)
            {
                this.Pen_p.Visibility = Visibility.Visible;
            }
            Span_current_index = index;
        }





        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void MIN(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MAX_NORMAL(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            if(this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                b.Content = "口";
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                b.Content = "せ";
            }
        }

        private void CLOSE(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Span_select(object sender, MouseButtonEventArgs e)
        {
            if(sender is Panel)
            {
                e.Handled = true;
            }
            else
            {
                Image b = sender as Image;
                if(b.Tag.ToString()=="0")
                {
                    selectSpan(0);
                }
                else
                {
                    selectSpan(1);
                }
            }
        }

        private void get_color(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            
            if(b.Tag.ToString()=="0")
            {
                da.Color = Colors.Black;
            }
            else if (b.Tag.ToString() == "1")
            {
                da.Color = Colors.BlueViolet;
            }
            else if (b.Tag.ToString() == "2")
            {
                da.Color = Colors.Red;
            }
            else if (b.Tag.ToString() == "3")
            {

            }
        }

        private void get_size(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;

            if (b.Tag.ToString() == "0")
            {
                da.Height = 2;
                da.Width = 2;
            }
            else if (b.Tag.ToString() == "1")
            {
                da.Height = 4;
                da.Width = 4;
            }
            else if (b.Tag.ToString() == "2")
            {
                da.Height = 66;
                da.Width = 66;
            }
            else if (b.Tag.ToString() == "3")
            {

            }
        }
    }
}
