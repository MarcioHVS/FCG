using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Context
{
    public class TestDbContext : ApplicationDbContext
    {
        public TestDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
