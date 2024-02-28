using MatchMasterWEB.Database;
using MatchMasterWEB.Database.DB_Models;
using MatchMasterWEB.DTO_Models.In_DepthAnalysis;
using Microsoft.EntityFrameworkCore;

namespace MatchMasterWEB.ControllerServices.In_DepthControllerServices
{
	public class GetInteractiveMetricControllerService
	{
		public DTO_GetPerformanceMetrics GetPerformanceMetrics(int playerId, int lowTemp, int highTemp, MatchMasterMySqlDatabaseContext dbContext)
		{
			// Get PlayerStats from database from playerId
			PlayerStat[] playerStats = dbContext.PlayerStats
				.Include(ps => ps.Fixture)
				.Where(ps => ps.PlayerId == playerId)
				.ToArray();

			// Filter PlayersStats to only include rated fixtures
			PlayerStat[] ratedPlayerStats = playerStats.Where(ps => ps.Rating > 0).ToArray();

			// Filter RatedPlayerStats to only include fixtures with temperature between lowTemp and highTemp
			PlayerStat[] filteredByTemperatureStats = ratedPlayerStats.Where(ps => ps.Fixture.Temperature >= lowTemp && ps.Fixture.Temperature <= highTemp).ToArray();

			// if filteredPlayerStats is null return empty DTO
			if (filteredByTemperatureStats == null)
				return new DTO_GetPerformanceMetrics { };

			// Create a new instance of MetricCalculations
			MetricCalculations metricCalculations = new MetricCalculations();

			return new DTO_GetPerformanceMetrics
			{
				StaticMetric = metricCalculations.GetAllMetrics(ratedPlayerStats),
				InteractiveMetric = metricCalculations.GetAllMetrics(filteredByTemperatureStats)
			};
		}

	}
}
