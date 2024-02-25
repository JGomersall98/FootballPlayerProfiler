using System.Net.Http.Headers;

namespace MatchMasterWebAPI.ControllerServices
{
	public class UpdateDatabaseControllerService
	{
		public void UpdateDatabase()
		{
			var fixtureIdResponse = GetFixtureIdAsync();
			
		}

		private async Task<object> GetFixtureIdAsync()
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
				var body = await response.Content.ReadAsStringAsync();
				Console.WriteLine(body);
				return body;
			}
			throw new NotImplementedException();
		}
	}
}
