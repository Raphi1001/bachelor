using tourPlanner.Models.Tour;
using tourPlanner.BL.Report;
using tourPlanner.Models.Route;
using tourPlanner.DAL.Mapquest;
using tourPlanner.BL.Exceptions;
using tourPlanner.DAL.TourRepository;
using tourPlanner.DAL.Exceptions;
using tourPlanner.BL.Managers.ApiManagers;

namespace tourPlanner.BL.Managers.ToursManagers
{
    public class ToursManager : IToursManager
    {
        public IEnumerable<TourInternal> Tours { get; } = new List<TourInternal>();

        private readonly IImageDAO imageDao;
        private readonly IFileDAO fileDao;
        private readonly ITourRepository tourRepo;
        private readonly ITourReportGenerator reportGenerator;
        private readonly IRouteManager routeManager;
        private readonly IStaticMapManager staticMapManager;

        public ToursManager(ITourRepository tourRepo, IImageDAO imageDao, ITourReportGenerator reportGenerator, IStaticMapManager staticMapManager , IFileDAO fileDao, IRouteManager routeManager)
        {
            this.imageDao = imageDao;
            this.tourRepo = tourRepo;
            this.reportGenerator = reportGenerator;
            this.staticMapManager = staticMapManager;
            this.fileDao = fileDao;
            this.routeManager = routeManager;
        }

        /*-----------------------------------------------------------------------------------------*/
        /* CRUD TOURS */
        public void DeleteTour(TourInternal tour)
        {
            tourRepo.DeleteTour(tour.Id);
            imageDao.DeleteImage(tour.ImagePath);
        }

        public TourInternal? AddTour(TourInternal tour)
        {
            return tourRepo.InsertTour(tour) ? tour : null;
        }

        public TourInternal? AddTour(string name, string description, RouteInternal route, string imgPath)
        {
            TourInternal tour = new(Guid.NewGuid(), name, description, route, DateOnly.FromDateTime(DateTime.Now), imgPath);
            return tourRepo.InsertTour(tour) ? tour : null;
        }

        public TourInternal? UpdateTour(TourInternal tour)
        {
            return tourRepo.UpdateTour(tour) ? tour : null;
        }

        public TourInternal? UpdateTour(Guid tourId, string name, string description, RouteInternal route, DateOnly date, string imgPath)
        {
            TourInternal tour = new(tourId, name, description, route, date, imgPath);

            return tourRepo.UpdateTour(tour) ? tour : null;
        }
/*-----------------------------------------------------------------------------------------*/

        public string? GetTourName(Guid id)
        {
            return tourRepo.GetTourById(id)?.Name;
        }

        public IEnumerable<TourInternal> GetTours()
        {
            return TourTransfereListToInternal(tourRepo.GetAllTours());
        }  

        public async Task<string> EnsureTourImage(TourInternal tour)
        {
            if (!imageDao.ImageExists(tour))
            {
                var route = await routeManager.CreateRouteAsync(tour.Route.To, tour.Route.From, tour.Route.RouteType);

                if(route is not null)
                {
                    tour.Route.SessionId = route.SessionId;
                }
                var imgPath = await staticMapManager.CreateImageForRoute(tour.Route);
                tour.ImagePath = imgPath;

                tourRepo.UpdateTour(tour);
            }
            return tour.ImagePath;
        }


        private static IEnumerable<TourInternal> TourTransfereListToInternal(IEnumerable<TourTransfere> toursTransfere)
        {
            IEnumerable<TourInternal> toursInternal = new List<TourInternal>();
            foreach (var tour in toursTransfere)
            {
                toursInternal = toursInternal.Append(tour.ToInternal());
            }
            return toursInternal;
        }
/*-----------------------------------------------------------------------------------------*/

        /* TOUR REPORTS */
        public void CreateTourReport(TourInternal tour)
        {
            reportGenerator.GenerateTourReport(tour);
        }

        public void CreateSummarizeReport(TourInternal tour)
        {
            reportGenerator.GenerateSummarizeReport(tour);
        }
/*-----------------------------------------------------------------------------------------*/

        /* EX-/IMPORT TOURS */
        public void ExportTour(TourInternal dto)
        {
            fileDao.ExportTour(dto);
        }

        public TourInternal? ImportTour(string fileName)
        {
            try
            {
                var tour2 = fileDao.ImportTour(fileName);
                var tour = tour2.ToInternal();
            
                return tourRepo.InsertTour(tour) ? tour : null;
            }
            catch (InvalidImportFileException)
            {
                return null;
            }
        }
        /*-----------------------------------------------------------------------------------------*/

        /* SEARCH TOURS */
        public IEnumerable<TourInternal> FindMatchingTours(string? searchText = null)
        {
            IEnumerable<TourInternal> allTours = TourTransfereListToInternal(tourRepo.GetAllTours());
            IEnumerable<TourInternal> toursToReturn = new List<TourInternal>();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                return allTours;
            }

            foreach(var tour in allTours)
            {
                if(
                    tour.Name.ToLower().Contains(searchText.ToLower()) ||
                    tour.Description.ToLower().Contains(searchText.ToLower()) ||
                    tour.CreationDate.ToString().ToLower().Contains(searchText.ToLower()) ||
                    tour.Route.From.ToLower().Contains(searchText.ToLower()) || 
                    tour.Route.To.ToLower().Contains(searchText.ToLower()) || 
                    tour.Route.Distance.ToString().ToLower().Contains(searchText.ToLower()) || 
                    tour.Route.RouteType.ToString().ToLower().Contains(searchText.ToLower()) ||
                    tour.Route.PlannedDurationH.ToString().ToLower().Contains(searchText.ToLower())
                    )
                {
                    toursToReturn = toursToReturn.Append(tour);
                }
            }

            return toursToReturn;
        }






    }
}
