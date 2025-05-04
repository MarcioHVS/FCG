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
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IJogoRepository _jogoRepository;
        private readonly IPromocaoRepository _promocaoRepository;

        public PedidoService(IPedidoRepository pedidoRepository,
                             IUsuarioRepository usuarioRepository,
                             IJogoRepository jogoRepository,
                             IPromocaoRepository promocaoRepository)
        {
            _pedidoRepository = pedidoRepository;
            _usuarioRepository = usuarioRepository;
            _jogoRepository = jogoRepository;
            _promocaoRepository = promocaoRepository;
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
            var pedidos = usuarioId.Equals(Guid.Empty)
                ? await _pedidoRepository.ObterTodosAsync()
                : await _pedidoRepository.ObterPedidosPorUsuarioAsync(usuarioId);

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
            var jogo = await ObterJogoValido(pedido.JogoId);
            var usuario = await ObterUsuarioValido(pedido.UsuarioId);
            var promocao = string.IsNullOrEmpty(cupom) ? null : await ObterPromocaoValida(cupom);

            pedido.CalcularValor(jogo.Valor, 
                                 promocao?.TipoDesconto ?? TipoDesconto.Moeda, 
                                 promocao?.ValorDesconto ?? 0);
        }

        private async Task<Jogo> ObterJogoValido(Guid jogoId)
        {
            var jogo = await _jogoRepository.ObterPorIdAsync(jogoId)
                ?? throw new KeyNotFoundException("Jogo não encontrado com o Id informado");

            if (!jogo.Ativo)
                throw new OperacaoInvalidaException("Este jogo não está disponível no momento");

            return jogo;
        }

        private async Task<Usuario> ObterUsuarioValido(Guid usuarioId)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId)
                ?? throw new KeyNotFoundException("Usuario não encontrado com o Id informado");

            if (!usuario.Ativo)
                throw new OperacaoInvalidaException("Este usuario não está disponível no momento");

            return usuario;
        }

        private async Task<Promocao?> ObterPromocaoValida(string cupom)
        {
            var promocao = await _promocaoRepository.ObterPromocaoPorCupomAsync(cupom);
            return promocao?.Ativo == true ? promocao : null;
        }
    }
}
