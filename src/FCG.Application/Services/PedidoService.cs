using FCG.Application.Entities;
using FCG.Application.Interfaces;
using FCG.Domain.Interfaces;

namespace FCG.Application.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoService(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<PedidoDto> ObterPedidoAsync(Guid pedidoId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PedidoDto>> ObterPedidosAsync(Guid usuarioId)
        {
            throw new NotImplementedException();
        }

        public async Task AdicionarPedido(PedidoDto pedidoDto)
        {
            throw new NotImplementedException();
        }

        public async Task AlterarPedido(PedidoDto pedidoDto)
        {
            throw new NotImplementedException();
        }

        public async Task AtivarPedido(Guid pedidoId)
        {
            throw new NotImplementedException();
        }

        public async Task DesativarPedido(Guid pedidoId)
        {
            throw new NotImplementedException();
        }
    }
}
