using FCG.Application.DTOs;

namespace FCG.Application.Interfaces
{
    public interface IPedidoService
    {
        Task<PedidoResponseDto> ObterPedidoAsync(Guid pedidoId, Guid usuarioId);
        Task<IEnumerable<PedidoResponseDto>> ObterPedidosAsync(Guid usuarioId);
        Task AdicionarPedido(PedidoAdicionarDto pedidoDto);
        Task AlterarPedido(PedidoAlterarDto pedidoDto);
        Task AtivarPedido(Guid pedidoId);
        Task DesativarPedido(Guid pedidoId);
    }
}
