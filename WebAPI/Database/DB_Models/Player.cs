namespace MatchMasterWEB.Database.DB_Models
{
	public class Player
	{
		public int PlayerId { get; set; }
		public string Name { get; set; }
		public string Photo { get; set; }
		public string Position { get; set; }
		public ICollection<PlayerStat> PlayerStats { get; set; }
	}
}
