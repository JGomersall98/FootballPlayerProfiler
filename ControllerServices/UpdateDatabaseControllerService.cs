using MatchMasterWEB.APIObject;
using MatchMasterWEB.Database;
using MatchMasterWEB.Database.DB_Models;
using MatchMasterWEB.ExternalAPICalls.PerformanceMetrics;
using MatchMasterWEB.ExternalAPICalls.Weather;
using Newtonsoft.Json;
using static MatchMasterWEB.APIObject.FixtureDetails;
using static MatchMasterWEB.APIObject.FixturesByLeagueIdObject;

namespace MatchMasterWebAPI.ControllerServices
{
	public class UpdateDatabaseControllerService
	{
		private readonly bool _useDummyData = true;
		private readonly WeatherAPICalls _weatherAPICall = new WeatherAPICalls();

		public async Task<string> UpdateDatabaseAsync(MatchMasterMySqlDatabaseContext dbContext)
		{
			//Get the fixture details
			FixtureDetails fixtureDetails = _useDummyData ? GetDummyData() : await FetchFixtureDetails(dbContext);

			//Check if fixtureDetails is null or empty
			if (fixtureDetails == null || fixtureDetails.fixtureDetails == null || !fixtureDetails.fixtureDetails.Any())
				return "No fixture details available.";

			foreach (var fixture in fixtureDetails.fixtureDetails)
			{
				// Convert fixture ID from string to int
				int fixtureId = int.Parse(fixture.fixtureIds[0]);
				// Check if a fixture with the same ID already exists in the database
				var existingFixture = dbContext.Fixtures.FirstOrDefault(f => f.FixtureId == fixtureId);

				if (existingFixture == null)
				{
					// If the fixture does not exist, proceed to add it
					string placeId = await GetPlaceIdAsync(fixture);
					double temperature = await GetTemperatureAsync(placeId, DateTime.Parse(fixture.dates[0]), fixture.times[0]);
					Fixture newFixture = CreateFixture(fixture, placeId, temperature);
					dbContext.Fixtures.Add(newFixture);
				}
				// Optionally, you can add an else block here to update the existing fixture if needed
			}

			// Save changes to the database
			await dbContext.SaveChangesAsync();

			//Loop Through Each Game For Player Stats
			// Inside UpdateDatabaseAsync method, after adding new fixtures to dbContext
			foreach (var fixture in fixtureDetails.fixtureDetails)
			{
				FootballAPICalls footballAPICalls = new FootballAPICalls();
				string playerStatsJson = await footballAPICalls.GetPlayerStatsByFixtureIdAsync(int.Parse(fixture.fixtureIds[0]));
				// Deserialize JSON response to your player stats model
				var playerStatsResponse = JsonConvert.DeserializeObject<PlayerStatResponse>(playerStatsJson);

				// Iterate through the response to get to the players
				foreach (var teamPlayerStats in playerStatsResponse.Response)
				{
					// Check if the team ID matches Hull City's ID (64)
					if (teamPlayerStats.Team.Id == 64) // Assuming Team object has an Id property
					{
						foreach (var playerStatDetail in teamPlayerStats.Players)
						{
							var playerStat = playerStatDetail.Statistics.FirstOrDefault(); // Assuming you want the first statistic entry
							if (playerStatDetail.Statistics[0].Games.Position != "G")
							{
								// Check if player exists in the database
								var player = dbContext.Players.FirstOrDefault(p => p.PlayerId == playerStatDetail.Player.PlayerId);
								if (player == null)
								{
									// Add new player
									player = new Player
									{
										PlayerId = playerStatDetail.Player.PlayerId,
										Name = playerStatDetail.Player.Name,
										Photo = playerStatDetail.Player.Photo,
										Position = playerStatDetail.Statistics[0].Games.Position
										// Set other player properties as necessary
									};
									dbContext.Players.Add(player);

								}
								else
								{
									// Update player details
									player.PlayerId = playerStatDetail.Player.PlayerId;
									player.Name = playerStatDetail.Player.Name;
									player.Photo = playerStatDetail.Player.Photo;
									player.Position = playerStatDetail.Statistics[0].Games.Position;
								}

								// Assuming FixtureId is an int and you have a way to convert fixture.fixtureIds[0] to int
								int fixtureId = int.Parse(fixture.fixtureIds[0]);

								// Check if player's stats for this fixture already exist
								var existingStat = dbContext.PlayerStats.FirstOrDefault(ps => ps.PlayerId == playerStatDetail.Player.PlayerId && ps.FixtureId == fixtureId);
								if (existingStat == null)
								{
									// Add new player stat
									var newStat = new PlayerStat
									{
										FixtureId = fixtureId,
										PlayerId = playerStatDetail.Player.PlayerId,
										Rating = playerStat?.Games?.Rating ?? 0,
										PassingAccuracy = playerStat?.Passes?.PassingAccuracy ?? 0,
										TotalTackles = playerStat?.Tackles?.TotalTackles ?? 0,
										FoulsCommitted = playerStat?.Fouls?.FoulsCommitted ?? 0,
										DuelsTotal = playerStat?.Duels?.DuelsTotal ?? 0,
										DuelsWon = playerStat?.Duels?.DuelsWon ?? 0,
										DribblingAttempts = playerStat?.Dribbles?.DribblingAttempts ?? 0,
										DribblingSuccess = playerStat?.Dribbles?.DribblingSuccess ?? 0,
										TotalShots = playerStat?.Shots?.TotalShots ?? 0,
										ShotsOnTarget = playerStat?.Shots?.ShotsOnTarget ?? 0
									};
									// Add new player stat to dbContext
									dbContext.PlayerStats.Add(newStat);
									dbContext.SaveChanges();
								}
							}
						}
					}
				}
			}
			//Save changes to the database
			await dbContext.SaveChangesAsync();
			return "Success";
		}

