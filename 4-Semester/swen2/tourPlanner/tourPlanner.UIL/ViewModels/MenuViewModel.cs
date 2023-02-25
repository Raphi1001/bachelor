using System;
using System.Windows.Input;
using tourPlanner.DAL.Mapquest;
using tourPlanner.BL.Report;
using tourPlanner.Models.Tour;
using tourPlanner.Models.TourLog;
using tourPlanner.BL.Managers.ToursManagers;
using Microsoft.Win32;
namespace tourPlanner.UIL.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        public event EventHandler<TourInternal?>? DeleteTourCommandExecuted;
        public event EventHandler<TourLogInternal?>? DeleteTourLogCommandExecuted;
        
        public event EventHandler<TourInternal?>? EditTourCommandExecuted;
        public event EventHandler<TourLogInternal?>? EditTourLogCommandExecuted;

        public event EventHandler<bool?>? ExitCommandExecuted;
        public event EventHandler<bool?>? ImportTourCommandExecuted;
        public event EventHandler<bool?>? AboutNavigationCommandExecuted;
        public event EventHandler<bool?>? AddRouteNavigationCommandExecuted;
        public event EventHandler<bool?>? AddTourLogNavigationCommandExecuted;

        public ICommand ExitCommand { get; }
        public ICommand EditTourCommand { get; }
        public ICommand DeleteTourCommand { get; }
        public ICommand ExportTourCommand { get; }
        public ICommand ImportTourCommand { get; }
        public ICommand EditTourLogCommand { get; }
        public ICommand DeleteTourLogCommand { get; }
        public ICommand AboutNavigationCommand { get; }
        public ICommand CreateTourReportCommand { get; }
        public ICommand AddRouteNavigationCommand { get; }
        public ICommand AddTourLogNavigationCommand { get; }
        public ICommand CreateSummarizeReportCommand { get; }

        public TourInternal? SelectedTour { get; set; }
        public TourLogInternal? SelectedTourLog { get; set; }

        private readonly IToursManager? _tourManager;

        public MenuViewModel(IToursManager toursManager)
        {
            _tourManager = toursManager;

            AddRouteNavigationCommand = new RelayCommand((_) =>
            {
                AddRouteNavigationCommandExecuted?.Invoke(this, true);
            });

            AddTourLogNavigationCommand = new RelayCommand((_) =>
            {
                AddTourLogNavigationCommandExecuted?.Invoke(this, true);
            }, (_) => SelectedTour != null);

            AboutNavigationCommand = new RelayCommand((_) =>
            {
                AboutNavigationCommandExecuted?.Invoke(this, true);
            });

            ExitCommand = new RelayCommand((_) =>
            {
                ExitCommandExecuted?.Invoke(this, true);
            });

            CreateTourReportCommand = new RelayCommand((_) =>
            {
                if (SelectedTour is null)
                    return;
                _tourManager.CreateTourReport(SelectedTour);
            }, (_) => SelectedTour != null);
            
            CreateSummarizeReportCommand = new RelayCommand((_) =>
            {
                if (SelectedTour is null)
                    return;
                _tourManager.CreateSummarizeReport(SelectedTour);
            }, (_) => SelectedTour != null);

            DeleteTourCommand = new RelayCommand((_) =>
            {
                DeleteTourCommandExecuted?.Invoke(this, SelectedTour);

            }, (_) => SelectedTour != null);

            DeleteTourLogCommand = new RelayCommand((_) =>
            {
                DeleteTourLogCommandExecuted?.Invoke(this, SelectedTourLog);

            }, (_) => SelectedTourLog != null);

            EditTourCommand = new RelayCommand((_) =>
            {
                EditTourCommandExecuted?.Invoke(this, SelectedTour);

            }, (_) => SelectedTour != null);
            
            EditTourLogCommand = new RelayCommand((_) =>
            {
                EditTourLogCommandExecuted?.Invoke(this, SelectedTourLog);

            }, (_) => SelectedTourLog != null);

            ExportTourCommand = new RelayCommand((_) =>
            {
                if (SelectedTour is null)
                    return;

                _tourManager.ExportTour(SelectedTour);

            }, (_) => SelectedTour != null);

            ImportTourCommand = new RelayCommand((_) =>
            {
                // Configure open file dialog box
                var dialog = new OpenFileDialog();
                dialog.FileName = "Document"; // Default file name
                dialog.DefaultExt = ".json"; // Default file extension
                dialog.Filter = "Json documents (.json)|*.json"; // Filter files by extension

                // Show open file dialog box
                bool? result = dialog.ShowDialog();

                // Process open file dialog box results
                if (result == true)
                {
                    // Open document
                    _tourManager.ImportTour(dialog.FileName);
                    ImportTourCommandExecuted?.Invoke(this, true);
                }
            });

        }        
    }
}
