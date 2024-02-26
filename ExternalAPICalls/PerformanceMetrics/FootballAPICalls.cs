using MatchMasterWEB.APIObject;
using Newtonsoft.Json;
using static MatchMasterWEB.APIObject.FixturesByLeagueIdObject;

namespace MatchMasterWEB.ExternalAPICalls.PerformanceMetrics
{
	public class FootballAPICalls
	{
		public async Task<FixtureResponse> GetFixtureIdAsync()
		{

			var client = new HttpClient();
			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri("https://api-football-v1.p.rapidapi.com/v3/fixtures?league=40&season=2023&team=64&status=FT"),
				Headers =
					{
						{ "X-RapidAPI-Key", "bdc8d9557cmshf0bec4adac0297bp142c9djsn35ff23a72fb7" },
						{ "X-RapidAPI-Host", "api-football-v1.p.rapidapi.com" },
					},
			};
			using (var response = await client.SendAsync(request))
			{
				response.EnsureSuccessStatusCode();
				string body = await response.Content.ReadAsStringAsync();
				Console.WriteLine(body);
				//Deserialize the response
				FixtureResponse fixturesByLeagueIdObject = JsonConvert.DeserializeObject<FixturesByLeagueIdObject.FixtureResponse>(body);

				return fixturesByLeagueIdObject;
			}
			throw new NotImplementedException();
		}
		public FixtureDetails GetFixtureDetails(FixturesByLeagueIdObject.FixtureResponse fixturesByLeagueIdObject, List<int> fixtureIds)
		{
			if (fixturesByLeagueIdObject?.Response == null)
				throw new ArgumentNullException(nameof(fixturesByLeagueIdObject));

			var fixtureDetailsList = new List<FixtureDetails.Fixtures>();

			foreach (var fixture in fixturesByLeagueIdObject.Response)
			{
				var fixtureDetail = new FixtureDetails.Fixtures
				{
					fixtureIds = new List<string> { fixture.Fixture.Id.ToString() },
					dates = new List<string> { fixture.Fixture.Date.ToString("yyyy-MM-dd") },
					times = new List<string> { fixture.Fixture.Date.ToString("HH:mm:ss") },
					city = new List<string> { fixture.Fixture.Venue.City }
				};

				fixtureDetailsList.Add(fixtureDetail);
			}
			//Filter the fixtureDetails to only include fixtures that are not in the database
			FixtureDetails filteredFixtureDetails = FilterFixtureDetails(new FixtureDetails { fixtureDetails = fixtureDetailsList }, fixtureIds);

			//Return filteredFixtureDetails
			return filteredFixtureDetails;
		}
		public FixtureDetails FilterFixtureDetails(FixtureDetails fixtureDetails, List<int> fixtureIds)
		{
			if (fixtureDetails?.fixtureDetails == null)
				throw new ArgumentNullException(nameof(fixtureDetails));

			//Filter the fixtureDetails to only include fixtures that are not in the database
			fixtureDetails.fixtureDetails = fixtureDetails.fixtureDetails.Where(f => !fixtureIds.Contains(int.Parse(f.fixtureIds[0]))).ToList();

			return fixtureDetails;
		}
	}
}
