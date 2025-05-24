using FCG.Application.Interfaces;

namespace FCG.Application.Services.Email
{
    public class ModeloEmail : IModeloEmail
    {
        private readonly IEmailService _emailService;

        public ModeloEmail(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task CodigoAtivacao(string email, string nome, string codigoAtivacao)
        {
            var assunto = "Código de Ativação da Conta";
            var mensagem = $@"
            <html>
                <body>
                    <h1>Olá, {nome}!</h1>
                    <p>Para ativar a sua conta você precisa realizar um login informando o código de ativação.</p>
                    <p>Seu código de ativação é: <strong>{codigoAtivacao}</strong></p>
                </body>
            </html>";

            await _emailService.EnviarEmail(email, assunto, mensagem);
        }

        public async Task SolicitacaoNovaSenha(string email, string nome, string codigoValidacao)
        {
            var assunto = "Solicitação de nova senha";
            var mensagem = $@"
            <html>
                <body>
                    <h1>Olá, {nome}!</h1>
                    <p>Recebemos uma solicitação para redefinir sua senha.</p>
                    <p>Você deverá acessar a opção 'Minha Nova Senha' e informar suas credenciais com a nova senha e o código de validação: <strong>{codigoValidacao}</strong></p>
                    <p>Se você não solicitou a recuperação de senha, ignore este e-mail.</p>
                </body>
            </html>";

            await _emailService.EnviarEmail(email, assunto, mensagem);
        }
    }
}
