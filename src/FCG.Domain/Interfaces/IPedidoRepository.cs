using FCG.Domain.Entities;

namespace FCG.Domain.Interfaces
{
    public interface IPedidoRepository
    {
        Task<Pedido> ObterPorIdAsync(Guid id);
        Task<IEnumerable<Pedido>> ObterTodosAsync();
        Task Adicionar(Pedido pedido);
        Task Alterar(Pedido pedido);
        Task Ativar(Guid id);
        Task Desativar(Guid id);

        Task<bool> ExistePedidoAsync(Pedido pedido);
        Task<IEnumerable<Pedido>> ObterPedidosAsync(Guid usuarioId);
    }
}
