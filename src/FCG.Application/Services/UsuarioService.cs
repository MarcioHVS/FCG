using FCG.Application.Entities;
using FCG.Application.Interfaces;
using FCG.Application.Mappers;
using FCG.Domain.Exceptions;
using FCG.Domain.Interfaces;

namespace FCG.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<UsuarioResponseDto> LoginAsync(LoginDto login)
        {
            var usuario = await _usuarioRepository.ObterUsuarioPorApelidoAsync(login.Apelido)
                ?? throw new CredenciaisInvalidasException();

            if (!usuario.ValidarSenha(login.Senha))
                throw new CredenciaisInvalidasException();

            if(!usuario.Ativo)
                throw new OperacaoInvalidaException("Login não permitido: sua conta não está ativa no sistema");

            return usuario.Retornar();
        }

        public async Task<UsuarioResponseDto> ObterUsuarioAsync(Guid usuarioId)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId)
                ?? throw new KeyNotFoundException("Usuário não encontrado com o Id informado");

            return usuario.Retornar();
        }

        public async Task<IEnumerable<UsuarioResponseDto>> ObterUsuariosAsync()
        {
            var usuarios = await _usuarioRepository.ObterTodosAsync();

            return usuarios.Select(u => u.Retornar());
        }

        public async Task AdicionarUsuario(UsuarioAdicionarDto usuarioDto)
        {
            await _usuarioRepository.Adicionar(usuarioDto.Adicionar());
        }

        public async Task AlterarUsuario(UsuarioAlterarDto usuarioDto)
        {
            var usuario = usuarioDto.Alterar();

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
