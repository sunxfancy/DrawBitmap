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
using System.Windows.Threading;

namespace DrawBitmap.Windows
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class MyImage : UserControl
    {
        protected BitmapImage[] btnimg;
        public void setCurrentImage(int index)
        {
            if(index<3&&index>=0)
                this.imgbtn.Source = btnimg[index];
        }

        public BitmapImage getSourceByIndex(int index)
        {
            if (index < 3 && index >= 0)
                return btnimg[index];
            else
                return null;
        }
        public MyImage()
        {
            InitializeComponent();
            btnimg = new BitmapImage[3];
        }

        public MyImage(String path0,String path1,String path2)
        {
            InitializeComponent();
            btnimg = new BitmapImage[3];
            btnimg[0] = new BitmapImage(new Uri(path0,UriKind.Relative));
            btnimg[1] = new BitmapImage(new Uri(path1, UriKind.Relative));
            btnimg[2] = new BitmapImage(new Uri(path2, UriKind.Relative));
            this.imgbtn.Source = btnimg[0];
        }
        public void reLoad(String path0, String path1, String path2)
        {
            btnimg[0] = new BitmapImage(new Uri(path0, UriKind.Relative));
            btnimg[1] = new BitmapImage(new Uri(path1, UriKind.Relative));
            btnimg[2] = new BitmapImage(new Uri(path2, UriKind.Relative));
            this.imgbtn.Source = btnimg[0];
            //this.Visibility = Visibility.Visible;
        }

        private void imgbtn_MouseEnter(object sender, MouseEventArgs e)
        {
            this.imgbtn.Source = btnimg[1];
        }

        private void imgbtn_MouseLeave(object sender, MouseEventArgs e)
        {
            this.imgbtn.Source = btnimg[0];
        }

        private void imgbtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show("aaa");
            this.imgbtn.Source = btnimg[2];
        }

        private void imgbtn_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            this.imgbtn.Source = btnimg[1];
        }
        public void resetImage(MyImage myimg)
        {
            btnimg[0] = myimg.getSourceByIndex(0);
            btnimg[1] = myimg.getSourceByIndex(1);
            btnimg[2] = myimg.getSourceByIndex(2);
            this.imgbtn.Source = btnimg[1];
        }
    }
}
