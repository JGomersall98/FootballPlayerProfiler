namespace MatchMasterWEB.DTO_Models.In_DepthAnalysis
{
	public class DTO_GetRatingsByTemperature
	{
		public DTO_DegreeRating[]? Ratings { get; set; }
	}
	public class DTO_DegreeRating
	{
		public int Order { get; set; }
		public double Rating { get; set; }
		public string? TextColor { get; set; }
	}

}
