using FCG.Domain.Enums;
using FCG.Domain.ValueObjects;

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

        public Usuario(Guid id, string nome, string apelido, Email email, string senha, Role role, DateTime dataCadastro)
        {
            Id = id;
            Nome = nome;
            Apelido = apelido;
            Email = email;
            Senha = senha;
            Role = role;
            DataCadastro = dataCadastro;
        }
    }
}
