using tourPlanner.Models.Enums;
using tourPlanner.Models.Exceptions;

namespace tourPlanner.Models.TourLog
{
    public class TourLogTransfere
    {
        public Guid? Id { get; set; }
        public Guid? TourId { get; set; }
        public string? TourRating { get; set; } 
        public string? TourDifficulty { get; set; } 
        public DateOnly? CreationDate { get; set; }
      
        private int? timeTakenS = 0;
        public int? TimeTakenS
        {
            get { return timeTakenS; }
            set
            {
                timeTakenS = value;
                timeTakenH = timeTakenS / 60 / 60;
            }
        }

        private double? timeTakenH = 0;

        public double? TimeTakenH
        {
            get { return timeTakenH; }
            set
            {
                timeTakenH = value;
                timeTakenS = (int?)timeTakenH * 60 * 60;
            }
        }
        public string? TourComment { get; set; }

        public TourLogInternal ToInternal()
        {
            return new TourLogInternal(Id, TourId, ConvertRating(TourRating), ConvertDifficulty(TourDifficulty), CreationDate, TimeTakenS, TourComment);
        }

        private static Rating ConvertRating(string? rating)
        {
            if (rating is null)
            {
                throw new InvalidParameterException($"The Paramter rating should not be null");
            }

            if (!Enum.TryParse(rating, out Rating ratingE))
            {
                throw new InvalidRatingException($"The Paramter {rating} is no valid rating");
            }

            return ratingE;
        }
        private static Difficulty ConvertDifficulty(string? difficulty)
        {
            if (difficulty is null)
            {
                throw new InvalidParameterException($"The Paramter difficulty should not be null"); 
            }

            if (!Enum.TryParse(difficulty, out Difficulty difficultyE))
            {
                throw new InvalidDifficultyException($"The Paramter {difficulty} is no valid difficulty");
            }

            return difficultyE;
        }

    }
}
