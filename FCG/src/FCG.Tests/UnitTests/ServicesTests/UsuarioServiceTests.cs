using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using FCG.Application.Services;
using FCG.Domain.Entities;
using FCG.Domain.Exceptions;
using FCG.Domain.Interfaces;
using Moq;
using Xunit;

namespace FCG.Tests.UnitTests.ServicesTests
{
    public class UsuarioServiceTests
    {
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly Mock<IModeloEmail> _email;
        private readonly Mock<IJwtService> _jwtService;
        private readonly IUsuarioService _usuarioService;

        public UsuarioServiceTests()
        {
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _email = new Mock<IModeloEmail>();
            _jwtService = new Mock<IJwtService>();
            _usuarioService = new UsuarioService(_usuarioRepositoryMock.Object, _email.Object, _jwtService.Object);
        }

        #region Login
        [Fact]
        public async Task Login_ComSucesso_DeveRetornarToken()
        {
            // Arrange
            var usuario = Usuario.CriarAlterar(null, "Maria Luiza", "Malu", "malu@email.com", "Senha@123");
            usuario.Ativar();

            _usuarioRepositoryMock.Setup(repo => repo.ObterPorApelido(usuario.Apelido))
                .ReturnsAsync(usuario);

            _jwtService.Setup(service => service.GerarToken(It.IsAny<UsuarioResponseDto>()))
                .Returns("TokenGeradoFake");

            var loginDto = new LoginDto { Apelido = usuario.Apelido, Senha = "Senha@123" };

            // Act
            var resultado = await _usuarioService.Login(loginDto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("TokenGeradoFake", resultado);
        }

        [Fact]
        public async Task Login_SenhaErrada_DeveLancarExcecao()
        {
            // Arrange
            var usuario = Usuario.CriarAlterar(null, "Maria Luiza", "Malu", "malu@email.com", "Senha@123");
            usuario.Ativar();

            _usuarioRepositoryMock.Setup(repo => repo.ObterPorApelido(usuario.Apelido))
                .ReturnsAsync(usuario);

            var loginDto = new LoginDto { Apelido = usuario.Apelido, Senha = "Senha@321" };

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _usuarioService.Login(loginDto));
        }

        [Fact]
        public async Task Login_UsuarioErrado_DeveLancarExcecao()
        {
            // Arrange
            var usuario = Usuario.CriarAlterar(null, "Maria Luiza", "Malu", "malu@email.com", "Senha@123");
            usuario.Ativar();

            _usuarioRepositoryMock.Setup(repo => repo.ObterPorApelido(usuario.Apelido))
                .ReturnsAsync(usuario);

            var loginDto = new LoginDto { Apelido = "Outro", Senha = usuario.Senha };

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _usuarioService.Login(loginDto));
        }

        [Fact]
        public async Task Login_UsuarioInativo_DeveLancarExcecao()
        {
            // Arrange
            var usuario = Usuario.CriarAlterar(null, "Maria Luiza", "Malu", "malu@email.com", "Senha@123");
            usuario.Desativar();

            _usuarioRepositoryMock.Setup(repo => repo.ObterPorApelido(usuario.Apelido))
                .ReturnsAsync(usuario);

            var loginDto = new LoginDto { Apelido = usuario.Apelido, Senha = usuario.Senha };

            // Act & Assert
            await Assert.ThrowsAsync<OperacaoInvalidaException>(() => _usuarioService.Login(loginDto));
        }
        #endregion

