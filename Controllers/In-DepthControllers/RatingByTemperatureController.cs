using MatchMasterWEB.ControllerServices.In_DepthControllerServices;
using MatchMasterWEB.Database;
using MatchMasterWEB.DTO_Models.In_DepthAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MatchMasterWEB.Controllers.In_DepthControllers
{
	[Route("api/v1/ratingbytemperature")]
	[ApiController]
	public class RatingByTemperatureController : ControllerBase
	{
		// Create a private readonly MatchMasterMySqlDatabaseContext
		private readonly MatchMasterMySqlDatabaseContext _dbContext;
		// Create a constructor for the PlayerCardController
		public RatingByTemperatureController(MatchMasterMySqlDatabaseContext dbContext)
		{
			_dbContext = dbContext;
		}
		[HttpGet]
		public IActionResult Get(int playerId)
		{
			// Create an instance of the RatingByTemperatureControllerService
			RatingByTemperatureControllerService ratingByTemperatureControllerService = new RatingByTemperatureControllerService();

			// Pass the playerId to the GetDegreeRatings method
			DTO_DegreeRating[] degreeRatings = ratingByTemperatureControllerService.GetDegreeRatings(playerId, _dbContext);

			return Ok(degreeRatings);
		}
	}
}
