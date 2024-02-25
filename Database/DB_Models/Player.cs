namespace temperatureVariationAnalysis.Database.DB_Models
{
	public class Player
	{
		public int PlayerId { get; set; }
		public string Name { get; set; }
		public string Photo { get; set; }
		public ICollection<PlayerStat> PlayerStats { get; set; }
	}
}
