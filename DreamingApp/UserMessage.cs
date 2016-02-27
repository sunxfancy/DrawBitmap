using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;

namespace DreamingApp
{
    /// <summary>
    /// 网络通信时，用户信息结构体
    /// </summary>
    [Serializable]
    public class UserMessage
    {
        /// <summary>
        /// 构建一个用户信息结构体
        /// </summary>
        /// <param name="_m">消息类型</param>
        /// <param name="_n">发送者名字</param>
        /// <param name="_o">数据</param>
        public UserMessage(byte _m,string _n, object _o)
        {
            type = _m;
            name = _n;
            data = _o;
        }

        public UserMessage()
        {

        }

        public byte type;
        public object data;
        public string name;
    }


    [Serializable]
    public class MyStroke
    {
        public Point[] point;
        public float[] press;
        public MyStroke(Stroke s)
        {
            var ps = s.GetBezierStylusPoints();
            var c = ps.Count;
            point = new Point[c];
            press = new float[c];
            for (int i = 0; i < c; ++i)
            {
                point[i] = ps[i].ToPoint();
                press[i] = ps[i].PressureFactor;
            }
        }

        public VisibleStroke ToStroke()
        {         
            var c = new StylusPointCollection();
            for (int i = 0; i < point.Length; ++i)
            {
                var p = new StylusPoint(point[i].X, point[i].Y, press[i]);
                c.Add(p);
            }
            var s = new VisibleStroke(c);
            return s;
        }

        internal VisibleStroke ToStroke(DrawingAttributes da)
        {
            var c = new StylusPointCollection();
            for (int i = 0; i < point.Length; ++i)
            {
                var p = new StylusPoint(point[i].X, point[i].Y, press[i]);
                c.Add(p);
            }
            var s = new VisibleStroke(c,da);
            return s;
        }
    }



    [Serializable]
    public class MyCustomStrokes
    {
        public MyCustomStrokes() { }

        /// <SUMMARY>
        /// The first index is for the stroke no.
        /// The second index is for the keep the 2D point of the Stroke.
        /// </SUMMARY>
        public Point[][] strokeCollection;

        public MyCustomStrokes(StrokeCollection strokes)
        {
            if (strokes.Count > 0)
            {

                strokeCollection = new Point[strokes.Count][];

                for (int i = 0; i < strokes.Count; i++)
                {
                    strokeCollection[i] =
                      new Point[strokes[i].StylusPoints.Count];

                    for (int j = 0; j < strokes[i].StylusPoints.Count; j++)
                    {
                        strokeCollection[i][j] = new Point();
                        strokeCollection[i][j].X =
                                              strokes[i].StylusPoints[j].X;
                        strokeCollection[i][j].Y =
                                              strokes[i].StylusPoints[j].Y;
                    }
                }
            }
        }

        public StrokeCollection ToStrokeCollection()
        {
            var Strokes = new StrokeCollection();
            for (int i = 0; i < strokeCollection.Length; i++)
            {
                if (strokeCollection[i] != null)
                {
                    StylusPointCollection stylusCollection = new
                      StylusPointCollection(strokeCollection[i]);

                    Stroke stroke = new Stroke(stylusCollection);
                    StrokeCollection strokes = new StrokeCollection();
                    strokes.Add(stroke);
                    
                    Strokes.Add(strokes);
                }
            }
            return Strokes;
        }

    }
    
}
