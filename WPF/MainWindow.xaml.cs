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
            MessageBox.Show("releaseEllispse: \r\t sender = " + sender.ToString() + " \r\t source = " + e.Source.ToString() + " \r\t original source = " + e.OriginalSource.ToString());
        }

        private void main_canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("releaseEllispse: \r\t sender = " + sender.ToString() + " \r\t source = " + e.Source.ToString() + " \r\t original source = " + e.OriginalSource.ToString());
        }
    }
}
