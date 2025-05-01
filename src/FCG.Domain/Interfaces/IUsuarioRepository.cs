using FCG.Domain.Entities;

namespace FCG.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario> ObterPorIdAsync(Guid id);
        Task<IEnumerable<Usuario>> ObterTodosAsync();
        Task Adicionar(Usuario usuario);
        Task Alterar(Usuario usuario);
        Task Ativar(Guid id);
        Task Desativar(Guid id);

        Task<Usuario?> ObterUsuarioPorApelidoAsync(string apelido);
        Task<bool> ExisteApelidoAsync(string email);
        Task<bool> ExisteEmailAsync(string email);
    }
}
