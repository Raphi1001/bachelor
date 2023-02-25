using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using tourPlanner.BL.Managers.TourLogsManagers;
using tourPlanner.BL.Managers.ToursManagers;
using tourPlanner.Models.Enums;
using tourPlanner.Models.Tour;
using tourPlanner.Models.TourLog;

namespace tourPlanner.UIL.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        private readonly IToursManager toursManager;
        private readonly ITourLogsManager tourLogsManager;

        public event EventHandler<IEnumerable<TourInternal>?>? TourSearchChanged;

        public event EventHandler<IEnumerable<TourLogInternal>?>? TourLogSearchChanged;

        public ICommand SearchCommand { get; }

        public ICommand ClearCommand { get; }

        private string? searchText;
        public string? SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                OnPropertyChanged();
            }
        }
        public TourInternal? SelectedTour { get; set; }

        private SearchTypes selectedSearchType;
        public SearchTypes SelectedSearchType
        {
            get { return selectedSearchType; }
            set
            {
                selectedSearchType = value;
                OnPropertyChanged();
            }
        }

        public static IEnumerable<SearchTypes> SearchTypeValues
        {
            get
            {
                return Enum.GetValues(typeof(SearchTypes))
                    .Cast<SearchTypes>();
            }
        }

        public SearchViewModel(IToursManager toursManager, ITourLogsManager tourLogsManager)
        {
            this.toursManager = toursManager;
            this.tourLogsManager = tourLogsManager;

            SearchCommand = new RelayCommand((_) =>
            {
                switch(selectedSearchType)
                {
                    case SearchTypes.TourLogs:
                        if (SelectedTour is null) return;
                        var tourLogs = this.tourLogsManager.FindMatchingTourLogs(SelectedTour.Id, searchText);
                        TourLogSearchChanged?.Invoke(this, tourLogs);
                        break;

                    default: //search tours
                        var tours = this.toursManager.FindMatchingTours(searchText);               
                        TourSearchChanged?.Invoke(this, tours);

                        break;
                }


             
            });

            ClearCommand = new RelayCommand((_) =>
            {
                SelectedSearchType = SearchTypes.Tours;
                SearchText = "";
                var tours = toursManager.FindMatchingTours(searchText);
                this.TourSearchChanged?.Invoke(this, tours);
            });
        }
    }
}
