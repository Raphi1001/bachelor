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
using tourPlanner.DAL.Mapquest;
using tourPlanner.UIL.Exceptions;

namespace tourPlanner.UIL.ViewModels
{
    public class AddRouteViewModel : BaseViewModel, ICloseWindow
    {
        private readonly IRouteManager routeManager;

        public event EventHandler<RouteInternal?>? ItemChanged;
        public ICommand AddCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand CloseCommand { get; }
        public Action? Close { get; set; }
        public string? From { get; set; }
        public string? To { get; set; }

        private RouteTypeEnum selectedRouteType;
        public RouteTypeEnum SelectedRouteType
        {
            get { return selectedRouteType; }
            set
            {
                selectedRouteType = value;
                OnPropertyChanged();
            }
        }

        public static IEnumerable<RouteTypeEnum> RouteTypeValues
        {
            get
            {
                return Enum.GetValues(typeof(RouteTypeEnum))
                    .Cast<RouteTypeEnum>();
            }
        }

        private RouteInternal? item;
        public RouteInternal? Item
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
        public void Reset()
        {
            SelectedRouteType = RouteTypeEnum.fastest;
            From = null;
            To = null;
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


        public AddRouteViewModel(IRouteManager routeManager)
        {
            this.routeManager = routeManager;

            CloseCommand = new RelayCommand((_) => Close?.Invoke());

            AddCommand = new RelayCommand(async (_) =>
            {
                IsBusy = true;

                if (string.IsNullOrEmpty(From) || string.IsNullOrEmpty(To))
                {
                    IsBusy = false;
                    return;
                }

                var route = await this.routeManager.CreateRouteAsync(To, From, SelectedRouteType);
                Item = route;
                IsBusy = false;
            }, (_) => IsBusy == false);

            ClearCommand = new RelayCommand((_) =>
            {
                Item = null;
            });

        }
    }
}
