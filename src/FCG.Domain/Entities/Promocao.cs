using FCG.Domain.Enums;

namespace FCG.Domain.Entities
{
    public class Promocao : EntityBase
    {
        public string Cupom { get; private set; }
        public string Descricao { get; private set; }
        public TipoDesconto TipoDesconto { get; private set; }
        public decimal ValorDesconto { get; private set; }
        public DateTime DataValidade { get; private set; }

        //EF
        protected Promocao() { }

        private Promocao(Guid id, string cupom, string descricao, TipoDesconto tipoDesconto,
                         decimal valorDesconto, DateTime dataValidade)
        {
            Id = id;
            Cupom = cupom;
            Descricao = descricao;
            TipoDesconto = tipoDesconto;
            ValorDesconto = valorDesconto;
            DataValidade = dataValidade;
        }

        public static Promocao Criar(Guid? id, string cupom, string descricao, TipoDesconto tipoDesconto,
                                     decimal valorDesconto, DateTime dataValidade)
        {
            return new Promocao(id ?? Guid.NewGuid(), cupom, descricao, tipoDesconto, valorDesconto, dataValidade);
        }
    }
}
