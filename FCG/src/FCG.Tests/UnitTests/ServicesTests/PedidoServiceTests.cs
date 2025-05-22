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
            var usuario = Usuario.CriarAlterar(null, "Maria Luiza", "Malu", "malu@email.com", "Senha@123");
            usuario.Ativar();

            var jogo = Jogo.CriarAlterar(null, "Super Mario Bros", "Uma jornada para salvar a Princesa Peach", Genero.Plataforma, 79.99m);
            jogo.Ativar();

            var pedido = Pedido.CriarAlterar(null, usuario.Id, jogo.Id);
            pedido.Usuario = usuario;
            pedido.Jogo = jogo;
            pedido.Ativar();

            var pedidoId = pedido.Id;
            _pedidoRepositoryMock.Setup(repo => repo.ObterPorId(pedidoId)).ReturnsAsync(pedido);

            // Act
            var resultado = await _pedidoService.ObterPedido(pedidoId, usuario.Id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(pedidoId, resultado.Id);
            Assert.NotNull(resultado.Usuario);
            Assert.NotNull(resultado.Jogo);
            Assert.Equal(usuario.Id, resultado.Usuario.Id);
            Assert.Equal(jogo.Id, resultado.Jogo.Id);
        }

        [Fact]
        public async Task ObterPedido_Inexistente_DeveLancarExcecao()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();

            _pedidoRepositoryMock.Setup(repo => repo.ObterPorId(pedidoId)).ReturnsAsync((Pedido)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _pedidoService.ObterPedido(pedidoId, usuarioId));
        }
        #endregion

        #region Obter Pedidos
        [Fact]
        public async Task ObterPedidos_Geral_DeveRetornarLista()
        {
            // Arrange
            var usuario1 = Usuario.CriarAlterar(null, "Maria Luiza", "Malu", "malu@email.com", "Senha@123");
            var usuario2 = Usuario.CriarAlterar(null, "Francisco Henrique", "Chico", "chico@email.com", "Senha@123");
            usuario1.Ativar();
            usuario2.Ativar();

            var jogo1 = Jogo.CriarAlterar(null, "Super Mario Bros", "Uma jornada para salvar a Princesa Peach", Genero.Plataforma, 79.99m);
            var jogo2 = Jogo.CriarAlterar(null, "Street Fighter", "Lutadores batalham em duelos épicos", Genero.Luta, 159.99m);
            jogo1.Ativar();
            jogo2.Ativar();

            var pedidos = new List<Pedido>
            {
                Pedido.CriarAlterar(null, usuario1.Id, jogo1.Id),
                Pedido.CriarAlterar(null, usuario2.Id, jogo2.Id)
            };

            pedidos[0].Usuario = usuario1;
            pedidos[0].Jogo = jogo1;
            pedidos[1].Usuario = usuario2;
            pedidos[1].Jogo = jogo2;
            pedidos[1].Desativar();

            _pedidoRepositoryMock.Setup(repo => repo.ObterTodos()).ReturnsAsync(pedidos);

            // Act
            var resultado = await _pedidoService.ObterPedidos();

            // Assert
            Assert.NotNull(resultado);
            Assert.NotEmpty(resultado);
            Assert.Equal(2, resultado.Count());
        }

        [Fact]
        public async Task ObterPedidos_SemPedidos_DeveRetornarListaVazia()
        {
            // Arrange
            _pedidoRepositoryMock.Setup(repo => repo.ObterTodos()).ReturnsAsync(new List<Pedido>());

            // Act
            var resultado = await _pedidoService.ObterPedidos();

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
            var usuario = Usuario.CriarAlterar(null, "Maria Luiza", "Malu", "malu@email.com", "Senha@123");
            var jogo = Jogo.CriarAlterar(null, "Super Mario Bros", "Uma jornada para salvar a Princesa Peach", Genero.Plataforma, 79.99m);
            jogo.Ativar();
            var pedidoDto = new PedidoAdicionarDto { UsuarioId = usuario.Id, JogoId = jogo.Id, Cupom = "" };

            _pedidoRepositoryMock.Setup(repo => repo.Existe(It.IsAny<Pedido>())).ReturnsAsync(false);
            _pedidoRepositoryMock.Setup(repo => repo.Adicionar(It.IsAny<Pedido>())).Returns(Task.CompletedTask);
            _jogoRepositoryMock.Setup(repo => repo.ObterPorId(jogo.Id)).ReturnsAsync(jogo);
            _usuarioRepositoryMock.Setup(repo => repo.ObterPorId(usuario.Id)).ReturnsAsync(usuario);

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

            _jogoRepositoryMock.Setup(repo => repo.ObterPorId(pedidoDto.JogoId)).ReturnsAsync((Jogo)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _pedidoService.AdicionarPedido(pedidoDto));
        }
        #endregion

        #region Alterar Pedido
        [Fact]
        public async Task AlterarPedido_ComSucesso_DeveAtualizar()
        {
            // Arrange
            var usuario = Usuario.CriarAlterar(null, "Maria Luiza", "Malu", "malu@email.com", "Senha@123");
            var jogo = Jogo.CriarAlterar(null, "Super Mario Bros", "Uma jornada para salvar a Princesa Peach", Genero.Plataforma, 79.99m);
            jogo.Ativar();
            var pedidoDto = new PedidoAlterarDto { Id = Guid.NewGuid(), UsuarioId = usuario.Id, JogoId = jogo.Id, Cupom = "" };

            _pedidoRepositoryMock.Setup(repo => repo.Existe(It.IsAny<Pedido>())).ReturnsAsync(false);
            _pedidoRepositoryMock.Setup(repo => repo.Alterar(It.IsAny<Pedido>(), false)).Returns(Task.CompletedTask);
            _jogoRepositoryMock.Setup(repo => repo.ObterPorId(jogo.Id)).ReturnsAsync(jogo);
            _usuarioRepositoryMock.Setup(repo => repo.ObterPorId(usuario.Id)).ReturnsAsync(usuario);

            // Act
            await _pedidoService.AlterarPedido(pedidoDto);

            // Assert
            _pedidoRepositoryMock.Verify(repo => repo.Alterar(It.IsAny<Pedido>(), false), Times.Once);
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
