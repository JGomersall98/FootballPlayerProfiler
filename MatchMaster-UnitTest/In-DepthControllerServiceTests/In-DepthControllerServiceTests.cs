using MatchMaster_UnitTest.SquadOverviewControllerServiceTests;
using MatchMasterWEB.ControllerServices.In_DepthControllerServices;
using MatchMasterWEB.Database;
using MatchMasterWEB.DTO_Models.In_DepthAnalysis;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaster_UnitTest.In_DepthControllerServiceTests
{
	[TestClass]
	public class In_DepthControllerServiceTests
	{
		private MatchMasterMySqlDatabaseContext? _mockDatabase;
		[TestInitialize]
		public void Initialize()
		{
			In_DepthControllerService_MockDatabase mockDatabase = new In_DepthControllerService_MockDatabase();
			_mockDatabase = mockDatabase.GetMockDatabase();
		}

		// ----------------------------- GetInteractiveMetricControllerService Tests -----------------------------

		[TestMethod]
		public void GetPerformanceMetrics_UnknownPlayerId_ReturnsNull()
		{
			// Arrange
			var service = new GetInteractiveMetricControllerService();

			// Act
			int playerId = -0; int lowTemp = 0; int highTemp = 0;
			var result = service.GetPerformanceMetrics(playerId, lowTemp, highTemp, _mockDatabase!);

			// Assert		
			Assert.IsNotNull(result);
		}
		[TestMethod]
		public void GetPerformanceMetrics_FiltersByTemperatureRange_IncludesCorrectStats()
		{
			// Arrange
			var service = new GetInteractiveMetricControllerService();
			int playerId = 1;
			int lowTemp = 15;
			int highTemp = 25;

			// Act
			var result = service.GetPerformanceMetrics(playerId, lowTemp, highTemp, _mockDatabase!);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(40, result.InteractiveMetric!.DefendingRating);
			Assert.AreEqual(50, result.InteractiveMetric!.DribblingRating);
			Assert.AreEqual(50, result.InteractiveMetric!.DuelsRating);
			Assert.AreEqual(80, result.InteractiveMetric!.PassingRating);
			Assert.AreEqual(60 ,result.InteractiveMetric!.ShootingRating);
		}
		[TestMethod]
		public void GetPerformanceMetrics_InvalidTemperatureRange_ThrowsException()
		{
			// Arrange
			var service = new GetInteractiveMetricControllerService();
			int playerId = 1;
			int lowTemp = 25;
			int highTemp = 15;
			Exception? caughtException = null;

			// Act
			try
			{
				var result = service.GetPerformanceMetrics(playerId, lowTemp, highTemp, _mockDatabase!);
			}
			catch (Exception ex)
			{
				caughtException = ex;
			}

			// Assert
			Assert.IsInstanceOfType(caughtException, typeof(ArgumentException));
		}

		// ----------------------------- GetStaticMetricControllerService Tests -----------------------------
		[TestMethod]
		public void GetStaticPerformanceMetric_UnknownPlayerId_ReturnsNull()
		{
			// Arrange
			var service = new GetStaticMetricControllerService();

			// Act
			int playerId = -0;
			var result = service.GetStaticPerformanceMetric(playerId, _mockDatabase!);

			// Assert		
			Assert.IsNotNull(result);
		}
		[TestMethod]
		public void GetStaticPerformanceMetric_CorrectCalculation_ReturnsExpectedMetrics()
		{
			// Arrange
			var service = new GetStaticMetricControllerService();
			int playerId = 1;

			// Expected metrics		
			int expectedDribblingRating = 50;
			int expectedDuelsRating = 50;
			int expectedDefendingRating = 40;
			int expectedPassingRating = 80;
			int expectedShootingRating = 60; 



			// Act
			var result = service.GetStaticPerformanceMetric(playerId, _mockDatabase!);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(expectedDribblingRating, result.DribblingRating);
			Assert.AreEqual(expectedDuelsRating, result.DuelsRating);
			Assert.AreEqual(expectedDefendingRating, result.DefendingRating);
			Assert.AreEqual(expectedPassingRating, result.PassingRating);
			Assert.AreEqual(expectedShootingRating, result.ShootingRating);		
		}
		[TestMethod]
		public void GetStaticPerformanceMetric_NoRatedFixtures_ReturnsZeroMetrics()
		{
			// Arrange
			var service = new GetStaticMetricControllerService();
			int playerId = 2;

			// Act
			var result = service.GetStaticPerformanceMetric(playerId, _mockDatabase!);

			// Assert
			Assert.IsNotNull(result);			
			Assert.AreEqual(0, result.DefendingRating);
			Assert.AreEqual(0, result.DribblingRating);
			Assert.AreEqual(0, result.DuelsRating);
			Assert.AreEqual(0, result.PassingRating);
			Assert.AreEqual(0, result.ShootingRating);
		}

		// ----------------------------- PlayerCardControllerService Tests -----------------------------
		[TestMethod]
		public void GetPlayerCard_UnknownPlayerId_ReturnsException()
		{
			// Arrange
			var service = new PlayerCardControllerService();
			Exception? caughtException = null;

			// Act
			int playerId = -0;
			try
			{
				var result = service.GetPlayerCard(playerId, _mockDatabase!);
			}
			catch (Exception ex)
			{
				caughtException = ex;
			}

			// Assert
			Assert.IsInstanceOfType(caughtException, typeof(ArgumentException));
			Assert.AreEqual("Player not found", caughtException!.Message);
		}
		[TestMethod]
		public void GetPlayerCard_ValidPlayerId_ReturnsExpectedPlayerDetails()
		{
			// Arrange
			var service = new PlayerCardControllerService();
			int validPlayerId = 1;

			// Act
			var result = service.GetPlayerCard(validPlayerId, _mockDatabase!);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual("TestPlayer", result.PlayerName);
			Assert.AreEqual("TestPhoto", result.PlayerPhoto);
		}
		[TestMethod]
		public void GetPlayerCard_PlayerWithNoStats_ReturnsDefaultRatingAndAdaptability()
		{
			// Arrange
			var service = new PlayerCardControllerService();
			int playerIdWithNoStats = 2;

			// Act
			var result = service.GetPlayerCard(playerIdWithNoStats, _mockDatabase!);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual("#808080", result.PlayerRating!.TextColor); 
			Assert.AreEqual(0, result.AdaptabilityPercentage!.AdaptabilityPercentage);
		}
		[TestMethod]
		public void GetPlayerCard_PlayerWithVaryingPerformance_ReturnsCorrectRatings()
		{
			// Arrange
			var service = new PlayerCardControllerService();
			int playerIdWithVaryingStats = 3;

			// Act
			var result = service.GetPlayerCard(playerIdWithVaryingStats, _mockDatabase!);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(7.85, result.PlayerRating!.PlayerRating); 
			Assert.AreEqual("#6BBE00", result.PlayerRating!.TextColor);
			Assert.AreEqual(93, result.AdaptabilityPercentage!.AdaptabilityPercentage); 
			Assert.AreEqual("#008000", result.AdaptabilityPercentage!.TextColor);
		}

		// ----------------------------- RatingByTemperatureControllerService Tests -----------------------------
		[TestMethod]
		public void GetRatingByTemperature_UnknownPlayerId_ReturnsNull()
		{
			// Arrange
			var service = new RatingByTemperatureControllerService();
			var caughtException = new ArgumentException();
			int playerId = -0;

			// Act
			try
			{
				var result = service.GetDegreeRatings(playerId, _mockDatabase!);
			}
			catch (Exception ex)
			{
				caughtException = ex as ArgumentException;
			}			

			// Assert		
			Assert.IsInstanceOfType(caughtException, typeof(ArgumentException));
			Assert.AreEqual("Player not found", caughtException!.Message);
		}
		[TestMethod]
		public void GetDegreeRatings_ValidPlayerId_CorrectlyCalculatesRatingsAndTextColors()
		{
			// Arrange
			var service = new RatingByTemperatureControllerService();
			int playerId = 3; 

			// Act
			var result = service.GetDegreeRatings(playerId, _mockDatabase!);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(4, result.Length, "Should return ratings for 4 temperature ranges.");

			// Expected ratings and text colors
			Assert.AreEqual(8, result[0].Rating);
			Assert.AreEqual("#FFA500", result[0].TextColor);

			Assert.AreEqual(7.75, result[1].Rating);
			Assert.AreEqual("#6BBE00", result[1].TextColor);

			Assert.AreEqual(8.25, result[2].Rating);
			Assert.AreEqual("#008000", result[2].TextColor);

			Assert.AreEqual(8.25, result[3].Rating);
			Assert.AreEqual("#008000", result[3].TextColor);
		}
		[TestMethod]
		public void GetDegreeRatings_NoStatsInTemperatureRange_ReturnsDefaultValues()
		{
			// Arrange
			var service = new RatingByTemperatureControllerService();
			int playerId = 2;

			// Act
			var result = service.GetDegreeRatings(playerId, _mockDatabase!);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(4, result.Length, "Should return ratings for 4 temperature ranges.");

			Assert.AreEqual(0, result[0].Rating);
			Assert.AreEqual("#FF0000", result[0].TextColor);

			Assert.AreEqual(0, result[1].Rating);
			Assert.AreEqual("#FF0000", result[1].TextColor);

			Assert.AreEqual(0, result[2].Rating);
			Assert.AreEqual("#FF0000", result[2].TextColor);

			Assert.AreEqual(0, result[3].Rating);
			Assert.AreEqual("#FF0000", result[3].TextColor);
		}
		[TestMethod]
		public void GetDegreeRatings_ValidPlayerId_RatingsAreRoundedCorrectly()
		{
			// Arrange
			var service = new RatingByTemperatureControllerService();
			int playerId = 3;

			// Act
			var degreeRatings = service.GetDegreeRatings(playerId, _mockDatabase!);

			// Assert
			// Verify that each rating is rounded to 2 decimal places
			foreach (var rating in degreeRatings)
			{
				double roundedRating = Math.Round(rating.Rating, 2);
				Assert.AreEqual(roundedRating, rating.Rating);
			}
		}
	}
}
