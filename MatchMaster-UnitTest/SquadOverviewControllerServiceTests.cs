using MatchMasterWEB.ControllerServices;
using MatchMasterWEB.Database;
using MatchMasterWEB.Database.DB_Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaster_UnitTest
{
	[TestClass]
	public class SquadOverviewControllerServiceTests
	{
		private MatchMasterMySqlDatabaseContext _mockDatabase;
		[TestInitialize]
		public void Initialize()
		{
			MockDatabase mockDatabase = new MockDatabase();
			_mockDatabase = mockDatabase.GetMockDatabase();
		}

		[TestMethod]
		public void GetSquadOverview_InvalidPosition_ThrowsArgumentException()
		{
			// Arrange
			var service = new SquadOverviewControllerService();
			Exception caughtException = null;

			// Act
			try
			{
				service.GetSquadOverview("InvalidPosition", null);
			}
			catch (Exception ex)
			{
				caughtException = ex;
			}

			// Assert
			Assert.IsNotNull(caughtException, "Expected an ArgumentException for invalid positions.");
			Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "Expected exception type to be ArgumentException.");
		}
		[TestMethod]
		public void GetSquadOverview_ValidPosition_ReturnsList()
		{
			// Arrange
			var service = new SquadOverviewControllerService();
			var position = "F";

			// Act
			var result = service.GetSquadOverview(position, _mockDatabase);

			// Assert
			Assert.IsNotNull(result, "Expected a list of players.");
			Assert.IsTrue(result.Players.Length > 0, "Expected a list of players.");
			Assert.AreEqual(result.Players[0].PlayerName, "TestPlayer");
		}
		
	}
	
}
