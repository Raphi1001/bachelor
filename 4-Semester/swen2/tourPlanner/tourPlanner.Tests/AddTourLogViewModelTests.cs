using System;
using System.Globalization;
using tourPlanner.BL.Managers.ApiManagers;
using tourPlanner.BL.Managers.TourLogsManagers;
using tourPlanner.BL.Managers.ToursManagers;
using tourPlanner.Models.Enums;
using tourPlanner.Models.Route;
using tourPlanner.Models.Tour;
using tourPlanner.Models.TourLog;
using tourPlanner.UIL.ViewModels;

namespace tourPlanner.Tests
{
    [TestFixture]
    public class AddTourLogViewModelTests
    {
        private AddTourLogViewModel AddTourLogViewModel { get; set; }
        private Mock<IToursManager> ToursManager { get; set; } = new();
        private Mock<ITourLogsManager> TourLogsManager { get; set; } = new();
      
        [SetUp]
        public void Setup()
        {
            AddTourLogViewModel = new(TourLogsManager.Object, ToursManager.Object);
        }

        [Test]
        public void AddCommandCreatesTourLogOnValidInput()
        {
            //arrange        
            Guid tourId = Guid.NewGuid();
            const double timeTakenH = 65;
            const string tourComment = "ImageIsHere";

            const Difficulty difficulty = Difficulty.medium;
            const Rating rating = Rating.medium;

            TourLogInternal tourLog = new(Guid.NewGuid(), tourId, rating, difficulty, DateOnly.FromDateTime(DateTime.Now), timeTakenH * 60 * 60, tourComment);

            TourLogsManager.Setup(m => m.AddTourLog(tourId, rating, difficulty, (int)timeTakenH * 60 * 60, tourComment)).Returns(tourLog);
            ToursManager.Setup(m => m.GetTourName(tourId)).Returns("TourName");

            AddTourLogViewModel.TourId = tourId;
            AddTourLogViewModel.TimeTakenH = timeTakenH;
            AddTourLogViewModel.TourComment = tourComment;
            AddTourLogViewModel.SelectedDifficulty = difficulty;
            AddTourLogViewModel.SelectedRating = rating;
            // act
            AddTourLogViewModel.AddCommand.Execute(null);

            //assert
            TourLogsManager.Verify(m => m.AddTourLog(tourId, rating, difficulty, (int)timeTakenH * 60 * 60, tourComment), Times.Once);
            ToursManager.Verify(m => m.GetTourName(tourId), Times.Once);    
            Assert.That(AddTourLogViewModel.Item, Is.SameAs(tourLog));
        }


        static object[] AddCommandInvalidInputTestCases = {
            new object[]
            {
                Guid.Empty,
                6.9,
                "comment"
            },
        };    

        [TestCaseSource(nameof(AddCommandInvalidInputTestCases))]
        [Test]
        public void AddCommandCreatesNoTourLogOnInvaildInput(Guid? tourId, double timeTakenH, string tourComment)
        {
            //arrange                    
            AddTourLogViewModel.TourId = tourId;
            AddTourLogViewModel.TimeTakenH = timeTakenH;
            AddTourLogViewModel.TourComment = tourComment;

            // act
            AddTourLogViewModel.AddCommand.Execute(null);

            //assert
            TourLogsManager.Verify(m => m.AddTourLog(It.IsAny<Guid>(), It.IsAny<Rating>(), It.IsAny<Difficulty>(), It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            ToursManager.Verify(m => m.GetTourName(It.IsAny<Guid>()), Times.Never);
            Assert.That(AddTourLogViewModel.Item, Is.Null);
        }
      
    }
}