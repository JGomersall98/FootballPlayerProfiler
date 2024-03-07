namespace MatchMasterWEB.Database.DB_Models
{
	public class PlayerStat
	{
		public int StatId { get; set; }
		public int FixtureId { get; set; }
		public Fixture Fixture { get; set; }
		public int PlayerId { get; set; }
		public Player Player { get; set; }
		public double Rating { get; set; }
		public int PassingAccuracy { get; set; }
		public int TotalTackles { get; set; }
		public int FoulsCommitted { get; set; }
		public int DuelsTotal { get; set; }
		public int DuelsWon { get; set; }
		public int DribblingAttempts { get; set; }
		public int DribblingSuccess { get; set; }
		public int TotalShots { get; set; }
		public int ShotsOnTarget { get; set; }
	}
}
