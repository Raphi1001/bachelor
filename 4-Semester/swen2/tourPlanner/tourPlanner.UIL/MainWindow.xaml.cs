using System.Windows;
using tourPlanner.UIL.ViewModels;

namespace tourPlanner.UIL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
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
