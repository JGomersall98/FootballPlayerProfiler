using MatchMasterWEB.Database;
using MatchMasterWEB.Database.DB_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaster_UnitTest.In_DepthControllerServiceTests
{
	[TestClass]
	public class In_DepthControllerService_MockDatabase
	{
		private static SimulateMockDatabaseForUnitTests? _mockDatabase;

		public MatchMasterMySqlDatabaseContext GetMockDatabase()
		{
			return _mockDatabase!.GetContext();
		}
		public In_DepthControllerService_MockDatabase()
		{
			_mockDatabase = new SimulateMockDatabaseForUnitTests();

			var testFixture = new Fixture
			{
				FixtureId = 1,
				PlaceId = "TestPlace",
				Date = new DateTime(2021, 1, 1),
				Time = new TimeSpan(12, 0, 0),
				Temperature = 20.0,
				PlayerStats = new List<PlayerStat>
				{
					new PlayerStat
					{
						StatId = 1,
						FixtureId = 1,
						Rating = 5.0,
						PassingAccuracy = 80,
						TotalTackles = 5,
						FoulsCommitted = 3,
						DuelsTotal = 10,
						DuelsWon = 5,
						DribblingAttempts = 10,
						DribblingSuccess = 5,
						TotalShots = 5,
						ShotsOnTarget = 3,
						Player = new Player
						{
							PlayerId = 1,
							Name = "TestPlayer",
							Photo = "TestPhoto",
							Position = "F",
						}
					}
				}
			};
			_mockDatabase.Add(testFixture);

		}
	}
}
