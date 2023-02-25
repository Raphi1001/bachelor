using System;
using System.Globalization;
using tourPlanner.BL.Managers.ApiManagers;
using tourPlanner.BL.Managers.TourLogsManagers;
using tourPlanner.BL.Managers.ToursManagers;
using tourPlanner.Models;
using tourPlanner.Models.Enums;
using tourPlanner.Models.Route;
using tourPlanner.Models.Tour;
using tourPlanner.Models.TourLog;
using tourPlanner.UIL.ViewModels;
using Newtonsoft.Json;

namespace tourPlanner.Tests
{
    [TestFixture]
    public class TourLogInternalTests
    {      

        [Test]
        public void TourLogInternalToTourLogTransfereTransformationWorkingCorrectly()
        {
            //arrange        
            TourLogInternal tourLog = new(
                    new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    Rating.good,
                    Difficulty.veryEasy,
                    new DateOnly(2022, 1, 1),
                    420,
                    "comment"
                    );

            TourLogTransfere tourLogTansfereExpected = new()
            {
                Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                TourId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                TourRating = "good",
                TourDifficulty = "veryEasy",
                CreationDate = new DateOnly(2022, 1, 1),
                TimeTakenS = 420,
                TourComment = "comment",
            };

            // act
            TourLogTransfere tourLogTansfereActual = tourLog.ToTransfere();

            //assert
            AreEqualByJson(tourLogTansfereExpected, tourLogTansfereActual);

        } 

        private static void AreEqualByJson(object expected, object actual)
        {
            var expectedJson = JsonConvert.SerializeObject(expected);
            var actualJson = JsonConvert.SerializeObject(actual);
            Assert.That(actualJson, Is.EqualTo(expectedJson));
        }
    }
}