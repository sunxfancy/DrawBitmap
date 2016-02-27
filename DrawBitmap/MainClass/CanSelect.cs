using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace DrawBitmap
{
    /// <summary>
    /// 描述一个通用的显示在界面上的好友或群组的接口
    /// </summary>
    public abstract class CanSelect
    {
        private bool _isSelected;

        public void Select()
        {
            _isSelected = _isSelected ^ true;
        }
        public void CancelSelect()
        {
            _isSelected = false;
        }

        public bool isSelected
        {
            get { return _isSelected; }
        }

        public abstract ImageSource GetImage();

        public abstract String GetName();

    }
    
}
