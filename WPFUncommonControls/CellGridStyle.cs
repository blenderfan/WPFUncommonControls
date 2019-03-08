using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WPFUncommonControls
{
    public class CellGridStyle : INotifyPropertyChanged
    {

        public CellGridStyle()
        {
            gridThickness = 1.0f;
            displayGrid = true;
            cellBackgroundColor = new SolidColorBrush(Colors.White);
            backgroundColor = new SolidColorBrush(Colors.Black);
            gridColor = new SolidColorBrush(Colors.Black);
            cellColors = new Dictionary<int, Brush>();
            margin = new Thickness(30, 30, 30, 30);
            forceSquare = false;
        }

        private double gridThickness;
        private bool displayGrid;
        private bool forceSquare;

        private Brush backgroundColor;
        private Brush cellBackgroundColor;
        private Brush gridColor;
        private Dictionary<int, Brush> cellColors;
        private Thickness margin;

        public double GridThickness
        {
            get { return gridThickness; }
            set
            {
                if(value != gridThickness)
                {
                    gridThickness = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool DisplayGrid
        {
            get { return displayGrid; }
            set
            {
                if(value != displayGrid)
                {
                    displayGrid = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool ForceSquare
        {
            get { return forceSquare; }
            set
            {
                if(value != forceSquare)
                {
                    forceSquare = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Brush BackgroundColor
        {
            get { return backgroundColor; }
            set
            {
                if(value != backgroundColor)
                {
                    backgroundColor = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Brush CellBackgroundColor
        {
            get { return cellBackgroundColor; }
            set
            {
                if(value != cellBackgroundColor)
                {
                    cellBackgroundColor = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Brush GridColor
        {
            get { return gridColor; }
            set
            {
                if(value != gridColor)
                {
                    gridColor = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Dictionary<int, Brush> CellColors
        {
            get { return cellColors; }
            set
            {
                if(value != cellColors)
                {
                    cellColors = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Thickness Margin
        {
            get { return margin; }
            set
            {
                if(value != margin)
                {
                    margin = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
