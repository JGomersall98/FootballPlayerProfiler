using MatchMasterWEB.APIObject;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace MatchMasterWebAPI.ControllerServices
{
	public class UpdateDatabaseControllerService
	{
		public async Task UpdateDatabaseAsync()
		{
			string fixtureIdResponse = await GetFixtureIdAsync();
			// Deserialize the response
			var fixturesByLeagueIdObject = JsonConvert.DeserializeObject<FixturesByLeagueIdObject.FixtureResponse>(fixtureIdResponse);
			// Get List of Fixtures and DateTimes
			FixtureDetails fixtureDetails = GetFixtureDetails(fixturesByLeagueIdObject); 
            // Get the temperature for each fixture

            // TODO: Add code to get the temperature for each fixture


        }

		public FixtureDetails GetFixtureDetails(FixturesByLeagueIdObject.FixtureResponse fixturesByLeagueIdObject)
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

			return new FixtureDetails { fixtureDetails = fixtureDetailsList };
		}

		private async Task<string> GetFixtureIdAsync()
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
				return body;
			}
			throw new NotImplementedException();
		}

	}
}
