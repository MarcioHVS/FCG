using FCG.Domain.Enums;
using FCG.Domain.Exceptions;
using Isopoh.Cryptography.Argon2;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace FCG.Domain.Entities
{
    public class Usuario : EntityBase
    {
        public string Nome { get; private set; }
        public string Apelido { get; private set; }
        public string Email { get; private set; }
        public string Senha { get; private set; }
        public string Salt { get; private set; }
        public Role Role { get; private set; }

        public ICollection<Pedido> Pedidos { get; set; }

        //EF
        protected Usuario() { }

        private Usuario(Guid id, string nome, string apelido, string email, string senhaHash, string salt, Role role)
        {
            Id = id;
            Nome = nome;
            Apelido = apelido;
            Email = email;
            Senha = senhaHash;
            Salt = salt;
            Role = role;
        }

        public static Usuario Criar(Guid? id, string nome, string apelido, string email, string senha, Role role)
        {
            if (!EmailValido(email))
                throw new OperacaoInvalidaException("Endereço de e-mail inválido.");

            if (!SenhaForte(senha))
                throw new OperacaoInvalidaException("A senha deve conter pelo menos uma letra, um número e um caractere especial.");

            var (senhaHash, salt) = GerarHashSenha(senha);

            return new Usuario(id ?? Guid.NewGuid(), nome, apelido, email, senhaHash, salt, role);
        }

        public static bool EmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var endereco = new MailAddress(email);
                return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            }
            catch
            {
                return false;
            }
        }

        public static bool SenhaForte(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha))
                return false;

            bool temLetra = Regex.IsMatch(senha, @"[a-zA-Z]");
            bool temNumero = Regex.IsMatch(senha, @"\d");
            bool temEspecial = Regex.IsMatch(senha, @"[!@#$%^&*(),.?""{}|<>]");

            return temLetra && temNumero && temEspecial;
        }

        public void AlterarSenha(string novaSenha)
        {
            var (novoHash, novoSalt) = GerarHashSenha(novaSenha);
            Senha = novoHash;
            Salt = novoSalt;
        }

        public bool ValidarSenha(string senha)
        {
            return Argon2.Verify(Senha, senha + Salt);
        }

        private static (string hash, string salt) GerarHashSenha(string senha)
        {
            var salt = Guid.NewGuid().ToString();
            var hash = Argon2.Hash(senha + salt);
            return (hash, salt);
        }
    }
}
