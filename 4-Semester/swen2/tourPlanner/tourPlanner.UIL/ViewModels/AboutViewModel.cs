using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace tourPlanner.UIL.ViewModels
{
    public class AboutViewModel : BaseViewModel, ICloseWindow
    {
        public string Disclaimer => "This is the best implementation of tour planner the world has ever seen.";
        public string Author => "By Raphi und Fabi";

        public ICommand CloseCommand { get; }

        public Action? Close { get; set; }

        public AboutViewModel()
        {
            CloseCommand = new RelayCommand((_) => Close?.Invoke());
        }
    }
}
