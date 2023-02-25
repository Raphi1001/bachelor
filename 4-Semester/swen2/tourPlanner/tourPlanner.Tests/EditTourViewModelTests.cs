using System;
using System.Globalization;
using System.Text.Json;
using tourPlanner.BL.Managers.ApiManagers;
using tourPlanner.BL.Managers.TourLogsManagers;
using tourPlanner.BL.Managers.ToursManagers;
using tourPlanner.DAL.Mapquest;
using tourPlanner.Models;
using tourPlanner.Models.Enums;
using tourPlanner.Models.Route;
using tourPlanner.Models.Tour;
using tourPlanner.Models.TourLog;
using tourPlanner.UIL.ViewModels;

namespace tourPlanner.Tests
{
    [TestFixture]
    public class EditTourViewModelTests
    {
        private EditTourViewModel EditTourViewModel { get; set; }
        private Mock<IToursManager> ToursManager { get; set; } = new();
        private Mock<IStaticMapManager> StaticMapManager { get; set; } = new();
        private Mock<IRouteManager> RouteManager { get; set; } = new();
        private Mock<IImageDAO> ImageDao { get; set; } = new();

        [SetUp]
        public void Setup()
        {
            EditTourViewModel = new(StaticMapManager.Object, ToursManager.Object, RouteManager.Object, ImageDao.Object);
        }

        [TestCaseSource(nameof(RouteShouldNotChangeTestCases))]
        [Test]
        public void EditCommandUpdatesTourWithoutCreatingNewRouteOnRouteParametersUnchanged(TourInternal tourOld, TourTransfere tourNew)
        {
            //arrange        
            var tourNewInternal = tourNew.ToInternal();
            RouteManager.Setup(m => m.CreateRouteAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<RouteTypeEnum>())).Returns(Task.Run(() => { return (RouteInternal?)tourNewInternal.Route; }));
            StaticMapManager.Setup(m => m.CreateImageForRoute(It.IsAny<RouteInternal>())).Returns(Task.Run(() => { return tourNewInternal.ImagePath; }));
            ToursManager.Setup(m => m.AddTour(tourNewInternal.Name, tourNewInternal.Description, tourNewInternal.Route, tourNewInternal.ImagePath)).Returns(tourNewInternal);

            EditTourViewModel.ItemOld = tourOld;
            EditTourViewModel.ItemNew = tourNew;

            // act
            EditTourViewModel.EditCommand.Execute(null);

            //assert
            StaticMapManager.Verify(m => m.CreateImageForRoute(It.IsAny<RouteInternal>()), Times.Never);
            RouteManager.Verify(m => m.CreateRouteAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<RouteTypeEnum>()), Times.Never);

            ToursManager.Verify(m => m.UpdateTour(tourOld.Id, tourNewInternal.Name, tourNewInternal.Description, tourOld.Route, tourOld.CreationDate, tourOld.ImagePath), Times.Once);

