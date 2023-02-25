using tourPlanner.Models.Tour;

namespace tourPlanner.DAL.TourRepository
{
    public interface ITourRepository
    {
        void DeleteTour(Guid id);
        bool InsertTour(TourInternal tour);
        bool UpdateTour(TourInternal tour);
        TourTransfere? GetTourById(Guid id);
        IEnumerable<TourTransfere> GetAllTours();

    }
}