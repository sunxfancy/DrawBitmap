using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Input.StylusPlugIns;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
namespace WpfControlLibrary1
{

    // A StylusPlugin that renders ink with a linear gradient brush effect.
    class CustomDynamicRenderer : DynamicRenderer
    {
        [ThreadStatic]
        static private Brush brush = null;

        [ThreadStatic]
        static private Pen pen = null;

        private Point prevPoint;

        protected override void OnStylusDown(RawStylusInput rawStylusInput)
        {
            // Allocate memory to store the previous point to draw from.
            prevPoint = new Point(double.NegativeInfinity, double.NegativeInfinity);
            base.OnStylusDown(rawStylusInput);
        }

        protected override void OnDraw(DrawingContext drawingContext,
                                       StylusPointCollection stylusPoints,
                                       Geometry geometry, Brush fillBrush)
        {
            // 如有必要，创建一个新的brush。
            if (brush == null)
            {
                brush = new LinearGradientBrush(Colors.Red, Colors.Blue, 20d);
            }

            // 如有必要，创建一个新的Pen。
            if (pen == null)
            {
                pen = new Pen(brush, 2d);
            }

            // Draw linear gradient ellipses between all the 手写笔点 that have come in.
            for (int i = 0; i < stylusPoints.Count; i++)
            {
                Point pt = (Point)stylusPoints[i];
                Vector v = Point.Subtract(prevPoint, pt);

                // Only draw if we are at least 4 units away 
                // from the end of the last ellipse. Otherwise, 
                // we're just redrawing and wasting cycles.
                if (v.Length > 4)
                {
                    //在这里显示即时的图像，但不是真正画在画布上的墨迹
                    // Set the thickness of the stroke based 
                    // on how hard the user pressed.
                    double radius = stylusPoints[i].PressureFactor * 20d;
                    //drawingContext.DrawEllipse(brush, pen, pt, radius, radius);
                    drawingContext.DrawLine(new Pen(brush, 2), prevPoint, pt);
                    prevPoint = pt;
                }
            }
        }
        public override void Reset(StylusDevice stylusDevice, StylusPointCollection stylusPoints)
        {
            //base.Reset(stylusDevice, stylusPoints);
        }
    }

    // A class for rendering custom strokes
    class CustomStroke : Stroke
    {
        Brush brush;
        Pen pen;

        public CustomStroke(StylusPointCollection stylusPoints)
            : base(stylusPoints)
        {
            // Create the Brush and Pen used for drawing.
            brush = new LinearGradientBrush(Colors.Red, Colors.Blue, 20d);
            pen = new Pen(brush, 2d);
        }




        protected override void DrawCore(DrawingContext drawingContext,
                                         DrawingAttributes drawingAttributes)
        {
            // Allocate memory to store the previous point to draw from.
            Point prevPoint = new Point(StylusPoints[0].X,
                                        StylusPoints[0].Y);
            Point pt0 = new Point(prevPoint.X, prevPoint.Y);
            // Draw linear gradient ellipses between 
            // all the StylusPoints in the Stroke.
            for (int i = 0; i < this.StylusPoints.Count; i++)
            {
                Point pt = (Point)this.StylusPoints[i];
                
                Vector v = Point.Subtract(prevPoint, pt);

                // Only draw if we are at least 4 units away 
                // from the end of the last ellipse. Otherwise, 
                // we're just redrawing and wasting cycles.
                if (v.Length > 4)
                {
                    // Set the thickness of the stroke 
                    // based on how hard the user pressed.
                    double radius = this.StylusPoints[i].PressureFactor * 20d;

                    //画贝赛尔曲线

                    drawBezier(drawingContext, prevPoint, pt0, pt);
                    //这一部分可以控制为绘图方式


                    prevPoint = pt0;
                    pt0 = pt;
                }
            }
        }
        private void drawBezier(DrawingContext drawingContext, Point p0, Point p1, Point p2)
        {
            PathFigure myPathFigure = new PathFigure();
            myPathFigure.StartPoint = p0;
            // myPathFigure.Segments.Add(new BezierSegment(prevPoint,pt0, pt, true));

            myPathFigure.Segments.Add(new QuadraticBezierSegment(p1, p2, true));

            PathGeometry myPathGeometry = new PathGeometry();
            myPathGeometry.Figures.Add(myPathFigure);
            drawingContext.DrawGeometry(Brushes.Transparent, new Pen(brush, 2),
              myPathGeometry);
        }
    }

    
    

    public class InkCanvasExt : InkCanvas
    {
        CustomDynamicRenderer customRenderer = new CustomDynamicRenderer();

        public InkCanvasExt() : base()
        {
            // Use the custom dynamic renderer on the
            // custom InkCanvas.
            this.DynamicRenderer = customRenderer;
        }

        protected override void OnStrokeCollected(InkCanvasStrokeCollectedEventArgs e)
        {
            // Remove the original stroke and add a custom stroke.
            this.Strokes.Remove(e.Stroke);
            CustomStroke customStroke = new CustomStroke(e.Stroke.StylusPoints);
            this.Strokes.Add(customStroke);
            MessageBox.Show("ke");
            // Pass the custom stroke to base class' OnStrokeCollected method.
            InkCanvasStrokeCollectedEventArgs args =
                new InkCanvasStrokeCollectedEventArgs(customStroke);
            base.OnStrokeCollected(args);

        }

    }
}
