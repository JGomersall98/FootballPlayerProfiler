using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MatchMasterWebAPI.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class UpdateDatabaseController : ControllerBase
	{
		[HttpPost]
		public IActionResult Get()
		{
			//TODO: Add code to update the database




			return Ok();
		}
	}
}
