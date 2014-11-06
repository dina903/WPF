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
        // The selected points.
        private List<Point> cp = new List<Point>();
        private List<myCustomPoint> customeList = new List<myCustomPoint>();

        private myCustomPoint myP;
        private Point last;
        private int radius = 2;
        private Color myColor = Colors.Blue;

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
            cp.Add(new Point(e.GetPosition(main_canvas).X, e.GetPosition(main_canvas).Y));
            radius = checkSize(); // check combobox for current value
            // Draw the new point.
            if (cp.Count == 1)
                main_canvas.Children.Clear();
            Ellipse controlPoint = new Ellipse();
            checkColor();
            controlPoint.Fill = new SolidColorBrush(myColor);
            controlPoint.Width = 2 * radius;
            controlPoint.Height = 2 * radius;

            myP = new myCustomPoint(new Point(e.GetPosition(main_canvas).X, e.GetPosition(main_canvas).Y), myColor, radius);
            customeList.Add(myP);

            Canvas.SetLeft(controlPoint, e.GetPosition(main_canvas).X - radius);
            Canvas.SetTop(controlPoint, e.GetPosition(main_canvas).Y - radius);
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



        //Run Button onClick code
        private void runBtn_Click(object sender, RoutedEventArgs e)
        {
            // Start running.
            if (cp.Count < 3)
            {
                // We need more points.
                var result = MessageBox.Show(this, "At least THREE points before clicking Run", "Need More Points", MessageBoxButton.OK,
                     MessageBoxImage.Exclamation);
            }
            else if (cp.Count > 6)
            {
                var result = MessageBox.Show(this, "At most SIX points before clicking Run. Start Over", "Clear The Canvas", MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
            }
            else
            {
                // Draw the corners.
                foreach (Point pt in cp)
                {
                    Rectangle sqr = new Rectangle();
                    sqr.Width = 2 * radius;
                    sqr.Height = 2 * radius;
                    sqr.Fill = new SolidColorBrush(Colors.Black);

                    Canvas.SetLeft(sqr, pt.X - 2 * radius);
                    Canvas.SetTop(sqr, pt.Y - 2 * radius);
                    main_canvas.Children.Add(sqr);
                }

                // Draw points.
                Random rand = new Random();
                last = cp[rand.Next(0, cp.Count)];
                // Draw 2000 points.
                for (int i = 1; i <= 2000; i++)
                {
                    int j = rand.Next(0, cp.Count);
                    last = new Point((last.X + cp[j].X) / 2, (last.Y + cp[j].Y) / 2);

                    Ellipse myElli = new Ellipse();
                    Color parentColor = customeList[j].pColor;

                    myElli.Fill = new SolidColorBrush(parentColor);
                    myElli.Width = 2 * customeList[j].pRadius;
                    myElli.Height = 2 * customeList[j].pRadius;

                    Canvas.SetLeft(myElli, last.X - customeList[j].pRadius);
                    Canvas.SetTop(myElli, last.Y - customeList[j].pRadius);

                    main_canvas.Children.Add(myElli);

                }

            }

        }

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            main_canvas.Children.Clear();
            customeList.Clear();
            cp.Clear();
        }
    }
}
//custom object to save parent color and size
public class myCustomPoint
{
    public Point coordinate { get; private set; }
    public Color pColor { get; private set; }
    public int pRadius { get; private set; }

    public myCustomPoint(Point p, Color c, int rad)
    {
        coordinate = p;
        pColor = c;
        pRadius = rad;
    }
    //Other properties, methods, events...
}
