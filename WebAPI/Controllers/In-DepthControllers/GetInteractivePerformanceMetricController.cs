using MatchMasterWEB.ControllerServices.In_DepthControllerServices;
using MatchMasterWEB.Database;
using MatchMasterWEB.DTO_Models.In_DepthAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MatchMasterWEB.Controllers.In_DepthControllers
{
	[Route("api/v1/interactiveperformancemetric")]
	[ApiController]
	public class GetInteractivePerformanceMetricController : ControllerBase
	{
		// Create a private readonly MatchMasterMySqlDatabaseContext
		private readonly MatchMasterMySqlDatabaseContext _dbContext;
		// Create a constructor for the PlayerCardController
		public GetInteractivePerformanceMetricController(MatchMasterMySqlDatabaseContext dbContext)
		{
			_dbContext = dbContext;
		}
		[HttpGet]
		public IActionResult Get(int playerId, int lowTemp, int highTemp)
		{
			GetInteractiveMetricControllerService getInteractiveMetricControllerService = new GetInteractiveMetricControllerService();

			DTO_GetPerformanceMetrics dTO_GetPerformanceMetrics = getInteractiveMetricControllerService.GetPerformanceMetrics(playerId, lowTemp, highTemp, _dbContext);

			return Ok(dTO_GetPerformanceMetrics);
		}
	}
}
