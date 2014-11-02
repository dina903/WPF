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
        private Point last;
        private int radius = 3;
        
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

            // Draw the new point.
            if (cp.Count == 1) 
                main_canvas.Children.Clear();
            Ellipse controlPoint= new Ellipse();
            controlPoint.Stroke = new SolidColorBrush(Colors.Black);
            controlPoint.StrokeThickness = 3;
            controlPoint.Width = 2 * radius;
            controlPoint.Height = 2 * radius;
            Canvas.SetLeft(controlPoint, e.GetPosition(main_canvas).X-radius);
            Canvas.SetTop(controlPoint, e.GetPosition(main_canvas).Y-radius);
            main_canvas.Children.Add(controlPoint);   
        }
        //Run Button onClick code
       private void runBtn_Click(object sender, RoutedEventArgs e)
        {
            // Start running.
            if (cp.Count < 3)
            {
                // We need more points.
               var result =  MessageBox.Show(this,"At least THREE points before clicking Run","Need More Points", MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
            }
            else if(cp.Count > 6)
            {
                var result = MessageBox.Show(this, "At most SIX points before clicking Run. Start Over", "Clear The Canvas", MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
            }
            else
            {
                // Draw the corners.
                foreach (Point pt in cp)
                {
                    Ellipse whiteSpace = new Ellipse();
                    whiteSpace.Width = 2 * radius;
                    whiteSpace.Height = 2 * radius;
                    whiteSpace.Fill = new SolidColorBrush(Colors.White);
                    Canvas.SetLeft(whiteSpace, pt.X-radius);
                    Canvas.SetTop(whiteSpace, pt.Y-radius);
                    main_canvas.Children.Add(whiteSpace);

                    Ellipse ellipse = new Ellipse();
                    ellipse.Width = 2 * radius;
                    ellipse.Height = 2 * radius;
                    ellipse.Stroke = new SolidColorBrush(Colors.Blue);
                    ellipse.StrokeThickness = 1;
                    //whiteSpace.Fill = new SolidColorBrush(Colors.White);
                    Canvas.SetLeft(ellipse, pt.X - radius);
                    Canvas.SetTop(ellipse, pt.Y - radius);
                    main_canvas.Children.Add(ellipse);
                }

                // Draw points.
                Random rand = new Random();
                last = cp[rand.Next(0, cp.Count)];
                // Draw 2000 points.
                for (int i = 1; i <= 2000; i++)
                {
                    int j = rand.Next(0, cp.Count);
                    last = new Point(
                        (last.X + cp[j].X) / 2,
                        (last.Y + cp[j].Y) / 2);
                    Line ellipseRepeat = new Line();
                    ellipseRepeat.X1 = last.X;
                    ellipseRepeat.Y1 = last.Y;
                    ellipseRepeat.X2 = last.X + 1;
                    ellipseRepeat.Y2 = last.Y + 1;
                    ellipseRepeat.Stroke = new SolidColorBrush(Colors.Blue);
                    ellipseRepeat.StrokeThickness = 1;
                    //whiteSpace.Fill = new SolidColorBrush(Colors.White);
                   
                    main_canvas.Children.Add(ellipseRepeat);
                   
                }
            } 
        }

       private void clearBtn_Click(object sender, RoutedEventArgs e)
       {
           main_canvas.Children.Clear();
           cp.Clear();
       }
    }
}
