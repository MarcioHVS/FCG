using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using FCG.Application.Mappers;
using FCG.Domain.Entities;
using FCG.Domain.Exceptions;
using FCG.Domain.Interfaces;

namespace FCG.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IModeloEmail _email;
        private readonly IJwtService _jwtService;

        public UsuarioService(IUsuarioRepository usuarioRepository, IModeloEmail email, IJwtService jwtService)
        {
            _usuarioRepository = usuarioRepository;
            _email = email;
            _jwtService = jwtService;
        }

        public async Task<string> Login(LoginDto login)
        {
            var usuario = await ValidarAcesso(login.Apelido, login.Senha, false);

            return _jwtService.GerarToken(usuario.ToDto());
        }

        public async Task<string> LoginAtivacao(LoginAtivacaoDto login)
        {
            var usuario = await ValidarAcesso(login.Apelido, login.Senha, true);

            if (!usuario.ValidarCodigoAtivacao(login.CodigoAtivacao))
                throw new OperacaoInvalidaException("Código de ativação inválido");

            usuario.ZerarTentativasLoginErrada();
            usuario.LimparCodigoAtivacao();
            usuario.Ativar();
            await _usuarioRepository.Alterar(usuario, true);

            return _jwtService.GerarToken(usuario.ToDto());
        }

        public async Task<string> LoginNovaSenha(LoginNovaSenhaDto login)
        {
            var usuario = await _usuarioRepository.ObterPorApelido(login.Apelido)
                ?? throw new OperacaoInvalidaException("Usuário ou Código inválido");

            if(!usuario.ValidarCodigoValidacao(login.CodigoValidacao))
                throw new OperacaoInvalidaException("Usuário ou Código inválido");

            usuario.AlterarSenha(login.NovaSenha);
            usuario.ZerarTentativasLoginErrada();
            usuario.LimparCodigoValidacao();
            usuario.Ativar();
            await _usuarioRepository.Alterar(usuario, true);

            return _jwtService.GerarToken(usuario.ToDto());
        }

        public async Task SolicitarNovaSenha(string email)
        {
            var usuario = await _usuarioRepository.ObterPorEmail(email)
                ?? throw new OperacaoInvalidaException("E-mail não encontrado");

            usuario.GerarCodigoValidacao();
            await _usuarioRepository.Alterar(usuario);

            await _email.SolicitacaoNovaSenha(usuario.Email, usuario.Nome, usuario.CodigoValidacao);
        }

        public async Task SolicitarReativacao(string email)
        {
            var usuario = await _usuarioRepository.ObterPorEmail(email)
                ?? throw new OperacaoInvalidaException("E-mail não encontrado");

            if(usuario.Ativo)
                throw new OperacaoInvalidaException("Sua conta já se encontra ativa");

            usuario.GerarCodigoAtivacao();
            await _usuarioRepository.Alterar(usuario);

            await _email.CodigoAtivacao(usuario.Email, usuario.Nome, usuario.CodigoAtivacao);
        }

        public async Task ReenviarCodigoAtivacao(string email)
        {
            var usuario = await _usuarioRepository.ObterPorEmail(email)
                ?? throw new KeyNotFoundException("O e-mail informado não foi encontrado");

            if (string.IsNullOrEmpty(usuario.CodigoAtivacao))
                throw new KeyNotFoundException("Você não tem um código de ativação");

            await _email.CodigoAtivacao(usuario.Email, usuario.Nome, usuario.CodigoAtivacao);
        }

        public async Task ReenviarCodigoValidacao(string email)
        {
            var usuario = await _usuarioRepository.ObterPorEmail(email)
                ?? throw new KeyNotFoundException("O e-mail informado não foi encontrado");

            if (string.IsNullOrEmpty(usuario.CodigoValidacao))
                throw new KeyNotFoundException("Você não tem um código de validação");

            await _email.SolicitacaoNovaSenha(usuario.Email, usuario.Nome, usuario.CodigoValidacao);
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
            var usuario = usuarioDto.ToDomain();
            await _usuarioRepository.Adicionar(usuario);

            await _email.CodigoAtivacao(usuario.Email, usuario.Nome, usuario.CodigoAtivacao);
        }

        public async Task AlterarUsuario(UsuarioAlterarDto usuarioDto)
            => await _usuarioRepository.Alterar(usuarioDto.ToDomain());

        public async Task AlterarSenha(Guid usuarioId, string novaSenha)
        {
            var usuario = await _usuarioRepository.ObterPorId(usuarioId);
            usuario?.AlterarSenha(novaSenha);

            await _usuarioRepository.Alterar(usuario);
        }

        public async Task AtivarUsuario(Guid usuarioId)
            => await _usuarioRepository.Ativar(usuarioId);

        public async Task DesativarUsuario(Guid usuarioId)
            => await _usuarioRepository.Desativar(usuarioId);

        public async Task TornarUsuario(Guid usuarioId)
            => await AlterarTipoUsuario(usuarioId, u => u.TornarUsuario());

        public async Task TornarAdministrador(Guid usuarioId)
            => await AlterarTipoUsuario(usuarioId, u => u.TornarAdministrador());

        #region Métodos Privados
        private async Task AlterarTipoUsuario(Guid usuarioId, Action<Usuario> alterarFunc)
        {
            var usuario = await _usuarioRepository.ObterPorId(usuarioId)
                ?? throw new KeyNotFoundException("Usuário não encontrado com o Id informado");

            alterarFunc(usuario);
            await _usuarioRepository.Alterar(usuario);
        }

        private async Task<Usuario> ValidarAcesso(string apelido, string senha, bool ehLoginAtivacao)
        {
            var usuario = await _usuarioRepository.ObterPorApelido(apelido)
                ?? throw new UnauthorizedAccessException("Usuário ou senha inválida");

            if (!ehLoginAtivacao)
            {
                if (!usuario.Ativo)
                    throw new OperacaoInvalidaException("Sua conta está bloqueada. Solicite a reativação da conta ou redefinir sua senha.");
            }

            if (!usuario.ValidarSenha(senha))
            {
                if (usuario.Ativo)
                {
                    usuario.AdicionarTentativaLoginErrada();
                    await _usuarioRepository.Alterar(usuario, true);

                    if (usuario.TentativasLogin >= 3)
                    {
                        await _usuarioRepository.Desativar(usuario.Id);

                        throw new UnauthorizedAccessException("Usuário bloqueado por excesso de tentativas. Solicite a reativação da conta ou redefinir sua senha.");
                    }
                }

                throw new UnauthorizedAccessException("Usuário ou senha inválida.");
            }

            if (usuario.TentativasLogin > 0)
            {
                usuario.ZerarTentativasLoginErrada();
                await _usuarioRepository.Alterar(usuario);
            }

            return usuario;
        }
        #endregion
    }
}
