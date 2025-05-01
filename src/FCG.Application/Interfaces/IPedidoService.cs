using FCG.Application.Entities;

namespace FCG.Application.Interfaces
{
    public interface IPedidoService
    {
        Task<PedidoDto> ObterPedidoAsync(Guid pedidoId);
        Task<IEnumerable<PedidoDto>> ObterPedidosAsync(Guid usuarioId);
        Task AdicionarPedido(PedidoDto pedidoDto);
        Task AlterarPedido(PedidoDto pedidoDto);
        Task AtivarPedido(Guid pedidoId);
        Task DesativarPedido(Guid pedidoId);
    }
}
