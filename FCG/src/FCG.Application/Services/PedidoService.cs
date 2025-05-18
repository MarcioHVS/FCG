using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using FCG.Application.Mappers;
using FCG.Domain.Entities;
using FCG.Domain.Enums;
using FCG.Domain.Interfaces;

namespace FCG.Application.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly ValidationService _validationService;


        public PedidoService(IPedidoRepository pedidoRepository,
                             ValidationService validationService)
        {
            _pedidoRepository = pedidoRepository;
            _validationService = validationService; 
        }

        public async Task<PedidoResponseDto> ObterPedido(Guid pedidoId, Guid usuarioId)
        {
            var pedido = await _pedidoRepository.ObterPorId(pedidoId);

            if(pedido == null || (usuarioId != Guid.Empty && pedido.UsuarioId != usuarioId))
                throw new KeyNotFoundException("Pedido não encontrado com o Id informado");

            return pedido.ToDto();
        }

        public async Task<IEnumerable<PedidoResponseDto>> ObterPedidos()
        {
            var pedidos = await _pedidoRepository.ObterTodos();

            return pedidos.Select(p => p.ToDto());
        }

        public async Task<IEnumerable<PedidoResponseDto>> ObterPedidosAtivos(Guid usuarioId)
        {
            var pedidos = usuarioId == Guid.Empty
                                     ? await _pedidoRepository.ObterTodosAtivos()
                                     : await _pedidoRepository.ObterTodosPorUsuario(usuarioId);

            return pedidos.Select(p => p.ToDto());
        }

        public async Task AdicionarPedido(PedidoAdicionarDto pedidoDto)
            => await ProcessarPedido(pedidoDto.ToDomain(), pedidoDto.Cupom, _pedidoRepository.Adicionar);

        public async Task AlterarPedido(PedidoAlterarDto pedidoDto)
            => await ProcessarPedido(pedidoDto.ToDomain(), pedidoDto.Cupom, _pedidoRepository.Alterar);

        public async Task AtivarPedido(Guid pedidoId)
            => await _pedidoRepository.Ativar(pedidoId);

        public async Task DesativarPedido(Guid pedidoId)
            => await _pedidoRepository.Desativar(pedidoId);

        #region Métodos Privados
        private async Task ProcessarPedido(Pedido pedido, string cupom, Func<Pedido, Task> operacao)
        {
            if (await _pedidoRepository.Existe(pedido))
                throw new Exception("Já existe um pedido com as mesmas informações.");

            await CalcularValorPedido(pedido, cupom);
            await operacao(pedido);
        }

        private async Task CalcularValorPedido(Pedido pedido, string cupom)
        {
            var jogo = await _validationService.ObterJogoValido(pedido.JogoId);
            var usuario = await _validationService.ObterUsuarioValido(pedido.UsuarioId);
            var promocao = string.IsNullOrEmpty(cupom) ? null : await _validationService.ObterPromocaoValida(cupom);

            pedido.CalcularValor(jogo.Valor, 
                                 promocao?.TipoDesconto ?? TipoDesconto.Moeda, 
                                 promocao?.ValorDesconto ?? 0);
        }
        #endregion
    }
}
