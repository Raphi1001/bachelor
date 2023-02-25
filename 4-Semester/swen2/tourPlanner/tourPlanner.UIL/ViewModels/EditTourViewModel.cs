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
    public class EditTourViewModel: BaseViewModel, ICloseWindow
    {
        private readonly IStaticMapManager staticMapManager;
        private readonly IRouteManager routeManager;
        private readonly IToursManager toursManager;
        private readonly IImageDAO imgDao;

        public event EventHandler<TourInternal?>? ItemChanged;
        public ICommand EditCommand { get; }
        public ICommand CloseCommand { get; }
        public Action? Close { get; set; }

        private TourInternal? itemOld;
        public TourInternal? ItemOld
        {
            get => itemOld;
            set
            {
                itemOld = value;
                OnPropertyChanged();
            }
        }
        private TourTransfere? itemNew;
        public TourTransfere? ItemNew
        {
            get => itemNew;
            set
            {
                itemNew = value;
                OnPropertyChanged();
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

        private RouteTypeEnum selectedRouteType;
        public RouteTypeEnum SelectedRouteType
        {
            get { return selectedRouteType; }
            set
            {
       
                selectedRouteType = value;

                if (ItemNew is not null && ItemNew.Route is not null)
                {
                    ItemNew.Route.RouteType = selectedRouteType.ToString();
                }
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

        public EditTourViewModel(IStaticMapManager staticMapManager, IToursManager toursManager, IRouteManager routeManager, IImageDAO imgDao)
        {
            this.staticMapManager = staticMapManager;
            this.toursManager = toursManager;
            this.routeManager = routeManager;
            this.imgDao = imgDao;

            CloseCommand = new RelayCommand((_) => Close?.Invoke());

            EditCommand = new RelayCommand(async (_) =>
            {
                if (ItemNew is null || ItemOld is null)
                {
                    return;
                }

                IsBusy = true;

                TourInternal itemNewInternal = ItemNew.ToInternal();
                RouteInternal route = ItemOld.Route;
                string imagePath = ItemOld.ImagePath;

                //if from, to or routetype is changed we need to fetch a new Route
                if (
                    ItemNew?.Route?.From != ItemOld?.Route.From ||
                    ItemNew?.Route?.To != ItemOld?.Route.To ||
                    ItemNew?.Route?.RouteType != ItemOld?.Route.RouteType.ToString()
                    )
                {
                    var newRoute = await this.routeManager.CreateRouteAsync(itemNewInternal.Route.To, itemNewInternal.Route.From, itemNewInternal.Route.RouteType);
                    if (newRoute != null)
                    {
                        route = newRoute;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        this.imgDao.DeleteImage(ItemOld.ImagePath);
                        imagePath = await this.staticMapManager.CreateImageForRoute(route); // warning is wrong because it is checked in line 71
                    }
                }
                               
                TourInternal? updatedTour = this.toursManager.UpdateTour(ItemOld.Id, itemNewInternal.Name, itemNewInternal.Description, route, ItemOld.CreationDate, imagePath);            
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                Close?.Invoke();
                ItemChanged?.Invoke(this, updatedTour);
                IsBusy = false;
            }, (_) => IsBusy == false);         
        }
    }
}
