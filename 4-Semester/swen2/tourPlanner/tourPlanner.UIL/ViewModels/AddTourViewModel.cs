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
using tourPlanner.DAL.Mapquest;

namespace tourPlanner.UIL.ViewModels
{
    public class AddTourViewModel : BaseViewModel, ICloseWindow
    {
        private readonly IStaticMapManager staticMapManager;
        private readonly IToursManager toursManager;

        public event EventHandler<TourInternal?>? ItemChanged;
        public ICommand AddCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand CloseCommand { get; }
        public Action? Close { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public RouteInternal? Route { get; set; }

        private TourInternal? item;
        public TourInternal? Item
        {
            get => item;
            set
            {
                item = value;
                OnPropertyChanged();

                Close?.Invoke();
                ItemChanged?.Invoke(this, Item);
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
            Name = null;
            Description = null;
        }

        public AddTourViewModel(IStaticMapManager staticMapManager, IToursManager toursManager)
        {
            this.staticMapManager = staticMapManager;
            this.toursManager = toursManager;

            CloseCommand = new RelayCommand((_) => Close?.Invoke());

            AddCommand = new RelayCommand(async (_) =>
            {

                if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Description) || Route is null)
                {
                    return;
                }

                IsBusy = true;

                var imgPath = await this.staticMapManager.CreateImageForRoute(Route);
                                                     
                Item = this.toursManager.AddTour(Name, Description, Route, imgPath); 
                
                IsBusy = false;
            }, (_) => IsBusy == false);

            ClearCommand = new RelayCommand((_) =>
            {
                Item = null;
            });

        }
    }
}
