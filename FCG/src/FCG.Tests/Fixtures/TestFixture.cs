using FCG.Domain.Interfaces;
using FCG.Infrastructure.Contexts;
using FCG.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FCG.Tests.Fixtures
{
    public class TestFixture : IDisposable
    {
        public TestDbContext Context { get; private set; }
        public IPedidoRepository PedidoRepository { get; private set; }

        public TestFixture()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite("Data Source=:memory:")
                .Options;

            Context = new TestDbContext(options);
            Context.Database.OpenConnection();
            Context.Database.EnsureCreated();

            ResetDatabase();
            PedidoRepository = new PedidoRepository(Context);
        }

        public void ResetDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            Context = new TestDbContext(options);
            Context.Database.OpenConnection();
            Context.Database.EnsureCreated();
            Context.Database.ExecuteSqlRaw("PRAGMA foreign_keys = ON;");
        }

        public void Dispose()
        {
            Context.Database.CloseConnection();
            Context.Dispose();
        }
    }
}
