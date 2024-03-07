using MatchMasterWEB.Database;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Microsoft.Data.Sqlite;

namespace MatchMaster_UnitTest
{
    public class SimulateMockDatabaseForUnitTests : IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<MatchMasterMySqlDatabaseContext> _contextOptions;
        private readonly MatchMasterMySqlDatabaseContext _context;

        public SimulateMockDatabaseForUnitTests()
        {
            SQLitePCL.Batteries.Init();
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            _contextOptions = new DbContextOptionsBuilder<MatchMasterMySqlDatabaseContext>()
                .UseSqlite(_connection)
                .Options;

            var context = new MatchMasterMySqlDatabaseContext(_contextOptions);

            //Ensure the database is created successfully
            context.Database.EnsureCreated();

            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public MatchMasterMySqlDatabaseContext GetContext()
        {
            return _context;
        }


        public void Dispose() => _connection.Dispose();
    }
}
