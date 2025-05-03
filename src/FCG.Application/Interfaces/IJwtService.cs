using FCG.Application.Entities;

namespace FCG.Application.Interfaces
{
    public interface IJwtService
    {
        string GerarToken(UsuarioResponseDto usuarioDto);
    }
}
