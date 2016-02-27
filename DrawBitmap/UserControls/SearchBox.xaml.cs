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

namespace DrawBitmap
{
    /// <summary>
    /// SearchBox.xaml 的交互逻辑
    /// </summary>
    public partial class SearchBox : TextBox
    {
        public SearchBox()
        {
            InitializeComponent();
        }

        string _tip ="";

        public string SearchTip
        {
            set { _tip = value; Text = value; }
            get { return _tip; }
        }

        private void TextBox_GotFocus_1(object sender, RoutedEventArgs e)
        {
            if (Text == _tip)
            {
                Text = "";
            }
        }

        private void TextBox_LostFocus_1(object sender, RoutedEventArgs e)
        {
            if (Text == "")
            {
                Text = _tip;
            }
        }

        
    }
}
