using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using FCG.Application.Mappers;
using FCG.Domain.Exceptions;
using FCG.Domain.Interfaces;

namespace FCG.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IJwtService _jwtService;

        public UsuarioService(IUsuarioRepository usuarioRepository, IJwtService jwtService)
        {
            _usuarioRepository = usuarioRepository;
            _jwtService = jwtService;
        }

        public async Task<string> LoginAsync(LoginDto login)
        {
            var usuario = await _usuarioRepository.ObterUsuarioPorApelidoAsync(login.Apelido)
                ?? throw new CredenciaisInvalidasException();

            if (!usuario.Ativo)
                throw new OperacaoInvalidaException("Login não permitido: sua conta não está ativa no sistema");

            if (!usuario.ValidarSenha(login.Senha))
                throw new CredenciaisInvalidasException();

            return _jwtService.GerarToken(usuario.ToDto());
        }

        public async Task<UsuarioResponseDto> ObterUsuarioAsync(Guid usuarioId)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId)
                ?? throw new KeyNotFoundException("Usuário não encontrado com o Id informado");

            return usuario.ToDto();
        }

        public async Task<IEnumerable<UsuarioResponseDto>> ObterUsuariosAsync()
        {
            var usuarios = await _usuarioRepository.ObterTodosAsync();

            return usuarios.Select(u => u.ToDto());
        }

        public async Task AdicionarUsuario(UsuarioAdicionarDto usuarioDto)
        {
            await _usuarioRepository.Adicionar(usuarioDto.ToDomain());
        }

        public async Task AlterarUsuario(UsuarioAlterarDto usuarioDto)
        {
            var usuario = usuarioDto.ToDomain();

            await _usuarioRepository.Alterar(usuario);
        }

        public async Task AlterarSenha(Guid usuarioId, string novaSenha)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId);
            usuario?.AlterarSenha(novaSenha);

            await _usuarioRepository.Alterar(usuario);
        }

        public async Task AtivarUsuario(Guid usuarioId)
        {
            await _usuarioRepository.Ativar(usuarioId);
        }

        public async Task DesativarUsuario(Guid usuarioId)
        {
            await _usuarioRepository.Desativar(usuarioId);
        }
    }
}
