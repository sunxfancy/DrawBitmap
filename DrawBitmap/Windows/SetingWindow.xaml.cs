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
using MulticastNetWork;
using System.Net;
using System.Security.Policy;
using WPF.Themes;
using System.Windows.Navigation;
using System.Drawing;
using System.Windows.Interop;
using DrawBitmap.Windows;

namespace DrawBitmap
{
    /// <summary>
    /// SetingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SetingWindow : FatherWindow
    {
        Button min_button = new Button();
        Button close_button = new Button();
        public static bool isOpen = false;
        
        public SetingWindow()
        {
            InitializeComponent();
            isOpen = true;
            FatherWindow.TitleBarInit(min_button, close_button);
            this.titleBar.Children.Add(min_button);
            this.titleBar.Children.Add(close_button);
            min_button.Click+=min_button_Click;
            close_button.Click+=close_button_Click;
            
            min_button.HorizontalAlignment = HorizontalAlignment.Right;
            close_button.HorizontalAlignment = HorizontalAlignment.Right;
            this.Closing += SetingWindow_Closing;
        }

        void SetingWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            isOpen = false;
        }

        private void min_button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void close_button_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void OK_button_Click(object sender, RoutedEventArgs e)
        {
            if (themes.Text!="")
            {
                ThemeManager.ApplyTheme(App.Current, themes.Text.ToString());
            }
           
            
            if (Sourcepath.Text != "")
            {
                System.Drawing.Image image = null;
                image = System.Drawing.Image.FromFile(Sourcepath.Text);
                var bitmap = new System.Drawing.Bitmap(image);
                var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(),
                                                                                      IntPtr.Zero,
                                                                                      Int32Rect.Empty,
                                                                                      BitmapSizeOptions.FromEmptyOptions()
                      );
                bitmap.Dispose();
                var brush = new ImageBrush(bitmapSource);

                App.mainWindow.Background = brush;
            }
           
        }

        private void Cancel_button_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = "*.jpg|*.jpg|*.jpeg|*.jpeg|*.bmp|*.bmp|*.gif|*.gif|*.png|*.png|*.Tiff|*.Tiff|*.Wmf|*.Wmf";
            if (ofd.ShowDialog() ==System.Windows.Forms.DialogResult.OK)
            {
                if (ofd.FileName == "")
                {
                    return;
                }
                Sourcepath.Text = ofd.FileName;
            }
        }
    }
}
