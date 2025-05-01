using FCG.Domain.Entities;

namespace FCG.Domain.Interfaces
{
    public interface IRepository<T> where T : EntityBase
    {
        Task<T> ObterPorIdAsync(Guid id);
        Task<IEnumerable<T>> ObterTodosAsync();
        Task Adicionar(T entidade);
        Task Alterar(T entidade);
        Task Ativar(Guid id);
        Task Desativar(Guid id);
    }
}
