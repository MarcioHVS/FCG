using FCG.Domain.Entities;

namespace FCG.Domain.Interfaces
{
    public interface IPromocaoRepository
    {
        Task<Promocao> ObterPorIdAsync(Guid id);
        Task<IEnumerable<Promocao>> ObterTodosAsync();
        Task Adicionar(Promocao promocao);
        Task Alterar(Promocao promocao);
        Task Ativar(Guid id);
        Task Desativar(Guid id);

        Task<Promocao?> ObterPromocaoPorCupomAsync(string cupom);
    }
}
