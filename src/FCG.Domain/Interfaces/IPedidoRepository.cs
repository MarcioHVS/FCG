using FCG.Domain.Entities;

namespace FCG.Domain.Interfaces
{
    public interface IPedidoRepository
    {
        Task<bool> ExistePedidoAsync(Pedido pedido);
        Task<IEnumerable<Pedido>> ObterPedidosPorUsuarioAsync(Guid usuarioId);
    }
}