		private async Task<FixtureDetails> FetchFixtureDetails(MatchMasterMySqlDatabaseContext dbContext)
		{
			FootballAPICalls footballAPICalls = new FootballAPICalls();
			FixtureResponse fixtureIdResponse = await footballAPICalls.GetFixtureIdAsync();
			List<int> fixtureIds = dbContext.Fixtures.Select(f => f.FixtureId).ToList();
			return footballAPICalls.GetFixtureDetails(fixtureIdResponse, fixtureIds);
		}

		private async Task<string> GetPlaceIdAsync(Fixtures fixture)
		{
			string placeId = await _weatherAPICall.GetPlaceIdAPICall(fixture.city[0]);
			return string.IsNullOrEmpty(placeId) ? "sheffield" : placeId;
		}

		private async Task<double> GetTemperatureAsync(string placeId, DateTime date, string time)
		{
			return await _weatherAPICall.GetTemperatureAsync(placeId, date, time);
		}

		private FixtureDetails GetDummyData()
		{
			string dummyData = "{\"fixtureDetails\":[{\"fixtureIds\":[\"1047438\"],\"dates\":[\"2023-08-05\"],\"times\":[\"15:00:00\"],\"city\":[\"Norwich, Norfolk\"]},{\"fixtureIds\":[\"1047450\"],\"dates\":[\"2023-08-12\"],\"times\":[\"15:00:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047460\"],\"dates\":[\"2023-08-19\"],\"times\":[\"15:00:00\"],\"city\":[\"Blackburn, Lancashire\"]},{\"fixtureIds\":[\"1047470\"],\"dates\":[\"2023-08-25\"],\"times\":[\"19:30:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047486\"],\"dates\":[\"2023-09-02\"],\"times\":[\"15:00:00\"],\"city\":[\"Leicester, Leicestershire\"]},{\"fixtureIds\":[\"1047494\"],\"dates\":[\"2023-09-15\"],\"times\":[\"19:45:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047514\"],\"dates\":[\"2023-09-20\"],\"times\":[\"19:45:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047526\"],\"dates\":[\"2023-09-24\"],\"times\":[\"12:00:00\"],\"city\":[\"Stoke-on-Trent, Staffordshire\"]},{\"fixtureIds\":[\"1047534\"],\"dates\":[\"2023-09-30\"],\"times\":[\"15:00:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047543\"],\"dates\":[\"2023-10-03\"],\"times\":[\"19:45:00\"],\"city\":[\"Ipswich, Suffolk\"]},{\"fixtureIds\":[\"1047559\"],\"dates\":[\"2023-10-07\"],\"times\":[\"15:00:00\"],\"city\":[\"London\"]},{\"fixtureIds\":[\"1047568\"],\"dates\":[\"2023-10-21\"],\"times\":[\"15:00:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047583\"],\"dates\":[\"2023-10-25\"],\"times\":[\"19:45:00\"],\"city\":[\"Birmingham\"]},{\"fixtureIds\":[\"1047593\"],\"dates\":[\"2023-10-28\"],\"times\":[\"15:00:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047612\"],\"dates\":[\"2023-11-04\"],\"times\":[\"15:00:00\"],\"city\":[\"West Bromwich\"]},{\"fixtureIds\":[\"1047616\"],\"dates\":[\"2023-11-11\"],\"times\":[\"15:00:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047635\"],\"dates\":[\"2023-11-25\"],\"times\":[\"15:00:00\"],\"city\":[\"Swansea\"]},{\"fixtureIds\":[\"1047639\"],\"dates\":[\"2023-11-28\"],\"times\":[\"19:45:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047650\"],\"dates\":[\"2023-12-02\"],\"times\":[\"15:00:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047668\"],\"dates\":[\"2023-12-09\"],\"times\":[\"15:00:00\"],\"city\":[\"London\"]},{\"fixtureIds\":[\"1047682\"],\"dates\":[\"2023-12-13\"],\"times\":[\"20:00:00\"],\"city\":[\"Middlesbrough\"]},{\"fixtureIds\":[\"1047687\"],\"dates\":[\"2023-12-16\"],\"times\":[\"15:00:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047697\"],\"dates\":[\"2023-12-22\"],\"times\":[\"19:45:00\"],\"city\":[\"Bristol\"]},{\"fixtureIds\":[\"1047714\"],\"dates\":[\"2023-12-26\"],\"times\":[\"15:00:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047725\"],\"dates\":[\"2023-12-29\"],\"times\":[\"19:45:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047741\"],\"dates\":[\"2024-01-01\"],\"times\":[\"17:15:00\"],\"city\":[\"Sheffield\"]},{\"fixtureIds\":[\"1047750\"],\"dates\":[\"2024-01-12\"],\"times\":[\"20:00:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047767\"],\"dates\":[\"2024-01-19\"],\"times\":[\"20:00:00\"],\"city\":[\"Sunderland\"]},{\"fixtureIds\":[\"1047778\"],\"dates\":[\"2024-02-20\"],\"times\":[\"19:45:00\"],\"city\":[\"Southampton, Hampshire\"]},{\"fixtureIds\":[\"1047783\"],\"dates\":[\"2024-02-03\"],\"times\":[\"15:00:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047795\"],\"dates\":[\"2024-02-10\"],\"times\":[\"15:00:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047808\"],\"dates\":[\"2024-02-13\"],\"times\":[\"19:45:00\"],\"city\":[\"Rotherham, South Yorkshire\"]},{\"fixtureIds\":[\"1047818\"],\"dates\":[\"2024-02-17\"],\"times\":[\"15:00:00\"],\"city\":[\"Huddersfield, West Yorkshire\"]},{\"fixtureIds\":[\"1047831\"],\"dates\":[\"2024-02-24\"],\"times\":[\"12:30:00\"],\"city\":[\"Hull\"]}]}\r\n";
			return JsonConvert.DeserializeObject<FixtureDetails>(dummyData);
		}
		private Fixture CreateFixture(Fixtures fixture, string placeId, double temperature)
		{
			return new Fixture
			{
				FixtureId = int.Parse(fixture.fixtureIds[0]),
				PlaceId = placeId,
				Date = DateTime.Parse(fixture.dates[0]),
				Time = TimeSpan.Parse(fixture.times[0]),
				Temperature = temperature
			};
		}
	}
}
