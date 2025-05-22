namespace FCG.Application.Interfaces
{
    public interface IModeloEmail
    {
        Task CodigoAtivacao(string email, string nome, string codigoAtivacao);
        Task SolicitacaoNovaSenha(string email, string nome, string codigoAtivacao);
    }
}
