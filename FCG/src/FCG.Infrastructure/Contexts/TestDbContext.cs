using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Contexts
{
    public class TestDbContext : ApplicationDbContext
    {
        public TestDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
