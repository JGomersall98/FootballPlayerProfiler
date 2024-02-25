using Microsoft.EntityFrameworkCore;
using temperatureVariationAnalysis.Database.DB_Models;

namespace temperatureVariationAnalysis.Database
{
	public class MatchMasterMySqlDatabaseContext : DbContext
	{
		public MatchMasterMySqlDatabaseContext(DbContextOptions<MatchMasterMySqlDatabaseContext> options) : base(options)
		{

		}

		public DbSet<Fixture> Fixtures { get; set; }
		public DbSet<Player> Players { get; set; }
		public DbSet<PlayerStat> PlayerStats { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Player
			modelBuilder.Entity<Player>()
				.HasKey(p => p.PlayerId);

			// PlayerStat
			modelBuilder.Entity<PlayerStat>()
				.HasKey(ps => ps.StatId);

			modelBuilder.Entity<PlayerStat>()
				.HasOne(ps => ps.Fixture)
				.WithMany(f => f.PlayerStats)
				.HasForeignKey(ps => ps.FixtureId);

			modelBuilder.Entity<PlayerStat>()
				.HasOne(ps => ps.Player)
				.WithMany(p => p.PlayerStats)
				.HasForeignKey(ps => ps.PlayerId);
		}
	}
}
