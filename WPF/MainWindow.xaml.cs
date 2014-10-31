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

namespace WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        // The selected points.
        private List<Point> Corners = new List<Point>();
        private Point LastPoint;
        private int RADIUS = 3;

        void Canvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
 
            Console.WriteLine("Canvas click");
            e.Handled = true;
        }

    }
}
