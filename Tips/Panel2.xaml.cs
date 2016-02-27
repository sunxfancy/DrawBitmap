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
    /// Panel2.xaml 的交互逻辑
    /// </summary>
    public partial class Panel2 : UserControl
    {
       
        public Panel2()
        {
            InitializeComponent();
            this.MouseDoubleClick += new MouseButtonEventHandler(Panel2_MouseDown);
        }
        
        void Panel2_MouseDown(object sender, MouseButtonEventArgs e)
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
            this.userc2.Width = 200;
            this.userc2.Height = 100;
        }

        private void userc2_MouseMove(object sender, MouseEventArgs e)
        {
            this.userc2.Width = 440;
            this.userc2.Height = 300;
        }
    }
}
