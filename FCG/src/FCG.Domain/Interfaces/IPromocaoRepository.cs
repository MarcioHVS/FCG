using FCG.Domain.Entities;

namespace FCG.Domain.Interfaces
{
    public interface IPromocaoRepository
    {
        Task<Promocao?> ObterPorId(Guid id);
        Task<IEnumerable<Promocao>> ObterTodos();
        Task<IEnumerable<Promocao>> ObterTodosAtivos();
        Task Adicionar(Promocao promocao);
        Task Alterar(Promocao promocao, bool AlterarAtivo = false);
        Task Remover(Guid id);
        Task Ativar(Guid id);
        Task Desativar(Guid id);

        Task<bool> Existe(string cupom);
        Task<Promocao?> ObterPorCupom(string cupom);
    }
}
