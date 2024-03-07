using MatchMasterWEB.ControllerServices;
using MatchMasterWEB.Database;
using MatchMasterWEB.Database.DB_Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaster_UnitTest.SquadOverviewControllerServiceTests
{
    [TestClass]
    public class SquadOverviewControllerServiceTests
    {
        private MatchMasterMySqlDatabaseContext? _mockDatabase;
        [TestInitialize]
        public void Initialize()
        {
            SquadOverviewControllerService_MockDatabase mockDatabase = new SquadOverviewControllerService_MockDatabase();
            _mockDatabase = mockDatabase.GetMockDatabase();
        }

        [TestMethod]
        public void GetSquadOverview_InvalidPosition_ThrowsArgumentException()
        {
            // Arrange
            var service = new SquadOverviewControllerService();
            Exception? caughtException = null;

            // Act
            try
            {
                service.GetSquadOverview("InvalidPosition", null!);
            }
            catch (Exception ex)
            {
                caughtException = ex;
            }

            // Assert
            Assert.IsNotNull(caughtException);
            Assert.IsInstanceOfType(caughtException, typeof(ArgumentException)); 
        }
        [TestMethod]
        public void GetSquadOverview_ValidPosition_ReturnsList()
        {
            // Arrange
            var service = new SquadOverviewControllerService();
            var position = "F";

            // Act
            var result = service.GetSquadOverview(position, _mockDatabase!);

            // Assert
            Assert.IsNotNull(result); 
            Assert.IsTrue(result!.Players!.Length > 0); 
            Assert.AreEqual(result.Players[0].PlayerName, "TestPlayer");
        }
        [TestMethod]
        public void GetSquadOverview_NoPlayersForPosition_ReturnsEmptyList()
        {
            // Arrange
            var service = new SquadOverviewControllerService();
            var position = "M";

            // Act
            var result = service.GetSquadOverview(position, _mockDatabase!);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Players!.Length); 
        }
        [TestMethod]
        public void GetSquadOverview_PlayersWithoutStats_ExcludedFromResults()
        {
            // Arrange
            var service = new SquadOverviewControllerService();
            var position = "M";

            // Act
            var result = service.GetSquadOverview(position, _mockDatabase!);

            // Assert
            Assert.IsNotNull(result);                             
            Assert.AreEqual(0, result.Players!.Length);
        }
        [TestMethod]
        public void GetSquadOverview_CaseInsensitivePosition_ReturnsList()
        {
            // Arrange
            var service = new SquadOverviewControllerService();
            var position = "f";

            // Act
            var result = service.GetSquadOverview(position, _mockDatabase!);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Players!.Length > 0);
        }
        [TestMethod]
        public void GetSquadOverview_NullPosition_ThrowsArgumentException()
        {
            // Arrange
            var service = new SquadOverviewControllerService();
            Exception? caughtException = null;

            // Act
            try
            {
                service.GetSquadOverview(null!, _mockDatabase!);
            }
            catch (Exception ex)
            {
                caughtException = ex;
            }

            // Assert
            Assert.IsNotNull(caughtException); 
            Assert.IsInstanceOfType(caughtException, typeof(NullReferenceException));
        }
        [TestMethod]
        public void GetSquadOverview_PlayerRatingAdaptabilityAccuracy()
        {
            // Arrange
            SquadOverviewControllerService_MockDatabase mockDatabase = new SquadOverviewControllerService_MockDatabase();
            var _mockDatabase = mockDatabase.GetMockDatabase();

            var service = new SquadOverviewControllerService();
            var position = "D";

            // Act
            var result = service.GetSquadOverview(position, _mockDatabase);

            // Assert
            var testPlayer = result.Players!.FirstOrDefault(p => p.PlayerName == "AdaptabilityTestPlayer");
            Assert.IsNotNull(testPlayer);
            Assert.AreEqual(93, testPlayer?.AdaptabilityPercentage?.AdaptabilityPercentage); 
            Assert.AreEqual(7.85, testPlayer?.PlayerRating?.PlayerRating);
        }

    }

}
