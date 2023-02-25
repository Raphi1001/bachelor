using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using tourPlanner.BL.Managers.ToursManagers;
using tourPlanner.BL.Managers.TourLogsManagers;
using tourPlanner.Models.Tour;
using tourPlanner.Models.TourLog;
using tourPlanner.BL.Managers.ApiManagers;
using tourPlanner.Models.Route;
using System.Threading.Tasks;
using tourPlanner.Logging;

namespace tourPlanner.UIL.ViewModels
{
    public class MainViewModel : BaseViewModel, ICloseWindow
    {
        public TourListViewModel TourListViewModel { get; }
        public TourDetailViewModel TourDetailViewModel { get; }
        public TourLogsViewModel TourLogsViewModel { get; }
        public SearchViewModel SearchViewModel { get; }
        public MenuViewModel MenuViewModel { get; }
        public AddTourViewModel AddTourViewModel { get; }
        public AddRouteViewModel AddRouteViewModel { get; }
        public AddTourLogViewModel AddTourLogViewModel { get; }
        public EditTourViewModel EditTourViewModel { get; }
        public EditTourLogViewModel EditTourLogViewModel { get; }
        public ErrorViewModel ErrorViewModel { get; }

        private readonly ILogger logger;

        public Action? Close { get; set; }
        public MainViewModel(
            SearchViewModel searchViewModel,
            MenuViewModel menuViewModel,
            TourListViewModel tourListViewModel,
            TourDetailViewModel tourDetailViewModel,
            TourLogsViewModel tourLogsViewModel,
            AddTourViewModel addTourViewModel,
            AddRouteViewModel addRouteViewModel,
            AddTourLogViewModel addTourLogViewModel,
            EditTourViewModel editTourViewModel,
            EditTourLogViewModel editTourLgViewModel,
            ErrorViewModel errorViewModel,
            ILogManager logManager)
        {
            SearchViewModel = searchViewModel;
            MenuViewModel = menuViewModel;
            TourListViewModel = tourListViewModel;
            TourDetailViewModel = tourDetailViewModel;
            TourLogsViewModel = tourLogsViewModel;
            AddTourViewModel = addTourViewModel;
            AddRouteViewModel = addRouteViewModel;
            AddTourLogViewModel = addTourLogViewModel;
            EditTourViewModel = editTourViewModel;
            EditTourLogViewModel = editTourLgViewModel;
            ErrorViewModel = errorViewModel;

            logger = logManager.GetLogger<MainViewModel>();
            logger.Debug("App started.");

            /* NAVIGATION */

            MenuViewModel.AddRouteNavigationCommandExecuted += (_, executed) =>
                {
                    if (executed == true)
                    {
                        AddRouteViewModel.Reset();
                        NavigationService?.NavigateTo(AddRouteViewModel);
                    }
                };

            MenuViewModel.AddTourLogNavigationCommandExecuted += (_, executed) =>
            {
                if (executed == true)
                {
                    AddTourLogViewModel.Reset();
                    NavigationService?.NavigateTo(AddTourLogViewModel);
                }
            };

            MenuViewModel.AboutNavigationCommandExecuted += (_, executed) =>
            {
                if (executed == true)
                {
                    NavigationService?.NavigateTo<AboutViewModel>();
                }
            };

            MenuViewModel.ExitCommandExecuted += (_, executed) =>
            {
                if (executed == true)
                {
                    Close?.Invoke();
                }
            };

            /* DELETE COMMANDS */

            MenuViewModel.DeleteTourCommandExecuted += (_, tourId) =>
            {
                TourListViewModel.DeleteTour(tourId);
            };

            MenuViewModel.DeleteTourLogCommandExecuted += (_, tourLog) =>
            {
                if(tourLog == null)
                {
                    return;
                }
                TourLogsViewModel.DeleteTourLog(tourLog.Id);
                TourDetailViewModel.RemoveTourLog(tourLog);
            };

            /* EDIT COMMANDS */

            MenuViewModel.EditTourCommandExecuted += (_, tour) =>
            {
                if (tour is null)
                {
                    return;
                }
                EditTourViewModel.ItemOld = tour;
                EditTourViewModel.ItemNew = tour.ToTransfere();
                EditTourViewModel.SelectedRouteType = tour.Route.RouteType;
                NavigationService?.NavigateTo(EditTourViewModel);
            };

            MenuViewModel.EditTourLogCommandExecuted += (_, tourLog) =>
            {
                if (tourLog is null)
                {
                    return;
                }
                EditTourLogViewModel.Item = tourLog.ToTransfere();
                EditTourLogViewModel.SelectedDifficulty = tourLog.TourDifficulty;
                EditTourLogViewModel.SelectedRating = tourLog.TourRating;
                NavigationService?.NavigateTo(EditTourLogViewModel);

            };

            /* UI UPDATES */

            TourListViewModel.SelectedItemChanged += (_, tour) =>
            {
                MenuViewModel.SelectedTour = tour;
                AddTourLogViewModel.TourId = tour?.Id;
                TourDetailViewModel.Item = tour;
                SearchViewModel.SelectedTour = tour;             
                
                TourDetailViewModel.SetTourLogs(TourLogsViewModel.GetItems(tour?.Id).ToList());
            };

            TourLogsViewModel.SelectedItemChanged += (_, tourLog) =>
            {
                MenuViewModel.SelectedTourLog = tourLog;
            };

            AddRouteViewModel.ItemChanged += (_, route) =>
            {
                if(route is null)
                {
                    ErrorViewModel.ErrorMessage = "Invalid Input! Please Try again!";
                    NavigationService?.NavigateTo(ErrorViewModel);
                    return;
                }
                AddTourViewModel.Reset();
                AddTourViewModel.Route = route;
                NavigationService?.NavigateTo(AddTourViewModel);
            };

            AddTourViewModel.ItemChanged += (_, tour) =>
            {
                if (tour is not null)
                {
                    try
                    {
                            //replaces item in Items which has the same id as tour with tour
                        TourListViewModel.Items[TourListViewModel.Items.IndexOf(TourListViewModel.Items.First(item => item.Id.Equals(tour.Id)))] = tour;
                    }
                    catch (InvalidOperationException)
                    {
                            //id not found in list, we add tour to end of items instead
                        TourListViewModel.AddItem(tour);
                    }
                }
            };
            AddTourLogViewModel.ItemChanged += (_, tourLog) =>
            {
                if (tourLog is not null)
                {
                    TourDetailViewModel.AddTourLog(tourLog);
                    TourLogsViewModel.AddItem(tourLog);

                }
            };

            EditTourViewModel.ItemChanged += (_, tour) =>
            {
                if (tour is not null)
                {
                    TourListViewModel.UpdateItem(tour);
                    TourListViewModel.SelectedItem = tour;

                }
            };
            EditTourLogViewModel.ItemChanged += (_, tourLog) =>
            {
                if (tourLog is not null)
                {
                    TourLogsViewModel.UpdateItem(tourLog);
                    TourLogsViewModel.SelectedItem = tourLog;

                }
            };

            MenuViewModel.ImportTourCommandExecuted += (_, isExecuted) =>
            {
                if (isExecuted is true)
                {
                    tourListViewModel.GetItems();
                }
            };

            SearchViewModel.TourSearchChanged += (_, tours) =>
            {
                if (tours is null)
                {
                    return;
                }

                TourListViewModel.SetItems(tours);
            };
            SearchViewModel.TourLogSearchChanged += (_, tourLogs) =>
            {
                if (tourLogs is null)
                {
                    return;
                }

                TourLogsViewModel.SetItems(tourLogs);
            };
        }
    }
}
