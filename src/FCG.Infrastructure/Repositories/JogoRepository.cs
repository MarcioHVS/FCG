using FCG.Domain.Entities;
using FCG.Domain.Interfaces;
using FCG.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Repositories
{
    public class JogoRepository : EFRepository<Jogo>, IJogoRepository
    {
        private readonly ApplicationDbContext _context;

        public JogoRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExisteTituloAsync(string titulo)
        {
            return await _context.Jogos.AsNoTracking()
                .Where(j => j.Titulo.Equals(titulo)).AnyAsync();
        }
    }
}
