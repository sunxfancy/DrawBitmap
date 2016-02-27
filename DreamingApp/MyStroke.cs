using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace DreamingApp
{
    public class VisualStroke : Stroke
    {
        Brush brush;
        Pen pen;

        StylusPointCollection collection;

        public VisualStroke(StylusPointCollection stylusPoints)
            : base(stylusPoints)
        {
            // Create the Brush and Pen used for drawing.
            DrawingVisual v = new DrawingVisual();
            VisualBrush vBrush = new VisualBrush(v);
            
            
           
        }

        protected virtual void DrawCore(DrawingContext drawingContext,DrawingAttributes drawingAttributes)
        {
            
        }
    }
}
