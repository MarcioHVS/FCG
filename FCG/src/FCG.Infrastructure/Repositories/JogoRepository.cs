using FCG.Domain.Entities;
using FCG.Domain.Interfaces;
using FCG.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Repositories
{
    public class JogoRepository : RepositoryBase<Jogo>, IJogoRepository
    {
        private readonly ApplicationDbContext _context;

        public JogoRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> Existe(string titulo)
        {
            return await _context.Jogos.AsNoTracking()
                .Where(j => j.Titulo == titulo).AnyAsync();
        }

        public async Task<Jogo?> ObterPorTitulo(string titulo)
        {
            return await _context.Jogos.AsNoTracking()
                .FirstOrDefaultAsync(j => j.Titulo == titulo);
        }
    }
}
