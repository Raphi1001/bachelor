using tourPlanner.Models.TourLog;

namespace tourPlanner.BL.Managers.TourLogsManagers
{
    public interface ITourLogsManager
    {
        public void DeleteTourLog(Guid id);
        public TourLogInternal? AddTourLog(TourLogInternal tourLog);
        public IEnumerable<TourLogInternal> GetTourLogs(Guid tourId);
        public TourLogInternal? UpdateTourLog(TourLogInternal tourLog);
        public TourLogInternal? AddTourLog(Guid tourId, Models.Enums.Rating rating, Models.Enums.Difficulty difficulty, int timeTaken, string tourComment);
        public TourLogInternal? UpdateTourLog(Guid logId, Guid tourId, Models.Enums.Rating rating, Models.Enums.Difficulty difficulty, DateOnly date, int timeTaken, string tourComment);
        IEnumerable<TourLogInternal> FindMatchingTourLogs(Guid tourId, string? searchText = null);

    }
}