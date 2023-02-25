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
    public class TourInternalTests
    {      

        [Test]
        public void TourInternalToTourTransfereTransformationWorkingCorrectly()
        {
            //arrange        
            TourInternal tour = new(
                    new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    "name",
                    "description",
                    new RouteInternal("to", "from", new Coordinates(6, 9), new Coordinates(6, 9), 6.9, RouteTypeEnum.fastest, 420, "sessionId"), new DateOnly(2022, 1, 1), "imgPath"
                    );

            TourTransfere tourTansfereExpected = new()
            {
                Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                Name = "name",
                Description = "description",
                Route = new RouteTransfere() { To = "to", From = "from", Ul_lat = 6, Ul_lng = 9, Lr_lat = 6, Lr_lng = 9, Distance = 6.9, RouteType = "fastest", PlannedDurationS = 420, SessionId = "sessionId" },
                CreationDate = new DateOnly(2022, 1, 1),
                ImagePath = "imgPath",
            };
                           
            // act
            TourTransfere tourTansfereActual = tour.ToTransfere();

            //assert
            AreEqualByJson(tourTansfereExpected, tourTansfereActual);

        } 

        private static void AreEqualByJson(object expected, object actual)
        {
            var expectedJson = JsonConvert.SerializeObject(expected);
            var actualJson = JsonConvert.SerializeObject(actual);
            Assert.That(actualJson, Is.EqualTo(expectedJson));
        }

    }
}