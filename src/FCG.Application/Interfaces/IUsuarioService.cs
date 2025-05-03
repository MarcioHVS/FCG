using FCG.Application.Entities;

namespace FCG.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioResponseDto> LoginAsync(LoginDto login);
        Task<UsuarioResponseDto> ObterUsuarioAsync(Guid usuarioId);
        Task<IEnumerable<UsuarioResponseDto>> ObterUsuariosAsync();
        Task AdicionarUsuario(UsuarioAdicionarDto usuarioDto);
        Task AlterarUsuario(UsuarioAlterarDto usuarioDto);
        Task AlterarSenha(Guid usuarioId, string novaSenha);
        Task AtivarUsuario(Guid usuarioId);
        Task DesativarUsuario(Guid usuarioId);
    }
}
