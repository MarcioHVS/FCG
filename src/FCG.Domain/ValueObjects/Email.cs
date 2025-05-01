using System.Net.Mail;
using System.Text.RegularExpressions;

namespace FCG.Domain.ValueObjects
{
    public class Email
    {
        public string Endereco { get; private set; }

        public Email(string endereco)
        {
            if (!Validar(endereco))
                throw new ArgumentException("Endereço de e-mail inválido.");

            Endereco = endereco;
        }

        public static bool Validar(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var endereco = new MailAddress(email);
                string padraoRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

                return Regex.IsMatch(email, padraoRegex);
            }
            catch
            {
                return false;
            }
        }
    }
}
