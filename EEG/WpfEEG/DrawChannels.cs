using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Data;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace WpfEEG
{
    /// <summary>
    /// Данные одного канала
    /// </summary>
    class DrawChannel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public DrawChannel()
        {
            Label = new TextBlock()
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center
            };

            Line = new Line() 
            { 
                Stroke = Brushes.Black, 
                StrokeThickness = 1
            };

            Polyline = new Polyline()
            {
                Stroke = Brushes.Red,
                StrokeThickness = 1
            };

            ScaleX = 1;
            ScaleY = 1;
        }

        /// <summary>
        /// Цвет отображения сигнала канала
        /// </summary>
        public Color Color
        {
            get
            {
                return (Polyline.Stroke as SolidColorBrush).Color;
            }
            set
            {
                Polyline.Stroke = new SolidColorBrush(value);

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Color"));
            }
        }

        /// <summary>
        /// Метка с названием канала
        /// </summary>
        public TextBlock Label
        {
            get;
            protected set;
        }

        /// <summary>
        /// Ось сигнала
        /// </summary>
        public Line Line
        {
            get;
            protected set;
        }

        /// <summary>
        /// Название канала
        /// </summary>
        public string Name
        {
            get
            {
                return Label.Text;
            }
            set
            {
                Label.Text = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));
            }
        }

        /// <summary>
        /// Спрятать или показать график
        /// </summary>
        public bool Visible
        {
            get
            {
                return Label.IsVisible;
            }
            set
            {
                if (value)
                {
                    Label.Visibility = Visibility.Visible;
                    Line.Visibility = Visibility.Visible;
                    Polyline.Visibility = Visibility.Visible;
                    Polyline.Points.Clear();
                }
                else
                {
                    Label.Visibility = Visibility.Collapsed;
                    Line.Visibility = Visibility.Collapsed;
                    Polyline.Visibility = Visibility.Collapsed;
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Visible"));
            }
        }

        /// <summary>
        /// Линия сигнала
        /// </summary>
        public Polyline Polyline
        {
            get;
            protected set;
        }

        private int _MaxPointCount;

        /// <summary>
        /// Максимальное количество точек в линии
        /// </summary>
        public int MaxPointCount
        {
            get
            {
                return _MaxPointCount;
            }
            protected set
            {
                _MaxPointCount = value;
                if (Polyline.Points.Count > 0)
                {
                    while (Polyline.Points.Count > value)
                        Polyline.Points.RemoveAt(0);
                    double X = Polyline.Points[0].X;
                    if (X > 1E-20)
                        for (int I = 0; I < Polyline.Points.Count; I++)
                            Polyline.Points[I] = new Point(Polyline.Points[I].X - X, Polyline.Points[I].Y);
                }
            }
        }

        private double _ScaleX;

        /// <summary>
        /// Масштаб по X
        /// </summary>
        public double ScaleX
        {
            get
            {
                return _ScaleX;
            }
            set
            {
                _ScaleX = value;
                MaxPointCount = (int)(Math.Abs(Line.X2 - Line.X1) / value);

            }
        }

        /// <summary>
        /// Масштаб по Y
        /// </summary>
        public double ScaleY
        {
            get;
            set;
        }

        /// <summary>
        /// Положение Оси графика канала относительно Y
        /// </summary>
        public double AxeY
        {
            get
            {
                return Line.Y1;
            }
            set
            {
                double shift = value - Line.Y1;
                Line.Y1 = value;
                Line.Y2 = value;
                for (int J = 0; J < Polyline.Points.Count; J++)
                    Polyline.Points[J] = new Point(Polyline.Points[J].X, Polyline.Points[J].Y + shift);
            }
        }

        /// <summary>
        /// Помещает данные на график
        /// </summary>
        /// <param name="value">Данные</param>
        public void PutValue(short value)
        {
            if (Polyline.Points.Count > (MaxPointCount - 1))
            {
                while (Polyline.Points.Count > (MaxPointCount - 1))
                    Polyline.Points.RemoveAt(0);

                double X = Polyline.Points[0].X;
                if (X > 1E-20)
                    for (int I = 0; I < Polyline.Points.Count; I++)
                        Polyline.Points[I] = new Point(Polyline.Points[I].X - X, Polyline.Points[I].Y);
            }
            if (Polyline.Points.Count > 0)
                Polyline.Points.Add(new Point(Polyline.Points[Polyline.Points.Count - 1].X + ScaleX, AxeY - value * ScaleY));
            else
                Polyline.Points.Add(new Point(0, AxeY - value * ScaleY));
        }
    }

    /// <summary>
    /// Конвертер для DataGrid
    /// </summary>
    internal class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new SolidColorBrush((Color)value);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
