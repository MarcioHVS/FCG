using FCG.Application.Interfaces;
using System.Net;
using System.Net.Mail;

namespace FCG.Application.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }

        public async Task EnviarEmail(string para, string assunto, string mensagem)
        {
            try
            {
                using (SmtpClient client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort))
                {
                    client.Credentials = new NetworkCredential(_emailSettings.SmtpUser, _emailSettings.SmtpPassword);
                    client.EnableSsl = true;

                    MailMessage mailMessage = new MailMessage
                    {
                        From = new MailAddress(_emailSettings.SmtpUser),
                        Subject = assunto,
                        Body = mensagem,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(para);

                    await client.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao enviar e-mail: {ex.Message}");
            }
        }

        public Task EnviarEmail(string para, (string assunto, string mensagem) modelo)
        {
            throw new NotImplementedException();
        }
    }
}
