using FCG.Application.DTOs;

namespace FCG.Application.Interfaces
{
    public interface IPedidoService
    {
        Task<PedidoResponseDto> ObterPedido(Guid pedidoId, Guid usuarioId);
        Task<IEnumerable<PedidoResponseDto>> ObterPedidos();
        Task<IEnumerable<PedidoResponseDto>> ObterPedidosAtivos(Guid usuarioId);
        Task AdicionarPedido(PedidoAdicionarDto pedidoDto);
        Task AlterarPedido(PedidoAlterarDto pedidoDto);
        Task AtivarPedido(Guid pedidoId);
        Task DesativarPedido(Guid pedidoId);
    }
}
