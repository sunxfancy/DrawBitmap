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

namespace DrawBitmap
{
    /// <summary>
    /// SwingImage.xaml 的交互逻辑
    /// </summary>
    public partial class SwingImage : UserControl
    {
        public SwingImage(CanSelect user)
        {
            InitializeComponent();
            User = user;
            Updata();
        }

        public void Updata()
        {
            Source = User.GetImage();
            UserName = User.GetName();
            if (User.isSelected)
            {
                selectImage.Visibility = Visibility.Visible;
            }
            else
            {
                selectImage.Visibility = Visibility.Hidden;
            }
        }

        public ImageSource Source
        {
            set
            {
                source.ImageSource = value;
            }
            get
            {
                return source.ImageSource;
            }
        }

        public String UserName
        {
            set
            {
                label.Text = value;
            }
            get
            {
                return label.Text as String;
            }
        }

        public CanSelect User;


        private void UserControl_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void UserControl_MouseRightButtonDown_1(object sender, MouseButtonEventArgs e)
        {
           // MessageBox.Show("RBClick");
            User.Select();
            Updata();
        }

        private void UserControl_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
