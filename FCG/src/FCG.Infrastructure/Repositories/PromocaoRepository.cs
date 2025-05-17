using FCG.Domain.Entities;
using FCG.Domain.Interfaces;
using FCG.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Repositories
{
    public class PromocaoRepository : RepositoryBase<Promocao>, IPromocaoRepository
    {
        private readonly ApplicationDbContext _context;

        public PromocaoRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Promocao>> ObterTodos()
        {
            return await _context.Promocoes.AsNoTracking()
                .Where(p => p.DataValidade >= DateTime.Now).ToListAsync();
        }

        public override async Task<IEnumerable<Promocao>> ObterTodosAtivos()
        {
            return await _context.Promocoes.AsNoTracking()
                .Where(p => p.Ativo && p.DataValidade >= DateTime.Now).ToListAsync();
        }

        public async Task<bool> Existe(string cupom)
        {
            return await _context.Promocoes
                .AnyAsync(p => p.Cupom == cupom && p.DataValidade >= DateTime.Now);
        }

        public async Task<Promocao?> ObterPorCupom(string cupom)
        {
            return await _context.Promocoes
                .FirstOrDefaultAsync(p => p.Cupom == cupom && p.DataValidade >= DateTime.Now);
        }
    }
}
