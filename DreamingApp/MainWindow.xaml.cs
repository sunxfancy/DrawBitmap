using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DreamingApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 跟踪鼠标的位置
        /// </summary>
        public Point p;

        /// <summary>
        /// 启动时的命令行参数，如果没有，则认为是运行在单机模式下。
        /// </summary>
        private string[] StartupArgs;

        /// <summary>
        /// 目标主机ip
        /// </summary>
        private string ip;

        /// <summary>
        /// 目标主机端口号
        /// </summary>
        private int port =9998;

        /// <summary>
        /// 判断是否为服务器，1为服务器，-1为客户端，0为单机模式
        /// </summary>
        public int isServer = 0;

        internal void init()
        {
            InitializeComponent();
            App.ink = this.ink;
            ink.DefaultDrawingAttributes = MainData.Me.da;
            MainData.Me.da.FitToCurve = true;
            _itransform = _transform.Inverse;
        }
        public MainWindow()
        {
            init();
        }

        public MainWindow(string[] p1)
        {
            this.StartupArgs = p1;
            init();
        }


        private void Window_Initialized_1(object sender, EventArgs e)
        {
            ///test
             if (StartupArgs == null)
             {
                 StartupArgs = new string[3];
                 StartupArgs[0] = "Server";
                 StartupArgs[1] = "sxf";
                 StartupArgs[2] = "172.16.49.192";
             }

            if (StartupArgs != null)
            {

                if (StartupArgs.Length > 2)
                {
                    MainData.Me.name = StartupArgs[1];
                    ip = StartupArgs[2];
                }
                else
                {
                    return;
                }

                if (StartupArgs[0] == "Server")
                {
                   // MessageBox.Show(String.Format("服务器，用户名{0},IP{1}", StartupArgs[1], ip.ToString()));
                    isServer = 1;
                    
                    try
                    {
                        Server.ServerRun(ip, port);
                    }
                    catch (Exception e1)
                    {
                        MessageBox.Show("Server.ServerRun" + e1.Message);
                    }
                    try
                    {
                        Client.Connect(ip, port);
                    }
                    catch(Exception e2)
                    {
                        MessageBox.Show("Client.Connect" + e2.Message);
                    }
                }
                if (StartupArgs[0] == "Client")
                {
                   // MessageBox.Show(String.Format("客户端，用户名{0},IP{1}", StartupArgs[1], ip.ToString()));
                    isServer = -1;
                    Client.Connect(ip, port);
                    Client.SendLogin();
                }
                
            }
        }



        private void InkCanvas_PreviewMouseRightButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            p = e.GetPosition(sender as InkCanvas);
            popup.HorizontalOffset = p.X - 75;
            popup.VerticalOffset = p.Y + 125;
            popup.IsOpen = true;
        }

        private void Window_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.C)
            {
                popup.HorizontalOffset = p.X - 75;
                popup.VerticalOffset = p.Y + 125;
                popup.IsOpen = true;
            }

            if (e.Key == Key.D)
            {
                ClearAction();
            }

            if (e.Key == Key.E)
            {
                if (ink.EditingMode == InkCanvasEditingMode.EraseByPoint)
                {
                    ink.EditingMode = InkCanvasEditingMode.Ink;
                }
                else
                {
                    ink.EditingMode = InkCanvasEditingMode.EraseByPoint;
                }
            }

            if (e.Key == Key.F)
            {
                if (ink.EditingMode == InkCanvasEditingMode.EraseByStroke)
                {
                    ink.EditingMode = InkCanvasEditingMode.Ink;
                }
                else
                {
                    ink.EditingMode = InkCanvasEditingMode.EraseByStroke;
                }
            }

            if (e.Key == Key.Z)
            {
                UndoAction();
            }

            if (e.Key == Key.Q)
            {
                ZoomAdd(p);
            }
            if (e.Key == Key.W)
            {
                ZoomDec(p);
            }
        }

        private void ClearAction()
        {
            if (StartupArgs!= null)
            {
                MainData.Me.Clear();
                Client.SendClear();
            }
            else
            {
                ink.Strokes.Clear();
            }
        }

        private void UndoAction()
        {
            if (StartupArgs != null)
            {
                MainData.Me.Undo();
                Client.SendUndo();
            }
            else
            {
                if (ink.Strokes.Count > 0)
                    ink.Strokes.RemoveAt(ink.Strokes.Count - 1);
            }
        }

        private void ink_MouseMove(object sender, MouseEventArgs e)
        {
            p = e.GetPosition(scrollViewer);
        }

        private void ink_StrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
        {
            //CheckandSend();
            if (isServer != 0)
            {
                MainData.Me.AddStroke(e.Stroke);
                Client.SendStroke(e.Stroke);
                //Client.SendMessage(2, "HelloWorld");
            }
        }

        public void CheckandSendColor()
        {
            if (isServer != 0)
            {
                Debug.WriteLine("Click");
                if (MainData.isColorNeedUpdate)
                {
                    Client.SendColor(MainData.Me.da.Color);
                    MainData.isColorNeedUpdate = false;
                }
                if (MainData.isWidthNeedUpdata)
                {
                    Client.SendLineWidth(MainData.LineWidth);
                    MainData.isWidthNeedUpdata = false;
                }
            }
        }

        private void ink_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CheckandSendColor();
        }

        private void ink_StrokeErased(object sender, RoutedEventArgs e)
        {
            var s = e.Source as InkCanvas;

            if (s != null)
                MessageBox.Show("Hello");
        }

        private void ink_StrokesReplaced(object sender, InkCanvasStrokesReplacedEventArgs e)
        {
            
        }


        ScaleTransform transform = new ScaleTransform(1, 1);
        ScaleTransform _transform = new ScaleTransform(1.1, 1.1);
        GeneralTransform _itransform;
        private void ZoomAdd(Point p)
        {
           // Point nowp = transform.Transform(p);
            transform.ScaleX *= 1.1;
            transform.ScaleY *= 1.1;
            ink.LayoutTransform = transform;
            var v = new Vector(scrollViewer.HorizontalOffset,scrollViewer.VerticalOffset);

            Point point = p + v;
            Point newCenter = _transform.Transform(point);
            var newp = newCenter - v;
            scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + newp.X - p.X);
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + newp.Y - p.Y);
        }
        private void ZoomDec(Point ppp)
        {
            transform.ScaleX = transform.ScaleX <= 1 ? 1 : transform.ScaleX / 1.1;
            transform.ScaleY = transform.ScaleY <= 1 ? 1 : transform.ScaleY / 1.1;
            ink.LayoutTransform = transform;

            var v = new Vector(scrollViewer.HorizontalOffset, scrollViewer.VerticalOffset);
            Point point = p + v;
            Point newCenter = _itransform.Transform(point);
            var newp = newCenter - v;
            scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + newp.X - p.X);
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + newp.Y - p.Y);
        }

        private void ink_MouseWheel(object sender, MouseWheelEventArgs e)
        {
           var point = e.GetPosition(scrollViewer);
           
           if (e.Delta >0 )
           {
               ZoomAdd(p);
           }
           else
           {
               ZoomDec(p);
           }
        }
       

    }
}
