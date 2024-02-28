using MatchMasterWEB.Database.DB_Models;
using MatchMasterWEB.Database;
using MatchMasterWEB.DTO_Models.SquadOverview;
using Microsoft.EntityFrameworkCore;

namespace MatchMasterWEB.ControllerServices.GetRatingMethods
{
	public class GetPlayerAdaptabilityPercentage
	{
		public DTO_AdaptabilityRating GetAdaptabilityRating(Player player, MatchMasterMySqlDatabaseContext dbContext)
		{
			TextColourMethod? _textColourMethod = new TextColourMethod();
			GetAverageRatingFromTemp? _getAverageRatingFromTemp = new GetAverageRatingFromTemp();

			PlayerStat[] playerStats = dbContext.PlayerStats
				.Include(ps => ps.Fixture)
				.Where(ps => ps.PlayerId == player.PlayerId)
				.ToArray();

			int ratedFixtures = playerStats.Count(ps => ps.Rating > 0);
			if (ratedFixtures < 10)
			{
				return new DTO_AdaptabilityRating { TextColor = "#808080" };
			}

			var highTempFixtures = playerStats.OrderByDescending(ps => ps.Fixture.Temperature).Take(5).Select(ps => ps.Fixture);
			var lowTempFixtures = playerStats.OrderBy(ps => ps.Fixture.Temperature).Take(5).Select(ps => ps.Fixture);

			//Check _getAverageRatingFromTemp is not null
			if (_getAverageRatingFromTemp == null)
			{
				return new DTO_AdaptabilityRating { AdaptabilityPercentage = 0, TextColor = "#808080" };
			}

			double highTempAverageRating = _getAverageRatingFromTemp.CalculateAverageRatingFromTemp(highTempFixtures, playerStats);
			double lowTempAverageRating = _getAverageRatingFromTemp.CalculateAverageRatingFromTemp(lowTempFixtures, playerStats);

			double adaptabilityRating = (lowTempAverageRating < highTempAverageRating ? lowTempAverageRating / highTempAverageRating : highTempAverageRating / lowTempAverageRating) * 100;
			int adaptabilityRatingInt = (int)adaptabilityRating;
			string? textColour = _textColourMethod?.GetTextColour(adaptabilityRating / 10);

			return new DTO_AdaptabilityRating { AdaptabilityPercentage = adaptabilityRatingInt, TextColor = textColour };
		}

	}

}
