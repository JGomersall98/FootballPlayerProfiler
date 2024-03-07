using MatchMasterWEB.Database.DB_Models;

namespace MatchMasterWEB.ControllerServices.GetRatingMethods
{
	public class GetAverageRatingFromTemp
	{
		internal double CalculateAverageRatingFromTemp(IEnumerable<Fixture> fixtures, PlayerStat[] playerStats)
		{
			double totalRating = fixtures
				.Select(fixture => playerStats?.FirstOrDefault(ps => ps.FixtureId == fixture.FixtureId)?.Rating)
				.Where(rating => rating.HasValue && rating.Value > 0)
				.Sum(rating => rating.Value);

			int count = fixtures
				.Select(fixture => playerStats.FirstOrDefault(ps => ps.FixtureId == fixture.FixtureId)?.Rating)
				.Count(rating => rating.HasValue && rating.Value > 0);

			return count > 0 ? totalRating / count : 0;
		}
	}
}
