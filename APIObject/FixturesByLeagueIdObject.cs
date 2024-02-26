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
			public List<Response> Response { get; set; }
		}

		public class Response
		{
			public FixtureInfo Fixture { get; set; }
		}

		public class FixtureInfo
		{
			public int Id { get; set; }
			public DateTime Date { get; set; }
			public Venue Venue { get; set; }
		}

		public class Venue
		{
			public string City { get; set; }
		}
	}
}