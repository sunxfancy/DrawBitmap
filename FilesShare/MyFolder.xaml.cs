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
using System.IO;
using DrawBitmap;
namespace FilesShare
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class MyFolder : UserControl
    {

        enum FolderState{Open,Close}
        int heightOfLine = 20;
        bool isLoad;
        static public  MyFolder top_parent=null;
        FolderState f_state=new FolderState();
        public DirectoryInfo di=null;
        static BitmapImage[] folder_img = new BitmapImage[2];
        static BitmapImage file_img = new BitmapImage();
        List<FileInfo> children_file_list=new List<FileInfo>();
        List<MyFolder> folder_list = new List<MyFolder>();
        static MyFolder()
        {
            folder_img[0] = new BitmapImage(new Uri(@"Resource/folder_o.ico", UriKind.Relative));
            folder_img[1] = new BitmapImage(new Uri(@"Resource/folder_c.ico", UriKind.Relative));
            file_img = new BitmapImage(new Uri(@"Resource/file.ico", UriKind.Relative));
        }

        public MyFolder()
        {
            InitializeComponent();
            top_parent = this;
            this.f_panel.MouseLeftButtonDown += o_c;
            this.f_image.MouseLeftButtonDown += o_c;
            inti();
            
        }
        public MyFolder(DirectoryInfo _di)
        {
            InitializeComponent();
            this.f_panel.MouseLeftButtonDown += o_c;
            this.f_image.MouseLeftButtonDown += o_c;
            di = _di;
            folder_name.Text = di.Name;
            inti();
        }

        public void Update(DirectoryInfo _di)
        {
            di = _di;
            top_parent = this;
            folder_name.Text = di.Name;
            isLoad = false;
            inti();
        }

        public void setChildrenPanelHeight(double h)
        {
            this.Children_scroll.Height = h;
        }


        private void inti()
        {
            f_state = FolderState.Close;
          //  this.Height = heightOfLine;
            this.Children.Children.Clear();
        }



        private void o_c(object sender, MouseButtonEventArgs e)
        {
            if (di == null)
                return;
            if(e.ClickCount==2)
            {

                if (f_state == FolderState.Close)
                {
                    f_state = FolderState.Open;
                    f_image.Source = folder_img[0];
                    if (!isLoad)
                    {
                        childrenLoad();
                        //folder_name.Text = folder_name.Text + "("+folder_list.Count.ToString()+")" ;
                        foreach (var item in folder_list)
                        {
                            this.Children.Children.Add(item);
                        }
                        foreach (var item in children_file_list)
                        {
                            //就先上文本框吧
                            DockPanel temp_panel = new DockPanel();
                            temp_panel.Height = heightOfLine;
                            temp_panel.MaxWidth = 121;
                            Image temp_img_control = new Image();
                            temp_img_control.Height = heightOfLine;
                            temp_img_control.Width = heightOfLine;
                            temp_img_control.Source = file_img;
                            temp_img_control.SetValue(Panel.HorizontalAlignmentProperty, HorizontalAlignment.Left);

                            TextBlock temp_text = new TextBlock();
                            temp_text.Height = heightOfLine;
                            temp_text.Text = item.Name;
                            temp_text.Foreground = Brushes.Black;
                            temp_text.SetValue(Panel.HorizontalAlignmentProperty, HorizontalAlignment.Left);

                            temp_panel.Children.Add(temp_img_control);
                            temp_panel.Children.Add(temp_text);

                            this.Children.Children.Add(temp_panel);
                            //this.Children.Height = this.Children.Height + heightOfLine;
                            
                        }

                    }
                    this.Children.Visibility = Visibility.Visible;
                    //this.Height = this.Children.ActualHeight + heightOfLine;
                    MyDetailFolder.top_parent.setParent(di);
                }
                else
                {
                    f_state = FolderState.Close;
                    f_image.Source = folder_img[1];
                    this.Children.Visibility = Visibility.Collapsed;
                    
                    //this.Height =heightOfLine;
                }
                //MyFolder.top_parent.updateHeight();
                return;
            }

 /*               if (this.f_panel.Background==Brushes.White)
                {
                    this.f_panel.Background = Brushes.Gray;
                }
                else
                {
                    this.f_panel.Background = Brushes.White;
                }
  */
        }

        public double updateHeight()
        {
            if(f_state==FolderState.Close)
            {
                this.Height = heightOfLine;
            }
            else
            {
                double temp_height = 0;
                foreach(var item in folder_list)
                {
                    temp_height += item.updateHeight();
                }
                this.Height = heightOfLine*(1+children_file_list.Count)+temp_height;
            }
            return this.Height;
        }

        private void childrenLoad()
        {

            children_file_list.Clear();
            folder_list.Clear();
            children_file_list.AddRange(di.GetFiles());
           // MessageBox.Show("发现" + di.GetDirectories().Length .ToString()+ "个文件夹");
           
            //children_folder_list.AddRange();
            foreach (var item in di.GetDirectories())
            {
                folder_list.Add(new MyFolder(item));
            }
            
            isLoad = true;
        }


        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {


        }



    }
}
