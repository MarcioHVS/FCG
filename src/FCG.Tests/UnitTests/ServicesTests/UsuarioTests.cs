using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using FCG.Application.Services;
using FCG.Domain.Entities;
using FCG.Domain.Enums;
using FCG.Domain.Exceptions;
using FCG.Domain.Interfaces;
using Moq;
using Xunit;

namespace FCG.Tests.UnitTests.ServicesTests
{
    public class UsuarioServiceTests
    {
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly IUsuarioService _usuarioService;

        public UsuarioServiceTests()
        {
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _usuarioService = new UsuarioService(_usuarioRepositoryMock.Object);
        }

        #region Login
        [Fact]
        public async Task Login_ComSucesso_DeveRetornarUsuario()
        {
            // Arrange
            var usuario = Usuario.Adicionar("Maria Luiza", "Malu", "malu@email.com", "SenhaValida", Role.Usuario);
            usuario.Ativar();

            _usuarioRepositoryMock.Setup(repo => repo.ObterUsuarioPorApelidoAsync(usuario.Apelido))
                .ReturnsAsync(usuario);

            var loginDto = new LoginDto { Apelido = usuario.Apelido, Senha = "SenhaValida" };

            // Act
            var resultado = await _usuarioService.LoginAsync(loginDto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(usuario.Apelido, resultado.Apelido);
        }

        [Fact]
        public async Task Login_SenhaErrada_DeveLancarExcecao()
        {
            // Arrange
            var usuario = Usuario.Adicionar("Maria Luiza", "Malu", "malu@email.com", "SenhaValida", Role.Usuario);
            usuario.Ativar();

            _usuarioRepositoryMock.Setup(repo => repo.ObterUsuarioPorApelidoAsync(usuario.Apelido))
                .ReturnsAsync(usuario);

            var loginDto = new LoginDto { Apelido = usuario.Apelido, Senha = "SenhaIncorreta" };

            // Act & Assert
            await Assert.ThrowsAsync<CredenciaisInvalidasException>(() => _usuarioService.LoginAsync(loginDto));
        }

        [Fact]
        public async Task Login_UsuarioErrado_DeveLancarExcecao()
        {
            // Arrange
            var usuario = Usuario.Adicionar("Maria Luiza", "Malu", "malu@email.com", "SenhaValida", Role.Usuario);
            usuario.Ativar();

            _usuarioRepositoryMock.Setup(repo => repo.ObterUsuarioPorApelidoAsync(usuario.Apelido))
                .ReturnsAsync(usuario);

            var loginDto = new LoginDto { Apelido = "Outro", Senha = usuario.Senha };

            // Act & Assert
            await Assert.ThrowsAsync<CredenciaisInvalidasException>(() => _usuarioService.LoginAsync(loginDto));
        }

        [Fact]
        public async Task Login_UsuarioInativo_DeveLancarExcecao()
        {
            // Arrange
            var usuario = Usuario.Adicionar("Maria Luiza", "Malu", "malu@email.com", "SenhaValida", Role.Usuario);
            usuario.Desativar();

            _usuarioRepositoryMock.Setup(repo => repo.ObterUsuarioPorApelidoAsync(usuario.Apelido))
                .ReturnsAsync(usuario);

            var loginDto = new LoginDto { Apelido = usuario.Apelido, Senha = usuario.Senha };

            // Act & Assert
            await Assert.ThrowsAsync<OperacaoInvalidaException>(() => _usuarioService.LoginAsync(loginDto));
        }
        #endregion

        #region ObterUsuario
        [Fact]
        public async Task ObterUsuario_Existente_DeveRetornarUsuario()
        {
            // Arrange
            var usuario = Usuario.Adicionar("Maria Luiza", "Malu", "malu@email.com", "SenhaValida", Role.Usuario);
            usuario.Ativar();

            _usuarioRepositoryMock.Setup(repo => repo.ObterPorIdAsync(usuario.Id)).ReturnsAsync(usuario);

            // Act
            var resultado = await _usuarioService.ObterUsuarioAsync(usuario.Id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(usuario.Id, resultado.Id);
        }

        [Fact]
        public async Task ObterUsuario_Inexistente_DeveLancarExcecao()
        {
            // Arrange
            _usuarioRepositoryMock.Setup(repo => repo.ObterPorIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Usuario)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _usuarioService.ObterUsuarioAsync(Guid.NewGuid()));
        }
        #endregion

        #region AdicionarUsuario
        [Fact]
        public async Task AdicionarUsuario_ComSucesso()
        {
            // Arrange
            var usuarioDto = new UsuarioAdicionarDto
            {
                Nome = "Maria Luiza",
                Apelido = "Malu",
                Email = "malu@email.com",
                Senha = "SenhaValida",
                Role = Role.Usuario
            };

            _usuarioRepositoryMock.Setup(repo => repo.Adicionar(It.IsAny<Usuario>())).Returns(Task.CompletedTask);

            // Act
            await _usuarioService.AdicionarUsuario(usuarioDto);

            // Assert
            _usuarioRepositoryMock.Verify(repo => repo.Adicionar(It.IsAny<Usuario>()), Times.Once);
        }
        #endregion

        #region AlterarSenha
        [Fact]
        public async Task AlterarSenha_ComSucesso()
        {
            // Arrange
            var usuario = Usuario.Adicionar("Maria Luiza", "Malu", "malu@email.com", "SenhaAntiga", Role.Usuario);

            _usuarioRepositoryMock.Setup(repo => repo.ObterPorIdAsync(usuario.Id)).ReturnsAsync(usuario);

            // Act
            await _usuarioService.AlterarSenha(usuario.Id, "NovaSenha");

            // Assert
            _usuarioRepositoryMock.Verify(repo => repo.Alterar(usuario), Times.Once);
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
