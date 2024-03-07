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

		// ----------------------------- GetPerformanceMetric Tests -----------------------------

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

		// ----------------------------- GetStaticPerformanceMetric Tests -----------------------------
	}
}
