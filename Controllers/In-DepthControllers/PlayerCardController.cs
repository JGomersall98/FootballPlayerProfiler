using MatchMasterWEB.ControllerServices.In_DepthControllerServices;
using MatchMasterWEB.Database;
using MatchMasterWEB.DTO_Models.SquadOverview;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MatchMasterWEB.Controllers.In_DepthControllers
{
	[Route("api/v1/playercard")]
	[ApiController]
	public class PlayerCardController : ControllerBase
	{
		// Create a private readonly MatchMasterMySqlDatabaseContext
		private readonly MatchMasterMySqlDatabaseContext _dbContext;
		// Create a constructor for the PlayerCardController
		public PlayerCardController(MatchMasterMySqlDatabaseContext dbContext)
		{
			_dbContext = dbContext;
		}

		[HttpGet]
		public IActionResult Get(int playerId)
		{
			// Create an instance of the PlayerCardControllerService
			PlayerCardControllerService playerCardControllerService = new PlayerCardControllerService();

			// Pass the playerId to the GetPlayerCard method
			DTO_SquadOverviewPlayer playerCard = playerCardControllerService.GetPlayerCard(playerId, _dbContext);

			// Return the playerCard
			return Ok(playerCard);
		}
	}
}
