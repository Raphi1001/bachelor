
using System;
using System.Globalization;
using tourPlanner.BL.Managers.ApiManagers;
using tourPlanner.BL.Managers.ToursManagers;
using tourPlanner.Models.Enums;
using tourPlanner.Models.Route;
using tourPlanner.Models.Tour;
using tourPlanner.UIL.ViewModels;

namespace tourPlanner.Tests
{
    [TestFixture]
    public class AddTourViewModelTests
    {
        private AddTourViewModel AddTourViewModel{ get; set; }
        private Mock<IStaticMapManager> StaticMapManager { get; set; }
        private Mock<IToursManager> ToursManager { get; set; }
      
        [SetUp]
        public void Setup()
        {
            StaticMapManager = new Mock<IStaticMapManager>();
            ToursManager = new Mock<IToursManager>();
            AddTourViewModel = new(StaticMapManager.Object, ToursManager.Object);
        }

        [Test]
        public void AddCommandCreatesTourOnValidInput()
        {
            //arrange        
            const string tourName = "Harry";
            const string description = "das ist eine desription";
            RouteInternal route = new("there", "here", 6.4, RouteTypeEnum.fastest, 7000);
            const string imgPath = "ImageIsHere";                    
            TourInternal tour = new(Guid.NewGuid(), tourName, description, route, DateOnly.FromDateTime(DateTime.Now), imgPath);
            
            StaticMapManager.Setup(m => m.CreateImageForRoute(route)).Returns(Task.Run(() => { return imgPath; }));
            ToursManager.Setup(m => m.AddTour(tourName, description, route, imgPath)).Returns(tour);

            AddTourViewModel.Name = tourName;
            AddTourViewModel.Description = description;
            AddTourViewModel.Route = route;


            // act
            AddTourViewModel.AddCommand.Execute(null);

            //assert
            StaticMapManager.Verify(m => m.CreateImageForRoute(route), Times.Once);
            ToursManager.Verify(m => m.AddTour(tourName, description, route, imgPath), Times.Once);    
            Assert.That(AddTourViewModel.Item, Is.SameAs(tour));
        }

        static readonly object[] AddCommandInvalidInputTestCases = {               
            new object?[] 
            {                      
                "",            
                "desription",          
                new RouteInternal("there", "here", 6.4, RouteTypeEnum.fastest, 7000)        
            }, 
            new object?[] 
            {                      
                null,            
                "desription",          
                new RouteInternal("there", "here", 6.4, RouteTypeEnum.fastest, 7000)        
            },
            new object[]
            {
                "name",
                "",
                new RouteInternal("there", "here", 6.4, RouteTypeEnum.fastest, 7000)
            },        
            new object?[]
            {
                "name",
                null,
                new RouteInternal("there", "here", 6.4, RouteTypeEnum.fastest, 7000)
            },
            new object?[]
            {
                "name",
                "desription",
                null
            }};


        [TestCaseSource(nameof(AddCommandInvalidInputTestCases))]
        [Test]
        public void AddCommandCreatesNoTourOnInvaildInput(string? tourName, string? description, RouteInternal? route)
        {
            //arrange                    
            AddTourViewModel.Name = tourName;
            AddTourViewModel.Description = description;
            AddTourViewModel.Route = route;

            // act
            AddTourViewModel.AddCommand.Execute(null);

            //assert
            StaticMapManager.Verify(m => m.CreateImageForRoute(It.IsAny<RouteInternal>()), Times.Never);
            ToursManager.Verify(m => m.AddTour(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<RouteInternal>(), It.IsAny<string>()), Times.Never);    
            Assert.That(AddTourViewModel.Item, Is.Null);
        }
    }
}
