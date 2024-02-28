using MatchMasterWEB.Database;
using MatchMasterWEB.DTO_Models.SquadOverview;

namespace MatchMasterWEB.ControllerServices.In_DepthControllerServices
{
	public class PlayerCardControllerService
	{
		public DTO_SquadOverviewPlayer GetPlayerCard(int playerId, MatchMasterMySqlDatabaseContext dbContext)
		{

			return new DTO_SquadOverviewPlayer { };
		}
	}
}
