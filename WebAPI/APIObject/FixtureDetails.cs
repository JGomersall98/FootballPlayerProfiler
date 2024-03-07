namespace MatchMasterWEB.APIObject
{
	public class FixtureDetails
	{
		public List<Fixtures>? fixtureDetails { get; set; }
		public class Fixtures
		{ 
			public List<string>? fixtureIds { get; set; }
			public List<string>? dates { get; set; }
			public List<string>? times { get; set; }
			public List<string>? city { get; set; }
		}
	}
}
