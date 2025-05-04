using FCG.Application.DTOs;

namespace FCG.Application.Interfaces
{
    public interface IJwtService
    {
        string GerarToken(UsuarioResponseDto usuarioDto);
    }
}
