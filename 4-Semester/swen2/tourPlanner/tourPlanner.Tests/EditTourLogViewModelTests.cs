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
    public class EditTourLogViewModelTests
    {
        private EditTourLogViewModel EditTourLogViewModel { get; set; }
        private Mock<ITourLogsManager> TourLogsManager { get; set; } = new();
      
        [SetUp]
        public void Setup()
        {
            EditTourLogViewModel = new(TourLogsManager.Object);
        }

        [Test]
        public void EditCommandCreatesTourLogOnValidInput()
        {
            //arrange        
            Guid tourId = Guid.NewGuid();
            const double timeTakenH = 65;
            const string tourComment = "ImageIsHere";

            const Difficulty difficulty = Difficulty.medium;
            const Rating rating = Rating.medium;

            TourLogInternal tourLog = new(Guid.NewGuid(), tourId, rating, difficulty, DateOnly.FromDateTime(DateTime.Now), timeTakenH * 60 * 60, tourComment);

            TourLogsManager.Setup(m => m.UpdateTourLog(It.IsAny<TourLogInternal>())).Returns(tourLog);

            EditTourLogViewModel.Item = tourLog.ToTransfere();

            // act
            EditTourLogViewModel.EditCommand.Execute(null);

            //assert
            TourLogsManager.Verify(m => m.UpdateTourLog(It.IsAny<TourLogInternal>()), Times.Once);
        }


        static readonly object[] EditCommandInvalidInputTestCases = {
            new object?[]
            {
                null
            },
            new object[]
            {
                new TourLogTransfere() 
            },
        };    

        [TestCaseSource(nameof(EditCommandInvalidInputTestCases))]
        [Test]
        public void EditCommandCreatesNoTourLogOnInvalidInput(TourLogTransfere? tour)
        {
            //arrange                    
            EditTourLogViewModel.Item = tour;

            // act
            EditTourLogViewModel.EditCommand.Execute(null);

            //assert
            TourLogsManager.Verify(m => m.UpdateTourLog(It.IsAny<TourLogInternal>()), Times.Never);

        }
    }
}