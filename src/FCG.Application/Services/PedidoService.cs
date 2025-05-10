using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using FCG.Application.Mappers;
using FCG.Domain.Entities;
using FCG.Domain.Enums;
using FCG.Domain.Exceptions;
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

        public async Task<PedidoResponseDto> ObterPedidoAsync(Guid pedidoId, Guid usuarioId)
        {
            var pedido = await _pedidoRepository.ObterPorIdAsync(pedidoId);

            if(pedido == null || (!usuarioId.Equals(Guid.Empty) && !pedido.UsuarioId.Equals(usuarioId)))
                throw new KeyNotFoundException("Pedido não encontrado com o Id informado");

            return pedido.ToDto();
        }

        public async Task<IEnumerable<PedidoResponseDto>> ObterPedidosAsync(Guid usuarioId)
        {
            var pedidos = await _pedidoRepository.ObterPedidosAsync(usuarioId);

            return pedidos.Select(p => p.ToDto());
        }

        public async Task AdicionarPedido(PedidoAdicionarDto pedidoDto)
        {
            var pedido = pedidoDto.ToDomain();
            if(await _pedidoRepository.ExistePedidoAsync(pedido))
                throw new OperacaoInvalidaException("Já existe um pedido com as mesmas informações");

            await CalcularValorPedido(pedido, pedidoDto.Cupom);

            await _pedidoRepository.Adicionar(pedido);
        }

        public async Task AlterarPedido(PedidoAlterarDto pedidoDto)
        {
            var pedido = pedidoDto.ToDomain();
            if (await _pedidoRepository.ExistePedidoAsync(pedido))
                throw new OperacaoInvalidaException("Já existe um pedido com as mesmas informações");

            await CalcularValorPedido(pedido, pedidoDto.Cupom);

            await _pedidoRepository.Alterar(pedidoDto.ToDomain());
        }

        public async Task AtivarPedido(Guid pedidoId)
        {
            await _pedidoRepository.Ativar(pedidoId);
        }

        public async Task DesativarPedido(Guid pedidoId)
        {
            await _pedidoRepository.Desativar(pedidoId);
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
    }
}
