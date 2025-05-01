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

        public Jogo(Guid id, string titulo, string descricao, Genero genero, decimal valor, DateTime dataCadastro)
        {
            Id = id;
            Titulo = titulo;
            Descricao = descricao;
            Genero = genero;
            Valor = valor;
            DataCadastro = dataCadastro;
        }
    }
}
