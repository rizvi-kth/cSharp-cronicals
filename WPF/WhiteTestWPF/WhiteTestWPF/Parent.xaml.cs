﻿using System;
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

namespace WhiteTestWPF
{
    /// <summary>
    /// Interaction logic for Parent.xaml
    /// </summary>
    public partial class Parent : UserControl
    {
        public Parent()
        {
            InitializeComponent();
        }

        private void yellowButton_Click(object sender, RoutedEventArgs e)
        {
            label2.Content = "Tessting riz";
        }
    }
}
