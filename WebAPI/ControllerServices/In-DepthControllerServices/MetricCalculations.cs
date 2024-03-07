using MatchMasterWEB.Database.DB_Models;
using MatchMasterWEB.DTO_Models.In_DepthAnalysis;

namespace MatchMasterWEB.ControllerServices.In_DepthControllerServices
{
	public class MetricCalculations
	{
		public DTO_PerformanceMetric GetAllMetrics(PlayerStat[] playerStats)
		{
			MetricCalculations metricCalculations = new MetricCalculations();

			return new DTO_PerformanceMetric
			{
				PassingRating = metricCalculations.GetPassingPercentage(playerStats),
				DefendingRating = metricCalculations.GetDefendingPercentage(playerStats),
				DuelsRating = metricCalculations.GetDuelsPercentage(playerStats),
				DribblingRating = metricCalculations.GetDribblingRating(playerStats),
				ShootingRating = metricCalculations.GetShootingRating(playerStats)

			};
		}
		public int GetPassingPercentage(PlayerStat[] playerStats)
		{
			// Get all passing accuracy from playerStats
			double totalPassingAccuracy = 0;
			int count = 0;
			foreach (var playerStat in playerStats)
			{
				if (playerStat.PassingAccuracy > 0)
				{
					totalPassingAccuracy += playerStat.PassingAccuracy;
					count++;
				}
			}
			return count > 0 ? (int)(totalPassingAccuracy / count) : 0;
		}
		public int GetDefendingPercentage(PlayerStat[] playerStats)
		{
			// For each playerStat, calculate the defending percentage
			List<double> defendingPercentages = new List<double>();
			int count = 0;
			foreach (var playerStat in playerStats)
			{
				if (playerStat.TotalTackles > 0)
				{
					// Calculate successful tackles (tackles made without committing fouls)
					int successfulTackles = playerStat.TotalTackles - playerStat.FoulsCommitted;
					// Calculate defending percentage based on successful tackles
					double defendingPercentage = (double)successfulTackles / playerStat.TotalTackles * 100;
					// Ensure defendingPercentage is not negative
					if (defendingPercentage < 0)
					{
						defendingPercentage = 0;
					}
					defendingPercentages.Add(defendingPercentage);
					count++;
				}
			}
			// Return average of defendingPercentages
			return count > 0 ? (int)(defendingPercentages.Average()) : 0;
		}
		public int GetDuelsPercentage(PlayerStat[] playerStats)
		{
			// For each playerStat, calculate the duels percentage
			List<double> duelPercentages = new List<double>();
			int count = 0;
			foreach (var playerStat in playerStats)
			{
				if (playerStat.DuelsTotal > 0)
				{
					double duelsPercentage = (double)playerStat.DuelsWon / playerStat.DuelsTotal * 100;

					if (duelsPercentage < 0)
					{
						duelsPercentage = 0;
					}
					duelPercentages.Add(duelsPercentage);
					count++;
				}
			}
			// Return average of duelsPercentage
			return count > 0 ? (int)(duelPercentages.Average()) : 0;
		}
		public int GetDribblingRating(PlayerStat[] playerStats)
		{
			// For each playerStat, calculate the dribbling percentage
			List<double> dribblingPercentages = new List<double>();
			int count = 0;
			foreach (var playerStat in playerStats)
			{
				if (playerStat.DribblingAttempts > 0)
				{
					double dribblingPercentage = (double)playerStat.DribblingSuccess / playerStat.DribblingAttempts * 100;

					if (dribblingPercentage < 0)
					{
						dribblingPercentage = 0;
					}
					dribblingPercentages.Add(dribblingPercentage);
					count++;
				}
			}
			// Return average of duelsPercentage
			return count > 0 ? (int)(dribblingPercentages.Average()) : 0;
		}
		public int GetShootingRating(PlayerStat[] playerStats)
		{
			// For each playerStat, calculate the shooting percentage
			List<double> shootingPercentages = new List<double>();
			int count = 0;
			foreach (var playerStat in playerStats)
			{
				if (playerStat.TotalShots > 0)
				{
					double shootingPercentage = (double)playerStat.ShotsOnTarget / playerStat.TotalShots * 100;

					if (shootingPercentage < 0)
					{
						shootingPercentage = 0;
					}
					shootingPercentages.Add(shootingPercentage);
					count++;
				}
			}
			// Return average of duelsPercentage
			return count > 0 ? (int)(shootingPercentages.Average()) : 0;
		}
	}
}
