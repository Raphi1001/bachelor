using tourPlanner.Models.Enums;
using tourPlanner.Models.Exceptions;

namespace tourPlanner.Models.TourLog
{
    public class TourLogInternal
    {
        public Guid Id { get; private set; }
        public Guid TourId { get; private set; }
        public  Rating TourRating { get; private set; }
        public Difficulty TourDifficulty { get; private set; }
        public DateOnly CreationDate { get; private set; }
        public int TimeTakenS { get; private set; }
        public double TimeTakenH { get; private set; }
        public string TourComment { get; private set; }

        public TourLogInternal(Guid? id, Guid? tourId, Rating? rating, Difficulty? difficulty, DateOnly? creationDate, double? timeTakenS, string? tourComment)
        {
            Id = id is null ? throw new InvalidParameterException($"The Paramter Id should not be null") : (Guid)id;
            TourId = tourId is null ? throw new InvalidParameterException($"The Paramter TourId should not be null") : (Guid)tourId;
            TourRating = rating is null ? throw new InvalidParameterException($"The Paramter TourRating should not be null") : (Rating) rating;
            TourDifficulty = difficulty is null ? throw new InvalidParameterException($"The Paramter TourDifficulty should not be null") : (Difficulty)difficulty;
            CreationDate = creationDate is null ? throw new InvalidParameterException($"The Paramter CreationDate should not be null") : (DateOnly) creationDate;
            TimeTakenS = timeTakenS is null ? throw new InvalidParameterException($"The Paramter TimeTakenS should not be null") : (int)timeTakenS;
            TimeTakenH = TimeTakenS / 60 / 60;
            TourComment = tourComment is null ? throw new InvalidParameterException($"The Paramter TourComment should not be null") : tourComment;
        }

        public TourLogTransfere ToTransfere()
        {
            return new TourLogTransfere()
            {
                Id = Id,
                TourId = TourId,
                TourRating = TourRating.ToString(),
                TourDifficulty = TourDifficulty.ToString(),
                CreationDate = CreationDate,
                TimeTakenS = TimeTakenS,
                TourComment = TourComment,
            };
        }
    }
}
