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
    public class FSMStyle : INotifyPropertyChanged
    {

        private double nodeSize;
        private double connectionThickness;
        private double borderThickness;
        private double fontSize;
        private Brush background;
        private Brush nodeFill;
        private Brush nodeBorder;
        private Brush selectedFill;
        private Brush selectedBorder;

        private Brush connectionOutline;

        private Thickness margin;

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

        public double NodeSize
        {
            get { return nodeSize; }
            set
            {
                if(value != nodeSize)
                {
                    nodeSize = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public double ConnectionThickness
        {
            get { return connectionThickness; }
            set
            {
                if(value != connectionThickness)
                {
                    connectionThickness = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public double BorderThickness
        {
            get { return borderThickness; }
            set
            {
                if(value != borderThickness)
                {
                    borderThickness = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public double FontSize
        {
            get { return fontSize; }
            set
            {
                if(value != fontSize)
                {
                    fontSize = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Brush Background
        {
            get { return background; }
            set
            {
                if(value != background)
                {
                    background = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Brush NodeFill
        {
            get { return nodeFill; }
            set
            {
                if(value != nodeFill)
                {
                    nodeFill = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Brush NodeBorder
        {
            get { return nodeBorder; }
            set
            {
                if(value != nodeBorder)
                {
                    nodeBorder = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Brush SelectedFill
        {
            get { return selectedFill; }
            set
            {
                if(value != selectedFill)
                {
                    selectedFill = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Brush SelectedBorder
        {
            get { return selectedBorder; }
            set
            {
                if(value != selectedBorder)
                {
                    selectedBorder = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Brush ConnectionOutline
        {
            get { return connectionOutline; }
            set
            {
                if(value != connectionOutline)
                {
                    connectionOutline = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public FSMStyle()
        {
            nodeSize = 50;
            connectionThickness = 10;
            borderThickness = 5;
            fontSize = 24;
            Background = new SolidColorBrush(Colors.White);
            nodeFill = new SolidColorBrush(Colors.Transparent);
            nodeBorder = new SolidColorBrush(Colors.Black);
            selectedFill = new SolidColorBrush(Colors.Transparent);
            selectedBorder = new SolidColorBrush(Colors.Red);
            connectionOutline = new SolidColorBrush(Colors.Black);
            margin = new Thickness(50, 50, 50, 50);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
