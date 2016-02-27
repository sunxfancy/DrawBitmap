using System;
using System.Windows;
using System.Windows.Controls;

namespace WPFDemo
{
    public class ContainerUtils
    {
        public static Container GetNearestContainer(Object e)
        {   
            FrameworkElement f = e as FrameworkElement;
            Control c = e as Control;
            Container result = e as Container;
            return f == null && c == null ?
                null : result != null ?
                result : f != null ?
                GetNearestContainer(f.Parent) : GetNearestContainer(c.Parent);
        }
    }
}
