using FCG.Domain.Enums;

namespace FCG.Domain.Entities
{
    public class Jogo : EntityBase
    {
        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public Genero Genero { get; private set; }
        public decimal Valor { get; private set; }

        public ICollection<Pedido> Pedidos { get; set; }

        //EF
        protected Jogo() { }

        private Jogo(Guid id, string titulo, string descricao, Genero genero, decimal valor)
        {
            Id = id;
            Titulo = titulo;
            Descricao = descricao;
            Genero = genero;
            Valor = valor;
        }

        public static Jogo Criar(Guid? id, string titulo, string descricao, Genero genero, decimal valor)
        {
            return new Jogo(id ?? Guid.NewGuid(), titulo, descricao, genero, valor);
        }
    }
}
