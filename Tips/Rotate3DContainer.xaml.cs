using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Media.Effects;
using System.Windows.Input;

namespace WPFDemo
{
    public partial class Rotate3DContainer : Canvas, Container
    {
        private Storyboard front2BackStory;
        private Storyboard back2FrontStory;
        private Border frontWarpper;
        private Border backWarpper;

        public Rotate3DContainer()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Container_Loaded);
        }

        private void Container_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
        }

        private void Init()
        {
            if (this.Children.Count == 2)
            {
                UIElement[] array = new UIElement[2];
                this.Children.CopyTo(array, 0);
                this.Children.Clear();
                frontWarpper = new Border()
                {
                    Child = array[0],
                    HorizontalAlignment=HorizontalAlignment.Left,
                    VerticalAlignment=VerticalAlignment.Top,
                    Background = Brushes.Transparent
                };

                backWarpper = new Border()
                {
                    Child = array[1],
                    Opacity = 0,
                    Visibility = Visibility.Hidden,
                    HorizontalAlignment=HorizontalAlignment.Left,
                    VerticalAlignment=VerticalAlignment.Top,
                    Background = Brushes.Transparent
                };

                Viewport3D viewport = Get3DView();
                viewport.HorizontalAlignment = HorizontalAlignment.Left;
                viewport.VerticalAlignment = VerticalAlignment.Top;
                viewport.Height = frontWarpper.Child.DesiredSize.Height;
                viewport.Width = frontWarpper.Child.DesiredSize.Width;

                front2BackStory = new Storyboard()
                {
                    Children = new TimelineCollection()
                    {
                        GetShowHideAnimation(viewport, 0, 1100),
                        GetShowHideAnimation(backWarpper, 1000, -1),
                        GetShowHideAnimation(frontWarpper, -1, 50),
                        GetFadeAnimation(frontWarpper, 0, -1, 50),
                        GetFadeAnimation(backWarpper, 1, 1050, 50),
                        GetCameraMoveAnimation(0, 0, 0.5, 0, 0, 1.1, 50, 500,viewport),
                        GetRotateAnimation(0, -180, 0.3, 0.3, 50, 1000)
                    }
                };

                back2FrontStory = new Storyboard()
                {
                    Children = new TimelineCollection()
                    {
                        GetShowHideAnimation(viewport, 0, 1100),
                        GetShowHideAnimation(frontWarpper, 1000, -1),
                        GetShowHideAnimation(backWarpper, -1, 50),
                        GetFadeAnimation(backWarpper, 0, -1, 50),
                        GetFadeAnimation(frontWarpper, 1, 1050, 50),
                        GetCameraMoveAnimation(0, 0, 0.5, 0, 0, 1.1, 50, 500, viewport),
                        GetRotateAnimation(180, 360, 0.3, 0.3, 50, 1000)
                    }
                };

                this.Effect = new DropShadowEffect()
                {
                    BlurRadius = 10,
                    Opacity = 0.8
                };

                this.Children.Add(frontWarpper);
                this.Children.Add(backWarpper);
                this.Children.Add(viewport);
                array = null;
            }
        }

        private DoubleAnimation GetFadeAnimation(UIElement target, int toOpacity, int beginTime, int duration)
        {
            DoubleAnimation result = new DoubleAnimation(toOpacity, new Duration(TimeSpan.FromMilliseconds(duration)));
            if (beginTime >= 0)
            {
                result.BeginTime = TimeSpan.FromMilliseconds(beginTime);
            }
            Storyboard.SetTarget(result, target);
            Storyboard.SetTargetProperty(result, new PropertyPath("Opacity"));
            return result;
        }

        private Point3DAnimation GetCameraMoveAnimation(double x1, double y1, double z1, double x2, double y2, double z2, int beginTime, int duration, Viewport3D view)
        {
            Point3DAnimation result = new Point3DAnimation(new Point3D(x1, y1, z1), new Point3D(x2, y2, z2), new Duration(TimeSpan.FromMilliseconds(duration)))
            {
                AutoReverse = true,
                BeginTime = TimeSpan.FromMilliseconds(beginTime),
                DecelerationRatio = 0.3
            };
            Storyboard.SetTarget(result, view);
            Storyboard.SetTargetProperty(result, new PropertyPath("Camera.Position"));
            return result;
        }

        private DoubleAnimation GetRotateAnimation(double fromDegree, double toDegree, double accelerationRatio, double decelerationRatio, int beginTime, int duration)
        {
            DoubleAnimation result = new DoubleAnimation(fromDegree, toDegree, new Duration(TimeSpan.FromMilliseconds(duration)), FillBehavior.HoldEnd)
            {
                AccelerationRatio = accelerationRatio,
                DecelerationRatio = decelerationRatio,
                BeginTime = TimeSpan.FromMilliseconds(beginTime)
            };
            Storyboard.SetTargetName(result, "AxisAngleRotation3D");
            Storyboard.SetTargetProperty(result, new PropertyPath("Angle"));
            return result;
        }

        Viewport3D Get3DView()
        {
            Viewport3D viewport = new Viewport3D()
            {
                Camera = new PerspectiveCamera(new Point3D(0, 0, 0.5), new Vector3D(0, 0, -1), new Vector3D(0, 1, 0), 90),
                Visibility = Visibility.Hidden
            };
            viewport.Children.Add(GetLightVisual3D());
            viewport.Children.Add(GetSceneVisual3D(frontWarpper, backWarpper));
            return viewport;
        }

        ModelVisual3D GetLightVisual3D()
        {
            Model3DGroup group = new Model3DGroup();
            group.Children.Add(new DirectionalLight(Color.FromRgb(0x44, 0x44, 0x44), new Vector3D(0, 0, -1)));
            group.Children.Add(new AmbientLight(Color.FromRgb(0xBB, 0xBB, 0xBB)));
            return new ModelVisual3D() { Content = group };
        }

        ModelVisual3D GetSceneVisual3D(Border frontElement, Border backElement)
        {
            MeshGeometry3D meshgmod = new MeshGeometry3D()
            {
                TriangleIndices = new Int32Collection(new int[] { 0, 1, 2, 2, 3, 0 }),
                TextureCoordinates = new PointCollection(new Point[]{new Point(0, 1),new Point(1, 1),new Point(1, 0),new Point(0, 0)}),
                Positions = new Point3DCollection(new Point3D[]{new Point3D(-0.5, -0.5, 0),new Point3D(0.5, -0.5, 0),new Point3D(0.5, 0.5, 0),new Point3D(-0.5, 0.5, 0)})
            };

            VisualBrush frontBrush = new VisualBrush(frontElement.Child)
            { 
                Stretch = Stretch.Uniform 
            };

            VisualBrush backBrush = new VisualBrush(backElement.Child)
            {
                Stretch = Stretch.Uniform,
                RelativeTransform = new ScaleTransform(-1, 1, 0.5, 0)
            };

            AxisAngleRotation3D rotate = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 0);
            this.RegisterName("AxisAngleRotation3D", rotate);

            GeometryModel3D gmode3d = new GeometryModel3D() 
            { 
                Geometry = meshgmod, 
                Material = new DiffuseMaterial(frontBrush), 
                BackMaterial = new DiffuseMaterial(backBrush), 
                Transform = new RotateTransform3D(rotate) 
            };
            return new ModelVisual3D() { Content = gmode3d };
        }

        ObjectAnimationUsingKeyFrames GetShowHideAnimation(UIElement element, int showTime, int hideTime)
        {
            ObjectAnimationUsingKeyFrames frame = new ObjectAnimationUsingKeyFrames();
            Storyboard.SetTarget(frame, element);
            Storyboard.SetTargetProperty(frame, new PropertyPath("Visibility"));
            if (showTime >= 0)
            {
                frame.KeyFrames.Add(new DiscreteObjectKeyFrame(Visibility.Visible, TimeSpan.FromMilliseconds(showTime)));
            }
            if (hideTime >= 0)
            {
                frame.KeyFrames.Add(new DiscreteObjectKeyFrame(Visibility.Hidden, TimeSpan.FromMilliseconds(hideTime)));
            }
            return frame;
        }

        public void Turn(bool isReverse)
        {
            Storyboard target = null;
            DoubleAnimation direction = null;
            double fromAngle = 0;
            double step = 180;
            if (frontWarpper.Opacity != 0)
            {
                target = front2BackStory;
            }
            else
            {
                fromAngle = 180;
                target = back2FrontStory;
            }
            direction = (DoubleAnimation)target.Children[6];
            if (!isReverse)
            {
                step = -180;
            }
            direction.From = fromAngle;
            direction.To = fromAngle + step;
            target.Begin(this);
        }

    }
}
