namespace FCG.Application.Interfaces
{
    public interface IEmailService
    {
        Task EnviarEmail(string para, string assunto, string mensagem);
    }
}
