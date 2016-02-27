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

namespace MyWork_D
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary> 
    public partial class MainWindow : Window
    {
        MyImage btn1;
        Canvas4All cc;
        BitmapImage rr,rr2;
        public MainWindow()
        {
            InitializeComponent();
            rr = new BitmapImage(new Uri(@"Resource\chat.jpg", UriKind.Relative));
            rr2 = new BitmapImage(new Uri(@"Resource\Pen.jpg", UriKind.Relative));
            this.img.Source = rr;


            btn1 = new MyImage(@"Resource\1.jpg", @"Resource\dd.png", @"Resource\chat.jpg");
            this.pp.Children.Add(btn1);
            btn1.Height = 150;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility=Visibility.Collapsed;
            cc = new Canvas4All();
            if(cc.ShowDialog()!=null)
            {
                this.Visibility = Visibility.Visible;
            }
        }
   
        private void img_MouseEnter(object sender, MouseEventArgs e)
        {
            this.img.Source = rr2;
            this.img.Height = 60;
        }

        private void img_MouseLeave(object sender, MouseEventArgs e)
        {
            this.img.Source = rr;
            this.img.Height = 40;
        }

        private void img_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            cc = new Canvas4All();
            if (cc.ShowDialog() != null)
            {
                this.Visibility = Visibility.Visible;
            }
        }


    }
}
