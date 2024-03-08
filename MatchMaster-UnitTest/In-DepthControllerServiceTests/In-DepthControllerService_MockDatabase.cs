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

			Player player0RatedFixture = new Player
			{
				PlayerId = 2,
				Name = "TestPlayer",
				Photo = "TestPhoto",
				Position = "F",
				PlayerStats = new List<PlayerStat>
				{
					new PlayerStat
					{
						StatId = 2,
						FixtureId = 1,
						Rating = 0.0,
						PassingAccuracy = 80,
						TotalTackles = 5,
						FoulsCommitted = 3,
						DuelsTotal = 10,
						DuelsWon = 5,
						DribblingAttempts = 10,
						DribblingSuccess = 5,
						TotalShots = 5,
						ShotsOnTarget = 3
					}
				}
			};
			_mockDatabase.Add(player0RatedFixture);

			//Fixture 2
			var testFixture2 = new Fixture
			{
				FixtureId = 2,
				PlaceId = "TestPlace",
				Date = new DateTime(2021, 1, 1),
				Time = new TimeSpan(12, 0, 0),
				Temperature = 15.0
			};
			_mockDatabase.Add(testFixture2);
			//Fixture 3
			var testFixture3 = new Fixture
			{
				FixtureId = 3,
				PlaceId = "TestPlace",
				Date = new DateTime(2021, 1, 1),
				Time = new TimeSpan(12, 0, 0),
				Temperature = 10.0
			};
			_mockDatabase.Add(testFixture3);
			//Fixture 4
			var testFixture4 = new Fixture
			{
				FixtureId = 4,
				PlaceId = "TestPlace",
				Date = new DateTime(2021, 1, 1),
				Time = new TimeSpan(12, 0, 0),
				Temperature = 5.0
			};
			_mockDatabase.Add(testFixture4);
			//Fixture 5
			var testFixture5 = new Fixture
			{
				FixtureId = 5,
				PlaceId = "TestPlace",
				Date = new DateTime(2021, 1, 1),
				Time = new TimeSpan(12, 0, 0),
				Temperature = 0.0
			};
			_mockDatabase.Add(testFixture5);
			//Fixture 6
			var testFixture6 = new Fixture
			{
				FixtureId = 6,
				PlaceId = "TestPlace",
				Date = new DateTime(2021, 1, 1),
				Time = new TimeSpan(12, 0, 0),
				Temperature = -5.0
			};
			_mockDatabase.Add(testFixture6);
			//Fixture 7
			var testFixture7 = new Fixture
			{
				FixtureId = 7,
				PlaceId = "TestPlace",
				Date = new DateTime(2021, 1, 1),
				Time = new TimeSpan(12, 0, 0),
				Temperature = 10.0
			};
			_mockDatabase.Add(testFixture7);
			//Fixture 8
			var testFixture8 = new Fixture
			{
				FixtureId = 8,
				PlaceId = "TestPlace",
				Date = new DateTime(2021, 1, 1),
				Time = new TimeSpan(12, 0, 0),
				Temperature = 5.0
			};
			_mockDatabase.Add(testFixture8);
			//Fixture 9
			var testFixture9 = new Fixture
			{
				FixtureId = 9,
				PlaceId = "TestPlace",
				Date = new DateTime(2021, 1, 1),
				Time = new TimeSpan(12, 0, 0),
				Temperature = 13.0
			};
			_mockDatabase.Add(testFixture9);
			//Fixture 10
			var testFixture10 = new Fixture
			{
				FixtureId = 10,
				PlaceId = "TestPlace",
				Date = new DateTime(2021, 1, 1),
				Time = new TimeSpan(12, 0, 0),
				Temperature = 25.0
			};
			_mockDatabase.Add(testFixture10);

			var playerStatsAccuracyPlayer = new Player
			{
				PlayerId = 3,
				Name = "AdaptabilityTestPlayer",
				Photo = "TestPhoto",
				Position = "D",
				PlayerStats = new List<PlayerStat>
				{
					new PlayerStat { StatId = 3, FixtureId = 1, Rating = 8.5, PassingAccuracy = 90, TotalTackles = 10, FoulsCommitted = 2, DuelsTotal = 20, DuelsWon = 15, DribblingAttempts = 12, DribblingSuccess = 8, TotalShots = 10, ShotsOnTarget = 7 },
					new PlayerStat { StatId = 4, FixtureId = 2, Rating = 7.0, PassingAccuracy = 0, TotalTackles = 0, FoulsCommitted = 0, DuelsTotal = 20, DuelsWon = 15, DribblingAttempts = 12, DribblingSuccess = 8, TotalShots = 10, ShotsOnTarget = 7 },
					new PlayerStat { StatId = 5, FixtureId = 3, Rating = 9.0, PassingAccuracy = 0, TotalTackles = 0, FoulsCommitted = 0, DuelsTotal = 20, DuelsWon = 15, DribblingAttempts = 12, DribblingSuccess = 8, TotalShots = 10, ShotsOnTarget = 7 },
					new PlayerStat { StatId = 6, FixtureId = 4, Rating = 6.5, PassingAccuracy = 0, TotalTackles = 0, FoulsCommitted = 0, DuelsTotal = 20, DuelsWon = 15, DribblingAttempts = 12, DribblingSuccess = 8, TotalShots = 10, ShotsOnTarget = 7 },
					new PlayerStat { StatId = 7, FixtureId = 5, Rating = 8.0, PassingAccuracy = 0, TotalTackles = 0, FoulsCommitted = 0, DuelsTotal = 20, DuelsWon = 15, DribblingAttempts = 12, DribblingSuccess = 8, TotalShots = 10, ShotsOnTarget = 7 },
					new PlayerStat { StatId = 8, FixtureId = 6, Rating = 8.0, PassingAccuracy = 0, TotalTackles = 0, FoulsCommitted = 0, DuelsTotal = 20, DuelsWon = 15, DribblingAttempts = 12, DribblingSuccess = 8, TotalShots = 10, ShotsOnTarget = 7 },
					new PlayerStat { StatId = 9, FixtureId = 7, Rating = 9.0, PassingAccuracy = 0, TotalTackles = 0, FoulsCommitted = 0, DuelsTotal = 20, DuelsWon = 15, DribblingAttempts = 12, DribblingSuccess = 8, TotalShots = 10, ShotsOnTarget = 7 },
					new PlayerStat { StatId = 10, FixtureId = 8, Rating = 6.5, PassingAccuracy = 0, TotalTackles = 0, FoulsCommitted = 0, DuelsTotal = 20, DuelsWon = 15, DribblingAttempts = 12, DribblingSuccess = 8, TotalShots = 10, ShotsOnTarget = 7 },
					new PlayerStat { StatId = 11, FixtureId = 9, Rating = 8.0, PassingAccuracy = 0, TotalTackles = 0, FoulsCommitted = 0, DuelsTotal = 20, DuelsWon = 15, DribblingAttempts = 12, DribblingSuccess = 8, TotalShots = 10, ShotsOnTarget = 7 },
					new PlayerStat { StatId = 12, FixtureId = 10, Rating = 8.0, PassingAccuracy = 0, TotalTackles = 0, FoulsCommitted = 0, DuelsTotal = 20, DuelsWon = 15, DribblingAttempts = 12, DribblingSuccess = 8, TotalShots = 10, ShotsOnTarget = 7 }
				}
			};
			_mockDatabase.Add(playerStatsAccuracyPlayer);
		}
	}
}