            //Reset mocks
            ToursManager.Invocations.Clear();
        }

        [TestCaseSource(nameof(RouteShouldChangeTestCases))]
        [Test]
        public void EditCommandUpdatesTourAndCreatesNewRouteOnRouteParametersChanged(TourInternal tourOld, TourTransfere tourNew)
        {
            //arrange        
            var tourNewInternal = tourNew.ToInternal();
            RouteManager.Setup(m => m.CreateRouteAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<RouteTypeEnum>())).Returns(Task.Run(() => { return (RouteInternal?)tourNewInternal.Route; }));
            StaticMapManager.Setup(m => m.CreateImageForRoute(It.IsAny<RouteInternal>())).Returns(Task.Run(() => { return tourNewInternal.ImagePath; }));
            ToursManager.Setup(m => m.AddTour(tourNewInternal.Name, tourNewInternal.Description, tourNewInternal.Route, tourNewInternal.ImagePath)).Returns(tourNewInternal);

            EditTourViewModel.ItemOld = tourOld;
            EditTourViewModel.ItemNew = tourNew;

            // act
            EditTourViewModel.EditCommand.Execute(null);

            //assert
            StaticMapManager.Verify(m => m.CreateImageForRoute(It.IsAny<RouteInternal>()), Times.Once);
            RouteManager.Verify(m => m.CreateRouteAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<RouteTypeEnum>()), Times.Once);

            ToursManager.Verify(m => m.UpdateTour(tourOld.Id, tourNewInternal.Name, tourNewInternal.Description, tourNewInternal.Route, tourOld.CreationDate, tourNewInternal.ImagePath), Times.Once);
            
            
            //Reset mocks
            ToursManager.Invocations.Clear();
            StaticMapManager.Invocations.Clear();
            RouteManager.Invocations.Clear();
        }

        private static object[] RouteShouldNotChangeTestCases = {
            new object[]
            {
                new TourInternal(
                    new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    "name",
                    "description",
                    new RouteInternal("to", "from", new Coordinates(6, 9), new Coordinates(6, 9), 6.9, RouteTypeEnum.fastest, 420, "sessionId"), new DateOnly(2022, 1, 1), "imgPath"
                    ),
                new TourTransfere()
                {
                    Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    Name = "name",
                    Description = "description",
                    Route = new RouteTransfere() {To = "to", From =  "from", Ul_lat=6, Ul_lng=9, Lr_lat=6, Lr_lng=9 ,Distance = 6.9, RouteType = "fastest", PlannedDurationS = 420, SessionId="sessionId"},
                    CreationDate = new DateOnly(2022, 1, 1),
                    ImagePath = "imgPath",
                }
            },
            new object[]
            {
                new TourInternal(
                    new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    "name",
                    "description",
                    new RouteInternal("to", "from", new Coordinates(6, 9), new Coordinates(6, 9), 6.9, RouteTypeEnum.fastest, 420, "sessionId"), 
                    new DateOnly(2022, 1, 1), 
                    "imgPath"
                    ),
                new TourTransfere()
                {
                    Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    Name = "otherName",
                    Description = "otherDescription",
                    Route = new RouteTransfere() {To = "to", From =  "from", Ul_lat=6, Ul_lng=9, Lr_lat=6, Lr_lng=9 ,Distance = 6.9, RouteType = "fastest", PlannedDurationS = 420, SessionId="sessionId"},
                    CreationDate = new DateOnly(2002, 2, 2),
                    ImagePath = "imgPath",
                }
            },       
        };

        /// ///////////////////////////////////////////////////////////////////////////////////////
        private static object[] RouteShouldChangeTestCases = {
            new object[]
            {
                new TourInternal(
                    new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    "name",
                    "description",
                    new RouteInternal("to", "from", new Coordinates(6, 9), new Coordinates(6, 9), 6.9, RouteTypeEnum.fastest, 420, "sessionId"), 
                    new DateOnly(2022, 1, 1), 
                    "imgPath"
                    ),
                new TourTransfere()
                {
                    Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    Name = "name",
                    Description = "description",
                    Route = new RouteTransfere() {To = "otherTo", From =  "from", Ul_lat=6, Ul_lng=9, Lr_lat=6, Lr_lng=9 ,Distance = 6.9, RouteType = "fastest", PlannedDurationS = 420, SessionId="sessionId"},
                    CreationDate = new DateOnly(2022, 1, 1),
                    ImagePath = "imgPath",
                }
            },
            new object[]
            {
                new TourInternal(
                    new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    "name",
                    "description",
                    new RouteInternal("to", "from", new Coordinates(6, 9), new Coordinates(6, 9), 6.9, RouteTypeEnum.fastest, 420, "sessionId"), 
                    new DateOnly(2022, 1, 1), 
                    "imgPath"
                    ),
                new TourTransfere()
                {
                    Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    Name = "name",
                    Description = "description",
                    Route = new RouteTransfere() {To = "to", From =  "otherFrom", Ul_lat=6, Ul_lng=9, Lr_lat=6, Lr_lng=9 ,Distance = 6.9, RouteType = "fastest", PlannedDurationS = 420, SessionId="sessionId"},
                    CreationDate = new DateOnly(2022, 1, 1),
                    ImagePath = "imgPath",
                }
            },
            new object[]
            {
                new TourInternal(
                    new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    "name",
                    "description",
                    new RouteInternal("to", "from", new Coordinates(6, 9), new Coordinates(6, 9), 6.9, RouteTypeEnum.fastest, 420, "sessionId"), new DateOnly(2022, 1, 1), "imgPath"
                    ),
                new TourTransfere()
                {
                    Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    Name = "name",
                    Description = "description",
                    Route = new RouteTransfere() {To = "to", From =  "from", Ul_lat=6, Ul_lng=9, Lr_lat=6, Lr_lng=9 ,Distance = 6.9, RouteType = "shortest", PlannedDurationS = 420, SessionId="sessionId"},
                    CreationDate = new DateOnly(2022, 1, 1),
                    ImagePath = "imgPath",
                }
            }
        };

    }
}
