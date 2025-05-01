using FCG.Application.Entities;

namespace FCG.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioDto> LoginAsync(LoginDto login);
        Task<UsuarioDto> ObterUsuarioAsync(Guid usuarioId);
        Task<IEnumerable<UsuarioDto>> ObterUsuariosAsync();
        Task AdicionarUsuario(UsuarioDto usuarioDto);
        Task AlterarUsuario(UsuarioDto usuarioDto);
        Task AtivarUsuario(Guid usuarioId);
        Task DesativarUsuario(Guid usuarioId);
    }
}
