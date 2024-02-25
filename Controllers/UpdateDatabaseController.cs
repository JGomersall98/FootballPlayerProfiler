using MatchMasterWebAPI.ControllerServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MatchMasterWebAPI.Controllers
{
	[Route("api/v1/test")]
	[ApiController]
	public class UpdateDatabaseController : ControllerBase
	{
		[HttpPost]
		public IActionResult Get()
		{
			//TODO: Add code to update the database
			UpdateDatabaseControllerService updateDatabaseControllerService = new UpdateDatabaseControllerService();
			updateDatabaseControllerService.UpdateDatabase();

			return Ok();
		}
	}
}
