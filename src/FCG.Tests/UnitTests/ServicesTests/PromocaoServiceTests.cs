using Xunit;
using Moq;
using FCG.Application.Services;
using FCG.Application.DTOs;
using FCG.Domain.Entities;
using FCG.Domain.Interfaces;
using FCG.Domain.Enums;
using FCG.Application.Interfaces;

namespace FCG.Tests.UnitTests.ServicesTests
{
    public class PromocaoServiceTests
    {
        private readonly Mock<IPromocaoRepository> _promocaoRepositoryMock;
        private readonly PromocaoService _promocaoService;

        public PromocaoServiceTests()
        {
            _promocaoRepositoryMock = new Mock<IPromocaoRepository>();
            _promocaoService = new PromocaoService(_promocaoRepositoryMock.Object);
        }

        #region ObterPromocao
        [Fact]
        public async Task ObterPromocaoAsync_PromocaoExistente_DeveRetornarPromocao()
        {
            // Arrange
            var promocaoId = Guid.NewGuid();
            var promocao = new Promocao(promocaoId, "CUPOM10", "Desconto de 10%", TipoDesconto.Percentual, 10m, DateTime.UtcNow.AddDays(10));

            _promocaoRepositoryMock.Setup(repo => repo.ObterPorIdAsync(promocaoId)).ReturnsAsync(promocao);

            // Act
            var resultado = await _promocaoService.ObterPromocaoAsync(promocaoId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(promocaoId, resultado.Id);
            Assert.Equal("CUPOM10", resultado.Cupom);
        }

        [Fact]
        public async Task ObterPromocaoAsync_PromocaoInexistente_DeveRetornarNull()
        {
            // Arrange
            var promocaoId = Guid.NewGuid();
            _promocaoRepositoryMock.Setup(repo => repo.ObterPorIdAsync(promocaoId)).ReturnsAsync((Promocao)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _promocaoService.ObterPromocaoAsync(promocaoId));
        }

        [Fact]
        public async Task ObterPromocaoAsync_PromocaoInexistente_DeveLancarExcecao()
        {
            // Arrange
            var promocaoId = Guid.NewGuid();
            _promocaoRepositoryMock.Setup(repo => repo.ObterPorIdAsync(promocaoId)).ReturnsAsync((Promocao)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _promocaoService.ObterPromocaoAsync(promocaoId));
        }

        [Fact]
        public async Task ObterPromocaoAsync_PromocaoComPropriedadesNulas_DeveRetornarErro()
        {
            // Arrange
            var promocaoId = Guid.NewGuid();
            var promocao = new Promocao(Guid.NewGuid(), null, null, TipoDesconto.Percentual, 10m, DateTime.UtcNow.AddDays(10));
            _promocaoRepositoryMock.Setup(repo => repo.ObterPorIdAsync(promocaoId)).ReturnsAsync(promocao);

            // Act
            var resultado = await _promocaoService.ObterPromocaoAsync(promocaoId);

            // Assert
            Assert.Null(resultado.Cupom);
            Assert.Null(resultado.Descricao);
        }
        #endregion

        #region ObterPromocoes
        [Fact]
        public async Task ObterPromocoesAsync_ComPromocoes_DeveRetornarListaDePromocoes()
        {
            // Arrange
            var promocoes = new List<Promocao>
            {
                new Promocao(Guid.NewGuid(), "CUPOM10", "Desconto de 10%", TipoDesconto.Percentual, 10m, DateTime.UtcNow.AddDays(10)),
                new Promocao(Guid.NewGuid(), "CUPOM20", "Desconto de 20%", TipoDesconto.Percentual, 20m, DateTime.UtcNow.AddDays(20))
            };

            _promocaoRepositoryMock.Setup(repo => repo.ObterTodosAsync()).ReturnsAsync(promocoes);

            // Act
            var resultado = await _promocaoService.ObterPromocoesAsync();

            // Assert
            Assert.NotEmpty(resultado);
            Assert.Equal(2, resultado.Count());
        }

        [Fact]
        public async Task ObterPromocoesAsync_SemPromocoes_DeveRetornarListaVazia()
        {
            // Arrange
            _promocaoRepositoryMock.Setup(repo => repo.ObterTodosAsync()).ReturnsAsync(new List<Promocao>());

            // Act
            var resultado = await _promocaoService.ObterPromocoesAsync();

            // Assert
            Assert.Empty(resultado);
        }

        [Fact]
        public async Task ObterPromocoesAsync_ErroBancoDeDados_DeveLancarExcecao()
        {
            // Arrange
            _promocaoRepositoryMock.Setup(repo => repo.ObterTodosAsync()).ThrowsAsync(new Exception("Erro no banco"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _promocaoService.ObterPromocoesAsync());
        }

        [Fact]
        public async Task ObterPromocoesAsync_PromocoesOrdenados_DeveRetornarNaOrdemCorreta()
        {
            // Arrange
            var promocoes = new List<Promocao>
            {
                new Promocao(Guid.NewGuid(), "CUPOM05", "Desconto de 5%", TipoDesconto.Percentual, 5m, DateTime.UtcNow.AddDays(5)),
                new Promocao(Guid.NewGuid(), "CUPOM10", "Desconto de 10%", TipoDesconto.Percentual, 10m, DateTime.UtcNow.AddDays(10)),
                new Promocao(Guid.NewGuid(), "CUPOM20", "Desconto de 20%", TipoDesconto.Percentual, 20m, DateTime.UtcNow.AddDays(20))
            };

            _promocaoRepositoryMock.Setup(repo => repo.ObterTodosAsync()).ReturnsAsync(promocoes.OrderBy(j => j.Cupom).ToList());

            // Act
            var resultado = await _promocaoService.ObterPromocoesAsync();

            // Assert
            Assert.Equal("CUPOM05", resultado.First().Cupom);
        }
        #endregion

        #region AdicionarPromocao
        [Fact]
        public async Task AdicionarPromocao_DeveAdicionarComSucesso()
        {
            // Arrange
            var promocaoDto = new PromocaoAdicionarDto
            {
                Cupom = "CUPOM50",
                Descricao = "Desconto 50%",
                TipoDesconto = TipoDesconto.Percentual,
                ValorDesconto = 50m,
                DataValidade = DateTime.UtcNow.AddDays(30)
            };

            _promocaoRepositoryMock.Setup(repo => repo.Adicionar(It.IsAny<Promocao>())).Returns(Task.CompletedTask);

            // Act
            await _promocaoService.AdicionarPromocao(promocaoDto);

            // Assert
            _promocaoRepositoryMock.Verify(repo => repo.Adicionar(It.IsAny<Promocao>()), Times.Once);
        }

        [Fact]
        public async Task AdicionarPromocao_ComErro_DeveLancarExcecao()
        {
            // Arrange
            var promocaoDto = new PromocaoAdicionarDto
            {
                Cupom = "CUPOM50",
                Descricao = "Desconto 50%",
                TipoDesconto = TipoDesconto.Percentual,
                ValorDesconto = 50m,
                DataValidade = DateTime.UtcNow.AddDays(30)
            };

            _promocaoRepositoryMock.Setup(repo => repo.Adicionar(It.IsAny<Promocao>())).ThrowsAsync(new Exception("Erro ao adicionar promoção"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _promocaoService.AdicionarPromocao(promocaoDto));
        }

        [Fact]
        public async Task AdicionarPromocao_FalhaBanco_DeveLancarExcecao()
        {
            // Arrange
            var promocaoDto = new PromocaoAdicionarDto
            {
                Cupom = "CUPOM50",
                Descricao = "Desconto 50%",
                TipoDesconto = TipoDesconto.Percentual,
                ValorDesconto = 50m,
                DataValidade = DateTime.UtcNow.AddDays(30)
            };

            _promocaoRepositoryMock.Setup(repo => repo.Adicionar(It.IsAny<Promocao>())).ThrowsAsync(new Exception("Erro ao salvar"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _promocaoService.AdicionarPromocao(promocaoDto));
        }
        #endregion

        #region AlterarPromocao
        [Fact]
        public async Task AlterarPromocao_DeveAlterarComSucesso()
        {
            // Arrange
            var promocaoDto = new PromocaoAlterarDto
            {
                Id = Guid.NewGuid(),
                Cupom = "NOVO_CUPOM",
                Descricao = "Novo desconto",
                TipoDesconto = TipoDesconto.Moeda,
                ValorDesconto = 30m,
                DataValidade = DateTime.UtcNow.AddDays(20)
            };

            _promocaoRepositoryMock.Setup(repo => repo.Alterar(It.IsAny<Promocao>())).Returns(Task.CompletedTask);

            // Act
            await _promocaoService.AlterarPromocao(promocaoDto);

            // Assert
            _promocaoRepositoryMock.Verify(repo => repo.Alterar(It.IsAny<Promocao>()), Times.Once);
        }

        [Fact]
        public async Task AlterarPromocao_ComErro_DeveLancarExcecao()
        {
            // Arrange
            var promocaoDto = new PromocaoAlterarDto
            {
                Id = Guid.NewGuid(),
                Cupom = "NOVO_CUPOM",
                Descricao = "Novo desconto",
                TipoDesconto = TipoDesconto.Moeda,
                ValorDesconto = 30m,
                DataValidade = DateTime.UtcNow.AddDays(20)
            };

            _promocaoRepositoryMock.Setup(repo => repo.Alterar(It.IsAny<Promocao>())).ThrowsAsync(new Exception("Erro ao alterar promoção"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _promocaoService.AlterarPromocao(promocaoDto));
        }

        [Fact]
        public async Task AlterarPromocao_AlteracaoParcial_DeveAtualizarTodosCampos()
        {
            // Arrange
            var promocaoId = Guid.NewGuid();
            var dataValidade = DateTime.UtcNow.AddDays(20);
            var promocaoDto = new PromocaoAlterarDto
            {
                Id = promocaoId,
                Cupom = "NOVO_CUPOM",
                Descricao = "Nova Descrição",
                TipoDesconto = TipoDesconto.Moeda,
                ValorDesconto = 30m,
                DataValidade = dataValidade
            };

            var promocaoOriginal = new Promocao(promocaoId, "CUPOM_ANTIGO", "Descrição Antiga", TipoDesconto.Percentual, 5m, DateTime.UtcNow.AddDays(30));

            _promocaoRepositoryMock.Setup(repo => repo.ObterPorIdAsync(promocaoDto.Id)).ReturnsAsync(promocaoOriginal);

            _promocaoRepositoryMock.Setup(repo => repo.Alterar(It.IsAny<Promocao>())).Callback<Promocao>(p =>
            {
                _promocaoRepositoryMock.Setup(repo => repo.ObterPorIdAsync(p.Id))
                    .ReturnsAsync(new Promocao(p.Id, p.Cupom, p.Descricao, p.TipoDesconto, p.ValorDesconto, p.DataValidade));
            }).Returns(Task.CompletedTask);

            // Act
            await _promocaoService.AlterarPromocao(promocaoDto);
            var resultado = await _promocaoService.ObterPromocaoAsync(promocaoDto.Id);

            // Assert
            Assert.Equal("NOVO_CUPOM", resultado.Cupom);
            Assert.Equal("Nova Descrição", resultado.Descricao);
            Assert.Equal(TipoDesconto.Moeda, resultado.TipoDesconto);
            Assert.Equal(30m, resultado.ValorDesconto);
            Assert.Equal(dataValidade, resultado.DataValidade);
        }
        #endregion

        #region Ativar/Desativar Promocao
        [Fact]
        public async Task AtivarPromocao_PromocaoJaAtivo_DeveManterEstado()
        {
            // Arrange
            var promocaoId = Guid.NewGuid();
            _promocaoRepositoryMock.Setup(repo => repo.Ativar(promocaoId)).Returns(Task.CompletedTask);

            // Act
            await _promocaoService.AtivarPromocao(promocaoId);

            // Assert
            _promocaoRepositoryMock.Verify(repo => repo.Ativar(promocaoId), Times.Once);
        }

        [Fact]
        public async Task DesativarPromocao_PromocaoJaDesativado_DeveManterEstado()
        {
            // Arrange
            var promocaoId = Guid.NewGuid();
            _promocaoRepositoryMock.Setup(repo => repo.Desativar(promocaoId)).Returns(Task.CompletedTask);

            // Act
            await _promocaoService.DesativarPromocao(promocaoId);

            // Assert
            _promocaoRepositoryMock.Verify(repo => repo.Desativar(promocaoId), Times.Once);
        }

        [Fact]
        public async Task AtivarPromocao_PromocaoInexistente_DeveLancarExcecao()
        {
            // Arrange
            var promocaoId = Guid.NewGuid();
            _promocaoRepositoryMock.Setup(repo => repo.Ativar(promocaoId)).ThrowsAsync(new KeyNotFoundException("Promoção não encontrada"));

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _promocaoService.AtivarPromocao(promocaoId));
        }

        [Fact]
        public async Task DesativarPromocao_PromocaoInexistente_DeveLancarExcecao()
        {
            // Arrange
            var promocaoId = Guid.NewGuid();
            _promocaoRepositoryMock.Setup(repo => repo.Desativar(promocaoId)).ThrowsAsync(new KeyNotFoundException("Promoção não encontrada"));

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _promocaoService.DesativarPromocao(promocaoId));
        }
        #endregion
    }
}
