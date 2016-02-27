using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Markup;
namespace DrawBitmap.Windows
{

 //   [ContentPropertyAttribute("MyContent")]
  public  class FatherWindow:Window
    {

        public object MyContent{
            set{
                ((Grid)Content).Children.Add((UIElement)value);
            }
        } 
      /// <summary>
      /// 背景画刷
      /// </summary>
        ImageBrush bk_brush;
      /// <summary>
      /// 透明度蒙版
      /// </summary>
        ImageBrush mask_brush;
        Canvas TitleBar;
        Button exit;
        Button min;
        Button menu;
        MyImage img_exit;
       protected  static BitmapImage[] min_buttonImg=new BitmapImage[3];
       protected static BitmapImage[] close_buttonImg=new BitmapImage[3];
        public FatherWindow()
        {
            InitializeComponent();
        }

        public static void TitleBarInit(Button min, Button close)
        {
            min.Background = new ImageBrush(min_buttonImg[1]);
            close.Background = new ImageBrush(close_buttonImg[1]);
            min.MouseEnter += (object sender, MouseEventArgs e)
                => { min.Background = new ImageBrush(FatherWindow.min_buttonImg[0]); };
            min.MouseLeave += (object sender, MouseEventArgs e)
                => { min.Background = new ImageBrush(FatherWindow.min_buttonImg[1]); };
            min.MouseLeftButtonUp += (object sender, MouseButtonEventArgs e)
                 => { min.Background = new ImageBrush(FatherWindow.min_buttonImg[0]); };
            min.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e)
                => { min.Background = new ImageBrush(FatherWindow.min_buttonImg[2]); };

            close.MouseEnter += (object sender, MouseEventArgs e)
                => { close.Background = new ImageBrush(FatherWindow.close_buttonImg[0]); };
            close.MouseLeave += (object sender, MouseEventArgs e)
                => { close.Background = new ImageBrush(FatherWindow.close_buttonImg[1]); };
            close.PreviewMouseLeftButtonUp += (object sender, MouseButtonEventArgs e)
                => { close.Background = new ImageBrush(FatherWindow.close_buttonImg[0]); };
            close.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e)
                => { close.Background = new ImageBrush(FatherWindow.close_buttonImg[2]); };

            min.BorderBrush = Brushes.Transparent;
            close.BorderBrush = Brushes.Transparent;
            min.Height = 40;
            min.Width = 40;
            close.Height = 40;
            close.Width = 40;

        }

      public static void buttonWrap(Button btn,Brush br)
      {
          if (br == null) br = Brushes.LightBlue;
          btn.BorderBrush = br;
          btn.Background = Brushes.Transparent;
          btn.Margin = new Thickness(5, 0, 5, 0);
      }
        static FatherWindow()
        {
            min_buttonImg[0] = new BitmapImage(new Uri(@"Resource/button/button50.png",UriKind.Relative));
            min_buttonImg[1] = new BitmapImage(new Uri(@"Resource/button/button51.png", UriKind.Relative));
            min_buttonImg[2] = new BitmapImage(new Uri(@"Resource/button/button52.png", UriKind.Relative));
            close_buttonImg[0] = new BitmapImage(new Uri(@"Resource/button/button40.png", UriKind.Relative));
            close_buttonImg[1] = new BitmapImage(new Uri(@"Resource/button/button41.png", UriKind.Relative));
            close_buttonImg[2] = new BitmapImage(new Uri(@"Resource/button/button42.png", UriKind.Relative));
            

        }


        /// <summary>
        /// 修改背景画刷，返回原先的画刷
        /// </summary>
        /// <param name="imgbrh"></param>
        /// <returns></returns>
        public ImageBrush setBackground(ImageBrush imgbrh)
        {
            ImageBrush re = mask_brush;
            mask_brush = imgbrh;
            this.Background = mask_brush;
            return re;
        }

      /// <summary>
      /// 修改蒙版画刷，返回原先的画刷
      /// </summary>
      /// <param name="imgbrh"></param>
      /// <returns></returns>
        public ImageBrush setMask(ImageBrush imgbrh)
        {
            ImageBrush re = bk_brush;
            bk_brush = imgbrh;
            this.OpacityMask = bk_brush;
            return re;
        }

        private void InitializeComponent()
        {
           //这里是父类的控件初始化部分
            this.WindowStyle = WindowStyle.None;
            this.AllowsTransparency = true;
            bk_brush=new ImageBrush();
            bk_brush.ImageSource=new BitmapImage(new Uri(@"Resource/father_bkg2.png",UriKind.Relative));
            this.Background = bk_brush;
            mask_brush=new ImageBrush();
            mask_brush.ImageSource = new BitmapImage(new Uri(@"Resource/father_mask.png", UriKind.Relative));
            this.OpacityMask = mask_brush;
            this.MouseLeftButtonDown += FatherWindow_MouseLeftButtonDown;
            this.Loaded += FatherWindow_Loaded;
           //this.Content = new Grid();
          //  addTitleButton((Grid)Content, true);
            img_exit = new MyImage(@"Resource/button/button40.png", @"Resource/button/button41.png", @"Resource/button/button42.png");


        }

        void FatherWindow_Loaded(object sender, RoutedEventArgs e)
        {
           // MessageBox.Show(this.Content.GetType().ToString());
          //  addTitleButton((Panel)this.Content);
        }




        void FatherWindow_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try{this.DragMove();}catch{}
        }
        public void addTitleButton(Panel content,bool isMenuNeed=false)
        {
            TitleBar = new Canvas();
            TitleBar.Height = 30;
            TitleBar.Width = 60;

            //TitleBar.HorizontalAlignment = HorizontalAlignment.Right;

            TitleBar.Background = Brushes.Transparent;
            DockPanel titlePanel = new DockPanel();
           // titlePanel.HorizontalAlignment = HorizontalAlignment.Right;
            titlePanel.Background = Brushes.Transparent;
            titlePanel.Height = 30;
            titlePanel.Width = 60;
            if (isMenuNeed)
            {
                TitleBar.Width += 30;
                titlePanel.Width += 30;
            }
            exit = new Button();
            exit.Height = 30;
            exit.Width = 30;
            exit.HorizontalAlignment = HorizontalAlignment.Right;
            exit.Content = "×";
            exit.FontSize = 20;
            exit.FontWeight = FontWeights.Heavy;
            exit.Click += exit_Click;
            min = new Button();
            min.Height = 30;
            min.Width = 30;
            min.Content = "—";
            exit.FontSize = 16;
            exit.FontWeight = FontWeights.Black;
            min.Click += min_Click;
            min.HorizontalAlignment = HorizontalAlignment.Right;
            if(isMenuNeed)
            {
                menu = new Button();
                menu.Height = 30;
                menu.HorizontalAlignment = HorizontalAlignment.Right;
                menu.Width = 30;
                menu.Content = "↓";
                titlePanel.Children.Add(menu);
            }
           // titlePanel.Children.Add(min);
            //titlePanel.Children.Add(exit);
            img_exit.setCurrentImage(0);

            content.Children.Add(img_exit);
            TitleBar.Children.Add(titlePanel);

            content.Children.Add(TitleBar);
            TitleBar.Margin = new Thickness(this.ActualWidth - TitleBar.Width, 0, 0, 0);
            TitleBar.SetValue(Grid.RowProperty, 0);
        }

        void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        void min_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        
    }
}
