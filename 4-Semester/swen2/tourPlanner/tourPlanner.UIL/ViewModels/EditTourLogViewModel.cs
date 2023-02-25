using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using tourPlanner.Models.Tour;
using tourPlanner.Models.Route;
using tourPlanner.Models;
using tourPlanner.BL.Managers.ApiManagers;
using tourPlanner.Models.Enums;
using tourPlanner.BL.Managers.ToursManagers;
using tourPlanner.BL.Managers.TourLogsManagers;
using tourPlanner.Models.TourLog;

namespace tourPlanner.UIL.ViewModels
{   
    public class EditTourLogViewModel: BaseViewModel, ICloseWindow
    {
        private readonly ITourLogsManager tourLogsManager;

        public event EventHandler<TourLogInternal?>? ItemChanged;
        public ICommand EditCommand { get; }
        public ICommand CloseCommand { get; }
        public Action? Close { get; set; }


        private TourLogTransfere? item;
        public TourLogTransfere? Item
        {
            get => item;
            set
            {
                item = value;
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


                if (Item is not null)
                {
                    Item.TourRating = selectedRating.ToString();
                }
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


                if (Item is not null)
                {
                    Item.TourDifficulty = selectedDifficulty.ToString();
                }
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

        public EditTourLogViewModel(ITourLogsManager tourLogsManager)
        {
            this.tourLogsManager = tourLogsManager;

            CloseCommand = new RelayCommand((_) => Close?.Invoke());

            EditCommand = new RelayCommand((_) =>
            {
                if (Item is null)
                {
                    return;
                }
                try
                {
                    TourLogInternal? updatedTourLog = this.tourLogsManager.UpdateTourLog(Item.ToInternal());
                    Close?.Invoke();
                    ItemChanged?.Invoke(this, updatedTourLog);
                }
                catch (Exception)
                {
                   
                }                    
            });         
        }
    }
}
