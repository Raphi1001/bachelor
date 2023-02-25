using tourPlanner.Models.TourLog;

namespace tourPlanner.DAL.TourLogRepository
{
    public interface ITourLogRepository
    {
        void DeleteTourLog(Guid id);
        bool InsertTourLog(TourLogInternal tour);
        bool UpdateTourLog(TourLogInternal tourLog);
        IEnumerable<TourLogTransfere> GetAllTourLogs();
        IEnumerable<TourLogTransfere> GetTourLogs(Guid tourId);

    }
}