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
    public class TourTransfereTests
    {

        [Test]
        public void TourTransfereToTourInternalTransformationSuccessfullOnCorrectData()
        {
            TourTransfere tourTansfere = new()
            {
                Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                Name = "name",
                Description = "description",
                Route = new RouteTransfere() { To = "to", From = "from", Ul_lat = 6, Ul_lng = 9, Lr_lat = 6, Lr_lng = 9, Distance = 6.9, RouteType = "fastest", PlannedDurationS = 420, SessionId = "sessionId" },
                CreationDate = new DateOnly(2022, 1, 1),
                ImagePath = "imgPath",
            };
            //arrange        
            TourInternal tourExpected = new(
                    new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    "name",
                    "description",
                    new RouteInternal("to", "from", new Coordinates(6, 9), new Coordinates(6, 9), 6.9, RouteTypeEnum.fastest, 420, "sessionId"), new DateOnly(2022, 1, 1), "imgPath"
                    );



            // act
            TourInternal tourActual = tourTansfere.ToInternal();

            //assert
            AreEqualByJson(tourExpected, tourActual);
        }

        [TestCaseSource(nameof(InvalidToursTransfereTestCases))]
        [Test]
        public void TourTransfereToTourInteralTransformationThrowsExceptionOnIncompleteTourTransfere(TourTransfere tourTansfere)
        {
            //act and assert
            Assert.Throws<InvalidParameterException>(() => tourTansfere.ToInternal());
        }

        private static void AreEqualByJson(object expected, object actual)
        {
            var expectedJson = JsonConvert.SerializeObject(expected);
            var actualJson = JsonConvert.SerializeObject(actual);
            Assert.That(actualJson, Is.EqualTo(expectedJson));
        }

        private static object[] InvalidToursTransfereTestCases = {
            new object[]
            {
                new TourTransfere()
                {
                    Id = null,
                    Name = "name",
                    Description = "description",
                    Route = new RouteTransfere() {To = "to", From =  "from", Ul_lat=6, Ul_lng=9, Lr_lat=6, Lr_lng=9 ,Distance = 6.9, RouteType = "fastest", PlannedDurationS = 420, SessionId="sessionId"},
                    CreationDate = new DateOnly(2022, 1, 1),       
                    ImagePath = "imgPath",
                }
            },
            new object[]
            {
                new TourTransfere()
                {
                    Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    Name = null,
                    Description = "otherDescription",
                    Route = new RouteTransfere() {To = "to", From =  "from", Ul_lat=6, Ul_lng=9, Lr_lat=6, Lr_lng=9 ,Distance = 6.9, RouteType = "fastest", PlannedDurationS = 420, SessionId="sessionId"},
                    CreationDate = new DateOnly(2002, 2, 2),
                    ImagePath = "imgPath",
                }
            },
            new object[]
            {
                new TourTransfere()
                {
                    Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    Name = "name",
                    Description = null,
                    Route = new RouteTransfere() {To = "to", From =  "from", Ul_lat=6, Ul_lng=9, Lr_lat=6, Lr_lng=9 ,Distance = 6.9, RouteType = "fastest", PlannedDurationS = 420, SessionId="sessionId"},
                    CreationDate = new DateOnly(2002, 2, 2),
                    ImagePath = "imgPath",
                }
            },             
            new object[]
            {
                new TourTransfere()
                {
                    Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    Name = "name",
                    Description = "otherDescription",
                    Route = null,
                    CreationDate = new DateOnly(2002, 2, 2),
                    ImagePath = "imgPath",
                }
            },               
            new object[]
            {
                new TourTransfere()
                {
                    Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    Name = "name",
                    Description = "otherDescription",
                    Route = new RouteTransfere() {To = "to", From =  "from", Ul_lat=6, Ul_lng=9, Lr_lat=6, Lr_lng=9 ,Distance = 6.9, RouteType = "fastest", PlannedDurationS = 420, SessionId="sessionId"},
                    CreationDate = null,
                    ImagePath = "imgPath",
                }
            },
            new object[]
            {
                new TourTransfere()
                {
                    Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    Name = "name",
                    Description = "otherDescription",
                    Route = new RouteTransfere() {To = "to", From =  "from", Ul_lat=6, Ul_lng=9, Lr_lat=6, Lr_lng=9 ,Distance = 6.9, RouteType = "fastest", PlannedDurationS = 420, SessionId="sessionId"},
                    CreationDate = new DateOnly(2002, 2, 2),
                    ImagePath = null,
                }
            },
            new object[]
            {
                new TourTransfere()
                {
                    Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    Name = "name",
                    Description = "otherDescription",
                    Route = new RouteTransfere() {To = "to", From =  "from", Ul_lat=null, Ul_lng=9, Lr_lat=6, Lr_lng=9 ,Distance = 6.9, RouteType = "fastest", PlannedDurationS = 420, SessionId="sessionId"},
                    CreationDate = new DateOnly(2002, 2, 2),
                    ImagePath = null,
                }
            },
            new object[]
            {
                new TourTransfere()
                {
                    Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    Name = "name",
                    Description = "otherDescription",
                    Route = new RouteTransfere() {To = "to", From =  "from", Ul_lat=6, Ul_lng=null, Lr_lat=6, Lr_lng=9 ,Distance = 6.9, RouteType = "fastest", PlannedDurationS = 420, SessionId="sessionId"},
                    CreationDate = new DateOnly(2002, 2, 2),
                    ImagePath = null,
                }
            },
            new object[]
            {
                new TourTransfere()
                {
                    Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    Name = "name",
                    Description = "otherDescription",
                    Route = new RouteTransfere() {To = "to", From =  "from", Ul_lat=6, Ul_lng=9, Lr_lat=null, Lr_lng=9 ,Distance = 6.9, RouteType = "fastest", PlannedDurationS = 420, SessionId="sessionId"},
                    CreationDate = new DateOnly(2002, 2, 2),
                    ImagePath = null,
                }
            },
            new object[]
            {
                new TourTransfere()
                {
                    Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    Name = "name",
                    Description = "otherDescription",
                    Route = new RouteTransfere() {To = "to", From =  "from", Ul_lat=6, Ul_lng=9, Lr_lat=6, Lr_lng=null ,Distance = 6.9, RouteType = "fastest", PlannedDurationS = 420, SessionId="sessionId"},
                    CreationDate = new DateOnly(2002, 2, 2),
                    ImagePath = null,
                }
            },          
        };

    }
}