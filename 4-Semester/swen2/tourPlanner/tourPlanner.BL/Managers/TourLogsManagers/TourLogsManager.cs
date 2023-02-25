using tourPlanner.Models.Tour;
using tourPlanner.Models.TourLog;
using tourPlanner.DAL.TourLogRepository;

namespace tourPlanner.BL.Managers.TourLogsManagers
{
    public class TourLogsManager : ITourLogsManager
    {
        private readonly ITourLogRepository _tourLogRepo;
        public IEnumerable<TourInternal> Tours { get; } = new List<TourInternal>();

        public TourLogsManager(ITourLogRepository tourLogRepo)
        {
            _tourLogRepo = tourLogRepo;
        }

        public void DeleteTourLog(Guid id)
        {
            _tourLogRepo.DeleteTourLog(id);
        }

        public IEnumerable<TourLogInternal> GetTourLogs(Guid tourId)
        {
            return TourLogTransfereListToInternal(_tourLogRepo.GetTourLogs(tourId));
        }

        public TourLogInternal? AddTourLog(TourLogInternal tourLog)
        {
            return _tourLogRepo.InsertTourLog(tourLog) ? tourLog : null;
        }

        public TourLogInternal? AddTourLog(Guid tourId, Models.Enums.Rating rating, Models.Enums.Difficulty difficulty, int timeTaken, string tourComment)
        {
            TourLogInternal tourLog = new(Guid.NewGuid(), tourId, rating, difficulty, DateOnly.FromDateTime(DateTime.Now), timeTaken,tourComment);
            return _tourLogRepo.InsertTourLog(tourLog) ? tourLog : null;
        }

        public TourLogInternal? UpdateTourLog(TourLogInternal tourLog)
        {
            return _tourLogRepo.UpdateTourLog(tourLog) ? tourLog : null;
        }

        public TourLogInternal? UpdateTourLog(Guid logId, Guid tourId, Models.Enums.Rating rating, Models.Enums.Difficulty difficulty, DateOnly date, int timeTaken, string tourComment)
        {
            TourLogInternal tourLog = new(logId, tourId, rating, difficulty, date, timeTaken, tourComment);
            return _tourLogRepo.UpdateTourLog(tourLog) ? tourLog : null;
        }

        private static IEnumerable<TourLogInternal> TourLogTransfereListToInternal(IEnumerable<TourLogTransfere> tourLogsTransfere)
        {
            IEnumerable<TourLogInternal> tourLogsInternal = new List<TourLogInternal>();
            foreach (var tourLog in tourLogsTransfere)
            {
                tourLogsInternal = tourLogsInternal.Append(tourLog.ToInternal());
            }
            return tourLogsInternal;
        }

        public IEnumerable<TourLogInternal> FindMatchingTourLogs(Guid tourId, string? searchText = null)
        {
            IEnumerable<TourLogInternal> allTourLogs = TourLogTransfereListToInternal(_tourLogRepo.GetTourLogs(tourId));
            IEnumerable<TourLogInternal> tourLogsToReturn = new List<TourLogInternal>();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                return allTourLogs;
            }

            foreach (var tourLog in allTourLogs)
            {
                if (
                    tourLog.TourRating.ToString().ToLower().Contains(searchText.ToLower()) ||
                    tourLog.TourDifficulty.ToString().ToLower().Contains(searchText.ToLower()) ||
                    tourLog.CreationDate.ToString().ToLower().Contains(searchText.ToLower()) ||
                    tourLog.TimeTakenH.ToString().ToLower().Contains(searchText.ToLower()) ||
                    tourLog.TourComment.ToLower().Contains(searchText.ToLower())
                    )
                {
                    tourLogsToReturn = tourLogsToReturn.Append(tourLog);
                }
            }

            return tourLogsToReturn;
        }
    }
}
