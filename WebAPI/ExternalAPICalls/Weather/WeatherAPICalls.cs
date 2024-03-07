using Newtonsoft.Json;

namespace MatchMasterWEB.ExternalAPICalls.Weather
{
	public class WeatherAPICalls
	{
		private const string RapidAPIKey = "bdc8d9557cmshf0bec4adac0297bp142c9djsn35ff23a72fb7";
		private const string RapidAPIHost = "ai-weather-by-meteosource.p.rapidapi.com";
		public async Task<string> GetPlaceIdAPICall(string cityName)
		{
			var client = new HttpClient();
			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				// Use string interpolation to dynamically insert the city name
				RequestUri = new Uri($"https://ai-weather-by-meteosource.p.rapidapi.com/find_places?text={Uri.EscapeDataString(cityName)}&language=en"),
				Headers =
				{
					{ "X-RapidAPI-Key", RapidAPIKey },
					{ "X-RapidAPI-Host", RapidAPIHost },
				},
			};

			using (var response = await client.SendAsync(request))
			{
				response.EnsureSuccessStatusCode();
				var body = await response.Content.ReadAsStringAsync();
				string placeId = GetPlaceId(body);
				Thread.Sleep(5000);
				return placeId;
			}
		}

		public async Task<string> GetTemperatureAPICall(string placeId, DateTime date)
		{
			using (var client = new HttpClient())
			{
				var request = new HttpRequestMessage
				{
					Method = HttpMethod.Get,
					// Use string interpolation to dynamically insert the placeId and date
					RequestUri = new Uri($"https://ai-weather-by-meteosource.p.rapidapi.com/time_machine?date={date.ToString("yyyy-MM-dd")}&place_id={placeId}&units=auto"),
					Headers =
					{
						{ "X-RapidAPI-Key", RapidAPIKey },
						{ "X-RapidAPI-Host", RapidAPIHost },
					},
				};

				using (var response = await client.SendAsync(request))
				{
					response.EnsureSuccessStatusCode();
					Thread.Sleep(5000);
					return await response.Content.ReadAsStringAsync();
				}
			}
		}
		public async Task<double> GetTemperatureAsync(string placeId, DateTime date, string time)
		{
			// Call the GetTemperatureAPICall method to get the temperature
			string body = await GetTemperatureAPICall(placeId, date);
			// Deserialize the response
			var temperatureData = JsonConvert.DeserializeObject<WeatherApiResponse>(body);
			// Find Closest Time For Temperature
			double temperature = FindClosestTime(temperatureData.Data, time);

			return temperature;
		}

		public class WeatherApiResponse
		{
			public List<WeatherData>? Data { get; set; }
		}

		public class WeatherData
		{
			public DateTime Date { get; set; }
			public double Temperature { get; set; }
		}
		private string GetPlaceId(string body)
		{
			var places = JsonConvert.DeserializeObject<List<MinimalPlace>>(body);
			var ukPlace = places.FirstOrDefault(p => p.Country == "United Kingdom");
			return ukPlace?.PlaceId;
		}
		public class MinimalPlace
		{
			[JsonProperty("place_id")]
			public string? PlaceId { get; set; }

			public string? Country { get; set; }
		}

        public double FindClosestTime(List<WeatherData> weatherData, string time)
        {
			// Convert the time to a TimeSpan
			TimeSpan timeSpan = TimeSpan.Parse(time);
			// Find the closest time
			var closestTime = weatherData.OrderBy(w => Math.Abs((w.Date - timeSpan).Ticks)).First();
			// Return the temperature
			return (double)closestTime.Temperature;
        }
    }
}
