using FCG.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public required DbSet<Usuario> Usuarios { get; set; }
        public required DbSet<Jogo> Jogos { get; set; }
        public required DbSet<Pedido> Pedidos { get; set; }
        public required DbSet<Promocao> Promocoes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        public async Task Salvar()
        {
            foreach (var entry in ChangeTracker.Entries()
                .Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DataCadastro").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DataCadastro").IsModified = false;
                }
            }

            var salvo = await base.SaveChangesAsync() > 0;

            if (!salvo)
                throw new DbUpdateException("Houve um erro ao tentar persistir os dados");
        }
    }
}
