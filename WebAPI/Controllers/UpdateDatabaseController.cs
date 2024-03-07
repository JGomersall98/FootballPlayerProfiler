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
		// Create a private readonly MatchMasterMySqlDatabaseContext
		private readonly MatchMasterMySqlDatabaseContext _dbContext;
		// Create a constructor for the UpdateDatabaseController
		public UpdateDatabaseController(MatchMasterMySqlDatabaseContext dbContext)
		{
			_dbContext = dbContext;
		}

		[HttpPost]
		public async Task<IActionResult> Get()
		{
			// Create an instance of the UpdateDatabaseControllerService
			UpdateDatabaseControllerService updateDatabaseControllerService = new UpdateDatabaseControllerService();

			// Pass the database context to the UpdateDatabaseAsync method
			string result = await updateDatabaseControllerService.UpdateDatabaseAsync(_dbContext);

			DTO_Tester dTO_Tester = new DTO_Tester
			{
				Test = result
			};
			// Return the result
			return Ok(dTO_Tester);
		}
		public class DTO_Tester
		{
			public string Test { get; set; }
		}
	}
}
