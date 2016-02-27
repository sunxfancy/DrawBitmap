using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FilesShare;
using DrawBitmap.Windows;
using MulticastNetWork;
using DrawBitmap;
namespace FilesShare
{
    
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    
    
    public partial class MainWindow : FatherWindow
    {

        MyFolder myf;
        MyDetailFolder mydf;
        public Friend friend;
        public static MainWindow window = null;
        private DirectoryInfo di;

        public DirectoryInfo Di
        {
            get { return di; }
            set { di = value; }
        }
        public MainWindow(Friend f)
        {
            friend = f;
            InitializeComponent();
            mydf = new MyDetailFolder();
            myf = new MyFolder();
            window = this;
            init();
            
        }

        private void init()
        {

            //myf.Update(new DirectoryInfo(@"E:\Documents\自用代码"));
            this.myGrid.Children.Add(myf);
            myf.SetValue(Grid.RowProperty, 2);
            myf.SetValue(Grid.ColumnProperty, 0);
            myf.SetValue(VerticalAlignmentProperty,VerticalAlignment.Top);
            myf.setChildrenPanelHeight(250);
            //this.folder_detail.setParent(MyFolder.top_parent.di);
            //mydf.setParent(MyFolder.top_parent.di);
             
            this.myGrid.Children.Add(mydf);
            mydf.SetValue(Grid.ColumnProperty, 1);
            mydf.SetValue(Grid.RowProperty, 2);
            if (!friend.Equals(API.Me))
            {
                Set.Visibility = System.Windows.Visibility.Hidden;
            }         

            
        }


        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public  void ShowShareFolder(DirectoryInfo di)
        {
            this.currentFolder.Text = di.FullName;
            if (di.Exists)
            {
                myf.Update(di);
                mydf.setParent(MyFolder.top_parent.di);
            }
        }
        private void SetShareFolder(object sender, RoutedEventArgs e)
        {
            
            
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "请选择你想要设置的共享文档";
            fbd.ShowNewFolderButton = true;

            fbd.ShowDialog();
            if (fbd.SelectedPath.Trim().Equals(""))
                return;
            
            this.currentFolder.Text = fbd.SelectedPath;
            DirectoryInfo temp_d=new DirectoryInfo(fbd.SelectedPath);
            if (temp_d.Exists)
            {
                myf.Update(temp_d);
                mydf.setParent(MyFolder.top_parent.di);
            }
        }

        private void Ensure(object sender, RoutedEventArgs e)
        {
            if (this.currentFolder.Text != null&&!this.currentFolder.Text.Equals(""))
            {
                if (friend.Equals(API.Me))
                {
                    FilesShare.PluginMain.di = new DirectoryInfo(this.currentFolder.Text);
                }
                else
                {
                    FilesShare.PluginMain.acceptData = PluginMain.AcceptState.Ready;
                    FilesShare.PluginMain.GetAnswers(null, null);
                }
            }
        }
    }
  
}
