using MatchMasterWEB.Database;
using MatchMasterWEB.Database.DB_Models;
using MatchMasterWEB.DTO_Models.SquadOverview;
using Microsoft.EntityFrameworkCore;

namespace MatchMasterWEB.ControllerServices
{
	public class SquadOverviewControllerService
	{
		public DTO_GetSquadOverviewPlayers GetSquadOverview(string position, MatchMasterMySqlDatabaseContext dbContext)
		{
			// Get all Players from the database where players.position == position including player stats and fixture
			Player[] players = dbContext.Players.Include(p => p.PlayerStats).Where(p => p.Position == position).ToArray();
			
			// Order Players Array By Amount of PlayerStats
			System.Array.Sort(players, (y, x) => x.PlayerStats.Count.CompareTo(y.PlayerStats.Count));

			// Create a new DTO_GetSquadOverviewPlayers
			DTO_GetSquadOverviewPlayers dTO_GetSquadOverviewPlayers = new DTO_GetSquadOverviewPlayers();

			List<DTO_SquadOverviewPlayer> dTO_SquadOverviewPlayerList = new List<DTO_SquadOverviewPlayer>();

			// Build DTO_GetSquadOverviewPlayers
			foreach (Player player in players)
			{
				double playerRating = GetAverageRating(player);
				if (playerRating != 0)
				{
					// Create New DTO_SquadOverviewPlayer
					DTO_SquadOverviewPlayer dTO_SquadOverviewPlayer = new DTO_SquadOverviewPlayer();
					{
						dTO_SquadOverviewPlayer.PlayerId = player.PlayerId;
						dTO_SquadOverviewPlayer.PlayerName = player.Name;
						dTO_SquadOverviewPlayer.PlayerPhoto = player.Photo;
						dTO_SquadOverviewPlayer.PlayerRating = playerRating;
						dTO_SquadOverviewPlayer.AdaptabilityPercentage = GetAdaptabilityRating(player, dbContext);
					}
					// Add DTO_SquadOverviewPlayer to DTO_SquadOverviewPlayerList
					dTO_SquadOverviewPlayerList.Add(dTO_SquadOverviewPlayer);
				}
			}
			// Create Array of DTO_SquadOverviewPlayer count of dTO_SquadOverviewPlayerList
			dTO_GetSquadOverviewPlayers.Players = dTO_SquadOverviewPlayerList.ToArray();

			// Return DTO_GetSquadOverviewPlayers
			return dTO_GetSquadOverviewPlayers;
		}
		public double GetAverageRating(Player player)
		{
			// Create a variable to store the total rating
			double totalRating = 0;
			// Loop through each player stat not counting if rating is 0
			foreach (PlayerStat playerStat in player.PlayerStats)
			{
				if (playerStat.Rating != 0)
				{
					totalRating += playerStat.Rating;
				}
			}
			// Return the average rating rounded to 2 decimal places
			return System.Math.Round(totalRating / player.PlayerStats.Count, 2);
		}
		public int GetAdaptabilityRating(Player player, MatchMasterMySqlDatabaseContext dbContext)
		{
			// Create a variable to store the total adaptability
			int totalAdaptability = 0;
			double highestTemperatureRating = 0;
			double lowestTemperatureRating = 0;
			// Find Fixture That the player has played in with the lowest and highest temperature, find players ratings for each fixtures
			foreach (PlayerStat playerStat in player.PlayerStats)
			{
				Fixture fixture = dbContext.Fixtures.FirstOrDefault(f => f.FixtureId == playerStat.FixtureId);
				if (fixture != null)
				{
					if (fixture.Temperature > highestTemperatureRating)
					{
						highestTemperatureRating = fixture.Temperature;
					}
					if (fixture.Temperature < lowestTemperatureRating)
					{
						lowestTemperatureRating = fixture.Temperature;
					}
				}
			}
			// Divide lowest temperature by highest temperature and multiply by 100 to get percentage
			totalAdaptability = (int)System.Math.Round((lowestTemperatureRating / highestTemperatureRating) * 100);
			// Return the total adaptability
			return totalAdaptability;
		}
	}
}
