using FCG.Application.Entities;
using FCG.Application.Interfaces;
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

        public async Task AdicionarUsuario(UsuarioDto usuarioDto)
        {
            throw new NotImplementedException();
        }

        public async Task AlterarUsuario(UsuarioDto usuarioDto)
        {
            throw new NotImplementedException();
        }

        public async Task AtivarUsuario(Guid usuarioId)
        {
            throw new NotImplementedException();
        }

        public async Task DesativarUsuario(Guid usuarioId)
        {
            throw new NotImplementedException();
        }

        public async Task<UsuarioDto> LoginAsync(LoginDto login)
        {
            throw new NotImplementedException();
        }

        public async Task<UsuarioDto> ObterUsuarioAsync(Guid usuarioId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UsuarioDto>> ObterUsuariosAsync()
        {
            throw new NotImplementedException();
        }
    }
}
