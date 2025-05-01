using FCG.Domain.Entities;
using FCG.Domain.Interfaces;
using FCG.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Repositories
{
    public class PedidoRepository : EFRepository<Pedido>, IPedidoRepository
    {
        private readonly ApplicationDbContext _context;

        public PedidoRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistePedidoAsync(Pedido pedido)
        {
            return await _context.Pedidos
                .AnyAsync(p => p.UsuarioId == pedido.UsuarioId && p.JogoId == pedido.JogoId);
        }

        public async Task<IEnumerable<Pedido>> ObterPedidosPorUsuarioAsync(Guid usuarioId)
        {
            return await _context.Pedidos.AsNoTracking()
                .Where(p => p.UsuarioId == usuarioId).ToListAsync();
        }
    }
}
