using FCG.Domain.Entities;
using FCG.Domain.Enums;
using FCG.Domain.Interfaces;
using FCG.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace FCG.Tests.IntegrationTests.ServicesTests
{
    public class PedidoServiceTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;

        public PedidoServiceTests(TestFixture fixture)
        {
            _fixture = fixture;

            var usuarioRepoMock = new Mock<IUsuarioRepository>();
            var jogoRepoMock = new Mock<IJogoRepository>();
            var promocaoRepoMock = new Mock<IPromocaoRepository>();
        }

        [Fact]
        public async Task AdicionarPedido_ComDadosValidos_DeveSalvarNoBanco()
        {
            _fixture.ResetDatabase();

            // Arrange
            var usuario = Usuario.CriarAlterar(null, "Maria Luiza", $"Malu", $"malu@email.com", "Senha@123");
            var jogo = Jogo.CriarAlterar(null, $"Super Mario Bros", "Uma jornada para salvar a Princesa Peach", Genero.Plataforma, 79.99m);
            var pedido = Pedido.CriarAlterar(null, usuario.Id, jogo.Id);
    
            await _fixture.Context.Usuarios.AddAsync(usuario);
            await _fixture.Context.Jogos.AddAsync(jogo);
            await _fixture.Context.Pedidos.AddAsync(pedido);
            await _fixture.Context.SaveChangesAsync();

            // Assert
            var pedidoSalvo = await _fixture.Context.Pedidos.FindAsync(pedido.Id);
            Assert.NotNull(pedidoSalvo);
        }

        [Fact]
        public async Task AdicionarPedido_ComUsuarioInvalido_NaoDeveSalvarNoBanco()
        {
            _fixture.ResetDatabase();

            // Arrange
            var jogo = Jogo.CriarAlterar(null, $"Super Mario Bros", "Uma jornada para salvar a Princesa Peach", Genero.Plataforma, 79.99m);
            var pedido = Pedido.CriarAlterar(null, Guid.NewGuid(), jogo.Id);

            await _fixture.Context.Jogos.AddAsync(jogo);
            await _fixture.Context.SaveChangesAsync();

            // Act & Assert
            await _fixture.Context.Pedidos.AddAsync(pedido);
            await Assert.ThrowsAsync<DbUpdateException>(async () => await _fixture.Context.SaveChangesAsync());
        }

        [Fact]
        public async Task AdicionarPedido_ComJogoInvalido_NaoDeveSalvarNoBanco()
        {
            _fixture.ResetDatabase();

            // Arrange
            var usuario = Usuario.CriarAlterar(null, "Maria Luiza", $"Malu", $"malu@email.com", "Senha@123");
            var pedido = Pedido.CriarAlterar(null, usuario.Id, Guid.NewGuid());

            await _fixture.Context.Usuarios.AddAsync(usuario);
            await _fixture.Context.SaveChangesAsync();

            // Act & Assert
            await _fixture.Context.Pedidos.AddAsync(pedido);
            await Assert.ThrowsAsync<DbUpdateException>(async () => await _fixture.Context.SaveChangesAsync());
        }

        [Fact]
        public async Task RemoverPedido_ComPedidoExistente_DeveRemoverDoBanco()
        {
            _fixture.ResetDatabase();

            // Arrange
            var usuario = Usuario.CriarAlterar(null, "Maria Luiza", $"Malu", $"malu@email.com", "Senha@123");
            var jogo = Jogo.CriarAlterar(null, $"Super Mario Bros", "Uma jornada para salvar a Princesa Peach", Genero.Plataforma, 79.99m);
            var pedido = Pedido.CriarAlterar(null, usuario.Id, jogo.Id);

            await _fixture.Context.Usuarios.AddAsync(usuario);
            await _fixture.Context.Jogos.AddAsync(jogo);
            await _fixture.Context.Pedidos.AddAsync(pedido);
            await _fixture.Context.SaveChangesAsync();

            // Act
            _fixture.Context.Pedidos.Remove(pedido);
            await _fixture.Context.SaveChangesAsync();

            // Assert
            var pedidoRemovido = await _fixture.Context.Pedidos.FindAsync(pedido.Id);
            Assert.Null(pedidoRemovido);
        }

        [Fact]
        public async Task BuscarPedido_PorId_DeveRetornarPedidoCorreto()
        {
            _fixture.ResetDatabase();

            // Arrange
            var usuario = Usuario.CriarAlterar(null, "Maria Luiza", $"Malu", $"malu@email.com", "Senha@123");
            var jogo = Jogo.CriarAlterar(null, $"Super Mario Bros", "Uma jornada para salvar a Princesa Peach", Genero.Plataforma, 79.99m);
            var pedido = Pedido.CriarAlterar(null, usuario.Id, jogo.Id);

            await _fixture.Context.Usuarios.AddAsync(usuario);
            await _fixture.Context.Jogos.AddAsync(jogo);
            await _fixture.Context.Pedidos.AddAsync(pedido);
            await _fixture.Context.SaveChangesAsync();

            // Act
            var pedidoRecuperado = await _fixture.Context.Pedidos.FindAsync(pedido.Id);

            // Assert
            Assert.NotNull(pedidoRecuperado);
            Assert.Equal(pedido.Id, pedidoRecuperado.Id);
        }
    }
}
