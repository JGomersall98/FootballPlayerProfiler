using MatchMasterWEB.ControllerServices.GetRatingMethods;
using MatchMasterWEB.Database;
using MatchMasterWEB.Database.DB_Models;
using MatchMasterWEB.DTO_Models.In_DepthAnalysis;
using Microsoft.EntityFrameworkCore;

namespace MatchMasterWEB.ControllerServices.In_DepthControllerServices
{
	public class RatingByTemperatureControllerService
	{
		public DTO_DegreeRating[] GetDegreeRatings(int playerId, MatchMasterMySqlDatabaseContext dbContext)
		{
			//Get player from database from playerId include PlayerStats
			Player? player = dbContext.Players
				.Include(p => p.PlayerStats)
				.Where(p => p.PlayerId == playerId)
				.FirstOrDefault();

			//If player is throw exception
			if (player == null)
				throw new System.ArgumentException("Player not found");

			int[] lowTemps = new int[] { -20, 3, 10, 17 };
			int[] highTemps = new int[] { 3, 10, 17, 50 };

			DTO_DegreeRating[] degreeRatings = new DTO_DegreeRating[4];
			for (int i = 0; i < 4; i++)
			{
				degreeRatings[i] = GetDegreeRating(player, dbContext, lowTemps[i], highTemps[i], i);
			}

			return degreeRatings;
		}
		private DTO_DegreeRating GetDegreeRating(Player player, MatchMasterMySqlDatabaseContext dbContext, int lowTemp, int highTemp, int order)
		{
			TextColourMethod? _textColourMethod = new TextColourMethod();

			//Get PlayerStats from database where player.playerstats.fixture.temperature is between lowTemp and highTemp
			PlayerStat[] playerStats = dbContext.PlayerStats
				.Include(ps => ps.Fixture)
				.Where(ps => ps.PlayerId == player.PlayerId && ps.Fixture.Temperature >= lowTemp && ps.Fixture.Temperature <= highTemp)
				.ToArray();

			//If playerStats is null return empty DTO
			if (playerStats == null)
				return new DTO_DegreeRating 
				{
					Order = order,
					Rating = 0,
					TextColor = "#808080"
				};

			// Get all player ratings from playerStats
			double totalRating = 0;
			int count = 0;
			foreach (var playerStat in playerStats)
			{
				if (playerStat.Rating > 0)
				{
					totalRating += playerStat.Rating;
					count++;
				}
			}
			double averageRating = count > 0 ? totalRating / count : 0;

			//Round AverageRating to 2 decimal places
			averageRating = System.Math.Round(averageRating, 2);

			//Return DTO with temperature range and average rating
			return new DTO_DegreeRating
			{
				Order = order,
				Rating = averageRating,
				TextColor = _textColourMethod.GetTextColour(averageRating)
			};
		}
	}
}
