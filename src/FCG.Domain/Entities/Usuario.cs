using FCG.Domain.Enums;
using FCG.Domain.ValueObjects;
using Isopoh.Cryptography.Argon2;

namespace FCG.Domain.Entities
{
    public class Usuario : EntityBase
    {
        public string Nome { get; private set; }
        public string Apelido { get; private set; }
        public Email Email { get; private set; }
        public string Senha { get; private set; }
        public string Salt { get; private set; }
        public Role Role { get; private set; }

        public ICollection<Pedido> Pedidos { get; set; }

        //EF
        protected Usuario() { }

        private Usuario(Guid id, string nome, string apelido, Email email, string senhaHash, string salt, Role role)
        {
            Id = id;
            Nome = nome;
            Apelido = apelido;
            Email = email;
            Senha = senhaHash;
            Salt = salt;
            Role = role;
        }

        public static Usuario Adicionar(string nome, string apelido, string email, string senha, Role role)
        {
            var emailObj = new Email(email);
            var (senhaHash, salt) = GerarHashSenha(senha);

            return new Usuario(Guid.NewGuid(), nome, apelido, emailObj, senhaHash, salt, role);
        }

        public static Usuario Alterar(Guid id, string nome, string apelido, string email, string senha, Role role)
        {
            var emailObj = new Email(email);
            var (senhaHash, salt) = GerarHashSenha(senha);

            return new Usuario(id, nome, apelido, emailObj, senhaHash, salt, role);
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
            var salt = Guid.NewGuid().ToString(); // Gera um salt único
            var hash = Argon2.Hash(senha + salt);
            return (hash, salt);
        }
    }
}
