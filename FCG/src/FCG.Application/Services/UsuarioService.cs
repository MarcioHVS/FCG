using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using FCG.Application.Mappers;
using FCG.Domain.Entities;
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

        public async Task<string> Login(LoginDto login)
        {
            var usuario = await ValidarAcesso(login.Apelido, login.Senha);

            return Convert.ToBase64String(usuario.Id.ToByteArray());
        }

        public async Task<string> LoginAtivacao(LoginAtivacaoDto login)
        {
            var usuario = await ValidarAcesso(login.Apelido, login.Senha);

            if (!usuario.ValidarCodigoAtivacao(login.CodigoAtivacao))
                throw new InvalidOperationException("Código de ativação inválido");

            usuario.Ativar();
            await _usuarioRepository.Alterar(usuario);

            return Convert.ToBase64String(usuario.Id.ToByteArray());
        }

        public async Task<UsuarioResponseDto> ObterUsuario(Guid usuarioId)
        {
            var usuario = await _usuarioRepository.ObterPorId(usuarioId)
                ?? throw new KeyNotFoundException("Usuário não encontrado com o Id informado");

            return usuario.ToDto();
        }

        public async Task<UsuarioResponseDto> ObterUsuarioPorApelido(string apelido)
        {
            var usuario = await _usuarioRepository.ObterPorApelido(apelido)
                ?? throw new KeyNotFoundException("Usuário não encontrado com o Apelido informado");

            return usuario.ToDto();
        }

        public async Task<UsuarioResponseDto> ObterUsuarioPorEmail(string email)
        {
            var usuario = await _usuarioRepository.ObterPorEmail(email)
                ?? throw new KeyNotFoundException("Usuário não encontrado com o Email informado");

            return usuario.ToDto();
        }

        public async Task<IEnumerable<UsuarioResponseDto>> ObterUsuarios()
        {
            var usuarios = await _usuarioRepository.ObterTodos();

            return usuarios.Select(u => u.ToDto());
        }

        public async Task<IEnumerable<UsuarioResponseDto>> ObterUsuariosAtivos()
        {
            var usuarios = await _usuarioRepository.ObterTodosAtivos();

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
            var usuario = await _usuarioRepository.ObterPorId(usuarioId);
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

        public async Task TornarUsuario(Guid usuarioId)
        {
            var usuario = await _usuarioRepository.ObterPorId(usuarioId)
                ?? throw new KeyNotFoundException("Usuário não encontrado com o Id informado");

            usuario.TornarUsuario();

            await _usuarioRepository.Alterar(usuario);
        }

        public async Task TornarAdministrador(Guid usuarioId)
        {
            var usuario = await _usuarioRepository.ObterPorId(usuarioId)
                ?? throw new KeyNotFoundException("Usuário não encontrado com o Id informado");

            usuario.TornarAdministrador();

            await _usuarioRepository.Alterar(usuario);
        }

        private async Task<Usuario> ValidarAcesso(string apelido, string senha)
        {
            var usuario = await _usuarioRepository.ObterPorApelido(apelido)
                ?? throw new UnauthorizedAccessException("Usuário ou senha inválida");

            if (!usuario.Ativo)
                throw new InvalidOperationException("Sua conta encontra-se bloqueada");

            if (!usuario.ValidarSenha(senha))
            {
                usuario.AdicionarTentativaLoginErrada();

                if (usuario.TentativasLogin >= 3) usuario.Desativar();

                await _usuarioRepository.Alterar(usuario);

                throw new UnauthorizedAccessException("Usuário ou senha inválida");
            }

            if (usuario.TentativasLogin > 0)
            {
                usuario.ZerarTentativasLoginErrada();
                await _usuarioRepository.Alterar(usuario);
            }

            return usuario;
        }
    }
}
