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
using System.Collections.ObjectModel;
using System.Drawing;
using System.Runtime.InteropServices;
using DrawBitmap;
namespace FilesShare
{
    /// <summary>
    /// MyDetailFolder.xaml 的交互逻辑
    /// </summary>
    public partial class MyDetailFolder : ListBox
    {

        private static BitmapImage f_img;
        private static BitmapImage d_img;
        
        public static MyDetailFolder top_parent=null;

        [DllImport("gdi32")]

        static extern int DeleteObject(IntPtr o);

        /*
        int current_row = 0;  //第0行为工具栏吧
        int current_col = 0;
        int col_max = 4;
        int row_max = 0;*/

        static DirectoryInfo parent=null;
        
        public bool IsReady=false;
        List<DirectoryInfo> d_list = new List<DirectoryInfo>();
        List<FileInfo> f_list = new List<FileInfo>();
        ObservableCollection<FileData> fd_list = new ObservableCollection<FileData>();

        static MyDetailFolder()
        {
            d_img = new BitmapImage(new Uri(@"Resource\folder_c.ico",UriKind.Relative));
            f_img = new BitmapImage(new Uri(@"Resource\file.ico", UriKind.Relative));
            //MessageBox.Show("正常");
            
        }
        public MyDetailFolder()
        {
            InitializeComponent();
            this.MouseDown += MyDetailFolder_MouseDown;
            FileListBox.ItemsSource = fd_list;
            top_parent = this;
            
        }

        void MyDetailFolder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show("正常");
        }

        public void setParent(DirectoryInfo _p)
        {
            if (!_p.Exists)
                return;
            parent=_p;
            f_list.Clear();
            d_list.Clear();
            fd_list.Clear();
            /*
            this.children.Children.Clear();
            current_col = 0;
            current_row = 0;*/

            IsReady=true;
            layout();
        }


        private void goToNextPosition()
        {
            /*
            if(++current_col>col_max)
            {
                if(++current_row>row_max)
                {
                    addRow();
                }
                current_col = 0;
            }*/
        }
        private  void addRow()
        {
            /*
            RowDefinition r_d = new RowDefinition();
            r_d.Height = new GridLength(40,GridUnitType.Pixel);
            this.children.RowDefinitions.Add(r_d);
            ++row_max;*/

        }
        private void layout()
        {
            if (!IsReady)
                return;
            d_list.AddRange(parent.GetDirectories());
            f_list.AddRange(parent.GetFiles());

            foreach(var item in d_list)
            {
                /*
                Image t_i = new Image();
                t_i.Source = d_img;
                t_i.ToolTip = item.Name;
                t_i.Tag = item.FullName;
                t_i.MouseLeftButtonUp+=updateFolderLayout;
                t_i.SetValue(Grid.RowProperty, current_row);
                t_i.SetValue(Grid.ColumnProperty, current_col);
                this.children.Children.Add(t_i);
                goToNextPosition();*/
                fd_list.Add(new FileData { Name = item.Name, Pic = d_img });
            }
            foreach(var item in f_list)
            {
              /*  Image t_i = new Image();
                t_i.Source = f_img;
                t_i.ToolTip = item.Name;
                t_i.Tag = item.FullName;
                t_i.SetValue(Grid.RowProperty, current_row);
                t_i.SetValue(Grid.ColumnProperty, current_col);
                this.children.Children.Add(t_i);
                goToNextPosition();*/
                if (MainWindow.window.friend.Equals(API.Me))
                {
                    Icon i = System.Drawing.Icon.ExtractAssociatedIcon(item.FullName);
                    IntPtr ip = i.ToBitmap().GetHbitmap();
                    BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                        ip, IntPtr.Zero, Int32Rect.Empty,
                        System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                    DeleteObject(ip);
                    fd_list.Add(new FileData { Name = item.Name, Pic = bitmapSource });
                }
                else
                {
                    fd_list.Add(new FileData { Name = item.Name, Pic = f_img });
                }
            }
            
        }

        private void updateFolderLayout(object sender, MouseButtonEventArgs e)
        {
                
               // Image s = sender as Image;
                ListBox lb = (ListBox)sender;
               // MessageBox.Show(parent.FullName + "\\" + ((FileData)lb.SelectedItem).Name);
                if (parent.FullName.Length > 3)
                MyDetailFolder.parent = new DirectoryInfo(parent.FullName + "\\" + ((FileData)lb.SelectedItem).Name);
                else
                MyDetailFolder.parent = new DirectoryInfo(parent.FullName  + ((FileData)lb.SelectedItem).Name);
                f_list.Clear();
                d_list.Clear();
                fd_list.Clear();
                //this.children.Children.Clear();
                //current_col = 0;
                //current_row = 0;
                if (parent.Exists)
                {
                    MainWindow.window.currentFolder.Text = parent.FullName;
                    MyFolder.top_parent.Update(parent);
                    this.layout();
                }
                else
                    MessageBox.Show("不存在呃呃");
          }
    

        private void up(object sender, RoutedEventArgs e)
        {

            if(IsReady)
            {
                if (parent.Root.FullName == parent.FullName)
                    return;
                setParent(parent.Parent);
            }
                
        }

        private void backspace(object sender, KeyEventArgs e)
        {   
            
            if (e.Key == Key.Back)
            {
                if (parent.FullName.Length <= 3)
                    return;
                parent = new DirectoryInfo(parent.FullName.Remove(parent.FullName.LastIndexOf(parent.Name)));
                if (parent.FullName.Length > 3)
                {
                    parent = new DirectoryInfo(parent.FullName.Remove(parent.FullName.Length - 1));
                }
                f_list.Clear();
                d_list.Clear();
                fd_list.Clear();
                //this.children.Children.Clear();
                //current_col = 0;
                //current_row = 0;
                if (parent.Exists)
                {
                    MainWindow.window.currentFolder.Text = parent.FullName;
                    MyFolder.top_parent.Update(parent);
                    this.layout();
                }
                else
                    MessageBox.Show("不存在呃呃");

            }
        }
    }

    public class FileData
    {
        public string Name { get; set; }
        public BitmapSource Pic { get; set; }
    }
}
