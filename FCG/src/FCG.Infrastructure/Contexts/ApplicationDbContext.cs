using FCG.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Jogo> Jogos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Promocao> Promocoes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        public async Task<bool> Salvar(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                if (entry.State == EntityState.Added)
                    entry.Property("DataCadastro").CurrentValue = DateTime.UtcNow;

                if (entry.State == EntityState.Modified)
                    entry.Property("DataCadastro").IsModified = false;
            }

            foreach (var entry in ChangeTracker.Entries<Usuario>())
            {
                var senhaProperty = entry.Property(nameof(Usuario.Senha));
                if (entry.State == EntityState.Modified && senhaProperty.CurrentValue is string senha && string.IsNullOrEmpty(senha))
                    senhaProperty.IsModified = false;

                var saltProperty = entry.Property(nameof(Usuario.Salt));
                if (entry.State == EntityState.Modified && saltProperty.CurrentValue is string salt && string.IsNullOrEmpty(salt))
                    saltProperty.IsModified = false;
            }

            var salvo = await base.SaveChangesAsync(cancellationToken) > 0;

            if (!salvo)
                throw new DbUpdateException("Houve um erro ao tentar persistir os dados");

            return salvo;
        }
    }
}