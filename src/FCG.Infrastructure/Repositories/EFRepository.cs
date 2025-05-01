using FCG.Domain.Entities;
using FCG.Domain.Interfaces;
using FCG.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Repositories
{
    public class EFRepository<T> : IRepository<T> where T : EntityBase
    {
        protected ApplicationDbContext _context;
        protected DbSet<T> _dbSet;

        public EFRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T> ObterPorIdAsync(Guid id)
        {
            return await _dbSet.AsNoTracking()
                .FirstAsync(e => e.Id == id);
        }

        public virtual async Task<IEnumerable<T>> ObterTodosAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task Adicionar(T entidade)
        {
            _dbSet.Add(entidade);
            await _context.Salvar();
        }

        public async Task Alterar(T entidade)
        {
            _dbSet.Update(entidade);
            await _context.Salvar();
        }

        public async Task Ativar(Guid id)
        {
            var entidade = await ObterEntidadeAsync(id);
            entidade.Ativar();

            _dbSet.Update(entidade);
            await _context.Salvar();
        }

        public async Task Desativar(Guid id)
        {
            var entidade = await ObterEntidadeAsync(id);
            entidade.Desativar();

            _dbSet.Update(entidade);
            await _context.Salvar();
        }

        private async Task<T> ObterEntidadeAsync(Guid id)
        {
            var entidade = await _dbSet.FindAsync(id);
            if (entidade == null)
                throw new InvalidOperationException($"{typeof(T).Name} com ID {id} não encontrado(a).");

            return entidade;
        }
    }
}
