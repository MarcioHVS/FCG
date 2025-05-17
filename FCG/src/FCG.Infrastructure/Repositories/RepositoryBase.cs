using FCG.Domain.Entities;
using FCG.Domain.Interfaces;
using FCG.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : EntityBase
    {
        protected ApplicationDbContext _context;
        protected DbSet<T> _dbSet;

        public RepositoryBase(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<bool> ExisteId(Guid id)
        {
            return await _dbSet.AnyAsync(e => e.Id == id);
        }

        public virtual async Task<T?> ObterPorId(Guid id)
        {
            return await _dbSet.AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public virtual async Task<IEnumerable<T>> ObterTodos()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> ObterTodosAtivos()
        {
            return await _dbSet.AsNoTracking()
                .Where(e => e.Ativo).ToListAsync();
        }

        public async Task Adicionar(T entidade)
        {
            entidade.Ativar();
            _dbSet.Add(entidade);
            await _context.Salvar();
        }

        public async Task Alterar(T entidade)
        {
            var entidadeBD = await ObterEntidade(entidade.Id);

            if (entidadeBD is null)
                throw new Exception("Registro não encontrado");

            if (entidadeBD.Ativo)
                entidade.Ativar();
            else
                entidade.Desativar();

            _dbSet.Update(entidade);
            await _context.Salvar();
        }

        public async Task Remover(Guid id)
        {
            var entidade = await ObterEntidade(id);

            if (entidade is null)
                throw new Exception("Registro não encontrado");

            _dbSet.Remove(entidade);
            await _context.Salvar();
        }

        public async Task Ativar(Guid id)
        {
            var entidade = await ObterEntidade(id);

            if (entidade is null)
                throw new Exception("Registro não encontrado");

            entidade.Ativar();

            _dbSet.Update(entidade);
            await _context.Salvar();
        }

        public async Task Desativar(Guid id)
        {
            var entidade = await ObterEntidade(id);

            if (entidade is null)
                throw new Exception("Registro não encontrado");

            entidade.Desativar();

            _dbSet.Update(entidade);
            await _context.Salvar();
        }

        private async Task<T> ObterEntidade(Guid id)
        {
            var entidade = await _dbSet.FindAsync(id);

            if (entidade == null)
                throw new InvalidOperationException($"{typeof(T).Name} com ID {id} não encontrado(a).");

            return entidade;
        }
    }
}
