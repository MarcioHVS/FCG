using Dapper;
using FCG.Domain.Entities;
using FCG.Domain.Interfaces;
using FCG.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Repositories
{
    public class PedidoRepository : RepositoryBase<Pedido>, IPedidoRepository
    {
        private readonly ApplicationDbContext _context;

        public PedidoRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<Pedido?> ObterPorId(Guid id)
        {
            return await _context.Pedidos.AsNoTracking()
                .Include(p => p.Usuario)
                .Include(p => p.Jogo)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<bool> Existe(Pedido pedido)
        {
            return await _context.Pedidos
                .AnyAsync(p => p.Id != pedido.Id && p.UsuarioId == pedido.UsuarioId && p.JogoId == pedido.JogoId);
        }

        public async Task<IEnumerable<Pedido>> ObterTodosPorUsuario(Guid usuarioId)
        {
            return await _context.Pedidos.AsNoTracking()
                .Include(p => p.Usuario)
                .Include(p => p.Jogo)
                .Where(e => e.UsuarioId == usuarioId).ToListAsync();
        }
    }
}
