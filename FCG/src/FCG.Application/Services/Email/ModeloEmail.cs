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

        public async Task Boas_Vindas(string email, string nome)
        {
            var assunto = "Boas-vindas";
            var mensagem = $@"
            <html>
                <body>
                    <h1>Olá, {nome}!</h1>
                    <h2>Agora a diversão está garantida!</h2>
                    <p>Seja bem-vindo à Plataforma de Games <strong>FCG - FIAP Cloud Games</strong></p>
                    <p>Tenho certeza que teremos muitas aventuras pela frente!</strong></p>
                </body>
            </html>";

            await _emailService.EnviarEmail(email, assunto, mensagem);
        }

        public async Task CodigoAtivacao(string email, string nome, string codigoAtivacao)
        {
            var assunto = "Código de Ativação da Conta";
            var mensagem = $@"
            <html>
                <body>
                    <h1>Olá, {nome}!</h1>
                    <p>Você preencheu o seu cadastro corretamente, mas para concluir o processo é necessário ativar a conta.</p>
                    <p>Seu código de ativação é: <strong>{codigoAtivacao}</strong></p>
                    <p>Use este código para ativar sua conta.</p>
                </body>
            </html>";

            await _emailService.EnviarEmail(email, assunto, mensagem);
        }
    }
}
