using MatchMasterWEB.ControllerServices.GetRatingMethods;
using MatchMasterWEB.Database;
using MatchMasterWEB.Database.DB_Models;
using MatchMasterWEB.DTO_Models.SquadOverview;
using Microsoft.EntityFrameworkCore;

namespace MatchMasterWEB.ControllerServices.In_DepthControllerServices
{
	public class PlayerCardControllerService
	{
		public DTO_SquadOverviewPlayer GetPlayerCard(int playerId, MatchMasterMySqlDatabaseContext dbContext)
		{
			//Get player from database from playerId
			Player? player = dbContext.Players
				.Include(p => p.PlayerStats)
				.Where(p => p.PlayerId == playerId)
				.FirstOrDefault();

			//If player is null return empty DTO
			if (player == null)
				return new DTO_SquadOverviewPlayer { };

			GetPlayerAdaptabilityPercentage? _getPlayerAdaptabilityPercentage = new GetPlayerAdaptabilityPercentage();
			GetPlayerAverageRating? _getPlayerAverageRating = new GetPlayerAverageRating();

			return new DTO_SquadOverviewPlayer
			{
				PlayerId = player.PlayerId,
				PlayerName = player.Name,
				PlayerPhoto = player.Photo,
				PlayerRating = _getPlayerAverageRating.GetAverageRating(player, dbContext),
				AdaptabilityPercentage = _getPlayerAdaptabilityPercentage.GetAdaptabilityRating(player, dbContext)
			};

		}
	}
}
