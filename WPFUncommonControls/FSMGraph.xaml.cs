using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFUncommonControls
{

    public partial class FSMGraph : UserControl
    {
        public FSMGraph()
        {
            Loaded += new RoutedEventHandler(OnLoaded);

            InitializeComponent();

            FSM = new FSM();
            FSMStyle = new FSMStyle();


        }

        private List<UIElement> Elements { get; set; } = new List<UIElement>();

        public static readonly DependencyProperty FSMProperty = DependencyProperty.Register("FSM", typeof(FSM), typeof(FSMGraph), new FrameworkPropertyMetadata(default(FSM), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty FSMStyleProperty = DependencyProperty.Register("FSMStyle", typeof(FSMStyle), typeof(FSMGraph), new FrameworkPropertyMetadata(default(FSMStyle), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));

        public FSM FSM
        {
            get { return (FSM)GetValue(FSMProperty); }
            set { SetValue(FSMProperty, value); }
        }

        public FSMStyle FSMStyle
        {
            get { return (FSMStyle)GetValue(FSMStyleProperty); }
            set { SetValue(FSMStyleProperty, value); }
        }

        private void UpdateGraphLayout()
        {
            FSMCanvas.Children.Clear();
            Elements.Clear();

            if (!(FSMStyle is null) && !(FSM is null))
            {
                Background = FSMStyle.Background;
                int states = FSM.States.Count();

                double degreePerState = (Math.PI * 2.0f) / states;
                double degree = 0.0f;

                double width = ActualWidth - FSMStyle.Margin.Left - FSMStyle.Margin.Right;
                double height = ActualHeight - FSMStyle.Margin.Top - FSMStyle.Margin.Bottom;
                double centerX = width / 2.0f;
                double centerY = height / 2.0f;

                double radius = Math.Min(width / 2.0f, height / 2.0f);

                Dictionary<int, Tuple<double, double>> circlePositions = new Dictionary<int, Tuple<double, double>>();
                Dictionary<object, int> stateNumberDict = new Dictionary<object, int>();

                for (int i = 0; i < states; i++)
                {
                    var state = FSM.States[i];
                    stateNumberDict.Add(state, i);

                    double x = FSMStyle.Margin.Left + centerX + radius * Math.Cos(degree);
                    double y = FSMStyle.Margin.Top + centerY - radius * Math.Sin(degree);
                    circlePositions.Add(i, new Tuple<double, double>(x, y));


                    Ellipse innerCircle = null;
                    TextBlock stateLabel = new TextBlock()
                    {
                        Text = state.ToString(),
                        Foreground = FSMStyle.NodeBorder,
                        FontSize = FSMStyle.FontSize
                    };

                    stateLabel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    stateLabel.Arrange(new Rect(stateLabel.DesiredSize));
                    stateLabel.Margin = new Thickness(x - stateLabel.ActualWidth / 2.0f, y - stateLabel.ActualHeight / 2.0f, 0, 0);

                    var circle = new Ellipse()
                    {
                        Width = FSMStyle.NodeSize,
                        Height = FSMStyle.NodeSize,
                        Stroke = FSMStyle.NodeBorder,
                        StrokeThickness = FSMStyle.BorderThickness,
                        Fill = FSMStyle.NodeFill,
                        Margin = new Thickness(x - FSMStyle.NodeSize / 2.0f, y - FSMStyle.NodeSize / 2.0f, 0, 0)
                    };

                    if (i == FSM.StartState)
                    {
                        var centerDirX = FSMStyle.Margin.Left + centerX - x;
                        var centerDirY = FSMStyle.Margin.Top + centerY - y;

                        Vector dir = new Vector(centerDirX, centerDirY);
                        dir.Normalize();

                        Point pos = new Point(x, y);

                        var endPos = pos - FSMStyle.NodeSize / 2.0f * dir;
                        var startPos = endPos - FSMStyle.ConnectionThickness * 5 * dir;

                        Line line = new Line()
                        {
                            Stroke = FSMStyle.ConnectionOutline,
                            StrokeThickness = FSMStyle.ConnectionThickness,
                            X1 = startPos.X,
                            X2 = endPos.X,
                            Y1 = startPos.Y,
                            Y2 = endPos.Y
                        };

                        var ortho = new Vector(-dir.Y, dir.X);
                        Polygon arrow = new Polygon()
                        {
                            Fill = FSMStyle.ConnectionOutline
                        };

                        Point arrowCenter = endPos - FSMStyle.ConnectionThickness * dir;
                        PointCollection polyPoints = new PointCollection();
                        polyPoints.Add(arrowCenter + FSMStyle.ConnectionThickness * 2 * dir);
                        polyPoints.Add(arrowCenter + FSMStyle.ConnectionThickness * ortho);
                        polyPoints.Add(arrowCenter - FSMStyle.ConnectionThickness * ortho);

                        arrow.Points = polyPoints;

                        Elements.Add(arrow);

                        Elements.Add(line);
                    }

                    if (FSM.FinalStates.Contains(i))
                    {
                        double size = FSMStyle.NodeSize - FSMStyle.BorderThickness * 3.0f;
                        innerCircle = new Ellipse()
                        {
                            Width = size,
                            Height = size,
                            Stroke = FSMStyle.NodeBorder,
                            StrokeThickness = FSMStyle.BorderThickness,
                            Fill = FSMStyle.NodeFill,
                            Margin = new Thickness(x - size / 2.0f, y - size / 2.0f, 0, 0)
                        };
                    }

                    if (i == FSM.CurrentState)
                    {
                        circle.Stroke = FSMStyle.SelectedBorder;
                        circle.Fill = FSMStyle.SelectedFill;
                        if (!(innerCircle is null))
                        {
                            innerCircle.Stroke = FSMStyle.SelectedBorder;
                            innerCircle.Fill = FSMStyle.SelectedFill;
                        }
                        stateLabel.Foreground = FSMStyle.SelectedBorder;
                    }

                    Elements.Add(circle);
                    Elements.Add(stateLabel);
                    if (!(innerCircle is null)) Elements.Add(innerCircle);
                    degree += degreePerState;
                }

                foreach (var transition in FSM.Transitions)
                {
                    int firstState = stateNumberDict[transition.Item1];
                    int secondState = stateNumberDict[transition.Item3];


                    var firstPos = circlePositions[firstState];
                    var secondPos = circlePositions[secondState];

                    var firstPoint = new Point(firstPos.Item1, firstPos.Item2);
                    var secondPoint = new Point(secondPos.Item1, secondPos.Item2);


                    if (firstState != secondState)
                    {

                        Vector dir = secondPoint - firstPoint;
                        var dist = dir.Length;


                        dir.Normalize();
                        double angle = Vector.AngleBetween(dir, new Vector(1.0f, 0.0f));


                        Point firstLinePoint = firstPoint + FSMStyle.NodeSize / 2.0f * dir;
                        Point secondLinePoint = secondPoint - FSMStyle.NodeSize / 2.0f * dir;
                        Point center = firstLinePoint + ((secondLinePoint - firstLinePoint) * 0.5f);

                        Vector ortho = secondLinePoint - firstLinePoint;
                        var t = ortho.X;
                        ortho.X = ortho.Y;
                        ortho.Y = t;
                        ortho.X *= -1;
                        ortho.Normalize();

                        Path connection = new Path()
                        {
                            Stroke = FSMStyle.ConnectionOutline,
                            StrokeThickness = FSMStyle.ConnectionThickness
                        };
                        StringBuilder pathData = new StringBuilder("M ");
                        pathData.Append((int)firstLinePoint.X);
                        pathData.Append(",");
                        pathData.Append((int)firstLinePoint.Y);
                        pathData.Append(" L ");
                        pathData.Append((int)secondLinePoint.X);
                        pathData.Append(",");
                        pathData.Append((int)secondLinePoint.Y);
                        pathData.Append(" ");
                        connection.Data = Geometry.Parse(pathData.ToString());

                        Elements.Add(connection);

                        if (FSMStyle.DisplayText)
                        {
                            if (angle > 90.0f || angle < -90.0f) angle += 180.0f;
                            TextBlock ioBlock = new TextBlock()
                            {
                                FontSize = FSMStyle.FontSize,
                                Foreground = FSMStyle.ConnectionOutline
                            };
                            StringBuilder io = new StringBuilder();
                            io.Append(transition.Item2);
                            io.Append("/");
                            io.Append(transition.Item4);
                            ioBlock.Text = io.ToString();

                            ioBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                            ioBlock.Arrange(new Rect(ioBlock.DesiredSize));
                            ioBlock.RenderTransform = new RotateTransform(360.0 - angle);
                            ioBlock.Margin = new Thickness(center.X - ioBlock.ActualWidth / 2.0f + ortho.X * FSMStyle.ConnectionThickness, center.Y - ioBlock.ActualHeight / 2.0f + ortho.Y * FSMStyle.ConnectionThickness, 0, 0);

                            Elements.Add(ioBlock);
                        }

                        Polygon arrow = new Polygon()
                        {
                            Fill = FSMStyle.ConnectionOutline
                        };

                        Point arrowCenter = secondLinePoint - FSMStyle.ConnectionThickness * dir;
                        PointCollection polyPoints = new PointCollection();
                        polyPoints.Add(arrowCenter + FSMStyle.ConnectionThickness * 2 * dir);
                        polyPoints.Add(arrowCenter + FSMStyle.ConnectionThickness * ortho);
                        polyPoints.Add(arrowCenter - FSMStyle.ConnectionThickness * ortho);

                        arrow.Points = polyPoints;

                        Elements.Add(arrow);
                    }
                    else
                    {
                        Path connection = new Path()
                        {
                            Stroke = FSMStyle.ConnectionOutline,
                            StrokeThickness = FSMStyle.ConnectionThickness
                        };
                        StringBuilder pathData = new StringBuilder("M ");
                        pathData.Append((int)(firstPoint.X + FSMStyle.NodeSize / 2.0f));
                        pathData.Append(",");
                        pathData.Append((int)firstPoint.Y);
                        pathData.Append(" A ");
                        pathData.Append((int)(FSMStyle.NodeSize / 2.0f));
                        pathData.Append(",");
                        pathData.Append((int)(FSMStyle.NodeSize / 2.0f));
                        pathData.Append(" 0 1 0 ");
                        pathData.Append((int)firstPoint.X);
                        pathData.Append(",");
                        pathData.Append((int)(firstPoint.Y - FSMStyle.NodeSize / 2.0f));
                        pathData.Append(" z");
                        connection.Data = Geometry.Parse(pathData.ToString());

                        Elements.Add(connection);

                        if (FSMStyle.DisplayText)
                        {
                            TextBlock ioBlock = new TextBlock()
                            {
                                FontSize = FSMStyle.FontSize,
                                Foreground = FSMStyle.ConnectionOutline
                            };
                            StringBuilder io = new StringBuilder();
                            io.Append(transition.Item2);
                            io.Append("/");
                            io.Append(transition.Item4);
                            ioBlock.Text = io.ToString();

                            ioBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                            ioBlock.Arrange(new Rect(ioBlock.DesiredSize));
                            ioBlock.Margin = new Thickness(firstPoint.X + FSMStyle.NodeSize + 10, firstPoint.Y, 0, 0);

                            Elements.Add(ioBlock);
                        }

                    }
                }
            }
            foreach (var element in Elements)
                FSMCanvas.Children.Add(element);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            UpdateGraphLayout();
            base.OnRender(drawingContext);
        }
    }
}
