using tourPlanner.Models.Tour;
using tourPlanner.Models.Route;

namespace tourPlanner.BL.Managers.ToursManagers
{
    public interface IToursManager
    {
        public void DeleteTour(TourInternal tour);
        public string? GetTourName(Guid id);
        public TourInternal? ImportTour(string fileName);
        public void ExportTour(TourInternal dto);
        public IEnumerable<TourInternal> GetTours();
        public void CreateTourReport(TourInternal tour);
        public TourInternal? AddTour(TourInternal tour);
        public TourInternal? UpdateTour(TourInternal tour);
        public void CreateSummarizeReport(TourInternal tour);
        public TourInternal? AddTour(string name, string description, RouteInternal route, string imgPath);
        public TourInternal? UpdateTour(Guid tourId, string name, string description, RouteInternal route, DateOnly date, string imgPath);
        Task<string> EnsureTourImage(TourInternal tour);
        public IEnumerable<TourInternal> FindMatchingTours(string? searchText = null);
    }
}