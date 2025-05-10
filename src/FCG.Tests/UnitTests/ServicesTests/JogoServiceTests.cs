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
            var jogo = Jogo.Criar(null, "Super Mario Bros", "Uma jornada para salvar a Princesa Peach", Genero.Aventura, 79.99m);
            jogo.Ativar();
            _jogoRepositoryMock.Setup(repo => repo.ObterPorIdAsync(jogo.Id)).ReturnsAsync(jogo);

            // Act
            var resultado = await _jogoService.ObterJogoAsync(jogo.Id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(jogo.Id, resultado.Id);
        }

        [Fact]
        public async Task ObterJogoAsync_JogoInexistente_DeveLancarExcecao()
        {
            // Arrange
            var jogoId = Guid.NewGuid();
            _jogoRepositoryMock.Setup(repo => repo.ObterPorIdAsync(jogoId)).ReturnsAsync((Jogo)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _jogoService.ObterJogoAsync(jogoId));
            Assert.Equal("Jogo não encontrado com o Id informado", exception.Message);
        }
        #endregion

        #region ObterJogosAsync
        [Fact]
        public async Task ObterJogosAsync_ComJogos_DeveRetornarListaDeJogos()
        {
            // Arrange
            var jogos = new List<Jogo>
            {
                Jogo.Criar(null, "Super Mario Bros", "Uma jornada para salvar a Princesa Peach", Genero.Plataforma, 79.99m),
                Jogo.Criar(null, "Street Fighter", "Lutadores batalham em duelos épicos", Genero.Luta, 159.99m)
            };

            _jogoRepositoryMock.Setup(repo => repo.ObterTodosAsync()).ReturnsAsync(jogos);

            // Act
            var resultado = await _jogoService.ObterJogosAsync();

            // Assert
            Assert.NotEmpty(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Contains(resultado, j => j.Titulo == "Super Mario Bros");
            Assert.Contains(resultado, j => j.Titulo == "Street Fighter");
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
            var exception = await Assert.ThrowsAsync<Exception>(() => _jogoService.ObterJogosAsync());
            Assert.Equal("Erro no banco", exception.Message);
        }

        [Fact]
        public async Task ObterJogosAsync_JogosOrdenados_DeveRetornarNaOrdemCorreta()
        {
            // Arrange
            var jogos = new List<Jogo>
            {
                Jogo.Criar(null, "Zelda", "Resgatar a Princesa Zelda das garras do maligno Ganon", Genero.Aventura, 99.99m),
                Jogo.Criar(null, "Metal Gear Solid", "Infiltrar-se em uma base secreta para impedir uma ameaça nuclear", Genero.Plataforma, 79.99m),
                Jogo.Criar(null, "Street Fighter", "Lutadores batalham em duelos épicos", Genero.Acao, 159.99m)
            };

            _jogoRepositoryMock.Setup(repo => repo.ObterTodosAsync()).ReturnsAsync(jogos.OrderBy(j => j.Titulo).ToList());

            // Act
            var resultado = await _jogoService.ObterJogosAsync();

            // Assert
            Assert.Equal(new[] { "Metal Gear Solid", "Street Fighter", "Zelda" }, resultado.Select(j => j.Titulo));
        }
        #endregion

        #region AdicionarJogo
        [Fact]
        public async Task AdicionarJogo_ComSucesso_DeveAdicionar()
        {
            // Arrange
            var jogoDto = new JogoAdicionarDto
            {
                Titulo = "Super Mario Bros", 
                Descricao = "Uma jornada para salvar a Princesa Peach", 
                Genero = Genero.Plataforma, 
                Valor = 79.99m
            };

            _jogoRepositoryMock.Setup(repo => repo.Adicionar(It.IsAny<Jogo>())).Returns(Task.CompletedTask);

            // Act
            await _jogoService.AdicionarJogo(jogoDto);

            // Assert
            _jogoRepositoryMock.Verify(repo => repo.Adicionar(It.Is<Jogo>(
                j => j.Titulo == jogoDto.Titulo &&
                     j.Descricao == jogoDto.Descricao &&
                     j.Genero == jogoDto.Genero &&
                     j.Valor == jogoDto.Valor
            )), Times.Once);
        }

        [Fact]
        public async Task AdicionarJogo_ComErro_DeveLancarExcecao()
        {
            // Arrange
            var jogoDto = new JogoAdicionarDto
            {
                Titulo = "Super Mario Bros",
                Descricao = "Uma jornada para salvar a Princesa Peach",
                Genero = Genero.Plataforma,
                Valor = 79.99m
            };

            _jogoRepositoryMock.Setup(repo => repo.Adicionar(It.IsAny<Jogo>())).ThrowsAsync(new Exception("Erro ao adicionar jogo"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _jogoService.AdicionarJogo(jogoDto));
            Assert.Equal("Erro ao adicionar jogo", exception.Message);
        }

        [Fact]
        public async Task AdicionarJogo_FalhaBanco_DeveLancarExcecao()
        {
            // Arrange
            var jogoDto = new JogoAdicionarDto
            {
                Titulo = "Super Mario Bros",
                Descricao = "Uma jornada para salvar a Princesa Peach",
                Genero = Genero.Plataforma,
                Valor = 79.99m
            };

            _jogoRepositoryMock.Setup(repo => repo.Adicionar(It.IsAny<Jogo>())).ThrowsAsync(new Exception("Erro ao salvar jogo"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _jogoService.AdicionarJogo(jogoDto));
            Assert.Equal("Erro ao salvar jogo", exception.Message);
        }
        #endregion

        #region AlterarJogo
        [Fact]
        public async Task AlterarJogo_ComSucesso_DeveAlterar()
        {
            // Arrange
            var jogoDto = new JogoAlterarDto
            {
                Id = Guid.NewGuid(),
                Titulo = "Super Mario Bros",
                Descricao = "Uma jornada para salvar a Princesa Peach",
                Genero = Genero.Plataforma,
                Valor = 79.99m
            };

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
            var jogoDto = new JogoAlterarDto
            {
                Id = Guid.NewGuid(),
                Titulo = "Super Mario Bros",
                Descricao = "Uma jornada para salvar a Princesa Peach",
                Genero = Genero.Plataforma,
                Valor = 79.99m
            };

            _jogoRepositoryMock.Setup(repo => repo.Alterar(It.IsAny<Jogo>())).ThrowsAsync(new Exception("Erro ao alterar jogo"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _jogoService.AlterarJogo(jogoDto));
            Assert.Equal("Erro ao alterar jogo", exception.Message);
        }

        [Fact]
        public async Task AlterarJogo_JogoInexistente_DeveLancarExcecao()
        {
            // Arrange
            var jogoDto = new JogoAlterarDto { Id = Guid.NewGuid(), Titulo = "Super Mario Bros", Descricao = "Uma jornada para salvar a Princesa Peach", Genero = Genero.Plataforma, Valor = 79.99m };
            _jogoRepositoryMock.Setup(repo => repo.Alterar(It.IsAny<Jogo>())).ThrowsAsync(new KeyNotFoundException("Jogo não encontrado"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _jogoService.AlterarJogo(jogoDto));
            Assert.Equal("Jogo não encontrado", exception.Message);
        }

        [Fact]
        public async Task AlterarJogo_AlteracaoParcial_DeveAtualizarTodosCampos()
        {
            // Arrange
            var jogoOriginal = Jogo.Criar(null, "Super Mario Bros I", "Uma jornada para salvar a Princesa Peach", Genero.Plataforma, 79.99m);
            jogoOriginal.Ativar();

            var jogoDto = new JogoAlterarDto
            {
                Id = jogoOriginal.Id,
                Titulo = "Super Mario Bros II",
                Descricao = "Outra jornada para salvar a Princesa Peach",
                Genero = Genero.Acao,
                Valor = 88.59m
            };

            _jogoRepositoryMock.Setup(repo => repo.ObterPorIdAsync(jogoDto.Id)).ReturnsAsync(jogoOriginal);

            _jogoRepositoryMock.Setup(repo => repo.Alterar(It.IsAny<Jogo>()))
                .Callback<Jogo>(j =>
                {
                    Jogo.Criar(jogoOriginal.Id, j.Titulo, j.Descricao, j.Genero, j.Valor);
                    _jogoRepositoryMock.Setup(repo => repo.ObterPorIdAsync(j.Id)).ReturnsAsync(j);
                })
                .Returns(Task.CompletedTask);

            // Act
            await _jogoService.AlterarJogo(jogoDto);
            var resultado = await _jogoService.ObterJogoAsync(jogoDto.Id);

            // Assert
            Assert.Equal("Super Mario Bros II", resultado.Titulo);
            Assert.Equal("Outra jornada para salvar a Princesa Peach", resultado.Descricao);
            Assert.Equal(Genero.Acao, resultado.Genero);
            Assert.Equal(88.59m, resultado.Valor);
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