        #region ObterUsuario
        [Fact]
        public async Task ObterUsuario_Existente_DeveRetornarUsuario()
        {
            // Arrange
            var usuario = Usuario.CriarAlterar(null, "Maria Luiza", "Malu", "malu@email.com", "Senha@123");
            usuario.Ativar();

            _usuarioRepositoryMock.Setup(repo => repo.ObterPorId(usuario.Id)).ReturnsAsync(usuario);

            // Act
            var resultado = await _usuarioService.ObterUsuario(usuario.Id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(usuario.Id, resultado.Id);
        }

        [Fact]
        public async Task ObterUsuario_Inexistente_DeveLancarExcecao()
        {
            // Arrange
            _usuarioRepositoryMock.Setup(repo => repo.ObterPorId(It.IsAny<Guid>()))
                .ReturnsAsync((Usuario)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _usuarioService.ObterUsuario(Guid.NewGuid()));
        }
        #endregion

        #region AdicionarUsuario
        [Fact]
        public async Task AdicionarUsuario_ComDadosCorreto_DeveAdicionarUsuario()
        {
            // Arrange
            var usuarioDto = new UsuarioAdicionarDto
            {
                Nome = "Maria Luiza",
                Apelido = "Malu",
                Email = "malu@email.com",
                Senha = "Senha@123"
            };

            _usuarioRepositoryMock.Setup(repo => repo.Adicionar(It.IsAny<Usuario>())).Returns(Task.CompletedTask);

            // Act
            await _usuarioService.AdicionarUsuario(usuarioDto);

            // Assert
            _usuarioRepositoryMock.Verify(repo => repo.Adicionar(It.IsAny<Usuario>()), Times.Once);
        }

        [Fact]
        public async Task AdicionarUsuario_ComSenhaIncorreta_DeveLancarExcecao()
        {
            // Arrange
            var usuarioDto = new UsuarioAdicionarDto
            {
                Nome = "Maria Luiza",
                Apelido = "Malu",
                Email = "malu@email.com",
                Senha = "SenhaSimples"
            };

            _usuarioRepositoryMock.Setup(repo => repo.Adicionar(It.IsAny<Usuario>())).Returns(Task.CompletedTask);

            // Act & Assert
            await Assert.ThrowsAsync<OperacaoInvalidaException>(() => _usuarioService.AdicionarUsuario(usuarioDto));
        }

        [Fact]
        public async Task AdicionarUsuario_ComEmailIncorreto_DeveLancarExcecao()
        {
            // Arrange
            var usuarioDto = new UsuarioAdicionarDto
            {
                Nome = "Maria Luiza",
                Apelido = "Malu",
                Email = "malu_email.com",
                Senha = "Senha@123"
            };

            _usuarioRepositoryMock.Setup(repo => repo.Adicionar(It.IsAny<Usuario>())).Returns(Task.CompletedTask);

            // Act & Assert
            await Assert.ThrowsAsync<OperacaoInvalidaException>(() => _usuarioService.AdicionarUsuario(usuarioDto));
        }
        #endregion

        #region AlterarSenha
        [Fact]
        public async Task AlterarSenha_ComSucesso()
        {
            // Arrange
            var usuario = Usuario.CriarAlterar(null, "Maria Luiza", "Malu", "malu@email.com", "Senha@123");

            _usuarioRepositoryMock.Setup(repo => repo.ObterPorId(usuario.Id)).ReturnsAsync(usuario);

            // Act
            await _usuarioService.AlterarSenha(usuario.Id, "Senha@321");

            // Assert
            _usuarioRepositoryMock.Verify(repo => repo.Alterar(usuario, false), Times.Once);
        }
        #endregion

        #region AtivarUsuario & DesativarUsuario
        [Fact]
        public async Task AtivarUsuario_DeveSerChamado()
        {
            // Act
            await _usuarioService.AtivarUsuario(Guid.NewGuid());

            // Assert
            _usuarioRepositoryMock.Verify(repo => repo.Ativar(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task DesativarUsuario_DeveSerChamado()
        {
            // Act
            await _usuarioService.DesativarUsuario(Guid.NewGuid());

            // Assert
            _usuarioRepositoryMock.Verify(repo => repo.Desativar(It.IsAny<Guid>()), Times.Once);
        }
        #endregion
    }
}
