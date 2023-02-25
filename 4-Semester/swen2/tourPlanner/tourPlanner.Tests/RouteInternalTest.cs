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
    public class RouteInternalTests
    {      

        [Test]
        public void RouteInternalToRouteTransfereTransformationWorkingCorrectly()
        {
            //arrange        
            RouteInternal routeInternal = new(
                "to",
                "from",
                new Coordinates(6, 9),
                new Coordinates(6, 9),
                6.9,
                RouteTypeEnum.fastest,
                420,
                "sessionId");

            RouteTransfere routeTransfereExpected = new()
            {
                To = "to",
                From = "from",
                Ul_lat = 6,
                Ul_lng = 9,
                Lr_lat = 6,
                Lr_lng = 9,
                Distance = 6.9,
                RouteType = "fastest",
                PlannedDurationS = 420,
                SessionId = "sessionId"
            };


            // act
            RouteTransfere routeTansfereActual = routeInternal.ToTransfere();

            //assert
            AreEqualByJson(routeTransfereExpected, routeTansfereActual);

        } 

        private static void AreEqualByJson(object expected, object actual)
        {
            var expectedJson = JsonConvert.SerializeObject(expected);
            var actualJson = JsonConvert.SerializeObject(actual);
            Assert.That(actualJson, Is.EqualTo(expectedJson));
        }
    }
}