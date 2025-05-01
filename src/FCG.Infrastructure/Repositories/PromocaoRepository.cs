using FCG.Domain.Entities;
using FCG.Domain.Interfaces;
using FCG.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Repositories
{
    public class PromocaoRepository : EFRepository<Promocao>, IPromocaoRepository
    {
        private readonly ApplicationDbContext _context;

        public PromocaoRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExisteCupomAsync(string cupom)
        {
            return await _context.Promocoes
                .AnyAsync(p => p.Cupom.Equals(cupom) && p.DataValidade >= DateTime.Now);
        }

        public override async Task<IEnumerable<Promocao>> ObterTodosAsync()
        {
            return await _context.Promocoes.AsNoTracking()
                .Where(p => p.DataValidade >= DateTime.Now).ToListAsync();
        }
    }
}
