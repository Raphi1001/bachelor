using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace tourPlanner.UIL.ViewModels
{
    public class ErrorViewModel : BaseViewModel, ICloseWindow
    {
        public string ErrorMessage { get; set; } = "An unkown error occured. Check logs for more information";
        public ICommand CloseCommand { get; }
        public Action? Close { get; set; }

        public void Reset()
        {
            ErrorMessage = "An unkown error occured. Check logs for more information";
        }

        public ErrorViewModel()
        {
            CloseCommand = new RelayCommand((_) => Close?.Invoke());
        }
    }
}
