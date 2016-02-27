
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
using WpfControlLibrary1;

namespace MyWork_D
{
    /// <summary>
    /// Canvas4All.xaml 的交互逻辑
    /// </summary>
    public partial class Canvas4All 
    {
        System.Windows.Ink.DrawingAttributes da;  //墨迹
        InkCanvasExt test = new InkCanvasExt();
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
            da = new System.Windows.Ink.DrawingAttributes();
            this.Canvas_p.DefaultDrawingAttributes = da;


            this.grid_0.Children.Add(test);
            test.SetValue(Grid.RowProperty, 1);
            test.SetValue(Grid.ColumnSpanProperty, 2);

            selectSpan();
        }



        private void selectSpan(int index=0)
        {
            double parent_width = this.select_Panel.ActualWidth;
            if (index == Span_current_index)
            {
                Span_image[index].Height = parent_width * 0.7;
                Span_image[index].Width = parent_width * 0.7;
                Span_image[1^index].Height = parent_width * 0.5;
                Span_image[1^index].Width = parent_width * 0.5;
                 return;
            }
              

            if (index < 2)
            {
               
                Span_image[Span_current_index].Height = parent_width*0.5;
                Span_image[Span_current_index].Width = parent_width * 0.5;
                Span_image[index].Height = parent_width * 0.7;
                Span_image[index].Width = parent_width * 0.7;
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
            selectSpan(this.Span_current_index);
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

        private void TEXT_Send(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Return)
            {
                //this.Chat_Show.Inlines.Add(new Run(this.Chat_Edit.Text));
                //this.Chat_Show.Inlines.Add(new LineBreak());
                if(this.Chat_Edit.Text != "")
                {
                     this.Chat_Show.AppendText(this.Chat_Edit.Text+ Environment.NewLine);
                     this.Chat_SHOW_SCroll.ScrollToBottom();
                }
                this.Chat_Edit.Text = "";
               // MessageBox.Show(this.Chat_Edit.Text);
            }

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            selectSpan(this.Span_current_index);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           // da.
        }
    }
}
