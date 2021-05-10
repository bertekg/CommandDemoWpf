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

namespace CommandDemoWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializePlacingMode();
        }
        enum PlacingShapeMode { Circle, Square, Triangle }
        PlacingShapeMode currentPlacingShapeMode; 
        private void InitializePlacingMode()
        {
            currentPlacingShapeMode = PlacingShapeMode.Circle;
        }
        List<Shape> listOfShapsAll = new List<Shape>();
        int indexOfShaps = 0;
        Random rand = new Random();
        private void mainCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point p = Mouse.GetPosition(mainCanvas);
            Shape tempShape = null;
            switch (currentPlacingShapeMode)
            {
                case PlacingShapeMode.Circle:
                    tempShape = MakeNewEllipse(p, 50);
                    break;
                case PlacingShapeMode.Square:
                    tempShape = MakeNewSquare(p, 50);
                    break;
                case PlacingShapeMode.Triangle:
                    tempShape = MakeNewTriangel(p, 50,50);
                    break;
            }
            listOfShapsAll.RemoveRange(indexOfShaps, listOfShapsAll.Count - indexOfShaps);
            indexOfShaps++;
            listOfShapsAll.Add(tempShape);
            UpdateCanvas();
        }
        private Ellipse MakeNewEllipse(Point place, double diameter)
        {
            Ellipse tempCircle = new Ellipse();
            tempCircle.Width = diameter; tempCircle.Height = diameter;
            tempCircle.Fill = new SolidColorBrush(Color.FromRgb((byte)rand.Next(1, 255), (byte)rand.Next(1, 255), (byte)rand.Next(1, 255)));
            Canvas.SetLeft(tempCircle, place.X - tempCircle.Width / 2); Canvas.SetTop(tempCircle, place.Y - tempCircle.Height / 2);
            return tempCircle;
        }
        private Rectangle MakeNewSquare(Point place, double length)
        {
            Rectangle tempSquare = new Rectangle();
            tempSquare.Width = length; tempSquare.Height = length;
            tempSquare.Fill = new SolidColorBrush(Color.FromRgb((byte)rand.Next(1, 255), (byte)rand.Next(1, 255), (byte)rand.Next(1, 255)));
            Canvas.SetLeft(tempSquare, place.X - tempSquare.Width / 2); Canvas.SetTop(tempSquare, place.Y - tempSquare.Height / 2);
            return tempSquare;
        }
        private Polygon MakeNewTriangel(Point place, double height, double baseLength)
        {
            Polygon tempPolygon = new Polygon();
            tempPolygon.HorizontalAlignment = HorizontalAlignment.Center;
            tempPolygon.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            PointCollection myPoints = new PointCollection();
            myPoints.Add(new Point(0, -height));
            myPoints.Add(new Point(-0.5 * baseLength, 0));
            myPoints.Add(new Point(0.5 * baseLength, 0));
            tempPolygon.Points = myPoints;
            tempPolygon.Fill = new SolidColorBrush(Color.FromRgb((byte)rand.Next(1, 255), (byte)rand.Next(1, 255), (byte)rand.Next(1, 255)));
            Canvas.SetLeft(tempPolygon, place.X); Canvas.SetTop(tempPolygon, place.Y + height / 2);
            return tempPolygon;
        }
        private void UpdateCanvas()
        {
            mainCanvas.Children.Clear();
            for (int i = 0; i < indexOfShaps; i++)
            {
                mainCanvas.Children.Add(listOfShapsAll[i]);
            }
        }
        private void UndoCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (indexOfShaps > 0)
            {
                e.CanExecute = true;
            }            
        }
        private void UndoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            indexOfShaps--;
            UpdateCanvas();
        }
        private void RedoCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (indexOfShaps < listOfShapsAll.Count)
            {
                e.CanExecute = true;
            }
        }
        private void RedoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            indexOfShaps++;
            UpdateCanvas();
        }
        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBoxResult mbResult = MessageBox.Show("Are you sure you want make new project?", "New", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(mbResult == MessageBoxResult.Yes)
            {
                listOfShapsAll.Clear();
                indexOfShaps = 0;
                UpdateCanvas();
            }
        }
        private void CircleCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if(currentPlacingShapeMode != PlacingShapeMode.Circle)
            {
                e.CanExecute = true;
            }
        }
        private void CircleCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            currentPlacingShapeMode = PlacingShapeMode.Circle;
        }
        private void SquareCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (currentPlacingShapeMode != PlacingShapeMode.Square)
            {
                e.CanExecute = true;
            }
        }
        private void SquareCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            currentPlacingShapeMode = PlacingShapeMode.Square;
        }
        private void TriangleCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (currentPlacingShapeMode != PlacingShapeMode.Triangle)
            {
                e.CanExecute = true;
            }
        }
        private void TriangleCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            currentPlacingShapeMode = PlacingShapeMode.Triangle;
        }
    }
    public static class CustomCommands
    {
        public static readonly RoutedUICommand Circle = new RoutedUICommand
            ("Circle", "Circle", typeof(CustomCommands),
                new InputGestureCollection()
				{
                    new KeyGesture(Key.D1, ModifierKeys.Alt)
				}
            );
        public static readonly RoutedUICommand Square = new RoutedUICommand
            ("Square", "Square", typeof(CustomCommands),
                new InputGestureCollection()
				{
                    new KeyGesture(Key.D2, ModifierKeys.Alt)
				}
            );
        public static readonly RoutedUICommand Triangle = new RoutedUICommand
            ("Triangle", "Triangle", typeof(CustomCommands),
                new InputGestureCollection()
				{
                    new KeyGesture(Key.D3, ModifierKeys.Alt)
				}
            );
    }
}