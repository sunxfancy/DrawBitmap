using ColorWheel.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Ink;

namespace DreamingApp
{
    /// <summary>
    /// ColorPopup.xaml 的交互逻辑
    /// </summary>
    public partial class ColorPopup : Popup
    {

        RGBColorWheel rgbWheel = new RGBColorWheel();
        public ColorPopup()
        {
            InitializeComponent();
            wheel.Palette = Palette.Create(rgbWheel, Colors.BlueViolet, PaletteSchemaType.Complementary, 1);
        }

        private void wheel_ColorSelected(object sender, ColorWheel.Controls.EventArg<int> e)
        {
            var b = wheel.Palette.Colors[0];
            var c = b.RgbColor;
            var d = b.Brightness255;
            MainData.color = b;
            slider.SliderColor = c;
            slider.Value = d;
            MainData.Me.da.Color = c;
            MainData.isColorNeedUpdate = true;
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MainData.color.Brightness255 = (byte)e.NewValue;
            MainData.Me.da.Color = MainData.color.RgbColor;
            MainData.isColorNeedUpdate = true;
        }

        private void wheel_ColorsUpdated(object sender, EventArgs e)
        {
            var b = wheel.Palette.Colors[0];
            MainData.color = b;
            slider.SliderColor = b.RgbColor;
            slider.Value = b.Brightness255;
            MainData.Me.da.Color = MainData.color.RgbColor;
            MainData.isColorNeedUpdate = true;
        }

        private void wheel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var b = wheel.Palette.Colors[0];
            MainData.color = b;
            slider.SliderColor = b.RgbColor;
            slider.Value = b.Brightness255;
            MainData.Me.da.Color = MainData.color.RgbColor;
            MainData.isColorNeedUpdate = true;
        }

        private void Slider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MainData.LineWidth = e.NewValue;
            if (MainData.Me != null)
            {
                MainData.Me.da.Width = MainData.LineWidth;
                MainData.isWidthNeedUpdata = true;
            }
        }

        private void Popup_Closed(object sender, EventArgs e)
        {
            App.main.CheckandSendColor();
        }

 
    }
}
