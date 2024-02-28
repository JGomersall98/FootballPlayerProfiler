namespace MatchMasterWEB.DTO_Models.In_DepthAnalysis
{
	public class DTO_GetPerformanceMetrics
	{
		public DTO_PerformanceMetric? StaticMetric { get; set; }
		public DTO_PerformanceMetric? InteractiveMetric { get; set; }

	}
	public class DTO_PerformanceMetric
	{
		public double PassingRating { get; set; }
		public double DefendingRating { get; set; }
		public double DuelsRating { get; set; }
		public double DribblingRating { get; set; }
		public double ShootingRating { get; set; }
	}
}
