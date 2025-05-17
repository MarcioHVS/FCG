using FCG.Domain.Entities;

namespace FCG.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> ObterPorId(Guid id);
        Task<IEnumerable<Usuario>> ObterTodos();
        Task<IEnumerable<Usuario>> ObterTodosAtivos();
        Task Adicionar(Usuario usuario);
        Task Alterar(Usuario usuario);
        Task Remover(Guid id);
        Task Ativar(Guid id);
        Task Desativar(Guid id);

        Task<bool> ExisteApelido(string apelido, Guid usuarioId);
        Task<bool> ExisteEmail(string email, Guid usuarioId);
        Task<Usuario?> ObterPorApelido(string apelido);
        Task<Usuario?> ObterPorEmail(string apelido);
    }
}
