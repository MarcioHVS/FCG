using FCG.Domain.Entities;
using FCG.Domain.Interfaces;
using FCG.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Repositories
{
    public class UsuarioRepository : EFRepository<Usuario>, IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Usuario?> ObterUsuarioPorApelidoAsync(string apelido)
        {
            return await _context.Usuarios.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Apelido.Equals(apelido));
        }

        public async Task<bool> ExisteApelidoAsync(string apelido, Guid usuarioId)
        {
            return await _context.Usuarios.AsNoTracking()
                .Where(u => !u.Id.Equals(usuarioId) && u.Apelido.Equals(apelido)).AnyAsync();
        }

        public async Task<bool> ExisteEmailAsync(string email, Guid usuarioId)
        {
            return await _context.Usuarios.AsNoTracking()
                .Where(u => !u.Id.Equals(usuarioId) && u.Email.Equals(email)).AnyAsync();
        }
    }
}
