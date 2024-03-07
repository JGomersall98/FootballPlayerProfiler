using MatchMasterWEB.ControllerServices.GetRatingMethods;
using MatchMasterWEB.Database;
using MatchMasterWEB.Database.DB_Models;
using MatchMasterWEB.DTO_Models.SquadOverview;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace MatchMasterWEB.ControllerServices
{
    public class SquadOverviewControllerService
    {
		public DTO_GetSquadOverviewPlayers GetSquadOverview(string position, MatchMasterMySqlDatabaseContext dbContext)
        {
            //position to uppercase
            position = position.ToUpper();
            // if position is not D,M,F then return error with message
            if (position != "D" && position != "M" && position != "F")
            {
				throw new ArgumentException("Invalid position.");
			}

            var players = GetPlayersWithStats(position, dbContext);

			var squadOverviewPlayers = new List<DTO_SquadOverviewPlayer>();

            GetPlayerAdaptabilityPercentage? _getPlayerAdaptabilityPercentage = new GetPlayerAdaptabilityPercentage();
            GetPlayerAverageRating? _getPlayerAverageRating = new GetPlayerAverageRating();

			foreach (var player in players)
			{
				var dtoPlayer = new DTO_SquadOverviewPlayer
				{
					PlayerId = player.PlayerId,
					PlayerName = player.Name,
					PlayerPhoto = player.Photo,
					PlayerRating = _getPlayerAverageRating.GetAverageRating(player, dbContext),
					AdaptabilityPercentage = _getPlayerAdaptabilityPercentage.GetAdaptabilityRating(player, dbContext)
				};

				// Handle null ratings or adaptability percentages
				if (dtoPlayer.PlayerRating?.PlayerRating > 0)
				{
					squadOverviewPlayers.Add(dtoPlayer);
				}
			}

			return new DTO_GetSquadOverviewPlayers
            {
                Players = squadOverviewPlayers.ToArray()
            };
        }

        private IEnumerable<Player> GetPlayersWithStats(string position, MatchMasterMySqlDatabaseContext dbContext)
        {
            return dbContext.Players
                .Include(p => p.PlayerStats)
                .Where(p => p.Position == position)
                .OrderByDescending(p => p.PlayerStats.Count)
                .ToList();
        }
    }
}
