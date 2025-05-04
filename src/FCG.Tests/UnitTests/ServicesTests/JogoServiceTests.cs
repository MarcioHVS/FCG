using Xunit;
using Moq;
using FCG.Application.Services;
using FCG.Application.DTOs;
using FCG.Domain.Entities;
using FCG.Domain.Interfaces;
using FCG.Domain.Enums;

namespace FCG.Tests.UnitTests.ServicesTests
{
    public class JogoServiceTests
    {
        private readonly Mock<IJogoRepository> _jogoRepositoryMock;
        private readonly JogoService _jogoService;

        public JogoServiceTests()
        {
            _jogoRepositoryMock = new Mock<IJogoRepository>();
            _jogoService = new JogoService(_jogoRepositoryMock.Object);
        }

        #region ObterJogoAsync
        [Fact]
        public async Task ObterJogoAsync_JogoExistente_DeveRetornarJogo()
        {
            // Arrange
            var jogoId = Guid.NewGuid();
            var jogo = new Jogo(jogoId, "Título Teste", "Descrição Teste", Genero.Aventura, 99.99m);
            _jogoRepositoryMock.Setup(repo => repo.ObterPorIdAsync(jogoId)).ReturnsAsync(jogo);

            // Act
            var resultado = await _jogoService.ObterJogoAsync(jogoId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(jogoId, resultado.Id);
        }

        [Fact]
        public async Task ObterJogoAsync_JogoInexistente_DeveRetornarNull()
        {
            // Arrange
            var jogoId = Guid.NewGuid();
            _jogoRepositoryMock.Setup(repo => repo.ObterPorIdAsync(jogoId)).ReturnsAsync((Jogo)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _jogoService.ObterJogoAsync(jogoId));
        }

        [Fact]
        public async Task ObterJogoAsync_JogoInexistente_DeveLancarExcecao()
        {
            // Arrange
            var jogoId = Guid.NewGuid();
            _jogoRepositoryMock.Setup(repo => repo.ObterPorIdAsync(jogoId)).ReturnsAsync((Jogo)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _jogoService.ObterJogoAsync(jogoId));
        }

        [Fact]
        public async Task ObterJogoAsync_JogoComPropriedadesNulas_DeveRetornarErro()
        {
            // Arrange
            var jogoId = Guid.NewGuid();
            var jogo = new Jogo(jogoId, null, null, default, 0);
            _jogoRepositoryMock.Setup(repo => repo.ObterPorIdAsync(jogoId)).ReturnsAsync(jogo);

            // Act
            var resultado = await _jogoService.ObterJogoAsync(jogoId);

            // Assert
            Assert.Null(resultado.Titulo);
            Assert.Null(resultado.Descricao);
        }
        #endregion

        #region ObterJogosAsync
        [Fact]
        public async Task ObterJogosAsync_ComJogos_DeveRetornarListaDeJogos()
        {
            // Arrange
            var jogos = new List<Jogo>
            {
                new Jogo(Guid.NewGuid(), "Jogo 1", "Descrição 1", Genero.Aventura, 59.99m),
                new Jogo(Guid.NewGuid(), "Jogo 2", "Descrição 2", Genero.Acao, 79.99m)
            };
            _jogoRepositoryMock.Setup(repo => repo.ObterTodosAsync()).ReturnsAsync(jogos);

            // Act
            var resultado = await _jogoService.ObterJogosAsync();

            // Assert
            Assert.NotEmpty(resultado);
            Assert.Equal(2, resultado.Count());
        }

        [Fact]
        public async Task ObterJogosAsync_SemJogos_DeveRetornarListaVazia()
        {
            // Arrange
            _jogoRepositoryMock.Setup(repo => repo.ObterTodosAsync()).ReturnsAsync(new List<Jogo>());

            // Act
            var resultado = await _jogoService.ObterJogosAsync();

            // Assert
            Assert.Empty(resultado);
        }

        [Fact]
        public async Task ObterJogosAsync_ErroBancoDeDados_DeveLancarExcecao()
        {
            // Arrange
            _jogoRepositoryMock.Setup(repo => repo.ObterTodosAsync()).ThrowsAsync(new Exception("Erro no banco"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _jogoService.ObterJogosAsync());
        }

        [Fact]
        public async Task ObterJogosAsync_JogosOrdenados_DeveRetornarNaOrdemCorreta()
        {
            // Arrange
            var jogos = new List<Jogo>
            {
                new Jogo(Guid.NewGuid(), "Zelda", "Aventura", Genero.Aventura, 99.99m),
                new Jogo(Guid.NewGuid(), "Mario", "Plataforma", Genero.Plataforma, 79.99m),
                new Jogo(Guid.NewGuid(), "Sonic", "Corrida", Genero.Acao, 59.99m)
            };

            _jogoRepositoryMock.Setup(repo => repo.ObterTodosAsync()).ReturnsAsync(jogos.OrderBy(j => j.Titulo).ToList());

            // Act
            var resultado = await _jogoService.ObterJogosAsync();

            // Assert
            Assert.Equal("Mario", resultado.First().Titulo);
        }
        #endregion

        #region AdicionarJogo
        [Fact]
        public async Task AdicionarJogo_ComSucesso_DeveAdicionar()
        {
            // Arrange
            var jogoDto = new JogoAdicionarDto { Titulo = "Novo Jogo", Descricao = "Teste", Genero = Genero.Aventura, Valor = 49.99m };

            _jogoRepositoryMock.Setup(repo => repo.Adicionar(It.IsAny<Jogo>())).Returns(Task.CompletedTask);

            // Act
            await _jogoService.AdicionarJogo(jogoDto);

            // Assert
            _jogoRepositoryMock.Verify(repo => repo.Adicionar(It.IsAny<Jogo>()), Times.Once);
        }

        [Fact]
        public async Task AdicionarJogo_ComErro_DeveLancarExcecao()
        {
            // Arrange
            var jogoDto = new JogoAdicionarDto { Titulo = "Novo Jogo", Descricao = "Teste", Genero = Genero.Aventura, Valor = 49.99m };

            _jogoRepositoryMock.Setup(repo => repo.Adicionar(It.IsAny<Jogo>())).ThrowsAsync(new Exception("Erro ao adicionar jogo"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _jogoService.AdicionarJogo(jogoDto));
        }

        [Fact]
        public async Task AdicionarJogo_FalhaBanco_DeveLancarExcecao()
        {
            // Arrange
            var jogoDto = new JogoAdicionarDto { Titulo = "Jogo Teste", Descricao = "Descrição", Genero = Genero.Acao, Valor = 49.99m };
            _jogoRepositoryMock.Setup(repo => repo.Adicionar(It.IsAny<Jogo>())).ThrowsAsync(new Exception("Erro ao salvar"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _jogoService.AdicionarJogo(jogoDto));
        }
        #endregion

        #region AlterarJogo
        [Fact]
        public async Task AlterarJogo_DeveAlterarComSucesso()
        {
            // Arrange
            var jogoDto = new JogoAlterarDto { Id = Guid.NewGuid(), Titulo = "Jogo Atualizado", Descricao = "Nova descrição", Genero = Genero.Acao, Valor = 59.99m };

            _jogoRepositoryMock.Setup(repo => repo.Alterar(It.IsAny<Jogo>())).Returns(Task.CompletedTask);

            // Act
            await _jogoService.AlterarJogo(jogoDto);

            // Assert
            _jogoRepositoryMock.Verify(repo => repo.Alterar(It.IsAny<Jogo>()), Times.Once);
        }

        [Fact]
        public async Task AlterarJogo_ComErro_DeveLancarExcecao()
        {
            // Arrange
            var jogoDto = new JogoAlterarDto { Id = Guid.NewGuid(), Titulo = "Jogo Atualizado", Descricao = "Nova descrição", Genero = Genero.Acao, Valor = 59.99m };

            _jogoRepositoryMock.Setup(repo => repo.Alterar(It.IsAny<Jogo>())).ThrowsAsync(new Exception("Erro ao alterar jogo"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _jogoService.AlterarJogo(jogoDto));
        }

        [Fact]
        public async Task AlterarJogo_JogoInexistente_DeveLancarExcecao()
        {
            // Arrange
            var jogoDto = new JogoAlterarDto { Id = Guid.NewGuid(), Titulo = "Jogo Alterado", Descricao = "Nova descrição", Genero = Genero.Acao, Valor = 79.99m };
            _jogoRepositoryMock.Setup(repo => repo.Alterar(It.IsAny<Jogo>())).ThrowsAsync(new KeyNotFoundException("Jogo não encontrado"));

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _jogoService.AlterarJogo(jogoDto));
        }

        [Fact]
        public async Task AlterarJogo_AlteracaoParcial_DeveAtualizarTodosCampos()
        {
            // Arrange
            var jogoId = Guid.NewGuid();
            var jogoDto = new JogoAlterarDto
            {
                Id = jogoId,
                Titulo = "Novo Título",
                Descricao = "Nova Descrição",
                Genero = Genero.RPG,
                Valor = 59.99m
            };

            var jogoOriginal = new Jogo(jogoId, "Título Antigo", "Descrição Antiga", Genero.Acao, 49.99m);

            _jogoRepositoryMock.Setup(repo => repo.ObterPorIdAsync(jogoDto.Id)).ReturnsAsync(jogoOriginal);

            _jogoRepositoryMock.Setup(repo => repo.Alterar(It.IsAny<Jogo>())).Callback<Jogo>(j =>
            {
                _jogoRepositoryMock.Setup(repo => repo.ObterPorIdAsync(j.Id))
                    .ReturnsAsync(new Jogo(j.Id, j.Titulo, j.Descricao, j.Genero, j.Valor));
            }).Returns(Task.CompletedTask);

            // Act
            await _jogoService.AlterarJogo(jogoDto);
            var resultado = await _jogoService.ObterJogoAsync(jogoDto.Id);

            // Assert
            Assert.Equal("Novo Título", resultado.Titulo);
            Assert.Equal("Nova Descrição", resultado.Descricao);
            Assert.Equal(Genero.RPG, resultado.Genero);
            Assert.Equal(59.99m, resultado.Valor);
        }
        #endregion

        #region Ativar/Desativar Jogo
        [Fact]
        public async Task AtivarJogo_JogoJaAtivo_DeveManterEstado()
        {
            // Arrange
            var jogoId = Guid.NewGuid();
            _jogoRepositoryMock.Setup(repo => repo.Ativar(jogoId)).Returns(Task.CompletedTask);

            // Act
            await _jogoService.AtivarJogo(jogoId);

            // Assert
            _jogoRepositoryMock.Verify(repo => repo.Ativar(jogoId), Times.Once);
        }

        [Fact]
        public async Task DesativarJogo_JogoJaDesativado_DeveManterEstado()
        {
            // Arrange
            var jogoId = Guid.NewGuid();
            _jogoRepositoryMock.Setup(repo => repo.Desativar(jogoId)).Returns(Task.CompletedTask);

            // Act
            await _jogoService.DesativarJogo(jogoId);

            // Assert
            _jogoRepositoryMock.Verify(repo => repo.Desativar(jogoId), Times.Once);
        }

        [Fact]
        public async Task AtivarJogo_JogoInexistente_DeveLancarExcecao()
        {
            // Arrange
            var jogoId = Guid.NewGuid();
            _jogoRepositoryMock.Setup(repo => repo.Ativar(jogoId)).ThrowsAsync(new KeyNotFoundException("Jogo não encontrado"));

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _jogoService.AtivarJogo(jogoId));
        }

        [Fact]
        public async Task DesativarJogo_JogoInexistente_DeveLancarExcecao()
        {
            // Arrange
            var jogoId = Guid.NewGuid();
            _jogoRepositoryMock.Setup(repo => repo.Desativar(jogoId)).ThrowsAsync(new KeyNotFoundException("Jogo não encontrado"));

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _jogoService.DesativarJogo(jogoId));
        }
        #endregion
    }
}
