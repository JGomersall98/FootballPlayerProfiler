using MatchMasterWEB.ControllerServices;
using MatchMasterWEB.Database;
using MatchMasterWEB.DTO_Models.SquadOverview;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MatchMasterWEB.Controllers
{
	[Route("api/v1/squadOverview")]
	[ApiController]
	public class SquadOverviewController : ControllerBase
	{
		// Create a private readonly MatchMasterMySqlDatabaseContext
		private readonly MatchMasterMySqlDatabaseContext _dbContext;
		// Create a constructor for the SquadOverviewController
		public SquadOverviewController(MatchMasterMySqlDatabaseContext dbContext)
		{
			_dbContext = dbContext;
		}

		[HttpGet]
		public IActionResult Get(string position)
		{
			// Create an instance of the SquadOverviewControllerService
			SquadOverviewControllerService squadOverviewControllerService = new SquadOverviewControllerService();

			// Pass the position to the GetSquadOverview method
			DTO_GetSquadOverviewPlayers dTO_GetSquadOverviewPlayers = squadOverviewControllerService.GetSquadOverview(position, _dbContext);

			// Return the result
			return Ok(dTO_GetSquadOverviewPlayers);
		}
	}
}
