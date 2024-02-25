using MatchMasterWebAPI.ControllerServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MatchMasterWebAPI.Controllers
{
	[ApiController]
	[Route("api/v1/updatedatabase")]
	public class UpdateDatabaseController : ControllerBase
	{
		[HttpPost]
		public IActionResult Get()
		{
			//TODO: Add code to update the database
			UpdateDatabaseControllerService updateDatabaseControllerService = new UpdateDatabaseControllerService();
			updateDatabaseControllerService.UpdateDatabase();
			string message = "Database updated successfully";
			return Ok(message);
		}
	}
}
