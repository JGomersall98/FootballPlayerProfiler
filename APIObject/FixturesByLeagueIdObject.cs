using Newtonsoft.Json;

namespace MatchMasterWEB.APIObject
{
	public class FixturesByLeagueIdObject
	{
		public class JsonDeserializer
		{
			public static FixturesByLeagueIdObject.FixtureResponse DeserializeFixtureResponse(string jsonString)
			{
				return JsonConvert.DeserializeObject<FixturesByLeagueIdObject.FixtureResponse>(jsonString);
			}
		}
		public class FixtureResponse
		{
			public string? Get { get; set; }
			public Parameters? Parameters { get; set; }
			public List<object>? Errors { get; set; }
			public int Results { get; set; }
			public Paging? Paging { get; set; }
			public List<Response>? Response { get; set; }
		}

		public class Parameters
		{
			public string? League { get; set; }
			public string? Season { get; set; }
			public string? Status { get; set; }
			public string? Team { get; set; }
		}

		public class Paging
		{
			public int Current { get; set; }
			public int Total { get; set; }
		}

		public class Response
		{
			public Fixture? Fixture { get; set; }
			public League? League { get; set; }
			public Teams? Teams { get; set; }
			public Goals? Goals { get; set; }
			public Score? Score { get; set; }
		}

		public class Fixture
		{
			public int Id { get; set; }
			public string? Referee { get; set; }
			public string? Timezone { get; set; }
			public DateTime Date { get; set; }
			public long Timestamp { get; set; }
			public Periods? Periods { get; set; }
			public Venue? Venue { get; set; }
			public Status? Status { get; set; }
		}

		public class Periods
		{
			public long First { get; set; }
			public long Second { get; set; }
		}

		public class Venue
		{
			public int Id { get; set; }
			public string? Name { get; set; }
			public string? City { get; set; }
		}

		public class Status
		{
			public string? Long { get; set; }
			public string? Short { get; set; }
			public int Elapsed { get; set; }
		}

		public class League
		{
			public int Id { get; set; }
			public string? Name { get; set; }
			public string? Country { get; set; }
			public string? Logo { get; set; }
			public string? Flag { get; set; }
			public int Season { get; set; }
			public string? Round { get; set; }
		}

		public class Teams
		{
			public Team? Home { get; set; }
			public Team? Away { get; set; }
		}

		public class Team
		{
			public int Id { get; set; }
			public string? Name { get; set; }
			public string? Logo { get; set; }
			public bool? Winner { get; set; }
		}

		public class Goals
		{
			public int? Home { get; set; }
			public int? Away { get; set; }
		}

		public class Score
		{
			public Halftime? Halftime { get; set; }
			public Fulltime? Fulltime { get; set; }
			public Extratime? Extratime { get; set; }
			public Penalty? Penalty { get; set; }
		}

		public class Halftime
		{
			public int? Home { get; set; }
			public int? Away { get; set; }
		}

		public class Fulltime
		{
			public int? Home { get; set; }
			public int? Away { get; set; }
		}

		public class Extratime
		{
			public int? Home { get; set; }
			public int? Away { get; set; }
		}

		public class Penalty
		{
			public int? Home { get; set; }
			public int? Away { get; set; }
		}
	}
}