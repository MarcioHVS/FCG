using Xunit;
using Moq;
using FCG.Application.Services;
using FCG.Application.DTOs;
using FCG.Domain.Entities;
using FCG.Domain.Interfaces;
using FCG.Domain.Enums;

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
        public async Task ObterPromocao_PromocaoExistente_DeveRetornarPromocao()
        {
            // Arrange
            var promocaoId = Guid.NewGuid();
            var promocao = Promocao.CriarAlterar(promocaoId, "CUPOM-10", "Desconto de 10%", TipoDesconto.Percentual, 10m, DateTime.UtcNow.AddDays(10));
            promocao.Ativar();
            
            _promocaoRepositoryMock.Setup(repo => repo.ObterPorId(promocaoId)).ReturnsAsync(promocao);

            // Act
            var resultado = await _promocaoService.ObterPromocao(promocaoId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(promocaoId, resultado.Id);
        }

        [Fact]
        public async Task ObterPromocao_PromocaoInexistente_DeveRetornarNull()
        {
            // Arrange
            var promocaoId = Guid.NewGuid();
            _promocaoRepositoryMock.Setup(repo => repo.ObterPorId(promocaoId)).ReturnsAsync((Promocao)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _promocaoService.ObterPromocao(promocaoId));
            Assert.Equal("Promoção não encontrada com o Id informado", exception.Message);
        }

        [Fact]
        public async Task ObterPromocao_PromocaoInexistente_DeveLancarExcecao()
        {
            // Arrange
            var promocaoId = Guid.NewGuid();
            _promocaoRepositoryMock.Setup(repo => repo.ObterPorId(promocaoId)).ReturnsAsync((Promocao)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _promocaoService.ObterPromocao(promocaoId));
        }
        #endregion

        #region ObterPromocaoPorCupom
        [Fact]
        public async Task ObterPromocaoPorCupom_PromocaoExistente_DeveRetornarPromocao()
        {
            // Arrange
            var cupom = "CUPOM-10";
            var promocao = Promocao.CriarAlterar(null, cupom, "Desconto de 10%", TipoDesconto.Percentual, 10m, DateTime.UtcNow.AddDays(10));
            promocao.Ativar();
            
            _promocaoRepositoryMock.Setup(repo => repo.ObterPorCupom(cupom)).ReturnsAsync(promocao);

            // Act
            var resultado = await _promocaoService.ObterPromocaoPorCupom(cupom);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(cupom, resultado.Cupom);
        }

        [Fact]
        public async Task ObterPromocaoPorCupom_PromocaoInexistente_DeveRetornarNull()
        {
            // Arrange
            var cupom = "CUPOM-10";
            _promocaoRepositoryMock.Setup(repo => repo.ObterPorCupom(cupom)).ReturnsAsync((Promocao)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _promocaoService.ObterPromocaoPorCupom(cupom));
            Assert.Equal("Promoção não encontrada com o Cupom informado", exception.Message);
        }

        [Fact]
        public async Task ObterPromocaoPorCupom_PromocaoInexistente_DeveLancarExcecao()
        {
            // Arrange
            var cupom = "CUPOM-10";
            _promocaoRepositoryMock.Setup(repo => repo.ObterPorCupom(cupom)).ReturnsAsync((Promocao)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _promocaoService.ObterPromocaoPorCupom(cupom));
        }
        #endregion

        #region ObterPromocoes
        [Fact]
        public async Task ObterPromocoes_ComPromocoes_DeveRetornarListaDePromocoes()
        {
            // Arrange
            var promocoes = new List<Promocao>
            {
                Promocao.CriarAlterar(null, "CUPOM-10", "Desconto de 10%", TipoDesconto.Percentual, 10m, DateTime.UtcNow.AddDays(10)),
                Promocao.CriarAlterar(null, "CUPOM-20", "Desconto de 20%", TipoDesconto.Percentual, 20m, DateTime.UtcNow.AddDays(20))
            };

            _promocaoRepositoryMock.Setup(repo => repo.ObterTodos()).ReturnsAsync(promocoes);

            // Act
            var resultado = await _promocaoService.ObterPromocoes();

            // Assert
            Assert.NotEmpty(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Contains(resultado, p => p.Cupom == "CUPOM-10");
            Assert.Contains(resultado, p => p.Cupom == "CUPOM-20");
        }

        [Fact]
        public async Task ObterPromocoes_SemPromocoes_DeveRetornarListaVazia()
        {
            // Arrange
            _promocaoRepositoryMock.Setup(repo => repo.ObterTodos()).ReturnsAsync(new List<Promocao>());

            // Act
            var resultado = await _promocaoService.ObterPromocoes();

            // Assert
            Assert.Empty(resultado);
        }

        [Fact]
        public async Task ObterPromocoes_ErroBancoDeDados_DeveLancarExcecao()
        {
            // Arrange
            _promocaoRepositoryMock.Setup(repo => repo.ObterTodos()).ThrowsAsync(new Exception("Erro no banco"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _promocaoService.ObterPromocoes());
            Assert.Equal("Erro no banco", exception.Message);
        }

        [Fact]
        public async Task ObterPromocoes_PromocoesOrdenados_DeveRetornarNaOrdemCorreta()
        {
            // Arrange
            var promocoes = new List<Promocao>
            {
                Promocao.CriarAlterar(null, "CUPOM-05", "Desconto de 5%", TipoDesconto.Percentual, 5m, DateTime.UtcNow.AddDays(5)),
                Promocao.CriarAlterar(null, "CUPOM-10", "Desconto de 10%", TipoDesconto.Percentual, 10m, DateTime.UtcNow.AddDays(10)),
                Promocao.CriarAlterar(null, "CUPOM-20", "Desconto de 20%", TipoDesconto.Percentual, 20m, DateTime.UtcNow.AddDays(20))
            };

            _promocaoRepositoryMock.Setup(repo => repo.ObterTodos()).ReturnsAsync(promocoes.OrderBy(p => p.Cupom).ToList());

            // Act
            var resultado = await _promocaoService.ObterPromocoes();

            // Assert
            Assert.Equal(new[] { "CUPOM-05", "CUPOM-10", "CUPOM-20" }, resultado.Select(p => p.Cupom));
        }
        #endregion

        #region ObterPromocoesAtivas
        [Fact]
        public async Task ObterPromocoesAtivas_ComPromocoes_DeveRetornarListaDePromocoes()
        {
            // Arrange
            var promocoes = new List<Promocao>
            {
                Promocao.CriarAlterar(null, "CUPOM-10", "Desconto de 10%", TipoDesconto.Percentual, 10m, DateTime.UtcNow.AddDays(10)),
                Promocao.CriarAlterar(null, "CUPOM-20", "Desconto de 20%", TipoDesconto.Percentual, 20m, DateTime.UtcNow.AddDays(20)),
                Promocao.CriarAlterar(null, "CUPOM-30", "Desconto de 30%", TipoDesconto.Percentual, 30m, DateTime.UtcNow.AddDays(30))
            };
            var promo1 = promocoes.Where(p => p.Cupom == "CUPOM-10").First();
            promo1.Ativar();
            var promo2 = promocoes.Where(p => p.Cupom == "CUPOM-20").First();
            promo2.Ativar();
            var promo3 = promocoes.Where(p => p.Cupom == "CUPOM-30").First();
            promo3.Desativar();

            _promocaoRepositoryMock.Setup(repo => repo.ObterTodosAtivos()).ReturnsAsync(promocoes.Where(p => p.Ativo).ToList());

            // Act
            var resultado = await _promocaoService.ObterPromocoesAtivas();

            // Assert
            Assert.NotEmpty(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Contains(resultado, p => p.Cupom == "CUPOM-10");
            Assert.Contains(resultado, p => p.Cupom == "CUPOM-20");
        }

        [Fact]
        public async Task ObterPromocoesAtivas_PromocoesOrdenados_DeveRetornarNaOrdemCorreta()
        {
            // Arrange
            var promocoes = new List<Promocao>
            {
                Promocao.CriarAlterar(null, "CUPOM-05", "Desconto de 5%", TipoDesconto.Percentual, 5m, DateTime.UtcNow.AddDays(5)),
                Promocao.CriarAlterar(null, "CUPOM-10", "Desconto de 10%", TipoDesconto.Percentual, 10m, DateTime.UtcNow.AddDays(10)),
                Promocao.CriarAlterar(null, "CUPOM-20", "Desconto de 20%", TipoDesconto.Percentual, 20m, DateTime.UtcNow.AddDays(20)),
                Promocao.CriarAlterar(null, "CUPOM-30", "Desconto de 30%", TipoDesconto.Percentual, 30m, DateTime.UtcNow.AddDays(30))
            };
            var promo1 = promocoes.Where(p => p.Cupom == "CUPOM-05").First();
            promo1.Ativar();
            var promo2 = promocoes.Where(p => p.Cupom == "CUPOM-10").First();
            promo2.Ativar();
            var promo3 = promocoes.Where(p => p.Cupom == "CUPOM-20").First();
            promo3.Ativar();
            var promo4 = promocoes.Where(p => p.Cupom == "CUPOM-30").First();
            promo4.Desativar();

            _promocaoRepositoryMock.Setup(repo => repo.ObterTodosAtivos()).ReturnsAsync(promocoes.Where(p => p.Ativo).ToList());

            // Act
            var resultado = await _promocaoService.ObterPromocoesAtivas();

            // Assert
            Assert.Equal(new[] { "CUPOM-05", "CUPOM-10", "CUPOM-20" }, resultado.Select(p => p.Cupom));
        }
        #endregion

        #region AdicionarPromocao
        [Fact]
        public async Task AdicionarPromocao_ComSucesso_DeveAdicionar()
        {
            // Arrange
            var promocaoDto = new PromocaoAdicionarDto
            {
                Cupom = "CUPOM-50",
                Descricao = "Desconto 50%",
                TipoDesconto = TipoDesconto.Percentual,
                ValorDesconto = 50m,
                DataValidade = DateTime.Now.AddDays(30)
            };

            _promocaoRepositoryMock.Setup(repo => repo.Adicionar(It.IsAny<Promocao>())).Returns(Task.CompletedTask);

            // Act
            await _promocaoService.AdicionarPromocao(promocaoDto);

            // Assert
            _promocaoRepositoryMock.Verify(repo => repo.Adicionar(It.Is<Promocao>(
                p => p.Cupom == promocaoDto.Cupom &&
                     p.Descricao == promocaoDto.Descricao &&
                     p.TipoDesconto == promocaoDto.TipoDesconto &&
                     p.ValorDesconto == promocaoDto.ValorDesconto &&
                     p.DataValidade == TimeZoneInfo.ConvertTimeToUtc
                                        (DateTime.SpecifyKind(promocaoDto.DataValidade, DateTimeKind.Unspecified), 
                                        TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo"))
            )), Times.Once);
        }

        [Fact]
        public async Task AdicionarPromocao_ComErro_DeveLancarExcecao()
        {
            // Arrange
            var promocaoDto = new PromocaoAdicionarDto
            {
                Cupom = "CUPOM-50",
                Descricao = "Desconto 50%",
                TipoDesconto = TipoDesconto.Percentual,
                ValorDesconto = 50m,
                DataValidade = DateTime.Now.AddDays(30)
            };

            _promocaoRepositoryMock.Setup(repo => repo.Adicionar(It.IsAny<Promocao>())).ThrowsAsync(new Exception("Erro ao adicionar promoção"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _promocaoService.AdicionarPromocao(promocaoDto));
            Assert.Equal("Erro ao adicionar promoção", exception.Message);
        }

        [Fact]
        public async Task AdicionarPromocao_FalhaBanco_DeveLancarExcecao()
        {
            // Arrange
            var promocaoDto = new PromocaoAdicionarDto
            {
                Cupom = "CUPOM-50",
                Descricao = "Desconto 50%",
                TipoDesconto = TipoDesconto.Percentual,
                ValorDesconto = 50m,
                DataValidade = DateTime.Now.AddDays(30)
            };

            _promocaoRepositoryMock.Setup(repo => repo.Adicionar(It.IsAny<Promocao>())).ThrowsAsync(new Exception("Erro ao salvar promoção"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _promocaoService.AdicionarPromocao(promocaoDto));
            Assert.Equal("Erro ao salvar promoção", exception.Message);
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
                DataValidade = DateTime.Now.AddDays(20)
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
                DataValidade = DateTime.Now.AddDays(20)
            };

            _promocaoRepositoryMock.Setup(repo => repo.Alterar(It.IsAny<Promocao>())).ThrowsAsync(new Exception("Erro ao alterar promoção"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _promocaoService.AlterarPromocao(promocaoDto));
            Assert.Equal("Erro ao alterar promoção", exception.Message);
        }

        [Fact]
        public async Task AlterarPromocao_PromocaoInexistente_DeveLancarExcecao()
        {
            // Arrange
            var promocaoDto = new PromocaoAlterarDto
            {
                Id = Guid.NewGuid(),
                Cupom = "NOVO_CUPOM",
                Descricao = "Novo desconto",
                TipoDesconto = TipoDesconto.Moeda,
                ValorDesconto = 30m,
                DataValidade = DateTime.Now.AddDays(20)
            };

            _promocaoRepositoryMock.Setup(repo => repo.Alterar(It.IsAny<Promocao>())).ThrowsAsync(new KeyNotFoundException("Promoção não encontrada"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _promocaoService.AlterarPromocao(promocaoDto));
            Assert.Equal("Promoção não encontrada", exception.Message);
        }

        [Fact]
        public async Task AlterarPromocao_AlteracaoParcial_DeveAtualizarTodosCampos()
        {
            // Arrange
            var promocaoOriginal = Promocao.CriarAlterar(null, "CUPOM_ANTIGO", "Desconto antigo", TipoDesconto.Moeda, 30m, DateTime.Now.AddDays(20));
            promocaoOriginal.Ativar();

            var promocaoDto = new PromocaoAlterarDto
            {
                Id = promocaoOriginal.Id,
                Cupom = "CUPOM_NOVO",
                Descricao = "Desconto novo",
                TipoDesconto = TipoDesconto.Percentual,
                ValorDesconto = 10m,
                DataValidade = DateTime.Now.AddDays(10)
            };

            _promocaoRepositoryMock.Setup(repo => repo.ObterPorId(promocaoDto.Id)).ReturnsAsync(promocaoOriginal);

            _promocaoRepositoryMock.Setup(repo => repo.Alterar(It.IsAny<Promocao>()))
                .Callback<Promocao>(p =>
                {
                    Promocao.CriarAlterar(promocaoOriginal.Id, p.Cupom, p.Descricao, p.TipoDesconto, p.ValorDesconto, p.DataValidade);
                    _promocaoRepositoryMock.Setup(repo => repo.ObterPorId(p.Id)).ReturnsAsync(p);
                })
                .Returns(Task.CompletedTask);

            // Act
            await _promocaoService.AlterarPromocao(promocaoDto);
            var resultado = await _promocaoService.ObterPromocao(promocaoDto.Id);

            // Assert
            Assert.Equal("CUPOM_NOVO", resultado.Cupom);
            Assert.Equal("Desconto novo", resultado.Descricao);
            Assert.Equal(TipoDesconto.Percentual, resultado.TipoDesconto);
            Assert.Equal(10m, resultado.ValorDesconto);
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
