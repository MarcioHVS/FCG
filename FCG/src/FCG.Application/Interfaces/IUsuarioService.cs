using FCG.Application.DTOs;

namespace FCG.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<string> Login(LoginDto login);
        Task<string> LoginAtivacao(LoginAtivacaoDto login);
        Task<string> LoginNovaSenha(LoginNovaSenhaDto login);
        Task EsqueciMinhaSenha(string email);
        Task<UsuarioResponseDto> ObterUsuario(Guid usuarioId);
        Task<UsuarioResponseDto> ObterUsuarioPorApelido(string apelido);
        Task<UsuarioResponseDto> ObterUsuarioPorEmail(string email);
        Task<IEnumerable<UsuarioResponseDto>> ObterUsuarios();
        Task<IEnumerable<UsuarioResponseDto>> ObterUsuariosAtivos();
        Task AdicionarUsuario(UsuarioAdicionarDto usuarioDto);
        Task AlterarUsuario(UsuarioAlterarDto usuarioDto);
        Task AlterarSenha(Guid usuarioId, string novaSenha);
        Task AtivarUsuario(Guid usuarioId);
        Task DesativarUsuario(Guid usuarioId);
        Task TornarUsuario(Guid usuarioId);
        Task TornarAdministrador(Guid usuarioId);
    }
}
