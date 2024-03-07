namespace MatchMasterWEB.Database.DB_Models
{
	public class Fixture
	{
		public int FixtureId { get; set; }
		public string PlaceId { get; set; }
		public DateTime Date { get; set; }
		public TimeSpan Time { get; set; }
		public double Temperature { get; set; }
		public ICollection<PlayerStat> PlayerStats { get; set; }
	}
}
