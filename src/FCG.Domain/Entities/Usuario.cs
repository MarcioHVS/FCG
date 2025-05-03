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
        public Role Role { get; private set; }

        public ICollection<Pedido> Pedidos { get; set; }

        //EF
        protected Usuario() { }

        public Usuario(Guid id, string nome, string apelido, Email email, string senha, Role role)
        {
            Id = id;
            Nome = nome;
            Apelido = apelido;
            Email = email;
            Senha = string.IsNullOrEmpty(senha) ? senha : Argon2.Hash(senha);
            Role = role;
        }

        public void AlterarSenha(string novaSenha) => Senha = Argon2.Hash(novaSenha);
        public bool ValidarSenha(string senha) => Argon2.Verify(Senha, senha);
    }
}
