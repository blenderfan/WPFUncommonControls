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
    /// <summary>
    /// Interaction logic for CellGrid.xaml
    /// </summary>
    public partial class CellGrid : UserControl
    {
        public CellGrid()
        {
            CellGridStyle = new CellGridStyle();
            CellData = null;

            InitializeComponent();
        }

        private List<UIElement> Elements { get; set; } = new List<UIElement>();

        public static readonly DependencyProperty CellGridStyleProperty = DependencyProperty.Register("CellGridStyle", typeof(CellGridStyle), typeof(CellGrid), new FrameworkPropertyMetadata(default(CellGridStyle), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty CellDataProperty = DependencyProperty.Register("CellData", typeof(int[,]), typeof(CellGrid), new FrameworkPropertyMetadata(default(int[,]), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));

        public CellGridStyle CellGridStyle
        {
            get { return (CellGridStyle)GetValue(CellGridStyleProperty); }
            set { SetValue(CellGridStyleProperty, value); }
        }

        public int[,] CellData
        {
            get { return (int[,])GetValue(CellDataProperty); }
            set { SetValue(CellDataProperty, value); }
        }

        private void UpdateGridLayout()
        {
            CellCanvas.Children.Clear();
            Elements.Clear();

                       

            if(!(CellData is null))
            {
                var style = CellGridStyle;

                CellCanvas.Background = style.BackgroundColor;
                double width = ActualWidth - style.Margin.Left - style.Margin.Right;
                double height = ActualHeight - style.Margin.Top - style.Margin.Bottom;

                int cellsY = CellData.GetLength(0);
                int cellsX = CellData.GetLength(1);

                double cellHeight = Math.Abs(height / cellsY);
                double cellWidth = Math.Abs(width / cellsX);

                double additionalMarginLeft = 0.0f;
                double additionalMarginTop = 0.0f;

                if(CellGridStyle.ForceSquare)
                {
                    if (cellHeight < cellWidth)
                    {
                        cellWidth = cellHeight;
                        additionalMarginLeft = (width - cellWidth * cellsX)/2.0f;
                    }
                    else
                    {
                        cellHeight = cellWidth;
                        additionalMarginTop = (height - cellHeight * cellsY)/2.0f;
                    }
                }
                double marginLeft = style.Margin.Left + additionalMarginLeft;
                double marginTop = style.Margin.Top + additionalMarginTop;

                for (int i = 0; i < cellsY; i++)
                    for (int j = 0; j < cellsX; j++)
                    {
                        int value = CellData[i, j];
                        Brush color = style.CellBackgroundColor;
                        if (style.CellColors.ContainsKey(value))
                            color = style.CellColors[value];

                        Rectangle rect = new Rectangle()
                        {
                            Fill = color,
                            Width = cellWidth,
                            Height = cellHeight,
                            Margin = new Thickness(marginLeft + j * cellWidth, marginTop + i * cellHeight, 0, 0)
                        };

                        Elements.Add(rect);
                    }

                if (style.DisplayGrid)
                {
                    for (int i = 0; i < cellsY+1; i++)
                    {
                        double x1 = marginLeft;
                        double x2 = x1 + cellsX * cellWidth;
                        double y1 = marginTop + i * cellHeight;
                        double y2 = y1;

                        Line line = new Line()
                        {
                            Stroke = style.GridColor,
                            StrokeThickness = style.GridThickness,
                            X1 = x1,
                            X2 = x2,
                            Y1 = y1,
                            Y2 = y2
                        };

                        Elements.Add(line);
                    }

                    for (int i = 0; i < cellsX+1; i++)
                    {
                        double x1 = marginLeft + i * cellWidth;
                        double x2 = x1;
                        double y1 = marginTop;
                        double y2 = y1 + cellsY * cellHeight;

                        Line line = new Line()
                        {
                            Stroke = style.GridColor,
                            StrokeThickness = style.GridThickness,
                            X1 = x1,
                            X2 = x2,
                            Y1 = y1,
                            Y2 = y2
                        };

                        Elements.Add(line);
                    }
                }

            }

            foreach (var element in Elements)
                CellCanvas.Children.Add(element);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            UpdateGridLayout();
            base.OnRender(drawingContext);
        }
    }
}
