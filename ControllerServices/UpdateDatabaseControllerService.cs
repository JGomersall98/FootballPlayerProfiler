using MatchMasterWEB.APIObject;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using MatchMasterWEB.Database;
using Microsoft.EntityFrameworkCore;
using MatchMasterWEB.Database.DB_Models;

namespace MatchMasterWebAPI.ControllerServices
{
	public class UpdateDatabaseControllerService
	{
		public async Task<string> UpdateDatabaseAsync(MatchMasterMySqlDatabaseContext dbContext)
		{
			bool dummyData = true;
			FixtureDetails dummyFixtureDetails = null;
			List<Fixture> fixtures = new List<Fixture>();
			if (dummyData)
			{
				//Get DummyData
				dummyFixtureDetails = GetDummyData();
			}
			else
			{
				//			UCOMMENT THE FOLLOWING LINE TO USE THE API
				string fixtureIdResponse = await GetFixtureIdAsync();
				//			UNCOMMENT THE ABOVE LINE TO USE THE API

				// Deserialize the response
				//			UNCOMMENT THE FOLLOWING LINE WHEN USING THE API
				var fixturesByLeagueIdObject = JsonConvert.DeserializeObject<FixturesByLeagueIdObject.FixtureResponse>(fixtureIdResponse);
				//			UNCOMMENT THE ABOVE LINE WHEN USING THE API

				// Get List of FixtureIds from the database
				List<int> fixtureIds = dbContext.Fixtures.Select(f => f.FixtureId).ToList();


				// Get List of Fixtures and DateTimes
				//			UNCOMMENT THE FOLLOWING LINE WHEN USING THE API
				FixtureDetails fixtureDetails = GetFixtureDetails(fixturesByLeagueIdObject, fixtureIds);
				//			UNCOMMENT THE ABOVE LINE WHEN USING THE API
			}

			// Get the temperature for each fixture
			foreach (var fixture in dummyFixtureDetails.fixtureDetails)
			{
				string placeId = await GetPlaceIdAPICall(fixture.city[0]);
				//New Fixture object
				Fixture newFixture = new Fixture
				{
					FixtureId = int.Parse(fixture.fixtureIds[0]),
					Date = DateTime.Parse(fixture.dates[0]),
					Time = TimeSpan.Parse(fixture.times[0]),
					PlaceId = placeId
				};	
				fixtures.Add(newFixture);
			}
            await Console.Out.WriteLineAsync(	);
            // TODO: Add code to get the temperature for each fixture


            return "Success";

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
		private FixtureDetails GetDummyData()
		{
			string dummyData = "{\"fixtureDetails\":[{\"fixtureIds\":[\"1047438\"],\"dates\":[\"2023-08-05\"],\"times\":[\"15:00:00\"],\"city\":[\"Norwich, Norfolk\"]},{\"fixtureIds\":[\"1047450\"],\"dates\":[\"2023-08-12\"],\"times\":[\"15:00:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047460\"],\"dates\":[\"2023-08-19\"],\"times\":[\"15:00:00\"],\"city\":[\"Blackburn, Lancashire\"]},{\"fixtureIds\":[\"1047470\"],\"dates\":[\"2023-08-25\"],\"times\":[\"19:30:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047486\"],\"dates\":[\"2023-09-02\"],\"times\":[\"15:00:00\"],\"city\":[\"Leicester, Leicestershire\"]},{\"fixtureIds\":[\"1047494\"],\"dates\":[\"2023-09-15\"],\"times\":[\"19:45:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047514\"],\"dates\":[\"2023-09-20\"],\"times\":[\"19:45:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047526\"],\"dates\":[\"2023-09-24\"],\"times\":[\"12:00:00\"],\"city\":[\"Stoke-on-Trent, Staffordshire\"]},{\"fixtureIds\":[\"1047534\"],\"dates\":[\"2023-09-30\"],\"times\":[\"15:00:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047543\"],\"dates\":[\"2023-10-03\"],\"times\":[\"19:45:00\"],\"city\":[\"Ipswich, Suffolk\"]},{\"fixtureIds\":[\"1047559\"],\"dates\":[\"2023-10-07\"],\"times\":[\"15:00:00\"],\"city\":[\"London\"]},{\"fixtureIds\":[\"1047568\"],\"dates\":[\"2023-10-21\"],\"times\":[\"15:00:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047583\"],\"dates\":[\"2023-10-25\"],\"times\":[\"19:45:00\"],\"city\":[\"Birmingham\"]},{\"fixtureIds\":[\"1047593\"],\"dates\":[\"2023-10-28\"],\"times\":[\"15:00:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047612\"],\"dates\":[\"2023-11-04\"],\"times\":[\"15:00:00\"],\"city\":[\"West Bromwich\"]},{\"fixtureIds\":[\"1047616\"],\"dates\":[\"2023-11-11\"],\"times\":[\"15:00:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047635\"],\"dates\":[\"2023-11-25\"],\"times\":[\"15:00:00\"],\"city\":[\"Swansea\"]},{\"fixtureIds\":[\"1047639\"],\"dates\":[\"2023-11-28\"],\"times\":[\"19:45:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047650\"],\"dates\":[\"2023-12-02\"],\"times\":[\"15:00:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047668\"],\"dates\":[\"2023-12-09\"],\"times\":[\"15:00:00\"],\"city\":[\"London\"]},{\"fixtureIds\":[\"1047682\"],\"dates\":[\"2023-12-13\"],\"times\":[\"20:00:00\"],\"city\":[\"Middlesbrough\"]},{\"fixtureIds\":[\"1047687\"],\"dates\":[\"2023-12-16\"],\"times\":[\"15:00:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047697\"],\"dates\":[\"2023-12-22\"],\"times\":[\"19:45:00\"],\"city\":[\"Bristol\"]},{\"fixtureIds\":[\"1047714\"],\"dates\":[\"2023-12-26\"],\"times\":[\"15:00:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047725\"],\"dates\":[\"2023-12-29\"],\"times\":[\"19:45:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047741\"],\"dates\":[\"2024-01-01\"],\"times\":[\"17:15:00\"],\"city\":[\"Sheffield\"]},{\"fixtureIds\":[\"1047750\"],\"dates\":[\"2024-01-12\"],\"times\":[\"20:00:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047767\"],\"dates\":[\"2024-01-19\"],\"times\":[\"20:00:00\"],\"city\":[\"Sunderland\"]},{\"fixtureIds\":[\"1047778\"],\"dates\":[\"2024-02-20\"],\"times\":[\"19:45:00\"],\"city\":[\"Southampton, Hampshire\"]},{\"fixtureIds\":[\"1047783\"],\"dates\":[\"2024-02-03\"],\"times\":[\"15:00:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047795\"],\"dates\":[\"2024-02-10\"],\"times\":[\"15:00:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047808\"],\"dates\":[\"2024-02-13\"],\"times\":[\"19:45:00\"],\"city\":[\"Rotherham, South Yorkshire\"]},{\"fixtureIds\":[\"1047818\"],\"dates\":[\"2024-02-17\"],\"times\":[\"15:00:00\"],\"city\":[\"Huddersfield, West Yorkshire\"]},{\"fixtureIds\":[\"1047831\"],\"dates\":[\"2024-02-24\"],\"times\":[\"12:30:00\"],\"city\":[\"Hull\"]}]}\r\n";
			//Parse into FixtureDetails object
			FixtureDetails fixtureDetails = JsonConvert.DeserializeObject<FixtureDetails>(dummyData);
			
			return fixtureDetails;

		}
		private async Task<string> GetPlaceIdAPICall(string cityName)
		{
			var client = new HttpClient();
			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				// Use string interpolation to dynamically insert the city name
				RequestUri = new Uri($"https://ai-weather-by-meteosource.p.rapidapi.com/find_places?text={Uri.EscapeDataString(cityName)}&language=en"),
				Headers =
				{
					{ "X-RapidAPI-Key", "your_rapidapi_key_here" },
					{ "X-RapidAPI-Host", "ai-weather-by-meteosource.p.rapidapi.com" },
				},
			};

			using (var response = await client.SendAsync(request))
			{
				response.EnsureSuccessStatusCode();
				var body = await response.Content.ReadAsStringAsync();
				string placeId = GetPlaceId(body);
				return placeId;
			}		
		}
		private string GetPlaceId(string body)
		{
			return "";
		}
	}
}
