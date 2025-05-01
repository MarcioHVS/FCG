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

        public Promocao(Guid id, string cupom, string descricao, TipoDesconto tipoDesconto,
                        decimal valorDesconto, DateTime dataValidade, DateTime dataCadastro)
        {
            Id = id;
            Cupom = cupom;
            Descricao = descricao;
            TipoDesconto = tipoDesconto;
            ValorDesconto = valorDesconto;
            DataValidade = dataValidade;
            DataCadastro = dataCadastro;
        }
    }
}
