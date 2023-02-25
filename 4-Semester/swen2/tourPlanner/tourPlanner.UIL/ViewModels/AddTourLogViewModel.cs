using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using tourPlanner.Models.Tour;
using tourPlanner.Models.Route;
using tourPlanner.BL.Managers.TourLogsManagers;
using tourPlanner.Models.TourLog;
using tourPlanner.Models.Enums;
using tourPlanner.BL.Managers.ToursManagers;

namespace tourPlanner.UIL.ViewModels
{      
    public class AddTourLogViewModel : BaseViewModel, ICloseWindow
    {
        private readonly ITourLogsManager tourLogsManager;
        private readonly IToursManager toursManager;

        public event EventHandler<TourLogInternal?>? ItemChanged;
        public ICommand AddCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand CloseCommand { get; }
        public Action? Close { get; set; }

        private Guid? tourId;
        public Guid? TourId {
            get { return tourId; }
            set
            {
                tourId = value;
                if(TourId is not null && TourId != Guid.Empty)
                {
                    TourName = toursManager.GetTourName((Guid)TourId);
                }
            } 
        }
        
        private string? tourName;
        public string? TourName
        {
            get { return tourName; }
            set
            {
                tourName = value;
                OnPropertyChanged();
            }
        }

        private Rating selectedRating;
        public Rating SelectedRating
        {
            get { return selectedRating; }
            set
            {
                selectedRating = value;
                OnPropertyChanged();
            }
        }

        public static IEnumerable<Rating> RatingValues
        {
            get
            {
                return Enum.GetValues(typeof(Rating))
                    .Cast<Rating>();
            }
        }

        private Difficulty selectedDifficulty;


        public Difficulty SelectedDifficulty
        {
            get { return selectedDifficulty; }
            set
            {
                selectedDifficulty = value;
                OnPropertyChanged();
            }
        }

        public static IEnumerable<Difficulty> DifficultyValues
        {
            get
            {
                return Enum.GetValues(typeof(Difficulty))
                    .Cast<Difficulty>();
            }
        }
        public double? TimeTakenH { get; set; }
        public string? TourComment { get; set; }

        private TourLogInternal? item;
        public TourLogInternal? Item
        {
            get => item;
            set
            {
                item = value;
                OnPropertyChanged();
                OnItemChanged();
            }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();

                CommandManager.InvalidateRequerySuggested();
            }
        }

        public void Reset()
        {
            SelectedRating = Rating.medium;
            SelectedDifficulty = Difficulty.medium;
            TimeTakenH = null;
            TourComment = null;
        }

        private void OnItemChanged()
        {
            Close?.Invoke();
            ItemChanged?.Invoke(this, Item);
        }

        public AddTourLogViewModel(ITourLogsManager tourLogsManager, IToursManager toursManager)
        {
            this.tourLogsManager = tourLogsManager;
            this.toursManager = toursManager;

            CloseCommand = new RelayCommand((_) => Close?.Invoke());

            AddCommand = new RelayCommand((_) =>
            {
                IsBusy = true;
                int? timeInSec = ((int?)TimeTakenH * 60 * 60); //Converts hours to seconds

                if (TourId is null || TourId == Guid.Empty || TimeTakenH is null || string.IsNullOrEmpty(TourComment) ||  timeInSec is null)
                {
                    IsBusy = false;
                    return;
                }
                Item = this.tourLogsManager.AddTourLog((Guid)TourId, SelectedRating, SelectedDifficulty, (int)timeInSec, TourComment);
                IsBusy = false;
            }, (_) => IsBusy == false);

            ClearCommand = new RelayCommand((_) =>
            {
                Item = null;
                ItemChanged?.Invoke(this, Item);
            });

        }
    }
}
