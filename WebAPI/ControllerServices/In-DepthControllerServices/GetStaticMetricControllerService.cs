using MatchMasterWEB.Database;
using MatchMasterWEB.Database.DB_Models;
using MatchMasterWEB.DTO_Models.In_DepthAnalysis;
using Microsoft.EntityFrameworkCore;

namespace MatchMasterWEB.ControllerServices.In_DepthControllerServices
{
	public class GetStaticMetricControllerService
	{
		public DTO_PerformanceMetric GetStaticPerformanceMetric(int playerId, MatchMasterMySqlDatabaseContext dbContext)
		{
			// Get PlayerStats from database from playerId
			PlayerStat[] playerStats = dbContext.PlayerStats
				.Include(ps => ps.Fixture)
				.Where(ps => ps.PlayerId == playerId)
				.ToArray();

			// Filter PlayersStats to only include rated fixtures
			PlayerStat[] ratedPlayerStats = playerStats.Where(ps => ps.Rating > 0).ToArray();

			// if ratedPlayerStats is null return empty DTO
			if (ratedPlayerStats == null)
				return new DTO_PerformanceMetric { };

			// Create a new instance of MetricCalculations
			MetricCalculations metricCalculations = new MetricCalculations();

			return new DTO_PerformanceMetric 
			{ 
				PassingRating = metricCalculations.GetPassingPercentage(ratedPlayerStats),
				DefendingRating = metricCalculations.GetDefendingPercentage(ratedPlayerStats),
				DuelsRating = metricCalculations.GetDuelsPercentage(ratedPlayerStats),
				DribblingRating = metricCalculations.GetDribblingRating(ratedPlayerStats),
				ShootingRating = metricCalculations.GetShootingRating(ratedPlayerStats)
			};
		}
	}
}
