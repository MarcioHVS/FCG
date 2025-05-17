using FCG.Domain.Entities;

namespace FCG.Domain.Interfaces
{
    public interface IJogoRepository
    {
        Task<Jogo?> ObterPorId(Guid id);
        Task<IEnumerable<Jogo>> ObterTodos();
        Task<IEnumerable<Jogo>> ObterTodosAtivos();
        Task Adicionar(Jogo jogo);
        Task Alterar(Jogo jogo);
        Task Remover(Guid id);
        Task Ativar(Guid id);
        Task Desativar(Guid id);

        Task<bool> Existe(string titulo);
        Task<Jogo> ObterPorTitulo(string titulo);
    }
}
