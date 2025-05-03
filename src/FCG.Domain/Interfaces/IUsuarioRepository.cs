using FCG.Domain.Entities;

namespace FCG.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<bool> ExisteIdAsync(Guid id);
        Task<Usuario?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<Usuario>> ObterTodosAsync();
        Task Adicionar(Usuario usuario);
        Task Alterar(Usuario usuario);
        Task Ativar(Guid id);
        Task Desativar(Guid id);

        Task<Usuario?> ObterUsuarioPorApelidoAsync(string apelido);
        Task<bool> ExisteApelidoAsync(string apelido, Guid usuarioId);
        Task<bool> ExisteEmailAsync(string email, Guid usuarioId);
    }
}
