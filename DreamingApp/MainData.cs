using ColorWheel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.IO;

namespace DreamingApp
{
    public static class MainData
    {
        public static Custom Me = new Custom();
        public static Dictionary<string, Custom> user_dic = new Dictionary<string, Custom>();

        public static PaletteColor color = new PaletteColor();
        public static double LineWidth;

        public static bool isColorNeedUpdate = false;
        public static bool isWidthNeedUpdata = false;

        public static void ImageSave(string _imageFile)
        {
            InkCanvas inkCanvas = App.ink;
            double width = inkCanvas.ActualWidth;
            double height = inkCanvas.ActualHeight;
            RenderTargetBitmap bmpCopied = new RenderTargetBitmap((int)Math.Round(width), (int)Math.Round(height), 96, 96, PixelFormats.Default);
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(inkCanvas);
                dc.DrawRectangle(vb, null, new Rect(new System.Windows.Point(), new System.Windows.Size(width, height)));
            }
            bmpCopied.Render(dv);
            using (FileStream file = new FileStream(_imageFile,
                                         FileMode.Create, FileAccess.Write))
            {
                BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bmpCopied));
                encoder.Save(file);
            }

        }
    }
        public class Custom
        {
            public string name;
            public DrawingAttributes da = new DrawingAttributes();
            public StrokeCollection strokes = new StrokeCollection();
            public Visibility Visibility = Visibility.Visible;

            public void AddStroke(MyStroke stroke)
            {
                VisibleStroke p = stroke.ToStroke(da.Clone());
                p.Visibility = this.Visibility;
                strokes.Add(p);
                App.ink.Dispatcher.Invoke(new Action(() => { App.ink.Strokes.Add(p); }));
            }

            public static Color ArgbToColor(int color)
            {
                return Color.FromArgb((byte)((color & 0xff000000) >> 24), (byte)(color & 0x0000ff), (byte)((color & 0x00ff00) >> 8), (byte)((color & 0xff0000) >> 16));
            }

            public void ChangeColor(string c)
            {
                da.Color = (Color)ColorConverter.ConvertFromString(c);
            }
            public void ChangeWidth(Double d)
            {
                da.Width = d;
            }

            public void Undo()
            {
                if (strokes.Count == 0) return;
                var c = strokes.Count;
                var p = strokes[c - 1];
                App.ink.Dispatcher.Invoke(new Action(() => App.ink.Strokes.Remove(p)));
                strokes.RemoveAt(c - 1);
            }

            public void HiddenStrokes(bool b)
            {
                App.ink.Dispatcher.BeginInvoke(new Action(
                    () =>
                    {
                        if (b)
                        {
                            foreach (var item in strokes)
                            {
                                var p = item as VisibleStroke;
                                if (p != null)
                                    p.Visibility = Visibility.Hidden;
                            }
                        }
                        else
                        {
                            foreach (var item in strokes)
                            {
                                var p = item as VisibleStroke;
                                if (p != null)
                                    p.Visibility = Visibility.Visible;
                            }
                        }
                    }
                ));
            }

            /// <summary>
            /// 仅用来添加自己的线条时使用
            /// </summary>
            /// <param name="stroke"></param>
            internal void AddStroke(Stroke stroke)
            {
                stroke.DrawingAttributes = da.Clone();
                strokes.Add(stroke);
            }

            internal void Clear()
            {
                App.ink.Dispatcher.Invoke(new Action(() => App.ink.Strokes.Remove(strokes)));
                strokes.Clear();
            }

            internal void Remove()
            {
                Clear();
            }
        }

        public class VisibleStroke : Stroke
        {
            public VisibleStroke(StylusPointCollection stylusPoints)
                : base(stylusPoints)
            { }
            public VisibleStroke(StylusPointCollection stylusPoints, DrawingAttributes da)
                : base(stylusPoints, da)
            { }

            public Visibility Visibility = Visibility.Visible;

            protected override void DrawCore(DrawingContext drawingContext, DrawingAttributes drawingAttributes)
            {
                if (Visibility == Visibility.Hidden)
                    return;
                base.DrawCore(drawingContext, drawingAttributes);
            }
        }
    
}
