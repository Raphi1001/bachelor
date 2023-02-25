using System.Windows;
using tourPlanner.UIL.ViewModels;
using tourPlanner.UIL.Configuration;

namespace tourPlanner.UIL
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {         
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var ioCConfig = new IoCContainerConfiguration();
            ioCConfig.NavigationService.NavigateTo<MainViewModel>();
        }
    }
}
