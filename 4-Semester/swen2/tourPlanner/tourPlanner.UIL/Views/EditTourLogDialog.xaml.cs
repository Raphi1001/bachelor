﻿using tourPlanner.UIL.ViewModels;
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
using System.Windows.Shapes;


namespace tourPlanner.UIL.Views
{
    /// <summary>
    /// Interaction logic for EditTourLogDialog.xaml
    /// </summary>
    public partial class EditTourLogDialog : Window
    {
        public EditTourLogDialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ICloseWindow cw)
            {
                cw.Close += () => Close();
            }
        }
    }
}
