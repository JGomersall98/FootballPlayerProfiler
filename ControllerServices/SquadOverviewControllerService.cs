using MatchMasterWEB.Database;
using MatchMasterWEB.Database.DB_Models;
using MatchMasterWEB.DTO_Models.SquadOverview;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

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

            List<DTO_SquadOverviewPlayer> squadOverviewPlayers = players
                .Select(player => new DTO_SquadOverviewPlayer
                {
                    PlayerId = player.PlayerId,
                    PlayerName = player.Name,
                    PlayerPhoto = player.Photo,
                    PlayerRating = GetAverageRating(player, dbContext),
                    AdaptabilityPercentage = GetAdaptabilityRating(player, dbContext)
                })
                .Where(player => player?.PlayerRating?.PlayerRating > 0)
                .ToList();

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

        private DTO_PlayerRating GetAverageRating(Player player, MatchMasterMySqlDatabaseContext dbContext)
        {
            List<double> validRatings = player.PlayerStats
                .Where(ps => ps.Rating > 0)
                .Select(ps => ps.Rating)
                .ToList();

            if (validRatings.Any())
            {
                double averageRating = Math.Round(validRatings.Average(), 2);
                string textColor = GetTextColour(averageRating);
                return new DTO_PlayerRating { PlayerRating = averageRating, TextColor = textColor };
            }
            else
            {
                return new DTO_PlayerRating { TextColor = "#808080" };
            }
        }

        private DTO_AdaptabilityRating GetAdaptabilityRating(Player player, MatchMasterMySqlDatabaseContext dbContext)
        {
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

            double highTempAverageRating = CalculateAverageRatingFromTemp(highTempFixtures, playerStats);
			double lowTempAverageRating = CalculateAverageRatingFromTemp(lowTempFixtures, playerStats);

			double adaptabilityRating = (lowTempAverageRating < highTempAverageRating ? lowTempAverageRating / highTempAverageRating : highTempAverageRating / lowTempAverageRating) * 100;
            int adaptabilityRatingInt = (int)adaptabilityRating;
            string textColour = GetTextColour(adaptabilityRating / 10);

            return new DTO_AdaptabilityRating { AdaptabilityPercentage = adaptabilityRatingInt, TextColor = textColour };
        }

        private double CalculateAverageRatingFromTemp(IEnumerable<Fixture> fixtures, PlayerStat[] playerStats)
        {
			double totalRating = fixtures
                .Select(fixture => playerStats.FirstOrDefault(ps => ps.FixtureId == fixture.FixtureId)?.Rating)
                .Where(rating => rating.HasValue && rating.Value > 0)
                .Sum(rating => rating.Value);

            int count = fixtures
                .Select(fixture => playerStats.FirstOrDefault(ps => ps.FixtureId == fixture.FixtureId)?.Rating)
                .Count(rating => rating.HasValue && rating.Value > 0);

            return count > 0 ? totalRating / count : 0;
        }

        private string GetTextColour(double rating)
        {
            return rating switch
            {
                < 5 => "#FF0000",
                >= 5 and < 7 => "#FFA500",
                >= 7 and < 8 => "#6BBE00",
                > 8 => "#008000",
                _ => "#FFA500",
            };
        }
    }
}
