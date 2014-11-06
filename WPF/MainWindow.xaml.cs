using System;
using System.IO;
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
using System.Security;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.ComponentModel;
using System.Timers;
using System.Runtime.InteropServices;

namespace WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        //The selected points.
        //private List<Point> cp = new List<Point>();
        private List<myCustomPoint> customeList = new List<myCustomPoint>();
        
        private myCustomPoint myP;
        private myCustomPoint last;
        private int radius = 2;
        private Color myColor = Colors.Blue;
        int selectedRectIndex = 0;

        //About menu item onClick listener
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            String message = "WPF Sierpinski Attractor:\nMembers:\n1- Dina Najeeb\n\tE-mail: dina_2552@yahoo.com\n2- " +
            "Karoon Gayzagian\n\tE-mail:karoon80@hotmail.com";
            var result = MessageBox.Show(
                message, "", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        //Usage menu item onClick listener
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            String message = "";
            var result = MessageBox.Show(
               message, "", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        //Right Mouse Button Up Event handler
        private void main_canvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Save the point.
            //cp.Add(new Point(e.GetPosition(main_canvas).X, e.GetPosition(main_canvas).Y));
            radius = checkSize(); // check combobox for current value
            // Draw the new point.
            Rectangle controlPoint = new Rectangle();
            checkColor();
            controlPoint.Fill = new SolidColorBrush(myColor);
            controlPoint.Stroke = new SolidColorBrush(Colors.Black);
            controlPoint.StrokeThickness = 2;
            controlPoint.Width = 2 * radius;
            controlPoint.Height = 2 * radius;

            controlPoint.MouseLeftButtonDown += selectRect;
            controlPoint.MouseLeftButtonUp += releaseRect;
            controlPoint.MouseMove += mouseMove;
                        
            myP = new myCustomPoint(e.GetPosition(main_canvas).X, e.GetPosition(main_canvas).Y, myColor, radius);
            customeList.Add(myP);

            if (customeList.Count == 1)
                main_canvas.Children.Clear();

            Canvas.SetLeft(controlPoint, e.GetPosition(main_canvas).X - 2 * radius);
            Canvas.SetTop(controlPoint, e.GetPosition(main_canvas).Y - 2 * radius);
            main_canvas.Children.Add(controlPoint);
        }

        private void checkColor()
        {
            if (rbRed.IsChecked == true)
            {
                myColor = Colors.Red;
            }
            else if (rbOlive.IsChecked == true)
            {
                myColor = Colors.Olive;
            }
            else if (rbOrchid.IsChecked == true)
            {
                myColor = Colors.Orchid;
            }
            else if (rbTurquoise.IsChecked == true)
            {
                myColor = Colors.Turquoise;
            }
            else if (rbBlue.IsChecked == true)
            {
                myColor = Colors.Blue;
            }
        }

        private int checkSize()
        {
            string selected = myComboBox.Text;
            int r = Convert.ToInt32(selected);
            return (r);
        }

        //drag and drop
        Rectangle selectedRect;
        bool captured = false;
        double x_shape, x_canvas, y_shape, y_canvas;
        private void selectRect(object sender, MouseButtonEventArgs e) // canvas MouseLeftButtonDown
        {
            selectedRect = (Rectangle) e.Source;

            foreach (myCustomPoint pt in customeList)
            {
                if ((Canvas.GetLeft(selectedRect) == pt.xCoordinate - 4) && (Canvas.GetTop(selectedRect) == pt.yCoordinate - 4))
                {
                    selectedRectIndex = customeList.IndexOf(pt);
                    Debug.WriteLine("Index of selected point is: " + selectedRectIndex);                    
                }
                
            }

            if (selectedRect != null)
            {
                Mouse.Capture(selectedRect);
                
                captured = true;
                x_shape = Canvas.GetLeft(selectedRect);
                x_canvas = e.GetPosition(main_canvas).X;
                y_shape = Canvas.GetTop(selectedRect);
                y_canvas = e.GetPosition(main_canvas).Y;
                
                //statusLabel.Content = "Selected " + selectedEllipse.Name;
            }
            
        }
        // point MouseLeftButtonUp 
        private void releaseRect(object sender, MouseButtonEventArgs e)
        {
            e.OriginalSource.ToString();
            if (captured)
            {
                Mouse.Capture(null);
                captured = false;
            }

        }

        //mouse move
        private void mouseMove(object sender, MouseEventArgs e)
        {
            double x = e.GetPosition(main_canvas).X;
            double y = e.GetPosition(main_canvas).Y;
            
            if (captured)
            { // rect has mouse capture
                x_shape += x - x_canvas;
                Canvas.SetLeft(selectedRect, x_shape);
                x_canvas = x;
                y_shape += y - y_canvas;
                Canvas.SetTop(selectedRect, y_shape);
                y_canvas = y;
                customeList[selectedRectIndex].xCoordinate = x_shape;
                customeList[selectedRectIndex].yCoordinate = y_shape;
            }
        }
        

        //Run Button onClick code
        private void runBtn_Click(object sender, RoutedEventArgs e)
        {
            // Start running.
            if (customeList.Count < 3)
            {
                // We need more points.
                var result = MessageBox.Show(this, "At least THREE points before clicking Run", "Need More Points", MessageBoxButton.OK,
                     MessageBoxImage.Exclamation);
            }
            else if (customeList.Count > 6)
            {
                var result = MessageBox.Show(this, "At most SIX points before clicking Run. Start Over", "Clear The Canvas", MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
            }
            else
            {
                /*/ Draw the corners.
                foreach (myCustomPoint pt in customeList)
                {
                    Rectangle sqr = new Rectangle();
                    sqr.Width = 2 * radius;
                    sqr.Height = 2 * radius;

                    sqr.Fill = new SolidColorBrush(pt.pColor);

                    Canvas.SetLeft(sqr, pt.coordinate.X - 2 * radius);
                    Canvas.SetTop(sqr, pt.coordinate.Y - 2 * radius);
                    main_canvas.Children.Add(sqr);
                }*/

                // Draw points.
                Random rand = new Random();
                last = customeList[rand.Next(0, customeList.Count)];
                // Draw 2000 points.
                for (int i = 1; i <= 2000; i++)
                {
                    int j = rand.Next(0, customeList.Count);
                    Color parentColor = customeList[j].pColor;
                    int parentRadius = customeList[j].pRadius;

                    last = new myCustomPoint(((last.xCoordinate + customeList[j].xCoordinate) / 2), ((last.yCoordinate + customeList[j].yCoordinate) / 2), parentColor, parentRadius);
                                       
                    Rectangle myRect = new Rectangle();

                    myRect.Fill = new SolidColorBrush(parentColor);
                    myRect.Width = customeList[j].pRadius;
                    myRect.Height = customeList[j].pRadius;

                    Canvas.SetLeft(myRect, last.xCoordinate - parentRadius);
                    Canvas.SetTop(myRect, last.yCoordinate - parentRadius);

                    main_canvas.Children.Add(myRect);
                }

            }

        }

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            main_canvas.Children.Clear();
            customeList.Clear();
            //cp.Clear();
        }
    }
}
//custom object to save parent color and size
public class myCustomPoint
{
    public double xCoordinate { get; set; }
    public double yCoordinate { get; set; }
    public Color pColor { get; private set; }
    public int pRadius { get; private set; }

    public myCustomPoint(double x, double y, Color c, int rad)
    {
        xCoordinate = x;
        yCoordinate = y;
        pColor = c;
        pRadius = rad;
    }
    
    //Other properties, methods, events...
}
