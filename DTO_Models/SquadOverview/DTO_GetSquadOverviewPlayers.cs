namespace MatchMasterWEB.DTO_Models.SquadOverview
{
	public class DTO_GetSquadOverviewPlayers
	{
		public DTO_SquadOverviewPlayer[] Players { get; set; }
	}
	public class DTO_SquadOverviewPlayer
	{
		public int PlayerId { get; set; }
		public string? PlayerName { get; set; }
		public string? PlayerPhoto { get; set; }
		public DTO_PlayerRating? PlayerRating { get; set; }
		public DTO_AdaptabilityRating? AdaptabilityPercentage { get; set; }
	}
	public class DTO_PlayerRating
	{
		public double PlayerRating { get; set; }
		public string? TextColor { get; set; }
	}
	public class DTO_AdaptabilityRating
	{
		public int AdaptabilityPercentage { get; set; }
		public string? TextColor { get; set; }
	}
}
