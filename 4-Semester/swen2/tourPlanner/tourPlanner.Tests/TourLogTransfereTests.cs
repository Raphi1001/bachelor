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
    public class TourLogTransfereTests
    {

        [Test]
        public void TourLogTransfereToTourLogInternalTransformationSuccessfullOnCorrectData()
        {
            //arrange        

            TourLogTransfere tourLogTansfere = new()
            {
                Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                TourId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                TourRating = "good",
                TourDifficulty = "veryEasy",
                CreationDate = new DateOnly(2022, 1, 1),
                TimeTakenS = 420,
                TourComment = "comment",
            };
            TourLogInternal tourLogExpected = new(
                    new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    Rating.good,
                    Difficulty.veryEasy,
                    new DateOnly(2022, 1, 1),
                    420,
                    "comment"
                    );

            // act
            TourLogInternal tourLogActual = tourLogTansfere.ToInternal();

            //assert
            AreEqualByJson(tourLogExpected, tourLogActual);
        }

        [TestCaseSource(nameof(InvalidTourLogsTransfereTestCases))]
        [Test]
        public void TourLogTransfereToTourLogInteralTransformationThrowsExceptionOnIncompleteTourLogTransfere(TourLogTransfere tourLogTansfere)
        {
            //act and assert
            Assert.Throws<InvalidParameterException> (() => tourLogTansfere.ToInternal());
        }

        [Test]
        public void TourLogTransfereToTourLogInteralTransformationThrowsExceptionOnIncorrectRating()
        {
            //arrange
            TourLogTransfere tourLogTansfere = new()
            {
                Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                TourId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                TourRating = "invalid",
                TourDifficulty = "veryEasy",
                CreationDate = new DateOnly(2022, 1, 1),
                TimeTakenS = 420,
                TourComment = "comment",
            };

            //act and assert
            Assert.Throws<InvalidRatingException>(() => tourLogTansfere.ToInternal());
        }       
        
        [Test]
        public void TourLogTransfereToTourLogInteralTransformationThrowsExceptionOnIncorrectDifficulty()
        {
            //arrange
            TourLogTransfere tourLogTansfere = new()
            {
                Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                TourId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                TourRating = "good",
                TourDifficulty = "invalid",
                CreationDate = new DateOnly(2022, 1, 1),
                TimeTakenS = 420,
                TourComment = "comment",
            };

            //act and assert
            Assert.Throws<InvalidDifficultyException>(() => tourLogTansfere.ToInternal());
        }

        private static void AreEqualByJson(object expected, object actual)
        {
            var expectedJson = JsonConvert.SerializeObject(expected);
            var actualJson = JsonConvert.SerializeObject(actual);
            Assert.That(actualJson, Is.EqualTo(expectedJson));
        }

        private static object[] InvalidTourLogsTransfereTestCases = {
            new object[]
            {
                new TourLogTransfere()
                {
                    Id = null,
                    TourId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    TourRating = "good",
                    TourDifficulty = "veryEasy",
                    CreationDate = new DateOnly(2022, 1, 1),
                    TimeTakenS = 420,
                    TourComment = "comment",
                }
            },
            new object[]
            {
                new TourLogTransfere()
                {
                    Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    TourId = null,
                    TourRating = "good",
                    TourDifficulty = "veryEasy",
                    CreationDate = new DateOnly(2022, 1, 1),
                    TimeTakenS = 420,
                    TourComment = "comment",
                }
            },
            new object[]
            {
                new TourLogTransfere()
                {
                    Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    TourId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    TourRating = null,
                    TourDifficulty = "veryEasy",
                    CreationDate = new DateOnly(2022, 1, 1),
                    TimeTakenS = 420,
                    TourComment = "comment",
                }
            },
            new object[]
            {
                new TourLogTransfere()
                {
                    Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    TourId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    TourRating = "good",
                    TourDifficulty = null,
                    CreationDate = null,
                    TimeTakenS = 420,
                    TourComment = "comment",
                }
            },
            new object[]
            {
                new TourLogTransfere()
                {
                    Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    TourId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    TourRating = "good",
                    TourDifficulty = "veryEasy",
                    CreationDate = new DateOnly(2022, 1, 1),
                    TimeTakenS = null,
                    TourComment = "comment",
                }
            },
            new object[]
            {
                new TourLogTransfere()
                {
                    Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    TourId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
                    TourRating = "good",
                    TourDifficulty = "veryEasy",
                    CreationDate = new DateOnly(2022, 1, 1),
                    TimeTakenS = 420,
                    TourComment = null,
                }
            },
        };
    }
}