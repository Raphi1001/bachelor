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
using tourPlanner.Models.Exceptions;

namespace tourPlanner.Tests
{
    [TestFixture]
    public class RouteTransfereTests
    {
        [TestCaseSource(nameof(ValidRoutesTransfereTestCases))]

        [Test]
        public void RouteTransfereToRouteInternalTransformationSuccessfullOnNecessaryPropertiesSet(RouteTransfere routeTransfere, RouteInternal routeInternalExpected)
        {
            // act
            RouteInternal routeActual = routeTransfere.ToInternal();

            //assert
            AreEqualByJson(routeInternalExpected, routeActual);
        }

        [TestCaseSource(nameof(InvalidRoutesTransfereTestCases))]
        [Test]
        public void RouteTransfereToRouteInteralTransformationThrowsExceptionOnIncompleteRouteTransfere(RouteTransfere routeTansfere)
        {
            //act and assert
            Assert.Throws<InvalidParameterException>(() => routeTansfere.ToInternal());
        }

        [Test]
        public void RouteTransfereToRouteInteralTransformationThrowsExceptionOnIncorrectRouteType()
        {
            //arrange
            RouteTransfere routeTansfere = new()
            {
                To = null,
                From = "from",
                Ul_lat = 6,
                Ul_lng = 9,
                Lr_lat = 6,
                Lr_lng = 9,
                Distance = 6.9,
                RouteType = "invalid",
                PlannedDurationS = 420,
                SessionId = "sessionId"
            };

            //act and assert
            Assert.Throws<InvalidRouteTypeException>(() => routeTansfere.ToInternal());
        }

 
        private static void AreEqualByJson(object expected, object actual)
        {
            var expectedJson = JsonConvert.SerializeObject(expected);
            var actualJson = JsonConvert.SerializeObject(actual);
            Assert.That(actualJson, Is.EqualTo(expectedJson));
        }

        private static object[] ValidRoutesTransfereTestCases = {
            new object[]
            {
                new RouteTransfere() {
                    To = "to",
                    From =  "from",
                    Ul_lat=6, Ul_lng=9,
                    Lr_lat=6, Lr_lng=9 ,
                    Distance = 6.9,
                    RouteType = "fastest",
                    PlannedDurationS = 420,
                    SessionId="sessionId"
                },
                 new RouteInternal(               
                     "to",               
                     "from",                
                     new Coordinates(6, 9),               
                     new Coordinates(6, 9),                
                     6.9,               
                     RouteTypeEnum.fastest,                
                     420,               
                     "sessionId")
            },           
                new object[]
            {
                new RouteTransfere() {
                    To = "to",
                    From =  "from",
                    Ul_lat=0, Ul_lng=0,
                    Lr_lat=0, Lr_lng=0 ,
                    Distance = 6.9,
                    RouteType = "fastest",
                    PlannedDurationS = 420,
                    SessionId=null
                },
                 new RouteInternal(
                     "to",
                     "from",
                     6.9,
                     RouteTypeEnum.fastest,
                     420)
            },
        };

        private static object[] InvalidRoutesTransfereTestCases = {
            new object[]
            {
                new RouteTransfere() {
                    To = null,
                    From =  "from",
                    Ul_lat=6, Ul_lng=9,
                    Lr_lat=6, Lr_lng=9 ,
                    Distance = 6.9,
                    RouteType = "fastest",
                    PlannedDurationS = 420,
                    SessionId="sessionId"},
            },
            new object[]
            {
                new RouteTransfere() {
                    To = "to",
                    From =  null,
                    Ul_lat=6, Ul_lng=9,
                    Lr_lat=6, Lr_lng=9 ,
                    Distance = 6.9,
                    RouteType = "fastest",
                    PlannedDurationS = 420,
                    SessionId="sessionId"},
            },
            new object[]
            {
                new RouteTransfere() {
                    To = "to",
                    From =  "from",
                    Ul_lat=6, Ul_lng=9,
                    Lr_lat=6, Lr_lng=9 ,
                    Distance = null,
                    RouteType = "fastest",
                    PlannedDurationS = 420,
                    SessionId="sessionId"},
            },
            new object[]
            {
                new RouteTransfere() {
                    To = "to",
                    From =  "from",
                    Ul_lat=6, Ul_lng=9,
                    Lr_lat=6, Lr_lng=9 ,
                    Distance = 6.9,
                    RouteType = null,
                    PlannedDurationS = 420,
                    SessionId="sessionId"},
            },
            new object[]
            {
                new RouteTransfere() {
                    To = "to",
                    From =  "from",
                    Ul_lat=6, Ul_lng=9,
                    Lr_lat=6, Lr_lng=9 ,
                    Distance = 6.9,
                    RouteType = "fastest",
                    PlannedDurationS = null,
                    SessionId="sessionId"},
            },
        };
    }
}