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
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace WPFDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public static int count=0;
        private void Container_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Rotate3DContainer r3c = new Rotate3DContainer();
            Panel1 panel1 = new Panel1(maingrid.Children,r3c);
            Panel2 panel2= new Panel2();
            maingrid.Children.Add(r3c);
            r3c.MouseDown+=r3c_MouseDown;
            r3c.Children.Add(panel1);
            r3c.Children.Add(panel2);
            count++;
            panel1.number.Content = count.ToString();
        }
        private int top;
        private void r3c_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Canvas canvas = (Canvas)sender;
            canvas.SetValue(Panel.ZIndexProperty,top++);
           
        }

        

    }
}
