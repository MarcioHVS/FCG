using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using FCG.Application.Services;
using FCG.Domain.Entities;
using FCG.Domain.Enums;
using FCG.Domain.Interfaces;
using Moq;
using Xunit;

namespace FCG.Tests.UnitTests.ServicesTests
{
    public class PedidoServiceTests
    {
        private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly Mock<IJogoRepository> _jogoRepositoryMock;
        private readonly Mock<IPromocaoRepository> _promocaoRepositoryMock;
        private readonly ValidationService _validationService;
        private readonly IPedidoService _pedidoService;

        public PedidoServiceTests()
        {
            _pedidoRepositoryMock = new Mock<IPedidoRepository>();
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _jogoRepositoryMock = new Mock<IJogoRepository>();
            _promocaoRepositoryMock = new Mock<IPromocaoRepository>();

            _validationService = new ValidationService(
                _usuarioRepositoryMock.Object,
                _jogoRepositoryMock.Object,
                _promocaoRepositoryMock.Object
            );

            _pedidoService = new PedidoService(
                _pedidoRepositoryMock.Object,
                _validationService
            );
        }

        #region Obter Pedido
        [Fact]
        public async Task ObterPedido_Existente_DeveRetornarPedido()
        {
            // Arrange
            var usuario = Usuario.Adicionar("Maria Luiza", "Malu", "malu@email.com", "SenhaValida", Role.Usuario);
            usuario.Ativar();
            var usuarioId = usuario.Id;

            var jogo = Jogo.Adicionar("Título Teste", "Descrição Teste", Genero.Aventura, 99.99m);
            jogo.Ativar();
            var jogoId = jogo.Id;

            var pedido = Pedido.Adicionar(usuarioId, jogoId);
            pedido.Usuario = usuario;
            pedido.Jogo = jogo;
            pedido.Ativar();

            var pedidoId = pedido.Id;
            _pedidoRepositoryMock.Setup(repo => repo.ObterPorIdAsync(pedidoId)).ReturnsAsync(pedido);

            // Act
            var resultado = await _pedidoService.ObterPedidoAsync(pedidoId, usuarioId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(pedidoId, resultado.Id);
            Assert.NotNull(resultado.Usuario);
            Assert.NotNull(resultado.Jogo);
            Assert.Equal(usuarioId, resultado.Usuario.Id);
            Assert.Equal(jogoId, resultado.Jogo.Id);
        }

        [Fact]
        public async Task ObterPedido_Inexistente_DeveLancarExcecao()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();

            _pedidoRepositoryMock.Setup(repo => repo.ObterPorIdAsync(pedidoId)).ReturnsAsync((Pedido)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _pedidoService.ObterPedidoAsync(pedidoId, usuarioId));
        }
        #endregion

