using MatchMasterWEB.Database.DB_Models;
using MatchMasterWEB.Database;
using MatchMasterWEB.DTO_Models.SquadOverview;

namespace MatchMasterWEB.ControllerServices.GetRatingMethods
{
	public class GetPlayerAverageRating
	{
		public DTO_PlayerRating GetAverageRating(Player player, MatchMasterMySqlDatabaseContext dbContext)
		{

			TextColourMethod? _textColourMethod = new TextColourMethod();

			List<double> validRatings = player.PlayerStats
				.Where(ps => ps.Rating > 0)
				.Select(ps => ps.Rating)
				.ToList();

			if (validRatings.Any())
			{
				double averageRating = Math.Round(validRatings.Average(), 2);
				string? textColor = _textColourMethod?.GetTextColour(averageRating);
				return new DTO_PlayerRating { PlayerRating = averageRating, TextColor = textColor };
			}
			else
			{
				return new DTO_PlayerRating { TextColor = "#808080" };
			}
		}
	}
}
