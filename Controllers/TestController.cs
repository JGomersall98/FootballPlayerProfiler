using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MatchMasterWEB.Controllers
{
	[Route("test")]
	[ApiController]
	public class TestController : ControllerBase
	{
		[HttpGet]
		public IActionResult Get()
		{
			return Ok("Test");
		}
	}
}
