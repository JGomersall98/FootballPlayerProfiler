using MatchMasterWEB.Database;
using MatchMasterWebAPI.ControllerServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MatchMasterWebAPI.Controllers
{
	[ApiController]
	[Route("api/v1/updatedatabase")]
	public class UpdateDatabaseController : ControllerBase
	{
		private readonly MatchMasterMySqlDatabaseContext _dbContext;
		public UpdateDatabaseController(MatchMasterMySqlDatabaseContext dbContext)
		{
			_dbContext = dbContext;
		}

		[HttpPost]
		public async Task<IActionResult> Get()
		{
			//TODO: Add code to update the database
			UpdateDatabaseControllerService updateDatabaseControllerService = new UpdateDatabaseControllerService();
			string result = await updateDatabaseControllerService.UpdateDatabaseAsync(_dbContext);

			return Ok(result);
		}
	}
}
