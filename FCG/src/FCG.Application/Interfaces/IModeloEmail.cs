namespace FCG.Application.Interfaces
{
    public interface IModeloEmail
    {
        Task Boas_Vindas(string email, string nome);
        Task CodigoAtivacao(string email, string nome, string codigoAtivacao);
        Task EsqueciMinhaSenha(string email, string nome, string codigoAtivacao);
    }
}
