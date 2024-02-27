using MatchMasterWEB.DTO_Models.SquadOverview;

namespace MatchMasterWEB.ControllerServices
{
	public class SquadOverviewControllerService
	{
		public DTO_GetSquadOverviewPlayers GetSquadOverview(string position)
		{

			return new DTO_GetSquadOverviewPlayers();
		}
	}
}
