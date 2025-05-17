using FCG.Domain.Entities;

namespace FCG.Domain.Interfaces
{
    public interface IRepositoryBase<T> where T : EntityBase
    {
        Task<T?> ObterPorId(Guid id);
        Task<IEnumerable<T>> ObterTodos();
        Task<IEnumerable<T>> ObterTodosAtivos();
        Task Adicionar(T entidade);
        Task Alterar(T entidade);
        Task Remover(Guid id);
        Task Ativar(Guid id);
        Task Desativar(Guid id);
    }
}
