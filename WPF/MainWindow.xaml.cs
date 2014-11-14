//Authors
//Karoon Gayzagian
    //karoon80@hotmail.com
//Dina Najeeb
    //dina_2552@yahoo.com

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
            //Initilaize the combo boxes of color picker
            for (int i = 0; i < 256; i++)
            {
                comboContent.Add(i.ToString());
            }
            initializeColor(redColorPicker);
            redColorPicker.SelectedIndex = 255;
            initializeColor(greenColorPicker);
            greenColorPicker.SelectedIndex = 0;
            initializeColor(blueColorPicker);
            blueColorPicker.SelectedIndex = 0;
            colorMixer();
        }
        //The selected points.
        //private List<Point> cp = new List<Point>();
        private List<myCustomPoint> customeList = new List<myCustomPoint>();
        Color myRgbColor = new Color();
        private myCustomPoint myP;
        private myCustomPoint last;
        private int ptSize = 2;
        private Color myColor = Colors.Blue;
        int selectedRectIndex;
        List<string> comboContent = new List<string>();

        private void initializeColor(ComboBox box)
        {
            foreach(string s in comboContent)
            {
                box.Items.Add(s);    
            }
        }
        //About menu item onClick listener
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            String message = "WPF Sierpinski Attractor:\nMembers:\n1- Dina Najeeb\n\tE-mail: dina_2552@yahoo.com\n2- " +
            "Karoon Gayzagian\n\tE-mail:karoon80@hotmail.com";
            var result = MessageBox.Show(
                message, "About Authors", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        //Usage menu item onClick listener
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            String message = "Updated Sierpinski Attractor is  a fractal and attractive fixed set with different shapes, such as triangle, rectangle, etc... ,"+
                "subdivided recursively into smaller matching shapes. Sierpinski Attractor uses control points that decide the corners of the desired original shape, "+
                "then draws smaller shapes by drawing 2000 points.\n\nWhat does the GUI include?\n  When the user runs the application, the GUI has two parts:\n  1- Left panel: "+
                "It allows the user to choose the color and size of each control point, to run the application, and to clear the canvas.\n  "+
	"2- Canvas: It is the remaining white space where the user chooses the control points' positions.\n\nHow to get the control points?"+
	"1- Choose the desired size and color for a control point from the left panel. To choose the color,you must choose the levels of Red, Green, and Blue, then review the resulting color."+
	"\n  2- Right-click on canvas at the desired position to create one control point.\n  3- Repeat steps 1 and 2until you get the corners of the desired shape, such as triangle."+
	"\nNote: The number of control points is constrained to (3 <= Num_Of_Control_Points <= 6)\n  4- Click Run to get the final drawing.\n  5- Optional: Click Clear to get an empty canvas.\n\n"+
"Additional Operations:\n  The user can drag any control point and get a new shape according to the new position of the control point(s).";
            var result = MessageBox.Show(
               message, "How to use?", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        //Right Mouse Button Up Event handler
        private void main_canvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Save the point.
            //cp.Add(new Point(e.GetPosition(main_canvas).X, e.GetPosition(main_canvas).Y));
            ptSize = checkSize(); // check combobox for current value
            // Draw the new point.
            Rectangle controlPoint = new Rectangle();
            checkColor();
            colorMixer();//to check the color mixer functionality
            controlPoint.Fill = new SolidColorBrush(myColor);
            controlPoint.Stroke = new SolidColorBrush(Colors.Black);
            controlPoint.StrokeThickness = 2;
            controlPoint.Width = 2 * ptSize;
            controlPoint.Height = 2 * ptSize;

            controlPoint.MouseLeftButtonDown += selectRect;
            controlPoint.MouseLeftButtonUp += releaseRect;
            controlPoint.MouseMove += mouseMove;
            if (customeList.Count <= 5)
            {
                myP = new myCustomPoint(e.GetPosition(main_canvas).X, e.GetPosition(main_canvas).Y, myColor, ptSize);
                customeList.Add(myP);

                if (customeList.Count == 1)
                    main_canvas.Children.Clear();

                Canvas.SetLeft(controlPoint, e.GetPosition(main_canvas).X - (2 * ptSize));
                Canvas.SetTop(controlPoint, e.GetPosition(main_canvas).Y - (2 * ptSize));
                main_canvas.Children.Add(controlPoint);
            }
            else
            {
                var result = MessageBox.Show(this, "Ooops! No More than Six Points Allowed!!\nPress Clear Button", "Error!", MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
            }

           
        }

        private void checkColor()
        {
          /*  if (rbRed.IsChecked == true)
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
            }*/
          //  else if (rbCustomColor.IsChecked == true)
           // {
                colorMixer();
                myColor = myRgbColor;
            //}
        }

        //RGB color
        private void colorMixer()
        {
            string rSelected = redColorPicker.Text;
            string gSelected = greenColorPicker.Text;
            string bSelected = blueColorPicker.Text;
            myRgbColor = Color.FromRgb(Convert.ToByte(rSelected), Convert.ToByte(gSelected), Convert.ToByte(bSelected));
            colorMixerContainer.Background = new SolidColorBrush(myRgbColor);
        }
        private void comboBox_DropDownClosed(object sender, EventArgs e)
        {
            colorMixer();
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
            selectedRect = (Rectangle)e.Source;

            foreach (myCustomPoint pt in customeList)
            {
                Debug.WriteLine("Point is: " + (pt.xCoordinate - (2 * pt.pSize)) + ", " + (pt.yCoordinate - (2 * pt.pSize)));
                Debug.WriteLine("Canvas Point is: " + Canvas.GetLeft(selectedRect) + ", " + Canvas.GetTop(selectedRect));

                if ((Canvas.GetLeft(selectedRect) == (pt.xCoordinate - (2 * pt.pSize))) && (Canvas.GetTop(selectedRect) == (pt.yCoordinate - (2 * pt.pSize))))
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
            }

        }
        //point MouseLeftButtonUp 
        private void releaseRect(object sender, MouseButtonEventArgs e)
        {
            e.OriginalSource.ToString();
            if (captured)
            {
                Mouse.Capture(null);
                captured = false;
            }
            Debug.WriteLine("Index Redrawn is: " + selectedRectIndex);
            //customeList[selectedRectIndex].xCoordinate = x_shape;
            //customeList[selectedRectIndex].yCoordinate = y_shape;
            main_canvas.Children.Clear();
            redrawPoints();
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

                // Draw points.
                Random rand = new Random();
                last = customeList[rand.Next(0, customeList.Count)];
                // Draw 2000 points.
                for (int i = 1; i <= 2000; i++)
                {
                    int j = rand.Next(0, customeList.Count);
                    Color parentColor = customeList[j].pColor;
                    int parentRadius = customeList[j].pSize;

                    last = new myCustomPoint(((last.xCoordinate + customeList[j].xCoordinate) / 2), ((last.yCoordinate + customeList[j].yCoordinate) / 2), parentColor, parentRadius);

                    Rectangle myRect = new Rectangle();

                    myRect.Fill = new SolidColorBrush(parentColor);
                    myRect.Width = customeList[j].pSize;
                    myRect.Height = customeList[j].pSize;

                    Canvas.SetLeft(myRect, last.xCoordinate - parentRadius);
                    Canvas.SetTop(myRect, last.yCoordinate - parentRadius);

                    main_canvas.Children.Add(myRect);
                }

            }

        }

        private void redrawPoints()
        {
            // Draw the corners.
            foreach (myCustomPoint pt in customeList)
            {
                Rectangle sqr = new Rectangle();
                sqr.Width = 2 * pt.pSize;
                sqr.Height = 2 * pt.pSize;
                
                sqr.Fill = new SolidColorBrush(pt.pColor);
                sqr.Stroke = new SolidColorBrush(Colors.Black);
                sqr.StrokeThickness = 2;
                Canvas.SetLeft(sqr, pt.xCoordinate - (2 * pt.pSize));
                Canvas.SetTop(sqr, pt.yCoordinate - (2 * pt.pSize));
                sqr.MouseLeftButtonDown += selectRect;
                sqr.MouseLeftButtonUp += releaseRect;
                sqr.MouseMove += mouseMove;
                main_canvas.Children.Add(sqr);
            }
            // Draw points.
            Random rand = new Random();
            last = customeList[rand.Next(0, customeList.Count)];
            // Draw 2000 points.
            for (int i = 1; i <= 2000; i++)
            {
                int j = rand.Next(0, customeList.Count);
                Color parentColor = customeList[j].pColor;
                int parentRadius = customeList[j].pSize;

                last = new myCustomPoint(((last.xCoordinate + customeList[j].xCoordinate) / 2), ((last.yCoordinate + customeList[j].yCoordinate) / 2), parentColor, parentRadius);

                Rectangle myRect = new Rectangle();

                myRect.Fill = new SolidColorBrush(parentColor);
                myRect.Width = customeList[j].pSize;
                myRect.Height = customeList[j].pSize;

                Canvas.SetLeft(myRect, last.xCoordinate - parentRadius);
                Canvas.SetTop(myRect, last.yCoordinate - parentRadius);

                main_canvas.Children.Add(myRect);
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
    public int pSize { get; private set; }

    public myCustomPoint(double x, double y, Color c, int rad)
    {
        xCoordinate = x;
        yCoordinate = y;
        pColor = c;
        pSize = rad;
    }

    //Other properties, methods, events...
}
