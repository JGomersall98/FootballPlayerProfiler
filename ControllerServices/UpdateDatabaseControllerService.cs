using MatchMasterWEB.APIObject;
using Newtonsoft.Json;
using MatchMasterWEB.Database;
using MatchMasterWEB.Database.DB_Models;
using MatchMasterWEB.ExternalAPICalls.Weather;
using MatchMasterWEB.ExternalAPICalls.PerformanceMetrics;
using static MatchMasterWEB.APIObject.FixturesByLeagueIdObject;
using static MatchMasterWEB.APIObject.FixtureDetails;

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
				FootballAPICalls footballAPICalls = new FootballAPICalls();
				FixtureResponse fixtureIdResponse = await footballAPICalls.GetFixtureIdAsync();

				// Get List of existing fixtureIds to filter out duplicates
				List<int> fixtureIds = dbContext.Fixtures.Select(f => f.FixtureId).ToList();

				// Get the fixture details
				FixtureDetails fixtureDetails = footballAPICalls.GetFixtureDetails(fixtureIdResponse, fixtureIds);
			}
			List<string> placeIds = new List<string>();
			List<double> temperatures = new List<double>();
			// Get the placeId for each fixture
			foreach (var fixture in dummyFixtureDetails.fixtureDetails)
			{
				// Get List of placeIds				
				placeIds.Add(await GetPlaceIdAsync(fixture));
				// Get List of Temperatures			
				temperatures.Add(await GetTemperatureAsync(placeIds[0], DateTime.Parse(fixture.dates[0]), fixture.times[0]));				
			}
			int counter = 0;
			// Add the fixtures to the database
			foreach (var fixture in dummyFixtureDetails.fixtureDetails)
			{
				
				// Add the fixture to the database
				Fixture newFixture = new Fixture
				{
					FixtureId = int.Parse(fixture.fixtureIds[0]),
					PlaceId = placeIds[counter],
					Date = DateTime.Parse(fixture.dates[0]),
					Time = TimeSpan.Parse(fixture.times[0]),
					Temperature = temperatures[counter]
				};
				dbContext.Fixtures.Add(newFixture);
				counter++;
			}
			// Save the changes to the database
			await dbContext.SaveChangesAsync();

            return "Success";

        }
		public async Task<string> GetPlaceIdAsync(Fixtures fixture)
		{
			WeatherAPICalls weatherAPICall = new WeatherAPICalls();
			string placeId = await weatherAPICall.GetPlaceIdAPICall(fixture.city[0]);
			if (placeId == "" || placeId == null)
				placeId = "sheffield";
			
			//Thread.Sleep(1000);

			return placeId;
		}
		public async Task<double> GetTemperatureAsync(string placeId, DateTime date, string time)
		{
			WeatherAPICalls weatherAPICall = new WeatherAPICalls();
			double temperature = await weatherAPICall.GetTemperatureAsync(placeId, date, time);
			//Thread.Sleep(1000);
			return temperature;
		}
		private FixtureDetails GetDummyData()
		{
			string dummyData = "{\"fixtureDetails\":[{\"fixtureIds\":[\"1047438\"],\"dates\":[\"2023-08-05\"],\"times\":[\"15:00:00\"],\"city\":[\"Norwich, Norfolk\"]},{\"fixtureIds\":[\"1047450\"],\"dates\":[\"2023-08-12\"],\"times\":[\"15:00:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047460\"],\"dates\":[\"2023-08-19\"],\"times\":[\"15:00:00\"],\"city\":[\"Blackburn, Lancashire\"]},{\"fixtureIds\":[\"1047470\"],\"dates\":[\"2023-08-25\"],\"times\":[\"19:30:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047486\"],\"dates\":[\"2023-09-02\"],\"times\":[\"15:00:00\"],\"city\":[\"Leicester, Leicestershire\"]},{\"fixtureIds\":[\"1047494\"],\"dates\":[\"2023-09-15\"],\"times\":[\"19:45:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047514\"],\"dates\":[\"2023-09-20\"],\"times\":[\"19:45:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047526\"],\"dates\":[\"2023-09-24\"],\"times\":[\"12:00:00\"],\"city\":[\"Stoke-on-Trent, Staffordshire\"]},{\"fixtureIds\":[\"1047534\"],\"dates\":[\"2023-09-30\"],\"times\":[\"15:00:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047543\"],\"dates\":[\"2023-10-03\"],\"times\":[\"19:45:00\"],\"city\":[\"Ipswich, Suffolk\"]},{\"fixtureIds\":[\"1047559\"],\"dates\":[\"2023-10-07\"],\"times\":[\"15:00:00\"],\"city\":[\"London\"]},{\"fixtureIds\":[\"1047568\"],\"dates\":[\"2023-10-21\"],\"times\":[\"15:00:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047583\"],\"dates\":[\"2023-10-25\"],\"times\":[\"19:45:00\"],\"city\":[\"Birmingham\"]},{\"fixtureIds\":[\"1047593\"],\"dates\":[\"2023-10-28\"],\"times\":[\"15:00:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047612\"],\"dates\":[\"2023-11-04\"],\"times\":[\"15:00:00\"],\"city\":[\"West Bromwich\"]},{\"fixtureIds\":[\"1047616\"],\"dates\":[\"2023-11-11\"],\"times\":[\"15:00:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047635\"],\"dates\":[\"2023-11-25\"],\"times\":[\"15:00:00\"],\"city\":[\"Swansea\"]},{\"fixtureIds\":[\"1047639\"],\"dates\":[\"2023-11-28\"],\"times\":[\"19:45:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047650\"],\"dates\":[\"2023-12-02\"],\"times\":[\"15:00:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047668\"],\"dates\":[\"2023-12-09\"],\"times\":[\"15:00:00\"],\"city\":[\"London\"]},{\"fixtureIds\":[\"1047682\"],\"dates\":[\"2023-12-13\"],\"times\":[\"20:00:00\"],\"city\":[\"Middlesbrough\"]},{\"fixtureIds\":[\"1047687\"],\"dates\":[\"2023-12-16\"],\"times\":[\"15:00:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047697\"],\"dates\":[\"2023-12-22\"],\"times\":[\"19:45:00\"],\"city\":[\"Bristol\"]},{\"fixtureIds\":[\"1047714\"],\"dates\":[\"2023-12-26\"],\"times\":[\"15:00:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047725\"],\"dates\":[\"2023-12-29\"],\"times\":[\"19:45:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047741\"],\"dates\":[\"2024-01-01\"],\"times\":[\"17:15:00\"],\"city\":[\"Sheffield\"]},{\"fixtureIds\":[\"1047750\"],\"dates\":[\"2024-01-12\"],\"times\":[\"20:00:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047767\"],\"dates\":[\"2024-01-19\"],\"times\":[\"20:00:00\"],\"city\":[\"Sunderland\"]},{\"fixtureIds\":[\"1047778\"],\"dates\":[\"2024-02-20\"],\"times\":[\"19:45:00\"],\"city\":[\"Southampton, Hampshire\"]},{\"fixtureIds\":[\"1047783\"],\"dates\":[\"2024-02-03\"],\"times\":[\"15:00:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047795\"],\"dates\":[\"2024-02-10\"],\"times\":[\"15:00:00\"],\"city\":[\"Hull\"]},{\"fixtureIds\":[\"1047808\"],\"dates\":[\"2024-02-13\"],\"times\":[\"19:45:00\"],\"city\":[\"Rotherham, South Yorkshire\"]},{\"fixtureIds\":[\"1047818\"],\"dates\":[\"2024-02-17\"],\"times\":[\"15:00:00\"],\"city\":[\"Huddersfield, West Yorkshire\"]},{\"fixtureIds\":[\"1047831\"],\"dates\":[\"2024-02-24\"],\"times\":[\"12:30:00\"],\"city\":[\"Hull\"]}]}\r\n";
			//Parse into FixtureDetails object
			FixtureDetails fixtureDetails = JsonConvert.DeserializeObject<FixtureDetails>(dummyData);
			
			return fixtureDetails;

		}
		public static List<Fixture> GetFixtures()
		{
			return new List<Fixture>
			{
			new Fixture { FixtureId = 1047438, PlaceId = "norwich", Date = DateTime.Parse("2023-08-05"), Time = TimeSpan.Parse("15:00:00"), Temperature = 0 },
			new Fixture { FixtureId = 1047450, PlaceId = "kingston-upon-hull", Date = DateTime.Parse("2023-08-12"), Time = TimeSpan.Parse("15:00:00"), Temperature = 0 },
			new Fixture { FixtureId = 1047460, PlaceId = "sheffield", Date = DateTime.Parse("2023-08-19"), Time = TimeSpan.Parse("15:00:00"), Temperature = 0 },
			new Fixture { FixtureId = 1047470, PlaceId = "kingston-upon-hull", Date = DateTime.Parse("2023-08-25"), Time = TimeSpan.Parse("19:30:00"), Temperature = 0 },
			new Fixture { FixtureId = 1047486, PlaceId = "leicestershire", Date = DateTime.Parse("2023-09-02"), Time = TimeSpan.Parse("15:00:00"), Temperature = 0 },
			new Fixture { FixtureId = 1047494, PlaceId = "kingston-upon-hull", Date = DateTime.Parse("2023-09-15"), Time = TimeSpan.Parse("19:45:00"), Temperature = 0 },
			new Fixture { FixtureId = 1047514, PlaceId = "kingston-upon-hull", Date = DateTime.Parse("2023-09-20"), Time = TimeSpan.Parse("19:45:00"), Temperature = 0 },
			new Fixture { FixtureId = 1047526, PlaceId = "basford-stokeontrent", Date = DateTime.Parse("2023-09-24"), Time = TimeSpan.Parse("12:00:00"), Temperature = 0 },
			new Fixture { FixtureId = 1047534, PlaceId = "kingston-upon-hull", Date = DateTime.Parse("2023-09-30"), Time = TimeSpan.Parse("15:00:00"), Temperature = 0 },
			new Fixture { FixtureId = 1047543, PlaceId = "ipswich", Date = DateTime.Parse("2023-10-03"), Time = TimeSpan.Parse("19:45:00"), Temperature = 0 },
			new Fixture { FixtureId = 1047559, PlaceId = "london", Date = DateTime.Parse("2023-10-07"), Time = TimeSpan.Parse("15:00:00"), Temperature = 0 },
			new Fixture { FixtureId = 1047568, PlaceId = "kingston-upon-hull", Date = DateTime.Parse("2023-10-21"), Time = TimeSpan.Parse("15:00:00"), Temperature = 0 },
			new Fixture { FixtureId = 1047583, PlaceId = "birmingham", Date = DateTime.Parse("2023-10-25"), Time = TimeSpan.Parse("19:45:00"), Temperature = 0 },
			new Fixture { FixtureId = 1047593, PlaceId = "kingston-upon-hull", Date = DateTime.Parse("2023-10-28"), Time = TimeSpan.Parse("15:00:00"), Temperature = 0 },
			new Fixture { FixtureId = 1047612, PlaceId = "west-bromwich", Date = DateTime.Parse("2023-11-04"), Time = TimeSpan.Parse("15:00:00"), Temperature = 0 },
			new Fixture { FixtureId = 1047616, PlaceId = "kingston-upon-hull", Date = DateTime.Parse("2023-11-11"), Time = TimeSpan.Parse("15:00:00"), Temperature = 0 },
			new Fixture { FixtureId = 1047635, PlaceId = "swansea", Date = DateTime.Parse("2023-11-25"), Time = TimeSpan.Parse("15:00:00"), Temperature = 0 },
			new Fixture { FixtureId = 1047639, PlaceId = "kingston-upon-hull", Date = DateTime.Parse("2023-11-28"), Time = TimeSpan.Parse("19:45:00"), Temperature = 0 },
			new Fixture { FixtureId = 1047650, PlaceId = "kingston-upon-hull", Date = DateTime.Parse("2023-12-02"), Time = TimeSpan.Parse("15:00:00"), Temperature = 0 },
			new Fixture { FixtureId = 1047668, PlaceId = "london", Date = DateTime.Parse("2023-12-09"), Time = TimeSpan.Parse("15:00:00"), Temperature = 0 },
			new Fixture { FixtureId = 1047682, PlaceId = "middlesbrough", Date = DateTime.Parse("2023-12-13"), Time = TimeSpan.Parse("20:00:00"), Temperature = 0 },
			new Fixture { FixtureId = 1047687, PlaceId = "kingston-upon-hull", Date = DateTime.Parse("2023-12-16"), Time = TimeSpan.Parse("15:00:00"), Temperature = 0 },
			new Fixture { FixtureId = 1047697, PlaceId = "bristol", Date = DateTime.Parse("2023-12-22"), Time = TimeSpan.Parse("19:45:00"), Temperature = 0 },
			new Fixture { FixtureId = 1047714, PlaceId = "kingston-upon-hull", Date = DateTime.Parse("2023-12-26"), Time = TimeSpan.Parse("15:00:00"), Temperature = 0 },
			new Fixture { FixtureId = 1047725, PlaceId = "kingston-upon-hull", Date = DateTime.Parse("2023-12-29"), Time = TimeSpan.Parse("19:45:00"), Temperature = 0 },
			new Fixture { FixtureId = 1047741, PlaceId = "sheffield", Date = DateTime.Parse("2024-01-01"), Time = TimeSpan.Parse("17:15:00"), Temperature = 0 },
			new Fixture { FixtureId = 1047750, PlaceId = "kingston-upon-hull", Date = DateTime.Parse("2024-01-12"), Time = TimeSpan.Parse("20:00:00"), Temperature = 0 },
			new Fixture { FixtureId = 1047767, PlaceId = "sunderland", Date = DateTime.Parse("2024-01-19"), Time = TimeSpan.Parse("20:00:00"), Temperature = 0 },
			new Fixture { FixtureId = 1047778, PlaceId = "southampton-weather-centre", Date = DateTime.Parse("2024-02-20"), Time = TimeSpan.Parse("19:45:00"), Temperature = 0 },
			new Fixture { FixtureId = 1047783, PlaceId = "kingston-upon-hull", Date = DateTime.Parse("2024-02-03"), Time = TimeSpan.Parse("15:00:00"), Temperature = 0 },
			new Fixture { FixtureId = 1047795, PlaceId = "kingston-upon-hull", Date = DateTime.Parse("2024-02-10"), Time = TimeSpan.Parse("15:00:00"), Temperature = 0 },
			new Fixture { FixtureId = 1047808, PlaceId = "maltby", Date = DateTime.Parse("2024-02-13"), Time = TimeSpan.Parse("19:45:00"), Temperature = 0 },
			new Fixture { FixtureId = 1047818, PlaceId = "sheffield", Date = DateTime.Parse("2024-02-17"), Time = TimeSpan.Parse("15:00:00"), Temperature = 0 },
			new Fixture { FixtureId = 1047831, PlaceId = "kingston-upon-hull", Date = DateTime.Parse("2024-02-24"), Time = TimeSpan.Parse("12:30:00"), Temperature = 0 }
			};
		}

	}
}
