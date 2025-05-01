using FCG.Domain.Entities;

namespace FCG.Domain.Interfaces
{
    public interface IJogoRepository
    {
        Task<Jogo> ObterPorIdAsync(Guid id);
        Task<IEnumerable<Jogo>> ObterTodosAsync();
        Task Adicionar(Jogo jogo);
        Task Alterar(Jogo jogo);
        Task Ativar(Guid id);
        Task Desativar(Guid id);

        Task<bool> ExisteTituloAsync(string titulo);
    }
}