        #region Obter Pedidos
        [Fact]
        public async Task ObterPedidos_PorUsuario_DeveRetornarLista()
        {
            // Arrange
            var usuario = Usuario.Adicionar("Maria Luiza", "Malu", "malu@email.com", "SenhaValida", Role.Usuario);
            usuario.Ativar();
            var usuarioId = usuario.Id;

            var jogo1 = Jogo.Adicionar("Título Teste 1", "Descrição Teste 1", Genero.Aventura, 99.99m);
            var jogo2 = Jogo.Adicionar("Título Teste 2", "Descrição Teste 2", Genero.Acao, 59.99m);
            jogo1.Ativar();
            jogo2.Ativar();

            var pedidos = new List<Pedido>
            {
                Pedido.Adicionar(usuarioId, jogo1.Id),
                Pedido.Adicionar(usuarioId, jogo2.Id)
            };

            pedidos[0].Usuario = usuario;
            pedidos[0].Jogo = jogo1;
            pedidos[1].Usuario = usuario;
            pedidos[1].Jogo = jogo2;

            _pedidoRepositoryMock.Setup(repo => repo.ObterPedidosPorUsuarioAsync(usuarioId)).ReturnsAsync(pedidos);

            // Act
            var resultado = await _pedidoService.ObterPedidosAsync(usuarioId);

            // Assert
            Assert.NotNull(resultado);
            Assert.NotEmpty(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.All(resultado, p => Assert.Equal(usuarioId, p.Usuario.Id));
        }

        [Fact]
        public async Task ObterPedidos_Geral_DeveRetornarLista()
        {
            // Arrange
            var usuario1 = Usuario.Adicionar("Maria Luiza", "Malu", "malu@email.com", "SenhaValida", Role.Usuario);
            var usuario2 = Usuario.Adicionar("Francisco Henrique", "Chico", "chico@email.com", "SenhaValida", Role.Usuario);
            usuario1.Ativar();
            usuario2.Ativar();

            var jogo1 = Jogo.Adicionar("Título Teste 1", "Descrição Teste 1", Genero.Aventura, 99.99m);
            var jogo2 = Jogo.Adicionar("Título Teste 2", "Descrição Teste 2", Genero.Acao, 59.99m);
            jogo1.Ativar();
            jogo2.Ativar();

            var pedidos = new List<Pedido>
            {
                Pedido.Adicionar(usuario1.Id, jogo1.Id),
                Pedido.Adicionar(usuario2.Id, jogo2.Id)
            };

            pedidos[0].Usuario = usuario1;
            pedidos[0].Jogo = jogo1;
            pedidos[1].Usuario = usuario2;
            pedidos[1].Jogo = jogo2;

            _pedidoRepositoryMock.Setup(repo => repo.ObterTodosAsync()).ReturnsAsync(pedidos);

            // Act
            var resultado = await _pedidoService.ObterPedidosAsync(Guid.Empty);

            // Assert
            Assert.NotNull(resultado);
            Assert.NotEmpty(resultado);
            Assert.Equal(2, resultado.Count());
        }

        [Fact]
        public async Task ObterPedidos_SemPedidos_DeveRetornarListaVazia()
        {
            // Arrange
            _pedidoRepositoryMock.Setup(repo => repo.ObterTodosAsync()).ReturnsAsync(new List<Pedido>());

            // Act
            var resultado = await _pedidoService.ObterPedidosAsync(Guid.Empty);

            // Assert
            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }
        #endregion

        #region Adicionar Pedido
        [Fact]
        public async Task AdicionarPedido_ComSucesso_DeveAdicionar()
        {
            // Arrange
            var usuario = Usuario.Adicionar("Márcio", "marciohvs", "email@email.com", "SenhaSegura", Role.Usuario);
            var jogo = Jogo.Adicionar("Jogo Teste", "Descrição Teste", Genero.Aventura, 79.99m);
            var pedidoDto = new PedidoAdicionarDto { UsuarioId = usuario.Id, JogoId = jogo.Id, Cupom = "" };

            _pedidoRepositoryMock.Setup(repo => repo.ExistePedidoAsync(It.IsAny<Pedido>())).ReturnsAsync(false);
            _pedidoRepositoryMock.Setup(repo => repo.Adicionar(It.IsAny<Pedido>())).Returns(Task.CompletedTask);
            _jogoRepositoryMock.Setup(repo => repo.ObterPorIdAsync(jogo.Id)).ReturnsAsync(jogo);
            _usuarioRepositoryMock.Setup(repo => repo.ObterPorIdAsync(usuario.Id)).ReturnsAsync(usuario);

            // Act
            await _pedidoService.AdicionarPedido(pedidoDto);

            // Assert
            _pedidoRepositoryMock.Verify(repo => repo.Adicionar(It.IsAny<Pedido>()), Times.Once);
        }

        [Fact]
        public async Task AdicionarPedido_JogoInexistente_DeveLancarExcecao()
        {
            // Arrange
            var pedidoDto = new PedidoAdicionarDto { UsuarioId = Guid.NewGuid(), JogoId = Guid.NewGuid(), Cupom = "" };

            _jogoRepositoryMock.Setup(repo => repo.ObterPorIdAsync(pedidoDto.JogoId)).ReturnsAsync((Jogo)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _pedidoService.AdicionarPedido(pedidoDto));
        }
        #endregion

        #region Alterar Pedido
        [Fact]
        public async Task AlterarPedido_ComSucesso_DeveAtualizar()
        {
            // Arrange
            var usuario = Usuario.Adicionar("Márcio", "marciohvs", "email@email.com", "SenhaSegura", Role.Usuario);
            var jogo = Jogo.Adicionar("Jogo Teste", "Descrição Teste", Genero.Aventura, 79.99m);
            var pedidoDto = new PedidoAlterarDto { Id = Guid.NewGuid(), UsuarioId = usuario.Id, JogoId = jogo.Id, Cupom = "" };

            _pedidoRepositoryMock.Setup(repo => repo.ExistePedidoAsync(It.IsAny<Pedido>())).ReturnsAsync(false);
            _pedidoRepositoryMock.Setup(repo => repo.Alterar(It.IsAny<Pedido>())).Returns(Task.CompletedTask);
            _jogoRepositoryMock.Setup(repo => repo.ObterPorIdAsync(jogo.Id)).ReturnsAsync(jogo);
            _usuarioRepositoryMock.Setup(repo => repo.ObterPorIdAsync(usuario.Id)).ReturnsAsync(usuario);

            // Act
            await _pedidoService.AlterarPedido(pedidoDto);

            // Assert
            _pedidoRepositoryMock.Verify(repo => repo.Alterar(It.IsAny<Pedido>()), Times.Once);
        }
        #endregion

        #region Ativar e Desativar Pedido
        [Fact]
        public async Task AtivarPedido_DeveSerChamado()
        {
            // Act
            await _pedidoService.AtivarPedido(Guid.NewGuid());

            // Assert
            _pedidoRepositoryMock.Verify(repo => repo.Ativar(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task DesativarPedido_DeveSerChamado()
        {
            // Act
            await _pedidoService.DesativarPedido(Guid.NewGuid());

            // Assert
            _pedidoRepositoryMock.Verify(repo => repo.Desativar(It.IsAny<Guid>()), Times.Once);
        }
        #endregion
    }
}
