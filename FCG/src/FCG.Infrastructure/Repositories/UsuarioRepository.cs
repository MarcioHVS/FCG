using FCG.Domain.Entities;
using FCG.Domain.Interfaces;
using FCG.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Repositories
{
    public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExisteApelido(string apelido, Guid usuarioId)
        {
            return await _context.Usuarios.AsNoTracking()
                .Where(u => u.Id != usuarioId && u.Apelido == apelido).AnyAsync();
        }

        public async Task<bool> ExisteEmail(string email, Guid usuarioId)
        {
            return await _context.Usuarios.AsNoTracking()
                .Where(u => u.Id != usuarioId && u.Email == email).AnyAsync();
        }

        public async Task<Usuario?> ObterPorApelido(string apelido)
        {
            return await _context.Usuarios.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Apelido == apelido);
        }

        public async Task<Usuario?> ObterPorEmail(string email)
        {
            return await _context.Usuarios.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
