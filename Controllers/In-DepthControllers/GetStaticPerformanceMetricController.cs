using MatchMasterWEB.ControllerServices.In_DepthControllerServices;
using MatchMasterWEB.Database;
using MatchMasterWEB.DTO_Models.In_DepthAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MatchMasterWEB.Controllers.In_DepthControllers
{
	[Route("api/v1/staticperformancemetriccontroller")]
	[ApiController]
	public class GetStaticPerformanceMetricController : ControllerBase
	{
		// Create a private readonly MatchMasterMySqlDatabaseContext
		private readonly MatchMasterMySqlDatabaseContext _dbContext;
		// Create a constructor for the PlayerCardController
		public GetStaticPerformanceMetricController(MatchMasterMySqlDatabaseContext dbContext)
		{
			_dbContext = dbContext;
		}
		[HttpGet]
		public IActionResult Get(int playerId)
		{
			GetStaticMetricControllerService getStaticMetricControllerService = new GetStaticMetricControllerService();

			DTO_PerformanceMetric performanceMetric = getStaticMetricControllerService.GetStaticPerformanceMetric(playerId, _dbContext);

			return Ok(performanceMetric);
		}
	}
}
