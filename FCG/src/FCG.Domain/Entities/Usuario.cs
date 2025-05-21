using FCG.Domain.Enums;
using FCG.Domain.Exceptions;
using Isopoh.Cryptography.Argon2;
using System.Drawing;
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
        public int TentativasLogin { get; private set; }
        public string CodigoAtivacao { get; private set; }
        public string CodigoValidacao { get; private set; }

        public ICollection<Pedido> Pedidos { get; set; }

        //EF
        protected Usuario() { }

        private Usuario(Guid id, string nome, string apelido, string email, string senhaHash, string salt)
        {
            Id = id;
            Nome = nome;
            Apelido = apelido;
            Email = email;
            Senha = senhaHash;
            Salt = salt;
        }

        public void AdicionarTentativaLoginErrada() => TentativasLogin ++;
        public void ZerarTentativasLoginErrada() => TentativasLogin = 0;
        public void TornarAdministrador() => Role = Role.Administrador;
        public void TornarUsuario() => Role = Role.Usuario;

        public static Usuario CriarAlterar(Guid? id, string nome, string apelido, string email, string senha)
        {
            if (!EmailValido(email))
                throw new OperacaoInvalidaException("Endereço de e-mail inválido.");

            var senhaHash = string.Empty;
            var salt = string.Empty;

            if (id == null)
            {
                if (!SenhaForte(senha))
                    throw new OperacaoInvalidaException("A senha deve conter pelo menos uma letra, um número e um caractere especial.");

                (senhaHash, salt) = GerarHashSenha(senha);
            }

            return new Usuario(id ?? Guid.NewGuid(), nome, apelido, email, senhaHash, salt);
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

        public void GerarCodigoAtivacao()
        {
            const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();

            CodigoAtivacao = new string(Enumerable.Repeat(caracteres, 8)
                                        .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public bool ValidarCodigoAtivacao(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                return false;

            if (CodigoAtivacao == codigo)
            {
                Ativar();
                return true;
            }

            Desativar();
            return false;
        }

        public void GerarCodigoValidacao()
        {
            CodigoValidacao = Guid.NewGuid().ToString().ToUpper();
        }

        public bool ValidarCodigoValidacao(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo) || !Guid.TryParse(codigo, out _))
            {
                Desativar();
                return false;
            }

            if (CodigoValidacao == codigo)
            {
                Ativar();
                return true;
            }

            Desativar();
            return false;
        }

        private static (string hash, string salt) GerarHashSenha(string senha)
        {
            var salt = Guid.NewGuid().ToString();
            var hash = Argon2.Hash(senha + salt);
            return (hash, salt);
        }
    }
}
