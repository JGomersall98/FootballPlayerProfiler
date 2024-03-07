using Newtonsoft.Json;

namespace MatchMasterWEB.APIObject
{
	public class PlayerStatResponse
	{
		[JsonProperty("response")]
		public List<TeamPlayerStats> Response { get; set; }
	}
	public class TeamPlayerStats
	{
		[JsonProperty("team")]
		public TeamDetail Team { get; set; }

		[JsonProperty("players")]
		public List<PlayerStatDetail> Players { get; set; }
	}
	public class TeamDetail
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("logo")]
		public string Logo { get; set; }
	}
	public class PlayerStatDetail
	{
		[JsonProperty("player")]
		public PlayerDetail Player { get; set; }

		[JsonProperty("statistics")]
		public List<StatisticDetail> Statistics { get; set; }
	}

	public class PlayerDetail
	{
		[JsonProperty("id")]
		public int PlayerId { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("photo")]
		public string Photo { get; set; }

		[JsonProperty("position")]
		public string Position { get; set; }
	}

	public class StatisticDetail
	{
		[JsonProperty("games")]
		public GameDetail Games { get; set; }

		[JsonProperty("shots")]
		public ShotDetail Shots { get; set; }

		[JsonProperty("passes")]
		public PassDetail Passes { get; set; }

		[JsonProperty("tackles")]
		public TackleDetail Tackles { get; set; }

		[JsonProperty("duels")]
		public DuelDetail Duels { get; set; }

		[JsonProperty("dribbles")]
		public DribbleDetail Dribbles { get; set; }

		[JsonProperty("fouls")]
		public FoulDetail Fouls { get; set; }
	}

	public class GameDetail
	{
		[JsonProperty("position")]
		public string Position { get; set; }

		[JsonProperty("rating")]
		public double? Rating { get; set; }
	}

	public class ShotDetail
	{
		[JsonProperty("total")]
		public int? TotalShots { get; set; }

		[JsonProperty("on")]
		public int? ShotsOnTarget { get; set; }
	}

	public class PassDetail
	{
		[JsonProperty("accuracy")]
		public int? PassingAccuracy { get; set; }
	}

	public class TackleDetail
	{
		[JsonProperty("total")]
		public int? TotalTackles { get; set; }
	}

	public class DuelDetail
	{
		[JsonProperty("total")]
		public int? DuelsTotal { get; set; }

		[JsonProperty("won")]
		public int? DuelsWon { get; set; }
	}

	public class DribbleDetail
	{
		[JsonProperty("attempts")]
		public int? DribblingAttempts { get; set; }

		[JsonProperty("success")]
		public int? DribblingSuccess { get; set; }
	}

	public class FoulDetail
	{
		[JsonProperty("committed")]
		public int? FoulsCommitted { get; set; }
	}
}



